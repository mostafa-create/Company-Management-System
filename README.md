# Company System

A comprehensive system for managing company operations, including employee management, project tracking, and financial analysis.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Technologies](#technologies)
- [Installation](#installation)
- [Setup](#setup)
- [Usage](#usage)
- [Project Structure](#project-structure)

## Introduction

The Company System is designed to streamline the operations of a company by managing employees, projects, and financial data. It aims to improve efficiency and data accuracy through an integrated platform.

## Features

- Employee management: Add, update, and track employee data.
- Project management: Track project progress and assign tasks.
- User roles: Admin and employee roles with different access levels.

## Technologies

- **Backend:** ASP.NET Core
- **Frontend:** Bootstrap, jQuery
- **Database:** SQL Server
- **Other:** Entity Framework Core, AutoMapper

## Installation

Follow these steps to set up the project on your local machine:

1. Clone the repository:

   ```bash
   git clone https://github.com/Mohamed-Hussein-dev/Company-System.git
   ```

2. Navigate to the project directory:

   ```bash
   cd Company-System
   ```

3. Restore the dependencies:

   ```bash
   dotnet restore
   ```

4. Set up the database by running the migrations:

   ```bash
   dotnet ef database update
   ```

## Setup

1. **Configuration:**
   - Set up the connection string in the `appsettings.json` file with your database credentials.

2. **Database Migration:**
   - Ensure the database is up to date by running the latest migrations using Entity Framework.

## Usage

1. Run the application:

   ```bash
   dotnet run
   ```

2. Access the application in your browser at `http://localhost:5000`.

## Project Structure

```
Company-System/
│
├── Controllers/             # API controllers
├── Models/                  # Data models
├── Views/                   # Razor views
├── wwwroot/                 # Static files (CSS, JavaScript, images)
├── Services/                # Business logic services
├── Data/                    # Database context and migrations
├── Migrations/              # Entity Framework migrations
└── appsettings.json         # Configuration settings
```
