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
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or SQLite (if used)
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

The app will be available at `https://localhost:5001` or `http://localhost:5000`.

---

## 📁 Project Structure

```
YourProjectName/
│
├── Controllers/          # MVC Controllers
├── Models/               # Domain models and ViewModels
├── Views/                # Razor Views (.cshtml)
├── Data/                 # DbContext and migrations
├── Services/             # Business logic / service layer
├── wwwroot/              # Static files (CSS, JS, images)
├── appsettings.json      # App configuration
└── Program.cs            # App entry point and middleware setup
```

---

## ✨ Features

- [ ] User registration and login (ASP.NET Identity)
- [ ] CRUD operations for [main entity]
- [ ] RESTful API endpoints
- [ ] Input validation (server-side & client-side)
- [ ] Responsive UI with Bootstrap

---

## 💻 Usage

Describe how to use the main features of the app after launching it. Add screenshots if possible.

```
1. Navigate to /Register to create an account.
2. Log in at /Login.
3. Use the dashboard to manage your [entities].
```

> 💡 **Tip:** You can add screenshots using: `![Screenshot](docs/screenshot.png)`

---

## 🗄️ Database Setup

The project uses **Entity Framework Core** with a Code-First approach.

Connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=YourDbName;Trusted_Connection=True;"
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
    "DefaultConnection": "your-connection-string-here"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

> ⚠️ **Never commit sensitive data** (passwords, API keys) to source control. Use `appsettings.Development.json` or environment variables for local secrets.

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