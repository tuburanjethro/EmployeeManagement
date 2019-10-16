using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() { Id=1, Name = "Jack", Department = Dept.HR, Email = "myemail@gmail.com"},
                new Employee() { Id=2, Name = "John", Department = Dept.IT, Email = "myemail@gmail.com"},
                new Employee() { Id=3, Name = "Mary", Department = Dept.Payroll, Email = "myemail@gmail.com"},
                new Employee() { Id=4, Name = "Anne", Department = Dept.HR, Email = "myemail@gmail.com"}
            };
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }
    }
}
