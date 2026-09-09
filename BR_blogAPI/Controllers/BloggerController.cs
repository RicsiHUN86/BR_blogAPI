using BR_blogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using MySqlConnector;

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
        public object AddNameBlogger(Blogger blogger)
        {
            return null;
        }

        [HttpPut]
        public object UpdateBlogger(int id, Blogger blogger)
        {
            return null;
        }

        [HttpDelete]
        public object DeleteBlogger(int id, Blogger blogger)
        {
            return null;
        }
    }
}
