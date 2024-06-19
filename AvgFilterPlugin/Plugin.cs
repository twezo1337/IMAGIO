using IPlugin;
using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace AvgFilterPlugin
{
    public class Plugin : Iplugin
    {
        private string _IconSrc = "~/img/avgFilter.svg";
        private string _Name = "Фильтр медианный";
        private bool _IsProVersion = false;
        public string Function(string src, int id, int width = 0, int height = 0, float value = 0, int filterFalue = 0, int b = 0)
        {
            // Загрузка изображения
            Mat image = CvInvoke.Imread(src);
            Mat resultImageFinal = image.Clone();
            CvInvoke.MedianBlur(image, resultImageFinal, 3);

            CvInvoke.Imwrite(".." + src.Split(".")[2] + $"_{id}" + "." + src.Split(".").Last(), resultImageFinal);

            return $"+ Фильтр среднего значения ";
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

