namespace IMAGIO.Entities
{
    public class ImageTemp
    {
        public int Id { get; set; }
        public string Src { get; set; }
        public string ChangeName { get; set; }
        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }

    }
}
