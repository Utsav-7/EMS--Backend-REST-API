using System.Numerics;
using AutoMapper;
using EMS_Backend_Project.EMS.Application.DTOs.EmployeeDTOs;
using EMS_Backend_Project.EMS.Application.DTOs.UserDTOs;
using EMS_Backend_Project.EMS.Domain.Entities;

namespace EMS_Backend_Project.EMS.Infrastructure.Mapping
{

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Mapping During Employee Profile creation
            CreateMap<EmplyeeUserDTO, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => System.DateTime.UtcNow))
                .ForMember(dest => dest.Password, opt => opt.Ignore())  // Don't overwrite existing
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true));

            CreateMap<EmplyeeUserDTO, Employee>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // Map EmployeeUpdateDTO to User
            CreateMap<EmployeeUpdateDTO, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())  // Prevent ID updates
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Preserve creation date
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // Don't overwrite password
                .ForMember(dest => dest.Employee, opt => opt.Ignore()); // Avoid circular dependency

            // Map EmployeeUpdateDTO to Employee
            CreateMap<EmployeeUpdateDTO, Employee>()
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore()) // Prevent ID updates
                .ForMember(dest => dest.User, opt => opt.Ignore()); // Avoid circular mapping issues

        }
    }
}