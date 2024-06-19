using IPlugin;

namespace DitherPlugin
{
    public class Plugin : Iplugin
    {
        private string _IconSrc = "~/img/dither.svg";
        private string _Name = "Дизеринг";
        private bool _IsProVersion = true;
        public string Function(string src, int id, int width = 0, int height = 0, float value = 0, int g = 0, int b = 0)
        {
            using (SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.Load(src))
            {
                img.Mutate(x => x.Dither());
                img.Save(".." + src.Split(".")[2] + $"_{id}" + "." + src.Split(".").Last());
            }
            return $"+ Дизеринг ";
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