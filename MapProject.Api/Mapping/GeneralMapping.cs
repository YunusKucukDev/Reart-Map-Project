using AutoMapper;
using MapProject.Api.DTOs.CategoryDto;
using MapProject.Api.DTOs.ContactDto;
using MapProject.Api.DTOs.CoureselDto;
using MapProject.Api.DTOs.MapIdentityDescriptionDto;
using MapProject.Api.DTOs.UserIdentityDto;
using MapProject.Api.DTOs.UserInformationDto;
using MapProject.Api.DTOs.VisitorLogDto;
using MapProject.Api.Entities;

namespace MapProject.Api.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, GetByIdCategoryId>().ReverseMap();

            CreateMap<Couresel, CreateCoureselDto>().ReverseMap();
            CreateMap<Couresel, UpdateCoureselDto>().ReverseMap();
            CreateMap<Couresel, ResultCoureselDto>().ReverseMap();

            CreateMap<Contact, CreateContactDto>().ReverseMap();
            CreateMap<Contact, GetByIdContactDto>().ReverseMap();
            CreateMap<Contact, ResultContactDto>().ReverseMap();
            CreateMap<Contact, UpdateContactDto>().ReverseMap();

            CreateMap<MapIdentityDescription, ResultMapIdentityDescriptionDto>().ReverseMap();
            CreateMap<MapIdentityDescription, UpdateMapIdentityDescriptionDto>().ReverseMap();
            CreateMap<MapIdentityDescription, CreateMapIdentityDescriptionDto>().ReverseMap();

            CreateMap<UserInformation, ResultUserInformationDto>().ReverseMap();
            CreateMap<UserInformation, UpdateUserInformationDto>().ReverseMap();
            CreateMap<UserInformation, CreateUserInformationDto>().ReverseMap();

            CreateMap<VisitorLog, CreateVisitorLogDto>().ReverseMap();
            CreateMap<VisitorLog, ResultVisitorLogDto>().ReverseMap();
        }
    }
}
