using AutoMapper;
using BusinessObject;
using DataAccess.Entity;
//using DataAccess.Entity;

namespace SalesWPFApp
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<MemberObject, DataAccess.Entity.Member>().ReverseMap();
            CreateMap<OrderDetailObject, OrderDetail>().ReverseMap();
            CreateMap<ProductObject, Product>().ReverseMap();
            CreateMap<OrderObject, Order>().ReverseMap();
        }

    }
}
