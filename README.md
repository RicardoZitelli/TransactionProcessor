# TransactionProcessor
TransactionProcessor is a modular system designed to process transactions, generate reports, and manage database infrastructure. This project is developed in C# using .NET 8.0 and follows a layered architecture to ensure scalability and maintainability.

## Table of Contents

- [Overview](#overview)
- [Project Structure](#project-structure)
- [Requirements](#requirements)
- [Setup](#setup)
- [Usage](#usage)
- [Docker](#docker)
- [License](#license)


## Overview

This system processes transactions stored in CSV files, loads the data into a database, performs calculations and analyses, and generates JSON reports. It is composed of multiple layers to organize domain logic, application logic, and infrastructure.

## Project Structure
## Project Structure

```plaintext
TransactionProcessor/
├── docker-compose/               # Docker Compose files
├── TransactionProcessor.App/     # Main application
│   ├── Program.cs                # Application entry point
│   ├── appsettings.json          # Application configuration
│   ├── transactions.csv          # Transactions file
│   └── Reports/                  # Generated reports
├── TransactionProcessor.Application/
│   ├── Interfaces/               # Application layer interfaces
│   ├── Reports/                  # Report generation logic
│   └── TransactionLoader.cs      # Transaction loading logic
├── TransactionProcessor.Domain/
│   ├── Repositories/             # Repository interfaces
│   └── Utilities/                # Common utilities
├── TransactionProcessor.Infrastructure/
│   ├── Interfaces/               # Domain interface implementations
│   ├── CreateDatabaseRepository.cs # Database management logic through sql script
│   ├── CreateTableRepository.cs  # Table management logic through sql script
│   ├── TransactionRepository.cs  # Transaction processing logic
│   └── DatabaseConnection.cs     # Database connection logic
```

## Requirements

- .NET 8.0+
- Docker 

## Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/RicardoZitelli/TransactionProcessor.git
   cd transaction-processor
   ```
## Usage
1. The program will search for the transactions.csv file in folder .\src\TransactionProcessor.App\. You can replace the file with new datas.
   
2. Build the project:
   ```bash
    dotnet build
   ```

3. Run the application:
   ```bash
    dotnet run --project TransactionProcessor.App
   ```
   
4. Generated reports will be located in the Reports folder in TransactionProcessor.App.

## Docker
This project includes SQL Server Docker support. Follow these steps to set it up:
1. Build and start the containers using Docker Compose:
   ```bash
   docker-compose up --build
   ```

2. The database will run inside the container and will be accessible as configured in the docker-compose.yml file.

## License
  This project is licensed under the MIT License.
  
### Customization

Feel free to customize this template to include specific details about your environment, repository, or additional instructions. Let me know if you need further assistance! 😊

