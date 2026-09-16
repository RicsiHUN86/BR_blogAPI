namespace BR_blogAPI.Models.DTOs
{
    public class UpdateBlogPostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int blogId { get; set; }
    }
}
