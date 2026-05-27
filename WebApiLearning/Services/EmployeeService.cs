using Microsoft.EntityFrameworkCore;
using WebApiLearning.Data;
using WebApiLearning.DTO;
using WebApiLearning.IService;

namespace WebApiLearning.Services
{
    // EmployeeService(AppDbContext _dbcontext) -- here we are using primary contructor which means we are injecting
    //dbcontext as constructor parameters withount writing traditional code. primary constructor introduced in c# 12.
    public class EmployeeService(AppDbContext _dbcontext) : IEmployeeService
    {
        public async Task<Tuple<int,List<EmployeeDTO>>> GetAllEmployeeAsync()
        {
            try
            {
                //this is called object projection or LINQ Projection which is done using select(). Mainlu used to convert the
                //one model into another model (entity to dto, ie. EmployeeUser to EmployeeDTO).
                return new Tuple<int, List<EmployeeDTO>>(1, await _dbcontext.EmployeeUser.Select(x => new EmployeeDTO
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Department = x.Department,
                    DOB = x.DOB,
                    EmailAddress = x.EmailAddress,
                    Name = x.Name,
                    LastModified = x.LastModified,
                    Position = x.Position
                }).ToListAsync());
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<Tuple<int,string>> CreateEmployee(EmployeeDTO empdto)
        {
            try
            {
                var existing = await _dbcontext.EmployeeUser.AnyAsync(x => x.EmailAddress == empdto.EmailAddress);
                if(existing)
                {
                    return new Tuple<int, string>(0, "Employee already exists with same ID");
                }

                await _dbcontext.EmployeeUser.AddAsync(new Entities.Employee
                {
                    CreatedAt = DateTime.Now,
                    LastModified = null,
                    Department = empdto.Department,
                    DOB = empdto.DOB,
                    Position = empdto.Position,
                    EmailAddress = empdto.EmailAddress,
                    Name = empdto.Name
                });
                await _dbcontext.SaveChangesAsync();
                return new Tuple<int, string>(1, "Employee Created Successfully");
            }
            catch(Exception ex)
            {
                throw;
            }
        }

    }
}
