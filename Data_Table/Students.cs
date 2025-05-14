using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Data_Table
{
    internal class Students
    {
        private DataTable originalTable;
        private DataTable FilteredTable;

        public Students()
        {
            originalTable = _CreateTable();

            FilteredTable = new DataTable();
            FilteredTable = originalTable.Clone();
        }

        private bool _IsValidInfo(string name, int age, char gender, string address, string phone, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty.");
                return false;
            }

            if (age < 18 || age > 65)
            {
                Console.WriteLine("Age must be between 18 and 65.");
                return false;
            }

            if (gender != 'M' && gender != 'F')
            {
                Console.WriteLine("Gender must be 'M' or 'F'.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                Console.WriteLine("Address cannot be empty.");
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^(0[5-7]\d{8}|\+?\d{10,15})$"))
            {
                Console.WriteLine("Invalid phone number. Example: 0612345678");
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Console.WriteLine("Invalid email format.");
                return false;
            }

            return true;
        }

        private void _SetPrimaryKey(DataTable table, string columnName)
        {
            if (table.Columns.Contains(columnName))
            {
                table.PrimaryKey = new DataColumn[] { table.Columns[columnName] };
                Console.WriteLine($"Primary key set to column: {columnName}");
            }
            else
            {
                Console.WriteLine($"Column {columnName} does not exist in the table.");
            }
        }

        private bool _IsEmailExists(string email, int excludeId = -1)
        {
            foreach (DataRow row in originalTable.Rows)
            {
                if (row["Email"].ToString().Equals(email, StringComparison.OrdinalIgnoreCase)
                    && Convert.ToInt32(row["ID"]) != excludeId)
                {
                    return true;
                }
            }
            return false;
        }

        private DataTable _CreateTable()
        {
            originalTable = new DataTable("Students");

            originalTable.Columns.Add(new DataColumn("ID", typeof(int))
            {
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1,
                ReadOnly = true,
                Unique = true,
                Caption = "Student ID"
            });

            originalTable.Columns.Add(new DataColumn("Name", typeof(string)) { Caption = "Student Name" });
            originalTable.Columns.Add(new DataColumn("Age", typeof(int)) { Caption = "Student Age" });
            originalTable.Columns.Add(new DataColumn("Gender", typeof(char)) { Caption = "Student Gender" });
            originalTable.Columns.Add(new DataColumn("Address", typeof(string)) { Caption = "Student Address" });
            originalTable.Columns.Add(new DataColumn("Phone", typeof(string)) { Caption = "Student Phone" });
            originalTable.Columns.Add(new DataColumn("Email", typeof(string)) { Caption = "Student Email" });

            _SetPrimaryKey(originalTable, "ID");

            return originalTable;
        }

        private void _AcceptChange()
        {
            originalTable.Clear();
            foreach (DataRow row in FilteredTable.Rows)
                originalTable.ImportRow(row);

        }

        private void _AddStudent(string name, int age, char gender, string address, string phone, string email)
        {
            if (!_IsValidInfo(name, age, gender, address, phone, email))
            {
                Console.WriteLine("Student info not added due to invalid input.");
                return;
            }
            if (!string.IsNullOrWhiteSpace(email) && _IsEmailExists(email))
            {
                Console.WriteLine($"Email '{email}' is already used by another student.");
                return;
            }


            DataRow dtRow = originalTable.NewRow();
            dtRow["Name"] = name;
            dtRow["Age"] = age;
            dtRow["Gender"] = gender;
            dtRow["Address"] = address;
            dtRow["Phone"] = phone;
            dtRow["Email"] = email;

            originalTable.Rows.Add(dtRow);
            Console.WriteLine("Student added successfully.");
        }

        private DataTable _FiltereTable(string ColumnName, object Value)
        {
            FilteredTable.Clear();

            if (string.IsNullOrWhiteSpace(ColumnName) || Value == null)
                return new DataTable();
            if (!originalTable.Columns.Contains(ColumnName))
            {
                Console.WriteLine($"Column '{ColumnName}' does not exist in the table.");
                return new DataTable();
            }

            foreach (DataRow Row in originalTable.Rows)
            {
                if (string.Equals(Row[ColumnName].ToString(),Value.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    FilteredTable.ImportRow(Row);

                    //FilteredTable.Rows.Add(Row);
                }
            }


            return FilteredTable;
        }

        private DataRow _CloneTableWithData(DataRow SourceRow, DataTable OriginalTable)
        {
            DataRow NewRow = OriginalTable.NewRow();

            for (int i = 0; i < OriginalTable.Columns.Count; i++)
            {
                NewRow[i] = SourceRow[i];
            }

            return NewRow;
        }

        private (DataTable OriginalTable, DataTable UpdateTable) _UpdateTable(int ID, string name = "", int age = -1, char gender = '.', string address = "", string phone = "", string email = "",bool acceptChange = false)
        {
            FilteredTable.Clear();

            if (originalTable == null || originalTable.Rows.Count == 0)
                return (new DataTable(), new DataTable());
            if (ID <= 0)
                return (originalTable,new DataTable());

            //if (!_IsValidInfo(name, age, gender, address, phone, email))
            //{
            //    Console.WriteLine("Invalid update info.");
            //    return (originalTable, new DataTable());
            //}


            FilteredTable = originalTable.Clone();

            foreach (DataRow Row in originalTable.Rows)
            {
                if (Row["ID"].Equals(ID))
                {
                    if (!string.IsNullOrWhiteSpace(name))
                        Row["Name"] = name;
                    if (age != -1)
                        Row["Age"] = age;
                    if (gender != '.')
                        Row["Gender"] = gender;
                    if (!string.IsNullOrWhiteSpace(address))
                        Row["Address"] = address;
                    if (!string.IsNullOrWhiteSpace(phone))
                        Row["Phone"] = phone;
                    if (!string.IsNullOrWhiteSpace(email) && _IsEmailExists(email, ID))
                    {
                        Console.WriteLine($"Email '{email}' is already used by another student.");
                        return (originalTable, new DataTable());
                    }


                    FilteredTable.Rows.Add(_CloneTableWithData(Row, FilteredTable));
                    continue;
                }

                FilteredTable.Rows.Add(_CloneTableWithData(Row, FilteredTable));
            }

            if (acceptChange)
                _AcceptChange();

            return (originalTable, FilteredTable);
        }

        private (DataTable OriginalTable, DataTable DeleteTable) _DeleteTable(int ID, bool acceptChange = false)
        {
            FilteredTable.Clear();

            if (originalTable == null || originalTable.Rows.Count == 0)
                return (new DataTable(), new DataTable());

            if (ID <= 0)
                return (originalTable, new DataTable());

            FilteredTable = originalTable.Clone();

            foreach (DataRow Row in originalTable.Rows)
            {
                if (Row["ID"].Equals(ID))
                    continue;

                FilteredTable.Rows.Add(_CloneTableWithData(Row, FilteredTable));
            }

            if(acceptChange)
                _AcceptChange();

            return (originalTable, FilteredTable);
        }

        private DataTable _SearchItem(object searchValue)
        {
            if (searchValue == null || originalTable == null || originalTable.Rows.Count == 0)
                return new DataTable();

            FilteredTable.Clear();

            foreach (DataRow row in originalTable.Rows)
            {
                if (searchValue is int ID && row["ID"].Equals(ID))
                {
                    FilteredTable.Rows.Add(_CloneTableWithData(row, FilteredTable));
                    return FilteredTable;
                }
                else if (searchValue is string Name && row["Name"].Equals(Name))
                {
                    FilteredTable.Rows.Add(_CloneTableWithData(row, FilteredTable));
                    return FilteredTable;
                }
            }

            return originalTable;
        }


        // =========== Public Interface ============

        public DataTable GetOriginalTable() => originalTable;

        public DataTable GetFilteredTable() => FilteredTable;

        public bool CreateTable() => _CreateTable() != null;

        public void AcceptChange() => _AcceptChange();

        public void AddStudent(string name, int age, char gender, string address, string phone, string email)
            => _AddStudent(name,age,gender,address,phone,email);

        public DataTable FiltereTable(string ColumnName, object Value)
            => _FiltereTable(ColumnName,Value);

        public (DataTable OriginalTable, DataTable UpdateTable) UpdateTable(int ID, string name = "", int age = -1, char gender = '.', string address = "", string phone = "", string email = "", bool acceptChange = false)
            => _UpdateTable(ID, name, age,gender,address,phone,email);

        public (DataTable OriginalTable, DataTable DeleteTable) DeleteTable(int ID, bool acceptChange = false)
            => _DeleteTable(ID, acceptChange);

        public DataTable SearchItem(object searchValue)
            => _SearchItem(searchValue);

        public int GetTotalStudents() => originalTable.Rows.Count;

        public double GetAverageAge()
        {
            if (originalTable.Rows.Count == 0) return 0;
            return originalTable.AsEnumerable().Average(row => row.Field<int>("Age"));
        }


    }
}