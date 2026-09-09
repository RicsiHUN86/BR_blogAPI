using BR_blogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace BR_blogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        [HttpGet]
        public List<Blogger> GetAllBlogger()
        {
            return null;
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
