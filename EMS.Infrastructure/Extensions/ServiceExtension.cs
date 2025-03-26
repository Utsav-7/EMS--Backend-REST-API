
using EMS_Backend_Project.EMS.Application.Interfaces.Authentication;
using EMS_Backend_Project.EMS.Application.Interfaces;
using EMS_Backend_Project.EMS.Infrastructure.Services;
using EMS_Backend_Project.EMS.Infrastructure.Repositories;
using EMS_Backend_Project.EMS.Application.Interfaces.UserManagement;
using EMS_Backend_Project.EMS.Application.Interfaces.TimeSheetManagement;
using EMS_Backend_Project.EMS.Application.Interfaces.LeaveManagement;
using EMS_Backend_Project.EMS.Application.Interfaces.ReportAnalyticsManagement;
using EMS_Backend_Project.EMS.Application.Interfaces.DepartmentManagement;
using EMS_Backend_Project.EMS.Application.Interfaces.EmployeeDashboard;
using EMS_Backend_Project.EMS.Application.Services;

namespace EMS_Backend_Project.EMS.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITimeSheetRepository, TimeSheetRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILeaveService, LeaveService>();
            services.AddScoped<ITimeSheetService, TimeSheetService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IReportService, ReportAnalyticsService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IAuthService, AuthHelper>();
            services.AddScoped<IEmailService, EmailHelper>();
            services.AddScoped<JWTTokenHelper>();

            return services;
        }
    }
}