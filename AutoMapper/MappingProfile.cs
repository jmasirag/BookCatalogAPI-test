using AutoMapper;
using BookCatalogApi.DTO;
using Repository.Models;

namespace BookCatalogApi.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookApi>().ReverseMap();
            CreateMap<Category, CategoryApi>().ReverseMap();
        }
    }
}
