using WebApiLearning.DTO;

namespace WebApiLearning.IService
{
    public interface IAuthService
    {
        Task<Tuple<int, string>> LoginUser(UserDTO userdto);
    }
}
