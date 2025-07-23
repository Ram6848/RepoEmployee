using Microsoft.AspNetCore.Mvc;
using EmployeeAPI.Model;

namespace EmployeeAPI.Controllers
{
    
    [ApiController]
    [Route("WebAPI/[controller]")]
    public class EmployeeController : Controller
    {
        public static List<clsEmployee> lstemp = new List<clsEmployee>()
        {
            new clsEmployee{ Id=1,Name="Rambhavan Sharma1",Department="IT",Salary=100000  },
            new clsEmployee{ Id=2,Name="Rambhavan Sharma2",Department="IT",Salary=200000  },
            new clsEmployee{ Id=3,Name="Rambhavan Sharma3",Department="IT",Salary=300000  },
            new clsEmployee{ Id=4,Name="Rambhavan Sharma4",Department="IT",Salary=400000  },
            new clsEmployee{ Id=5,Name="Rambhavan Sharma5",Department="IT",Salary=500000  }
        };

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            if (lstemp == null || lstemp.Count() == 0)
                return BadRequest("No data found.");

            return Ok(lstemp);
        }

        [HttpPost]
        public IActionResult AddEmployee([FromBody] clsEmployee emp)
        {
            if (emp == null)
                return BadRequest("Employee data is missing");
            else
            {
                if (lstemp.Where(x=> x.Id==emp.Id).Count()>0)
                {
                    return Ok(new { Message ="Employee Id is already Present."});
                }
                else
                {
                    lstemp.Add(emp);
                    return Ok(new { Message = "Employee added successfully" });
                }
            }    
                
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] clsEmployee updateemp)
        {
            var emp = lstemp.FirstOrDefault(x => x.Id == id);
            if(emp==null)
                return BadRequest("Employee not found");
            
            if(updateemp==null)
                return BadRequest("Employee data is missing");

            emp.Name = updateemp.Name;
            emp.Department = updateemp.Department;
            emp.Salary = updateemp.Salary;
            return Ok(new { Message = "Employee updated successfully" });

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var emp = lstemp.FirstOrDefault(x => x.Id == id);
            if (emp == null)
            {
                return BadRequest("Employee not found");
            }
                
            lstemp.Remove(emp);
            return Ok(new { Message = "Employee deleted successfully" });
        }
    }
}
