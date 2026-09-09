using BR_blogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using MySqlConnector;
using BR_blogAPI.Models.DTOs;

namespace BR_blogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;uid=root;password=;database=blog";
        [HttpGet]
        public List<Blogger> GetAllBlogger()
        {
            List<Blogger> bloggers = new();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT * FROM blogger";

            var cmd = new MySqlCommand(sql, connector);

            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var blogger = new Blogger
                {
                    Id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0),
                    Name = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1),
                    Email = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2),
                    Age = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt32(3),
                    Password = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4),
                    RegistrationTime = dataReader.IsDBNull(5) ? DateTime.MinValue : dataReader.GetDateTime(5)
                };
                bloggers.Add(blogger);
            }

            connector.Close();
            return bloggers;
        }
        
        [HttpPost]
        public Blogger AddNewBlogger(AddBloggerDTO blogger)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var blg = new Blogger
            {
                Name = blogger.Name,
                Email = blogger.Email,
                Age = blogger.Age,
                Password = blogger.Password,
                RegistrationTime = DateTime.Now
            };

            var sql = $"INSERT INTO `blogger`(`Name`, `Email`, `Age`, `Password`, `RegistrationTime`) VALUES (@name,@email,@age,@password,@registrationtime)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", blg.Name);
            cmd.Parameters.AddWithValue("@email", blg.Email);
            cmd.Parameters.AddWithValue("@age", blg.Age);
            cmd.Parameters.AddWithValue("@password", blg.Password);
            cmd.Parameters.AddWithValue("@registrationtime", blg.RegistrationTime);
            cmd.ExecuteNonQuery();
            connector.Close();
            return blg;
        }

        [HttpPut]
        public object UpdateBlogger(int id, Blogger blogger)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = $"UPDATE `blogger` SET `Name`=@name,`Email`=@email,`Age`=@age,`Password`=@password WHERE Id=@id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", blogger.Name);
            cmd.Parameters.AddWithValue("@email", blogger.Email);
            cmd.Parameters.AddWithValue("@age", blogger.Age);
            cmd.Parameters.AddWithValue("@password", blogger.Password);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            return null;
        }

        [HttpDelete]
        public object DeleteBlogger(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = $"DELETE FROM `blogger` WHERE Id=@id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue(@"id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Blogger sikeresen törölve" };
        }
    }
}
