using IPlugin;
using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace AutoDefectRemoval
{
    public class Plugin : Iplugin
    {
        private string _IconSrc = "~/img/autoDefectRemoval.svg";
        private string _Name = "Авто реставрация";
        private bool _IsProVersion = false;
        public string Function(string src, int id, int width = 0, int height = 0, float value = 0, int filterFalue = 0, int b = 0)
        {
            string stockImgSrc = "..\\IMAGIO\\wwwroot\\img\\img_test.png";


            // Загрузка изображения
            Mat image = CvInvoke.Imread(src);
            Mat image_mask = CvInvoke.Imread(stockImgSrc);

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
             
            Mat resultImageFinal = resultImage.Clone();
            double alpha = 0.82; // коэффициент контраста
            double beta = 0.5; // смещение яркости

            // Изменение контраста и яркости
            CvInvoke.ConvertScaleAbs(resultImage, resultImageFinal, alpha, beta);
            CvInvoke.MedianBlur(resultImageFinal, resultImageFinal, 3);

            CvInvoke.Imwrite(".." + src.Split(".")[2] + $"_{id}" + "." + src.Split(".").Last(), resultImageFinal);

            return $"+ авто реставрация ";
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

