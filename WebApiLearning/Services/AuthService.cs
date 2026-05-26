using Microsoft.EntityFrameworkCore;
using WebApiLearning.Data;
using WebApiLearning.DTO;
using WebApiLearning.IService;

namespace WebApiLearning.Services
{
    public class AuthService :IAuthService
    {
        private readonly AppDbContext _dbcontext;

        public AuthService(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Tuple<int,string>> LoginUser(UserDTO userdto)
        {
            try
            {
                var existinguser = await _dbcontext.MstUser.FirstOrDefaultAsync(x => x.Email == userdto.Email);
                if (existinguser == null)
                {
                    return new Tuple<int, string>(0, "User not exists");
                }

                if (existinguser.Password != userdto.Password)
                {
                    return new Tuple<int, string>(1, "Invalid password");
                }
                return new Tuple<int, string>(2, "Login Successful");
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(3, "something wen wrong");
            }
            
        }
    }
}
