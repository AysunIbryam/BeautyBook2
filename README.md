# # 🚀 BeautyBook2

> A dynamic ASP.NET Core MVC web application designed for beauty salon appointment management and tracking.

![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Features](#features)
- [Usage](#usage)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## 📖 About the Project

**BeautyBook2** is a modern web application built to streamline beauty service bookings and management. It allows users to browse available beauty services, view details, and manage appointments with an intuitive user interface.

This application was developed as part of the **ASP.NET Fundamentals** course. It demonstrates key architectural patterns and features including ASP.NET Core MVC, Object-Relational Mapping (ORM) with Entity Framework Core, SQL Server persistence, comprehensive data validation (server & client-side), and responsive UI layouts.


---

## 🛠️ Technologies Used

| Technology            | Version  | Purpose                          |
|-----------------------|----------|----------------------------------|
| ASP.NET Core MVC      | 8.0      | Web framework                    |
| Entity Framework Core | 8.0      | ORM / Database access            |
| SQL Server / SQLite   | -        | Database                         |
| Bootstrap             | 5.3      | Frontend styling                 |
| Razor Pages / Views   | -        | Server-side HTML rendering       |

---

## ✅ Prerequisites

Make sure you have the following installed before running the project:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) 
- [Git](https://git-scm.com/)

---

## 🚀 Getting Started

Follow these steps to get the project running locally.

### 1. Clone the repository

```bash
https://github.com/AysunIbryam/BeautyBook2.git
cd BeautyBook2
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Apply database migrations

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

The app will be available at `https://localhost:7174` or `http://localhost:5004`.

---

## 📁 Project Structure

```
BeautyBook2/
│
├── BeautyBook.Data/
│   ├── ApplicationDbContext.cs
│   └── Migrations/
│
├── BeautyBook.Data.Models/Models/
│   ├── ApplicationUser.cs
│   ├── Appointment.cs
│   ├── Category.cs
│   └── Service.cs
│
├── BeautyBook.Services.Core/
│   ├── Contracts/
│   └── Services/
│
└── BeautyBook.Web/
    │
    ├── Properties/
    │   ├── launchSettings.json
    │   ├── servicesDependencies.json
    ├── wwwroot/
    │   ├── css/
    │   ├── js/
    │   ├── images/
    │   └── lib/
    ├── Areas/
    │   ├── Identity/
    │       ├── Pages.cshtml
    │    	   ├── Account.cshtml
    │       		├── ValidationScriptPartial.cshtml
    │       		├── _ViewImports.cshtml
    │       		├── _ViewStart.cshtml
    ├── Controllers/
    │   ├── AppointmentsController.cs
    │   ├── CategoriesController.cs
    │   ├── HomeController.cs
    │   └── ServicesController.cs 
    │
    ├── ViewModels/
    │   ├── CreateAppointmentViewModel.cs
    │   └── ErrorViewModel.cs
    │
    ├── Views/
    │   ├── Appointments/
    │       ├── Create.cshtml
    │       ├── Delete.cshtml
    │       ├── Details.cshtml
    │       ├── Edit.cshtml
    │       └── Index.cshtml
    │   ├── Categories/
    │       ├── Details.cshtml
    │       └── Idex.cshtml
    │   ├── Home/
    │       ├── Index.cshtml
    │       └── Privacy.cshtml
    │   ├── Services/
    │       ├── Details.cshtml
    │       └── Index.cshtml
    │   └── Shared/
    │       ├── _ViewImports.cshtml
    │       └── ViewStart.cshtml
    │   └── _ViewImports.cshtml
    │
    ├── appsettings.json
    ├── Program.cs

```

---

## ✨ Features

- [ ] User registration and login (ASP.NET Identity)
- [ ] CRUD operations for Appointments
- [ ] RESTful API endpoints
- [ ] Input validation (server-side & client-side)
- [ ] Responsive UI with Bootstrap

---

## 💻 Usage


```
1.Open the Website
Launch the application and open it in your web browser.

2.Register an Account
Click the Регистрация button to open the registration page. Fill in the required fields and submit the form to create a new account. The registration form is displayed in Bulgarian.

3.Log In
Use the login page to sign in with your registered account.

4.Browse Beauty Services
Explore the available beauty services and view their details and categories.

5.Manage Appointments
Navigate to the appointments section to view appointment information and use the available options to create, edit, view, or delete appointments
```


---

## 🗄️ Database Setup

The project uses **Entity Framework Core** with a Code-First approach.

Connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=beautyBook; Trusted_Connection=True;TrustServerCertificate=True;"
}
```

To create and seed the database:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## ⚙️ Configuration

Key settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=beautyBook; Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```



---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a new branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "Add some feature"`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## 📬 Contact

**Aysun Yasharova** – [@your-github](https://github.com/AysunIbryam)

Project Link: [https://github.com/AysunIbryam/BeautyBook2]

---

*Built as part of the **ASP.NET Fundamentals** course.*