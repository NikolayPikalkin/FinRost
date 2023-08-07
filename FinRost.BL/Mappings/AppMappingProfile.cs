using AutoMapper;
using FinRost.BL.Dto.Web.Investor;
using FinRost.BL.Dto.Web.Lots;
using FinRost.BL.Dto.Web.Notifications;
using FinRost.BL.Dto.Web.Orders;
using FinRost.BL.Dto.Web.Users;
using FinRost.DAL.Entities.Archi;
using FinRost.DAL.Entities.Web;

namespace FinRost.DAL.Mappings
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<User, UserDetailResponse>().ReverseMap();
            CreateMap<User,ProfileResponse>().ReverseMap();
            CreateMap<User, UserRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<Investor, InvestorRequest>().ReverseMap();
            CreateMap<Investor, InvestorResponse>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<Lot, LotRequest>().ReverseMap();
            CreateMap<Lot, LotResponse>().ReverseMap();
            CreateMap<NotificationDto, Notification>().ReverseMap();
        }
    }
}
