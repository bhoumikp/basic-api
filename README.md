
# Basic API using ASP .NET

## Table of Contents

1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Getting Started](#getting-started)
4. [Running Tests](#running-tests)
5. [Dependencies](#dependencies)
6. [Features](#features)
7. [Contact](#contact)


## Overview

The basic API project is an ASP.NET Core 8 API that provides endpoints for managing entities with related addresses, dates, and names. It allows users to create, retrieve, update, and delete entities, as well as perform advanced filtering and sorting operations. It contains retry mechanism to adapts to various situations where the API can fail.

## Prerequisites

Before running the application, ensure you have the following software installed on your machine:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8 or later)
- [SQL Server](https://www.microsoft.com/en-in/sql-server/sql-server-downloads)
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/your-repo.git
   ```

2. Navigate to the project directory:

   ```bash
   cd your-repo
   ```

3. Open the project in your preferred code editor:

   ```bash
   code .  # For Visual Studio Code
   ```

   Or use Visual Studio.

4. Set up your database connection:   
Open `appsettings.json` and replace your connection string in DefaultConnection.
   ```json
    "Data Source={{YOUR_DESKTOP_NAME}}\\SQLEXPRESS;Initial Catalog={{YOUR_DB_NAME}};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
   ```

5. Navigate to the project directory in the command line. Run the following commands to update the database:

   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef database update
   ```

6. Run the application:
   ```bash
   dotnet run
   ```

7. Access the API:

   Open your web browser and go to [https://localhost:5015/api/entities](https://localhost:5015/api/entities) to access the API.


## Running Tests

To run tests, use the following command:

```bash
dotnet test
```

## Dependencies
- **[net8.0]**: 
  - coverlet.collector                           
  - Microsoft.AspNetCore.Mvc.NewtonsoftJson      
  - Microsoft.AspNetCore.OpenApi                 
  - Microsoft.EntityFrameworkCore.Design         
  - Microsoft.EntityFrameworkCore.SqlServer      
  - Microsoft.EntityFrameworkCore.Tools          
  - Microsoft.NET.Test.Sdk                       
  - Serilog                                      
  - Serilog.Sinks.Console                        
  - Serilog.Sinks.File                           
  - Swashbuckle.AspNetCore                       
  - xunit                                        
  - xunit.runner.visualstudio

## Features

### Database Seeding
- On running the project first time, if your database doesn't have any entities already, it will populate 100 entities with random data.

### Entity CRUD Operations

- **Create Entity:**
  - Route: `POST /api/entities`
  - Description: It accepts a JSON object and inserts a new entity  in the database.
  - Format of JSON object: 
    ```
    {
        "id":"1",
        "deceased":true,
        "gender":"Male",
        "addresses":[
            {
                "addressLine":"81160 Margaret Streets",
                "city":"Melissashire",
                "country":"British Indian Ocean Territory (Chagos Archipelago)"
            }
        ],
        "dates":[
            {
                "dateType":"Birth",
                "dateValue":"1969-10-30"
            },
            {
                "dateType":"Death",
                "dateValue":"1981-10-17"
            }
        ],
        "names":[
            {
                "firstName":"Kristi",
                "middleName":"Matthew",
                "surname":"Wilkerson"
            }
        ]
    }
    ```

- **Get Entity by ID:**
  - Route: `GET /api/entities/{id}`
  - Description: It simply retrieve an entity based on the provided Id.

- **Get All Entities with Search, Pagination, and Filtering:**
  - Route: `GET /api/entities`
  - Query Parameters:
    - `search`: Searches through all the entities and returns related entities based on the query term. Ex. `"?search=jon doe"`.
    - `page`, `pageSize`: Clients can specify which page they want to retrieve and how many entities should retrieve in one page.
    - `gender`, `startDate`, `endDate`, `countries`: More filtered entities will retrieve using these parameters.
  - Description: This route returns all the entities and accepts some searching and filtering parameters to get results close to user's expectations

- **Update Entity by ID:**
  - Route: `PUT /api/entities/{id}`
  - Description: It accepts two required parameters to update the entity.
  - Parameters:
      ```json
        {
          "deceased": "true",
          "gender": "Female"
        }
      ```

- **Delete Entity by ID:**
  - Route: `DELETE /api/entities/{id}`
  - Description: It simply matches the entity with given Id and remove it from the database.

### Retry and Backoff Mechanism

- **Retry Mechanism:**
  - It is the ability of the API to auto-retry a failed operation.
  - In this API, the methods that contains write-to-database operations, have ability to retry the operation several times.
  - The default rate of maximum retries is 3 and they have delays between them.

- **Backoff Strategy:**
  - This API uses the exponential Backoff Strategy, in which the delay between attempts will increase exponentially after each attempt.
  - The retry mechanism uses initial delay and backoff multiplier.

### Logging

- **Logging Retry Details:**
  - API logs every failed attempts into a log file, including details like, number of attempts, delay and the success or failure of the operation.

### Test case
- This project contains a test case, that verifies the retry mechanism, which try to insert an entity with Id already present in the database and expects to create that entity with other Id without cancelling the operation

## Contact
- If there is any query or question related to basic-api project or have errors installing or getting started, Please contact me on bhoumikpagdhare2002@gmail.com