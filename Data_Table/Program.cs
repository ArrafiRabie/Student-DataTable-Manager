using System;
using System.Data;

namespace Data_Table
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Students students = new Students();

            Console.WriteLine("\n--- Adding Students ---");
            students.AddStudent("Rania", 22, 'F', "Rabat", "0612345678", "rania@example.com");
            students.AddStudent("Youssef", 30, 'M', "Casablanca", "0654321987", "youssef@example.com");

            Console.WriteLine("\n--- Original Table ---");
            DisplayTable(students.GetOriginalTable());

            Console.WriteLine("\n--- Filter by Gender = F ---");
            var females = students.FiltereTable("Gender", 'F');
            DisplayTable(females);

            Console.WriteLine("\n--- Update Youssef's Age to 35 ---");
            students.UpdateTable(2, age: 35, acceptChange: true);
            DisplayTable(students.GetOriginalTable());

            Console.WriteLine("\n--- Delete Rania ---");
            students.DeleteTable(1, acceptChange: true);
            DisplayTable(students.GetOriginalTable());

            Console.WriteLine("\n--- Search by Name: Youssef ---");
            var searchResult = students.SearchItem("Youssef");
            DisplayTable(searchResult);

            Console.WriteLine("\n--- Stats ---");
            Console.WriteLine($"Total Students: {students.GetTotalStudents()}");
            Console.WriteLine($"Average Age: {students.GetAverageAge():0.00}");
        }

        static void DisplayTable(DataTable table)
        {
            if (table.Rows.Count == 0)
            {
                Console.WriteLine("No data to display.");
                return;
            }

            foreach (DataColumn column in table.Columns)
                Console.Write($"{column.ColumnName,-12} | ");
            Console.WriteLine("\n" + new string('-', 80));

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                    Console.Write($"{row[column],-12} | ");
                Console.WriteLine();
            }
        }
    }
}

