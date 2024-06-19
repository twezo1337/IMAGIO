using NuGet.ContentModel;

namespace IMAGIO.Entities
{
    public class Image
    {
        public int Id { get; set; }

        public string Src { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }

        public List<ImageTemp> ImageTemps { get; set; } = new List<ImageTemp>();
    }
}
