using WebApiLearning.DTO;

namespace WebApiLearning.IService
{
    public interface IEmployeeService
    {
        Task<Tuple<int, List<EmployeeDTO>>> GetAllEmployeeAsync();
       Task<Tuple<int, EmployeeDTO>> GetEmpByID(int id);
        Task<Tuple<int, string>> CreateEmployee(EmployeeDTO empdto);

        Task<Tuple<int,string>> UpdateEmployee(EmployeeDTO empdto);
        Task<Tuple<int, string>> DeleteEmployee(EmployeeDTO empdto);
    }

}
