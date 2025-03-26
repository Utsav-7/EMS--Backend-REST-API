using EMS_Backend_Project.EMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMS_Backend_Project.EMS.Infrastructure.Database
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique Mail ID 
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            //// CHECK constraint for LeaveType
            modelBuilder.Entity<Leave>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Leave_LeaveType", "LeaveType IN ('Sick Leave', 'Casual Leave', 'Vacation', 'Paid Leave', 'Maternity Leave', 'Paternity Leave','Unpaid Leave', 'Others')"));

            // CHECK constraint for LeaveStatus
            modelBuilder.Entity<Leave>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Leave_Status", "Status IN ('Pending', 'Approved', 'Rejected', 'Cancelled')"));

            // One-to-One: User ↔ Employee
            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Role ↔ User
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: Department ↔ Employee
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Employee ↔ TimeSheet
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.TimeSheets)
                .WithOne(t => t.Employee)
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Employee ↔ Leave
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Leaves)
                .WithOne(l => l.Employee)
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Data seeding for Role Table
            modelBuilder.Entity<Role>().HasData(
                        new Role { RoleId = 1, RoleName = "Admin" },
                        new Role { RoleId = 2, RoleName = "Employee" }
                    );

            base.OnModelCreating(modelBuilder);
        }
    }
}