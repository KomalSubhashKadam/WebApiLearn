using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
                //to handle null (green underline shows 'may be null here' warning
                if(userdto == null)
                {
                    return new Tuple<int, string>(1, "Please fill all the details.");
                }

                var existinguser = await _dbcontext.MstUser.FirstOrDefaultAsync(x => x.Email == userdto.Email);
                if (existinguser == null)
                {
                    return new Tuple<int, string>(0, "User not exists");
                }

                //if (existinguser.Password != userdto.Password)
                //{
                //    return new Tuple<int, string>(1, "Invalid password");
                //}
                //here we are checking the password using hashing

                var passwordhasher = new PasswordHasher<string>();
                var verifypassword = passwordhasher.VerifyHashedPassword(userdto.Email, existinguser.Password, userdto.Password);

                if(verifypassword == PasswordVerificationResult.Success)
                {
                    return new Tuple<int, string>(1, "Login Successful");
                }
                else if(verifypassword == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    existinguser.Password = HashPassword(userdto);
                    _dbcontext.MstUser.Update(existinguser);
                    _dbcontext.SaveChanges();
                    return new Tuple<int, string>(1, "Login Successful and new hash generated");
                }
                else if(verifypassword == PasswordVerificationResult.Failed)
                {
                    return new Tuple<int, string>(1, "Invalid password");
                }
                return new Tuple<int, string>(1, "");


            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(3, "something wen wrong");
            }
            
        }
        public async Task<Tuple<int,string>> RegisterUser(UserDTO userdto)
        {
            try
            {
                var existinguser = await _dbcontext.MstUser.AnyAsync(x => x.Email == userdto.Email);
                    if(existinguser)
                {
                    return new Tuple<int, string>(0, "User already exists, please register with new user");
                }

                _dbcontext.MstUser.Add(new Entities.User
                {
                    UserId = userdto.UserId,
                    Name = userdto.Name,
                    Email = userdto.Email,
                    //Password = userdto.Password
                    //instead of userdto passowrd use passwordhashing, gerenarting hashpassword of password
                    Password = HashPassword(userdto)
                });

                await _dbcontext.SaveChangesAsync();
                return new Tuple<int, string>(1, "User registered successfully");

            }
            catch(Exception ex)
            {
                throw;
            }
        }

        //private method to generate the hashpassword of passoword field
        private string HashPassword(UserDTO userdto)
        {
            var passwordhasher = new PasswordHasher<string>();
            var hash =  passwordhasher.HashPassword(userdto.Email, userdto.Password);
            return hash;
        }
    }
}
