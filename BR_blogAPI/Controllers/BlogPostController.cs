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
    public class BlogPostController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;uid=root;password=;database=blog";
        [HttpGet]
        public List<BlogPost> GetAllBlogPosts()
        {
            List<BlogPost> blogPosts = new();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT * FROM blogpost";
            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                var blogPost = new BlogPost
                {
                    Id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0),
                    Title = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1),
                    Content = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2),
                    postTime = dataReader.IsDBNull(3) ? DateTime.MinValue : dataReader.GetDateTime(3),
                    updateTime = dataReader.IsDBNull(4) ? DateTime.MinValue : dataReader.GetDateTime(4),
                    blogId = dataReader.IsDBNull(5) ? 0 : dataReader.GetInt32(5)
                };
                blogPosts.Add(blogPost);
            }
            connector.Close();
            return blogPosts;

        }
        [HttpPost]
        public BlogPost AddNewBlogPost(AddBlogPostDTO blogPost)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var bp = new BlogPost
            {
                Title = blogPost.Title,
                Content = blogPost.Content,
                postTime = DateTime.Now,
                updateTime = DateTime.Now,
                blogId = blogPost.blogId
            };
            string sql = "INSERT INTO blogpost (Title, Content, postTime, updateTime, blogId) VALUES (@Title, @Content, @postTime, @updateTime, @blogId)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@Title", bp.Title);
            cmd.Parameters.AddWithValue("@Content", bp.Content);
            cmd.Parameters.AddWithValue("@postTime", bp.postTime);
            cmd.Parameters.AddWithValue("@updateTime", bp.updateTime);
            cmd.Parameters.AddWithValue("@blogId", bp.blogId);
            cmd.ExecuteNonQuery();
            connector.Close();
            return bp;
        }

        [HttpPut]
        public BlogPost UpdateBlogPost([FromQuery] int id, [FromBody] UpdateBlogPostDTO updateBlogPostDTO)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "UPDATE blogpost SET Title = @Title, Content = @Content, updateTime = @updateTime WHERE Id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@Title", updateBlogPostDTO.Title);
            cmd.Parameters.AddWithValue("@Content", updateBlogPostDTO.Content);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            var updatedBlogPost = new BlogPost
            {
                Id = id,
                Title = updateBlogPostDTO.Title,
                Content = updateBlogPostDTO.Content,
                updateTime = DateTime.Now,
                blogId = updateBlogPostDTO.blogId
            };
            return updatedBlogPost;
        }

        [HttpDelete]
        public object DeleteBlogPost([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "DELETE FROM blogpost WHERE Id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Törölve!" };
        }
    }
}
