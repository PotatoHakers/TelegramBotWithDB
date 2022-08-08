using AutoMapper;
using Common.ModelDTO;
using Model.Model;

namespace Common.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
        }
    }
}
