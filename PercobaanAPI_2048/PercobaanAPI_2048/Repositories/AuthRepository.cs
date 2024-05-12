using PercobaanAPI_2048.Utils;
using PercobaanAPI_2048.Entities;
using Npgsql;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using PercobaanAPI_2048.DTOs;
using System;

namespace PercobaanAPI_2048.Repositories
{
    public class AuthRepository
    {
        private DbUtil dbUtil;
        public AuthRepository(DbUtil dbUtil)
        {
            this.dbUtil = dbUtil;
        }

        public User register(User user)
        {
            string query = "INSERT INTO users.person (name, email, password) VALUES (@name, @email, @password)";
            try
            {
                NpgsqlCommand cmd = dbUtil.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("name", user.name);
                cmd.Parameters.AddWithValue("email", user.email);
                cmd.Parameters.AddWithValue("password", user.password);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbUtil.closeConnection();
                return user;
            }
            catch (NpgsqlException e)
            {
                throw new NpgsqlException(e.Message);
            }
            return null;
        }

        public User Login(User user, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.password))
            {
                throw new ArgumentNullException("email or password cannot be null or empty.");
            }
            string query = @"SELECT id_person, name, address, email FROM users.person ;";
            try
            {
                NpgsqlCommand cmd = dbUtil.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("email", user.email);
                cmd.Parameters.AddWithValue("password", user.password);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user.id_person = int.Parse(reader["id_person"].ToString());
                    user.name = reader["name"].ToString();
                    user.address = reader["address"].ToString();
                    user.email = reader["email"].ToString();
                    user.token = GenereJwtToken(user, configuration);
                    cmd.Dispose();
                    dbUtil.closeConnection();
                    return user;
                }
                cmd.Dispose();
                dbUtil.closeConnection();
            }
            catch (Exception ex)
            {
                dbUtil.closeConnection();
                throw new NpgsqlException(ex.Message);
            }
            return user;
        }

        

        private string GenereJwtToken(User user, IConfiguration pConfig)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(pConfig["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.name),
                new Claim(ClaimTypes.Email, user.email),
            };
            var token = new JwtSecurityToken(pConfig["Jwt:Issuer"],
                pConfig["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}