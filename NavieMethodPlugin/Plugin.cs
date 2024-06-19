using IPlugin;
using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace NavieMethodPlugin
{
    public class Plugin : Iplugin
    {
        private string _IconSrc = "~/img/interpolation.svg";
        private string _Name = "Метод Навье-Стокса";
        private bool _IsProVersion = false;
        public string Function(string src, int id, int width = 0, int height = 0, float value = 0, int filterFalue = 0, int b = 0)
        {
            string stockImgSrc = src.Split('_').First() + '.' + src.Split('.').Last();


            // Загрузка изображения
            Mat image = CvInvoke.Imread(stockImgSrc);
            Mat image_mask = CvInvoke.Imread(src);

            int newWidth = image.Width;
            int newHeight = image.Height;

            Mat resizedMask = new Mat();
            CvInvoke.Resize(image_mask, resizedMask, new Size(newWidth, newHeight), 0, 0, Inter.Linear);


            // Создание маски для дефектов
            Mat mask = new Mat();
            // Заполнение маски
            CvInvoke.InRange(resizedMask, new ScalarArray(new MCvScalar(0, 0, 100)), new ScalarArray(new MCvScalar(255, 50, 255)), mask);

            Mat resultImage = image.Clone();

            // Интерполяция для ретуширования дефектов
            CvInvoke.Inpaint(image, mask, resultImage, 3, InpaintType.NS);

            CvInvoke.Imwrite(".." + src.Split(".")[2] + $"_{id}" + "." + src.Split(".").Last(), resultImage);

            return $"+ интерполяция Навье-Стокса";
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
