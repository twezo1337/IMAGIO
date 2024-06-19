using IPlugin;

namespace SaturationPlugin
{
    public class Plugin : Iplugin
    {
        private string _IconSrc = "~/img/saturation.svg";
        private string _Name = "Насыщенность";
        private bool _IsProVersion = false;
        public string Function(string src, int id, int width = 0, int height = 0, float value = 0, int g = 0, int b = 0)
        {
            using (SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.Load(src))
            {
                img.Mutate(x => x.Saturate(value));
                img.Save(".." + src.Split(".")[2] + $"_{id}" + "." + src.Split(".").Last());
            }
            return $"+ Изменение насыщенности(на{value}) ";
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