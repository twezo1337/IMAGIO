namespace IMAGIO.ViewModels
{
    public class ImageViewModel
    {
        public string Src { get; set; }

        public ImageViewModel(string src) 
        { 
            Src = src;
        }
    }
}
