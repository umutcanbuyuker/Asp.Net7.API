using Asp.Net7.API.DTOs.Incoming;
using Asp.Net7.API.DTOs.Outgoing;
using Asp.Net7.API.Models;
using AutoMapper;

namespace Asp.Net7.API.Profiles
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile()
        {
            // Gelen Dto'lar

            CreateMap<ToDoForCreatedDto, ToDo>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(
                    dest => dest.Category,
                    opt => opt.MapFrom(src => src.Category))
                .ForMember(
                    dest => dest.PublishDate,
                    opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<ToDoForUpdatedDto, ToDo>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(
                    dest => dest.Category,
                    opt => opt.MapFrom(src => src.Category))
                .ForMember(
                    dest => dest.PublishDate,
                    opt => opt.MapFrom(src => DateTime.Now));
              
            // Giden Dto'lar

            CreateMap<ToDo, ToDoDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.NameWithCategory,
                    opt => opt.MapFrom(src => $"{src.Name} - {src.Category}"));
        }
    }
}
