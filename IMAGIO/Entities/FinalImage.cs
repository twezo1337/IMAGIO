namespace IMAGIO.Entities
{
    public class FinalImage
    {
        public int Id { get; set; }
        public string Src { get; set; }
        public string Changes { get; set; }
        public DateTime DateTimes { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
