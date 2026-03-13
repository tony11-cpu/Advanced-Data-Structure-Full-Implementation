using System;
using System.Collections.Generic;
using System.Text;

namespace C23_DS
{
    public class Employee
    {
        public int Id { get; set; } = -1;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; } = 0;
        public DateTime HireDate { get; set; } = DateTime.Now;
        public int Age { get; set; } = 0;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;

        public static IEnumerable<Employee> GenerateRandomEmployees(int count)
        {
            Random random = new Random();
            List<Employee> employees = new List<Employee>();

            string[] firstNames = { "John", "Sarah", "Michael", "Emma", "David", "Lisa","James", "Maria", "Robert", "Jennifer", "William", "Linda" };
            string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia","Miller", "Davis", "Martinez", "Wilson", "Anderson", "Taylor" };
            string[] departments = { "IT", "HR", "Sales", "Marketing", "Finance","Operations", "Engineering", "Support" };
            string[] positions = { "Manager", "Senior Developer", "Analyst", "Specialist","Coordinator", "Director", "Associate", "Lead" };

            for (int i = 1; i <= count; i++)
            {
                string firstName = firstNames[random.Next(firstNames.Length)];
                string lastName = lastNames[random.Next(lastNames.Length)];

                Employee emp = new Employee
                {
                    Id = i,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}@company.com",
                    Department = departments[random.Next(departments.Length)],
                    Position = positions[random.Next(positions.Length)],
                    Salary = random.Next(40000, 150001), 
                    HireDate = DateTime.Now.AddDays(-random.Next(1, 3650)),
                    Age = random.Next(22, 66),
                    PhoneNumber = $"({random.Next(200, 999)}) {random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    IsActive = random.Next(100) > 10
                };

                employees.Add(emp);
            }

            return employees.OrderByDescending(n => n.IsActive).ThenByDescending(n => n.Salary).ThenBy(n => n.Age);
        }
    }
}