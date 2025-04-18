using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTOs;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, AppDBContext db, IJwtTokenGenerator jwtTokenGenerator)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if(user == null || !isValid)
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = "",                  
                };
            }

            //If user was found, generate token
            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDTO userDTO = new()
            {
                ID = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                User = userDTO,
                Token = token // Generate JWT token here
            };
            return loginResponseDTO;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                Name = registrationRequestDTO.Name,
                PhoneNumber = registrationRequestDTO.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);

                if(result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u=>u.UserName == registrationRequestDTO.Email);

                    UserDTO userDTO = new()
                    {
                        ID = userToReturn.Id,
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                   return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception)
            {

               
            }
            return "Error";
            
        }
    }
}
