using AutoMapper;
using FinRost.BL.Dto.Web.Lots;
using FinRost.BL.Extensions;
using FinRost.DAL;
using FinRost.DAL.Entities.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Telegram.Bot.Types;

namespace FinRost.BL.Services
{
    public class LotService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly string _pathLotFiles;
        public LotService(ApplicationDbContext db, IMapper mapper, IConfiguration config)
        {
            _db = db;
            _mapper = mapper;
            _pathLotFiles = config["PathLotFiles"] ?? string.Empty;
        }

        public async Task<Lot?> GetLotByIdAsync(int Id)
        {
            return await _db.Lots.FindAsync(Id);
        }

        public async Task<Lot?> GetLotByOrderIdAsync(int orderId)
        {
            return await _db.Lots.FirstOrDefaultAsync(it => it.OrderId == orderId && it.Enabled);
        }

        public async Task<Lot> CreateLotAsync(LotRequest lotRequest)
        {
            var lot = await _db.Lots.FirstOrDefaultAsync(it => it.OrderId == lotRequest.OrderId && it.Enabled);
            if (lot != null)
                throw new CustomException("Договор уже имеет лот, отредактируйте или удалите старый!");

            var newLot = _mapper.Map<Lot>(lotRequest);

            await _db.Lots.AddAsync(newLot);
            await _db.SaveChangesAsync();

            return newLot;

        }

        public async Task<Lot> EditLotAsync(LotRequest lotRequest)
        {
            var lot = await _db.Lots.FindAsync(lotRequest.Id);
            if (lot is null)
                throw new CustomException("Лот не найден!");

            lot.PledgeType = lotRequest.PledgeType;
            lot.Price = lotRequest.Price;
            lot.Address = lotRequest.Address;
            lot.Square = lotRequest.Square;
            lot.InvestorId = lotRequest.InvestorId;
            
            _db.Entry(lot).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return lot;
        }

        public async Task DeleteLotAsync(int Id)
        {
            var lot = await _db.Lots.FindAsync(Id);
            if (lot is null)
                throw new Exception("Лот не найден!");

            lot.Enabled = false;
            _db.Entry(lot).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task AddFilesAsync(int lotId, List<FileDto> files)
        {

            if (string.IsNullOrEmpty(_pathLotFiles))
                throw new Exception("Ошибка в пути до папки с файлами!");

            var lot = await _db.Lots.FindAsync(lotId);
            if (lot is null)
                throw new CustomException("Лот не найден!");

            if (files.Any(it => it.FileStream is null))
                throw new Exception("Ошибка при загрузке одного или нескольких файлов! Пустая ссылка на stream!");

            if (!Directory.Exists(_pathLotFiles))
            {
                Directory.CreateDirectory(_pathLotFiles);
            }

            var pathToFolder = Path.Combine(_pathLotFiles, lotId.ToString());

            if (!Directory.Exists(Path.Combine(pathToFolder)))
            {
                Directory.CreateDirectory(pathToFolder);
            }


            foreach (var file in files)
            {
                var filePath = Path.Combine(pathToFolder, file.FileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.FileStream.CopyToAsync(fileStream);

            }


        }

        public async Task<List<string>> GetLotFileNamesAsync(int lotId)
        {
            var lot = await _db.Lots.FindAsync(lotId);

            if (lot is null)
                throw new CustomException("Лот не найден!");

            if (!Directory.Exists(_pathLotFiles))
                Directory.CreateDirectory(_pathLotFiles);

            var pathToFolder = Path.Combine(_pathLotFiles, lotId.ToString());

            if (!Directory.Exists(Path.Combine(pathToFolder)))
                Directory.CreateDirectory(pathToFolder);

            return Directory.GetFiles(pathToFolder).Select(it => Path.GetFileName(it)).ToList();

        }

        public async Task<List<string>> GetLotFilesAsync(int lotId)
        {
            var lot = await _db.Lots.FindAsync(lotId);

            if (lot is null)
                throw new CustomException("Лот не найден!");

            if (!Directory.Exists(_pathLotFiles))
                Directory.CreateDirectory(_pathLotFiles);

            var pathToFolder = Path.Combine(_pathLotFiles, lotId.ToString());

            if (!Directory.Exists(Path.Combine(pathToFolder)))
                Directory.CreateDirectory(pathToFolder);

            return Directory.GetFiles(pathToFolder).ToList();

        }

        public async Task<string> GetPathToFile(int lotId, string name)
        {
            return Path.Combine(_pathLotFiles, lotId.ToString(), name);
        }

        public async Task<string> AddFeedBackAsync(int lotId, int investorId, int orderId)
        {
            if (await _db.LotFeedbacks.AnyAsync(it => it.Enabled && it.InvestorId == investorId && it.LotId == lotId))
                return "Отклик уже оставлен.";

            var feedBack = new LotFeedback
            {
                Checked = false,
                CreationDate = DateTime.Now,
                Enabled = true,
                InvestorId = investorId,
                LotId = lotId,
                OrderId = orderId
            };

            await _db.LotFeedbacks.AddAsync(feedBack);
            await _db.SaveChangesAsync();

            return $"Спасибо за отклик! В ближайшее время с вами свяжется наш специалист!";
        }

        public async Task<List<LotFeedbackDto>> GetLotFeedbacksAsync(int lotId)
        {
            var lot = await _db.Lots.FindAsync(lotId);
            if (lot is null)
                throw new CustomException("Лот не найден!");

            var feedBacks = await _db.LotFeedbacks.Where(it => it.Enabled && it.LotId == lotId)
                .Join(_db.Investors, lot => lot.InvestorId, inv => inv.Id, 
                (lot, inv) => new LotFeedbackDto
                {
                    InvestorId = inv.Id,
                    FeedBackDate = lot.CreationDate,
                    FullName = inv.FullName
                })
                .OrderByDescending(it => it.FeedBackDate)
                .ToListAsync();

            return feedBacks;
        }

        public async Task DeleteFeedBacksAsync(int lotId)
        {
            var feedBacks = await _db.LotFeedbacks.Where(it => it.LotId == lotId).ToListAsync();

            foreach(var it in feedBacks)
            {
                _db.LotFeedbacks.Remove(it);
                await _db.SaveChangesAsync();
            }
        }

        public async Task SetLotInvestorAsync(int Id, int investorId)
        {
            var lot = await _db.Lots.FindAsync(Id);
            if (lot is null) 
                throw new CustomException("Лот не найден!");

            var investor = await _db.Investors.FindAsync(investorId);
            if (investor is null)
                throw new CustomException("Инвестор не найден!");

            lot.InvestorId = investorId;
            await _db.SaveChangesAsync();
        }




    }
}
