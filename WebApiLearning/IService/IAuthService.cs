using WebApiLearning.DTO;

namespace WebApiLearning.IService
{
    public interface IAuthService
    {
        Task<Tuple<int, TokenDTO>> LoginUser(UserDTO userdto);
        Task<Tuple<int, string>> RegisterUser(UserDTO userdto);
    }
}
