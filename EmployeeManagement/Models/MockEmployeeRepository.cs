using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeRepository
    {
        private List<Employee>  _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee () {Id = 1, Name = "Mary", Department = Dept.HR, Email = "mary@gmail.com" },
                new Employee () {Id = 2, Name = "John", Department = Dept.IT, Email = "john@gmail.com" },
                new Employee () {Id = 3, Name = "Sam", Department = Dept.IT, Email = "sam @gmail.com" }
        };
        }

        public Employee Add(Employee employee)
        {
            employee.Id =_employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
          Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

        public Employee Update(Employee employeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id ==employeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeChanges.Name;
                employee.Email = employeChanges.Email;
                employee.Department = employeChanges.Department;
            }
            return employee;
        }
    }
}
