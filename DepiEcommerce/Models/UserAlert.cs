namespace DepiEcommerce.Models
{
    public class UserAlert
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
