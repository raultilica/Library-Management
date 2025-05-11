# Library Management System

## Table of Contents
1. [Introduction](#introduction)  
2. [Requirements](#requirements)  
3. [Running Instructions](#running-instructions)  
4. [Menu Options](#menu-options)  
5. [Top N Most Borrowed Books Feature](#top-n-most-borrowed-books-feature)  
    - [How It Works](#how-it-works)  
    - [Why It’s Special](#why-its-special)  
6. [Unit Testing](#unit-testing)  

---

## Introduction
A simple console-based Library Management System written in C#.  
Supports basic CRUD operations on books, as well as borrowing and returning functionality, search, and analytics on most-borrowed titles.

## Requirements
- .NET SDK (>= .NET 6.0)  
- Visual Studio 2022 or later (or VS Code with C# extensions)  

## Running Instructions
1. **Set up your environment**  
   - Install the .NET SDK from the [official website](https://dotnet.microsoft.com/download).  
   - Open the solution in Visual Studio or your preferred IDE.

2. **Run the Application**  
   - In Visual Studio, press **F5** or click **Run**.  
   - In a terminal, navigate to the project folder and execute:  
     ```bash
     dotnet run --project LibraryManagementSystem
     ```

3. **Interacting with the Menu**  
   When the application starts, you will see:
Enter the number corresponding to your choice and follow the prompts.

## Menu Options
- **Add book**: Enter title, author, ISBN, quantity, etc.  
- **View all books**: Display a list of all books in the library.  
- **Update book**: Modify details of an existing book.  
- **Delete book**: Remove a book from the library.  
- **Search books**: Search by title, author, or ISBN.  
- **Borrow book**: Decrease the available quantity by one.  
- **Return book**: Increase the available quantity by one.  
- **Show most borrowed books**: Analyze and display data-driven insights.  
- **Exit**: Close the application.

## Top N Most Borrowed Books Feature

### How It Works
1. **Borrowing Calculation**  
- For each book, calculate:  
  ```
  borrowCount = initialQuantity – currentQuantity
  ```
2. **Sorting**  
- Sort books in **descending** order by `borrowCount`.
3. **Top N**  
- Prompt the user to enter **N** to display the top N most-borrowed titles.

### Why It’s Special
- **Data-Driven Insights**  
Transforms raw inventory data into actionable trends to help librarians make informed decisions.  
- **Seamless Integration**  
Fully integrated into the service layer without disrupting core CRUD operations, ensuring clean separation of concerns.  
- **Custom Ranking on Demand**  
Users can dynamically choose how many top titles to view—perfect for both quick overviews and deep dives.

## Unit Testing
- **Framework**: xUnit  
- **Running Tests**:  
1. Ensure xUnit is installed and referenced in the test project.  
2. In Visual Studio, open **Test Explorer** and click **Run All**.  
3. Or via command line:  
  ```bash
  dotnet test LibraryManagementSystem.Tests
  ```
- **Troubleshooting**:  
- Verify test project references the correct versions of the main project assemblies.  
- If specific tests fail or cause issues, comment them out temporarily to continue development.

## License
This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.
