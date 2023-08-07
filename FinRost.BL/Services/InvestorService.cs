using AutoMapper;
using FinRost.BL.Dto.Web.Investor;
using FinRost.BL.Extensions;
using FinRost.DAL;
using FinRost.DAL.Entities.Web;
using Microsoft.EntityFrameworkCore;

namespace FinRost.BL.Services
{
    public class InvestorService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public InvestorService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<Investor>> GetInvestorsAsync(string name)
        {
            var investros = await _db.Investors.Where(it => (it.LastName.Contains(name) || it.FirstName.Contains(name)) && it.Enabled)
                                               .Take(20).ToListAsync();

            return investros;
        }

        public async Task<Investor?> GetInvestorByChatIdAsync(long chatId)
        {
            return await _db.Investors.FirstOrDefaultAsync(it => it.ChatId == chatId);
        }

        public async Task<Investor?> GetInvestorByIdAsync(int Id)
        {
            var investor = await _db.Investors.FindAsync(Id);

            return investor;
        }

        public async Task<Investor> CreateInvestorAsync(InvestorRequest investorRequest)
        {
            var newInvestor = _mapper.Map<Investor>(investorRequest);

            await _db.Investors.AddAsync(newInvestor);
            await _db.SaveChangesAsync();

            return newInvestor;
            
        }

        public async Task<Investor> EditInvestorAsync(InvestorRequest investorRequest)
        {
            var investorDb = await _db.Investors.FindAsync(investorRequest.Id);
            if (investorDb is null)
                throw new CustomException("Инвестор не найден!");

            investorDb.FirstName = investorRequest.FirstName;
            investorDb.LastName = investorRequest.LastName;
            investorDb.Address = investorRequest.Address;
            investorDb.Patronymic = investorRequest.Patronymic;
            investorDb.PhoneNumber = investorRequest.PhoneNumber;
            investorDb.ChatId = investorRequest.ChatId;
            investorDb.INN = investorRequest.INN;   
            investorDb.BIK = investorRequest.BIK;
            investorDb.BirthDate = investorRequest.BirthDate;
            investorDb.BirthPlace = investorRequest.BirthPlace;
            investorDb.MinLoanSum = investorRequest.MinLoanSum;
            investorDb.Passport = investorRequest.Passport;
            investorDb.IssueDate = investorRequest.IssueDate;
            investorDb.IssueCode = investorRequest.IssueCode;
            investorDb.IssueName = investorRequest.IssueName;
            investorDb.CurrentAccount = investorRequest.CurrentAccount;
            investorDb.CorespondeAccount = investorRequest.CorespondeAccount;
            investorDb.BankName = investorRequest.BankName;

            _db.Entry(investorDb).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return investorDb;
        }

        public async Task DeleteInvestorAsync(int Id)
        {
            var investor = await _db.Investors.FindAsync(Id);
            if (investor is null)
                throw new CustomException("Инвестор не найден!");

            investor.Enabled = false;
            _db.Entry(investor).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<List<Investor>> GetInvestorsByMinSumAsync(int minLoanSum = 0)
        {
            var investors = await _db.Investors.Where(it => it.Enabled && it.ChatId != null && minLoanSum >= it.MinLoanSum).ToListAsync();
            return investors;
        }

    }
}
