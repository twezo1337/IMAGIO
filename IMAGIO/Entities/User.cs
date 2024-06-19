using IMAGIO.Services;

namespace IMAGIO.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public List<FinalImage> FinalImages { get; set; } = new List<FinalImage>();
    }
}
