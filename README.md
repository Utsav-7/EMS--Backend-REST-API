# Employee Management System API 🚀

This is a backend API for an Employee Management System built with .NET Core following a multi-tiered architecture. The API supports authentication, employee management, timesheets, leave management, and reporting.

## Features 🎯
- User authentication & authorization (Employee/Admin)
- CRUD operations for Employees & Departments
- Timesheet management
- Leave application system
- Reporting and analytics

## Technology Stack
- **Backend:** ASP.NET Core 8.0 Web API (C#)
- **Database:** SQL Server Management Studio 20.2
- **Authentication:** JWT (JSON Web Tokens)
- **External Packages:** EPPlus 7.7.0 (For Excel)
---

## Authentication 🔑
| Method | Endpoint | Description |
|--------|---------|-------------|
| `POST` | `/api/Auth/Login` | User login with email & password |
| `POST` | `/api/Auth/ForgotPassword` | Request password reset link |
| `POST` | `/api/Auth/ResetPassword` | Reset user password |

---

## Department 📁
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/Department/GetAll` | Get all departments |
| `GET` | `/api/Department/GetById/{id}` | Get department by ID |
| `POST` | `/api/Department` | Create a new department |
| `PUT` | `/api/Department` | Update department details |
| `DELETE` | `/api/Department` | Delete a department |

---

## Employee 👨‍💼
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/Employee/Profile` | Get employee profile |
| `PUT` | `/api/Employee/UpdateProfile` | Update employee profile |
| `GET` | `/api/Employee/YourLeaves` | Get leaves of the employee |
| `POST` | `/api/Employee/TakeLeave` | Apply for leave |
| `GET` | `/api/Employee/GetYourSheets` | Get employee's timesheets |
| `POST` | `/api/Employee/CreateTimeSheet` | Create a new timesheet entry |
| `PUT` | `/api/Employee/UpdateTimeSheet` | Update timesheet entry |
| `PUT` | `/api/Employee/ChangePassword` | Change employee password |

---

## Leave Management 📆
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/Leave` | Get all leave requests |
| `POST` | `/api/Leave` | Apply for leave |
| `PUT` | `/api/Leave` | Update leave details |
| `DELETE` | `/api/Leave` | Delete a leave request |
| `GET` | `/api/Leaves/GetById/{id}` | Get leave by ID |

---

## Reports 📊
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/Report/GetReportsByWeekly` | Get weekly reports |
| `GET` | `/api/Report/GetReportsByMonthly` | Get monthly reports |
| `GET` | `/api/Report/GetAllEmployeeReportByWeekly` | Get all employees' weekly reports |
| `GET` | `/api/Report/GetAllEmployeeReportByMonthly` | Get all employees' monthly reports |
| `GET` | `/api/Report/GetCustomDateReport` | Get reports for custom date range |

---

## Timesheet Management 🕒
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/Timesheet/GetAllSheet` | Get all timesheets |
| `GET` | `/api/Timesheet/GetSheetsByID&Date` | Get timesheet by employee ID & date |
| `POST` | `/api/Timesheet` | Create a new timesheet entry |
| `PUT` | `/api/Timesheet` | Update timesheet entry |
| `DELETE` | `/api/Timesheet` | Delete timesheet entry |
| `GET` | `/api/Timesheet/GenerateExcel` | Export timesheets to Excel |
| `GET` | `/api/Timesheet/GenerateExcelByEmpID` | Export timesheets to Excel By Employee Id|

---

## User Management 👥
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/User` | Get all users |
| `GET` | `/api/User/{id}` | Get user by ID |
| `DELETE` | `/api/User/{id}` | Delete user by ID |
| `POST` | `/api/User/Admin` | Create an admin user |
| `POST` | `/api/User/Employee` | Create an employee user |
| `PUT` | `/api/User/Admin/{id}` | Update admin user details |
| `PUT` | `/api/User/Employee/{id}` | Update employee user details |

---

## Security 🔒
- Uses JWT-based authentication
- Passwords are securely hashed
- Role-based access control for Admins & Employees

## Setup Instructions 🛠️
1. Clone the repository:  
   ```sh
   git clone https://github.com/Utsav-7/EMS--Backend-REST-API.git
   ```
2. Navigate to the project directory:  
   ```sh
   cd EMS--Backend-REST-API
   ```
3. Restore dependencies:  
   ```sh
   dotnet restore
   ```
4. Configure the database in `appsettings.json`.
5. Run database migrations:  
   ```sh
   dotnet ef database update
   ```
6. Start the API:  
   ```sh
   dotnet run
   ```
7. Access Swagger UI at:  
   ```
    http://localhost:5240/swagger/index.html
   ```
### Documentation: 
https://drive.google.com/file/d/1TmG1Vc7M1OgPRiJJY1gQAAtKaOXg88cP/view?usp=sharing
