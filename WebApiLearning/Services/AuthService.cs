using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public async Task<Tuple<int,TokenDTO>> LoginUser(UserDTO userdto)
        {
            try
            {
                var tokendto = new TokenDTO();
                //to handle null (green underline shows 'may be null here' warning
                if(userdto == null)
                {

                    tokendto.Token = string.Empty;
                    tokendto.Message = "Please fill all the details.";
                  
                    return new Tuple<int,TokenDTO>(0,tokendto);

                  // return new Tuple<int, string>(1, "Please fill all the details.");
                }

                var existinguser = await _dbcontext.MstUser.FirstOrDefaultAsync(x => x.Email == userdto.Email);
                if (existinguser == null)
                {

					tokendto.Token = string.Empty;
					tokendto.Message = "User not exists";
                    
                    return new Tuple<int, TokenDTO>(0, tokendto);
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

                    UserDTO users = new();
                    users.Email = existinguser.Email;
                    users.Name = existinguser.Name;
                    users.UserId = existinguser.UserId;
                    var token = GetJwtToken(users);

					tokendto.Token = token;
					tokendto.Message = "Login Successful";
                   
                    return new Tuple<int, TokenDTO>(1, tokendto);
                }
                else if(verifypassword == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    UserDTO users = new();
                    users.Name = existinguser.Name;
                    users.Email = existinguser.Email;
                    users.UserId = existinguser.UserId;
                    var token = GetJwtToken(users);
                    existinguser.Password = HashPassword(userdto);
                    _dbcontext.MstUser.Update(existinguser);
                    _dbcontext.SaveChanges();

					tokendto.Token = token;
                    tokendto.Message = "Login Successful and new hash generated";
				
					// return new Tuple<int, string>(1, "Login Successful and new hash generated");
					return new Tuple<int, TokenDTO>(1, tokendto);
				}
                else if(verifypassword == PasswordVerificationResult.Failed)
                {

                    tokendto.Token = string.Empty;
                    tokendto.Message = "Invalid password";
                  
					return new Tuple<int, TokenDTO>(1, tokendto);
					//return new Tuple<int, string>(1, "Invalid password");
				}

				tokendto.Token = string.Empty;
				tokendto.Message = "";
			
				// return new Tuple<int, string>(1, "");
				return new Tuple<int, TokenDTO>(1, tokendto);

			}
            catch (Exception ex)
            {
                throw;
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

        private string GetJwtToken(UserDTO userdto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userdto.Name),
                new Claim(ClaimTypes.Email, userdto.Email),
                new Claim(ClaimTypes.NameIdentifier, userdto.UserId.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AWqlp8ecMcd0NUh9g3fX9AI66u9pCPLMv6ZILTR6uRk"));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "WebLearningFirstApi",
                audience: "WebLearningFirstApi",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
