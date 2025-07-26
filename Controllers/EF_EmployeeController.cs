using EmployeeAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EF_EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        public EF_EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployee()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Model.clsEmployee emp)
        {
            if(emp==null)
                return BadRequest("Employee data is missing");

            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Employee Saved Successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Model.clsEmployee updateemp)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
                return NotFound($"Employee id {id} not found");

            emp.Name = updateemp.Name;
            emp.Department = updateemp.Department;
            emp.Salary = updateemp.Salary;

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Employee Updated Successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if(emp==null)
                return NotFound($"Employee Id {id} not found");

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Employee Deleted Successfully." });
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchEmployee(string? name,string? department,string? sortby="name",int page=1,int pageSize = 5)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e=> e.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(e => e.Department.Contains(department));
            }

            switch (sortby.ToLower())
            {
                case "name":
                    query = query.OrderBy(e => e.Name);
                    break;
                case "department":
                default:
                    query = query.OrderBy(e => e.Department);
                    break;
            }

            int totalRecords = await query.CountAsync();
            var employees = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return Ok(new
            {
                TotalRecords=totalRecords,
                Page=page,
                PageSize=pageSize,
                data=employees
            }
                );
        }

    }
}
