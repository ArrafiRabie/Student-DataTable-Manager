Student DataTable Manager in C#
This project is a fully self-contained C# class that simulates a student data management system using System.Data.DataTable. It showcases fundamental concepts of software engineering such as data validation, separation of concerns, clean interfaces, and efficient data filtering and manipulation — all without using a database or any external packages.

✅ Key Features
In-Memory Data Handling using DataTable with strong typing and constraints

Custom Data Validation for name, age, gender, phone number, and email format

CRUD Operations (Create, Read, Update, Delete) with graceful error handling

Search & Filtering by various fields

Data Integrity enforced by primary key and email uniqueness

Statistical Insights like total number of students and average age

Cloning and Change Management using manual row tracking and AcceptChange() mechanism

Designed Without ORMs to demonstrate raw handling of data in memory

Fully Extensible for future enhancements (e.g., file export/import, UI integration)

🧠 Technical Concepts Demonstrated
Encapsulation and abstraction
Safe casting and data conversion
Use of Regex for input validation
Use of Clone() and ImportRow() for deep copying rows between tables
Defensive programming techniques
Clear, maintainable structure following object-oriented design principles

💡 Use Cases:
Educational projects and coding exercises:
 Ideal for learners who wish to apply object-oriented programming concepts and data structures in a practical environment without the complexity of databases.
Prototypes where no database is needed:
 Can be used as a quick prototype to experiment with certain ideas without needing to set up a database.
Local data simulation:
 Used to simulate data locally using DataTable and DataRow without relying on external sources.
Teaching data structure concepts in C#:
 A powerful tool for demonstrating how to organize, filter, and sort data using standard C# tools.
