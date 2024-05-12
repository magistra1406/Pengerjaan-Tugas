using PercobaanAPI_2048.DTOs;
using PercobaanAPI_2048.Entities;
using PercobaanAPI_2048.Repositories;
using System;

namespace PercobaanAPI_2048.Service
{
    public class AuthService 
    {

        private AuthRepository authRepository;
        public AuthService(AuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        public User register(RegisterDTO dto)
        {
            User user = new User();
            user.name = dto.name;
            user.email = dto.email;
            user.password = dto.password;
            return this.authRepository.register(user);
        }

        public User Login(LoginDTO dto, IConfiguration configuration)
        {
            User user = new User();
            user.email = dto.email;
            user.password = dto.password;
            if (this.authRepository.Login(user, configuration) == null)
            {
                return null;
            }
            return this.authRepository.Login(user, configuration);
        }

    }
}