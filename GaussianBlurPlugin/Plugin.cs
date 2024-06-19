using IPlugin;

namespace GaussianBlurPlugin
{
    public class Plugin : Iplugin
    {
        private string _IconSrc = "~/img/blur.svg";
        private string _Name = "Размытие Гаусса";
        private bool _IsProVersion = true;
        public string Function(string src, int id, int width = 0, int height = 0, float value = 0, int filterFalue = 0, int b = 0)
        {
            using (SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.Load(src))
            {
                img.Mutate(x => x.GaussianBlur(filterFalue));
                img.Save(".." + src.Split(".")[2] + $"_{id}" + "." + src.Split(".").Last());
            }
            return $"+ Размытие по Гауссу ";
        }

        public string IconSrc
        {
            get
            {
                return _IconSrc;
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }
        }

        public bool IsProVersion
        {
            get
            {
                return _IsProVersion;
            }
        }
    }
}