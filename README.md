
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

The basic API project is an ASP.NET Core 8 API that provides endpoints for managing entities with related addresses, dates, and names. It allows users to create, retrieve, update, and delete entities and perform advanced filtering and sorting operations. It contains a retry mechanism to adapt to various situations where the API can fail.

## Prerequisites

Before running the application, ensure you have the following software installed on your machine:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8 or later)
- [SQL Server](https://www.microsoft.com/en-in/sql-server/sql-server-downloads)
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)

## 🔧 Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/your-repo.git
   ```
   <br />

2. Navigate to the project directory:

   ```bash
   cd basic-api
   ```

<br />

3. Open the project in your preferred code editor:

   ```bash
   code .  # For Visual Studio Code
   ```

   Or in Visual Studio, open the basic-api project and build/run.

<br />

4. Set up your database connection:   
Open `appsettings.json` and replace your connection string in DefaultConnection.
   ```json
    "Data Source={{YOUR_DESKTOP_NAME}}\\SQLEXPRESS;Initial Catalog={{YOUR_DB_NAME}};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
   ```
   - [Find Your Desktop Name](https://it.umn.edu/services-technologies/how-tos/find-your-computer-name)
   - It isn't necessary to add a database to the SQL server, add the database name in the connection string.

<br />

5. Navigate to the project directory in the command line. Run the following commands to update the database:

   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef database update
   ```

<br />

6. Run the application:
   ```bash
   dotnet run
   ```

<br />

7. Access the API:

   Open your web browser and go to [https://localhost:5015/api/entities](https://localhost:5015/api/entities) to access the API.

<br>

## 📝 Running Tests

To run tests, use the following command:

```bash
dotnet test
```

<br>

## 📚 Dependencies
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

<br>

## 🔎 Features

### Database Seeding
- On running the project for the first time, if your database already has no entities, it will populate 100 entities with random data.

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

<br />

- **Get Entity by ID:**
  - Route: `GET /api/entities/{id}`
  - Description: It simply retrieves an entity based on the provided Id.

<br />

- **Get All Entities with Search, Pagination, and Filtering:**
  - Route: `GET /api/entities`
  - Query Parameters:
    - `search`: Searches through all the entities and returns related entities based on the query term. Ex. `"?search=jon doe"`.
    - `page`, `pageSize`: Clients can specify which page they want to retrieve and how many entities should retrieve in one page.
    - `gender`, `startDate`, `endDate`, `countries`: More filtered entities will retrieve using these parameters.
  - Description: This route returns all the entities and accepts some searching and filtering parameters to get results close to the user's expectations.

<br />

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

<br />

- **Delete Entity by ID:**
  - Route: `DELETE /api/entities/{id}`
  - Description: It simply matches the entity with the given Id and removes it from the database.

### Retry and Backoff Mechanism

- **Retry Mechanism:**
  - The API can auto-retry a failed operation.
  - In this API, the methods which contain write-to-database operations, can retry the operation several times.
  - The default rate of maximum retries is 3 and each retry has a delay between them.

- **Backoff Strategy:**
  - This API uses the exponential Backoff Strategy, in which the delay between attempts will increase exponentially after each attempt.
  - The retry mechanism uses initial delay and backoff multiplier.

### Logging

- **Logging Retry Details:**
  - API logs every failed attempt into a log file, including details like the number of attempts, delay, and the success or failure of the operation.

### Test case
- This project contains a test case, that verifies the retry mechanism, which tries to insert an entity with Id already present in the database and expects to create that entity with another Id without canceling the operation.

## 📞 Contact
- If there is any query or question related to the basic-api project or have errors installing or getting started, contact me at bhoumikpagdhare2002@gmail.com.

<br />


[![github](https://img.shields.io/badge/github-000?style=for-the-badge&logo=github&logoColor=white)](https://github.com/bhoumikp)
[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/bhoumikp/)
[![instagram](https://img.shields.io/badge/instagram-B344A8?style=for-the-badge&logo=instagram&logoColor=white)](https://www.instagram.com/_.bhoumik._/)
