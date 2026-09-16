using BR_blogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using MySqlConnector;
using BR_blogAPI.Models.DTOs;
using System.Security.Cryptography;

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
        public UpdateBloggerDTO UpdateBlogger([FromQuery]int id, [FromBody]UpdateBloggerDTO updateBloggerDTO)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            
            string sql = $"UPDATE `blogger` SET `Name`=@name,`Email`=@email,`Age`=@age,`Password`=@password WHERE Id=@id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", updateBloggerDTO.Name);
            cmd.Parameters.AddWithValue("@email", updateBloggerDTO.Email);
            cmd.Parameters.AddWithValue("@age", updateBloggerDTO.Age);
            cmd.Parameters.AddWithValue("@password", updateBloggerDTO.Password);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            
            var updatedBlogger = new UpdateBloggerDTO
            {
                Name = updateBloggerDTO.Name,
                Email = updateBloggerDTO.Email,
                Age = updateBloggerDTO.Age,
                Password = updateBloggerDTO.Password
            };

            connector.Close();
            return updatedBlogger;

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

        [HttpGet("byId")]
        public object GetBloggerById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = $"SELECT name, email FROM blogger WHERE Id=@id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var dataReader = cmd.ExecuteReader();
            dataReader.Read();

            var blogger = new Blogger
            {
                Name = dataReader.GetString(0),
                Email = dataReader.GetString(1)
            };

            connector.Close();
            return blogger;
            
        }
        [HttpGet("bloggerOwnPost")]
        public List<object> GetBloggerWithPost(int id)
        {
            List<object> ownPost = new List<object>();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = "SELECT blogger.name, blogpost.Title, blogpost.Content FROM `blogger` INNER JOIN blogpost ON blogger.id = blogpost.blogId WHERE blogger.`id` = @id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                var bloggerOwnPost = new
                {
                    Name = dataReader.GetString(0),
                    Title = dataReader.GetString(1),
                    Content = dataReader.GetString(2)
                };
                ownPost.Add(bloggerOwnPost);
            }
            connector.Close();
            return ownPost;
        }
        [HttpGet("NumberOfPosts")]
        public object GetNumberOfPosts(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = "SELECT COUNT(*) FROM blogpost";
            var cmd = new MySqlCommand(sql, connector);
            var db = cmd.ExecuteScalar();
            connector.Close();
            return new { message = $"Posztok száma: {db}" };
        }

        [HttpGet("GetBloggerPostNumber")]
        public object GetBloggerPostNumber(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "SELECT blogger.name, COUNT(*) FROM `blogger` INNER JOIN blogpost on blogger.id = blogpost.blogId GROUP BY blogger.Id HAVING `Id` = @id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var dataReader = cmd.ExecuteReader();
            dataReader.Read();
            var bloggerPostNumber = new
            {
                Name = dataReader.GetString(0),
                NumberOfPosts = dataReader.GetInt32(1)
            };

            connector.Close();

            return bloggerPostNumber;
        }
    }
}
