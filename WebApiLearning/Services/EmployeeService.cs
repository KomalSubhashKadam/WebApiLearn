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

        public async Task<Tuple<int,EmployeeDTO>> GetEmpByID(int id)
        {
            try
            {
                var data = await _dbcontext.EmployeeUser.Select(x => new EmployeeDTO
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Department = x.Department,
                    DOB = x.DOB,
                    EmailAddress = x.EmailAddress,
                    Name = x.Name,
                    LastModified = x.LastModified,
                    Position = x.Position
                }).FirstOrDefaultAsync(x => x.Id == id);

                if(data == null)
                {
                    return new Tuple<int, EmployeeDTO>(0, null);
                }
                return new Tuple<int, EmployeeDTO> (1, data);
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

        public async Task<Tuple<int,string>> UpdateEmployee(EmployeeDTO empdto)
        {
            try
            {
                if(empdto == null)
                {
                    return new Tuple<int, string>(0, "Please fill all the details");
                }    
                var existing = await _dbcontext.EmployeeUser.FirstOrDefaultAsync(x => x.EmailAddress == empdto.EmailAddress);
                if (existing == null)
                {
                    return new Tuple<int, string>(0, "Employee does not exists.");
                }

                existing.Position = empdto.Position ?? existing.Position;
                existing.Name = empdto.Name ?? existing.Name;
                existing.DOB = empdto.DOB ?? existing.DOB;
                existing.Department = empdto.Department ?? existing.Department;
                existing.EmailAddress = empdto.EmailAddress ?? existing.EmailAddress;

                _dbcontext.EmployeeUser.Update(existing);
                await _dbcontext.SaveChangesAsync();

                return new Tuple<int, string>(2, "Employee Updated Successfully.");

            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<Tuple<int,string>> DeleteEmployee(EmployeeDTO empdto)
        {
            try
            {
                var existing = await _dbcontext.EmployeeUser.FirstOrDefaultAsync(x => x.Id == empdto.Id);
                if(existing == null)
                {
                    return new Tuple<int, string>(0, "Employee Does not exist");
                }

                _dbcontext.EmployeeUser.Remove(existing);
                await _dbcontext.SaveChangesAsync();

                return new Tuple<int, string>(1, "Employee deleted.");
            }
            catch(Exception ex)
            {
                throw;  
            }
        }

    }
}
