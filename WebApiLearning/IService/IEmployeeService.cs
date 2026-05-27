using WebApiLearning.DTO;

namespace WebApiLearning.IService
{
    public interface IEmployeeService
    {
        Task<Tuple<int, List<EmployeeDTO>>> GetAllEmployeeAsync();
        Task<Tuple<int, string>> CreateEmployee(EmployeeDTO empdto);
    }

}
