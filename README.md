# Time Tracker App

## Overview

The Workbench Time Tracker App is a full-stack application for tracking time entries. It consists of an Angular frontend and an ASP.NET Core backend. The application allows users to add, edit, and view time entries associated with specific people and tasks.

## Features

- Add new time entries
- Edit existing time entries
- List all time entries for a selected person
- Validate entries on both client and server sides
- Transaction management with Unit Of Work Pattern

## Technologies Used

- **Frontend**: Angular
- **Backend**: ASP.NET Core
- **Database**: Entity Framework Core (with SQLite or SQL Server)
- **Styling**: Bootstrap

## Installation

### Prerequisites

- Node.js and npm
- .NET SDK
- Angular CLI

### Backend Setup

1. Navigate to the `WorkbenchTimeTrackerApp.Server` directory.
2. Restore the .NET dependencies:
   ```bash
   dotnet restore
   ```
3. Apply the database migrations:
```bash
dotnet ef database update
```

4. Run the ASP.NET Core API:
 ```bash
dotnet run
```

### Frontend Setup
1. Navigate to the `workbenchtimetrackerapp.client` directory.
2. Install the npm dependencies:
```bash
npm install
```
3. Run the Angular application
```bash
ng serve
```
## Usage
- Open your browser and navigate to http://localhost:4200.
- Use the interface to manage time entries, people, and work tasks
