# MauiPlate - Desktop System Management Application

## Overview
MauiPlate is a learning-focused desktop application built with .NET MAUI (Multi-platform App UI) framework. It serves as a comprehensive system management solution that includes user management, role-based access control, tenant management, menu management, and resource management modules.

## Purpose
This project is primarily developed for educational purposes, demonstrating the implementation of a modern desktop application using .NET MAUI. It showcases best practices in desktop application development while providing practical system management functionalities.

## Key Features
- **User Management**: Handle user profiles and authentication
- **Role Management**: Configure and manage user roles and permissions
- **Tenant Management**: Multi-tenant system support
- **Menu Management**: Dynamic menu configuration
- **Resource Management**: Manage system resources efficiently

## Technical Stack
- **.NET MAUI**: Core framework for cross-platform desktop application
- **CommunityToolkit.Maui**: Enhanced UI components and utilities
- **Syncfusion Tools**: Advanced UI controls and components
- **Microsoft Extensions**: Logging and dependency injection
- **Repository Pattern**: For data access and management

## Architecture
The application follows a modern architectural approach with:
- MVVM (Model-View-ViewModel) pattern
- Repository pattern for data access
- Dependency Injection for better modularity
- Service-based architecture

## Components
- **Project Management**
  - Project creation and tracking
  - Task management
  - Category organization
  
- **UI Features**
  - Custom fonts integration
  - FluentUI icons support
  - Modal error handling
  - Responsive layouts

## Getting Started
1. Clone the repository
```bash
git clone https://github.com/vnobo/MauiPlate.git
```

2. Install prerequisites
- .NET 9.0 SDK or later
- Visual Studio 2022 with MAUI workload
- Required NuGet packages will be restored automatically

3. Build and run the application
```bash
dotnet build
dotnet run
```

## Project Structure
- `MainPage`: Primary application interface
- `ProjectListPage`: Project management interface
- `ManageMetaPage`: Metadata management
- `ProjectDetailPage`: Detailed project view
- `TaskDetailPage`: Task management interface

## Learning Resources
This project serves as a practical example for learning:
- .NET MAUI development
- Desktop application architecture
- System management implementation
- MVVM pattern in practice
- Repository pattern implementation

## Contributing
This is a learning-focused project, and contributions are welcome. Please feel free to:
- Submit bug reports
- Propose new features
- Create pull requests
- Share improvement suggestions

## License
[MIT License](LICENSE)

## Contact
- GitHub: [@vnobo](https://github.com/vnobo)