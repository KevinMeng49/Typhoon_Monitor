using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Display;

namespace dev_try2
{
    class ColorHelper
    {
        public static IRandomColorRamp makeRandomColorRamp(int recNum)
        {
            IRandomColorRamp randomColorRamp = new RandomColorRamp();  //这个是随机色带，使用HSV模型来确定一串颜色
            randomColorRamp.Size = recNum;
            bool bture = true;
            randomColorRamp.CreateRamp(out bture);
            return randomColorRamp;
        }
        public static IRgbColor makeRGBColor(int r, int g, int b)
        {
            IRgbColor rgbColor = new RgbColor();
            rgbColor.Red = r;
            rgbColor.Green = g;
            rgbColor.Blue = b;
            return rgbColor;
        }

        public static IAlgorithmicColorRamp makeAlgorithmicColorRamp(IRgbColor fromColor, IRgbColor toColor, int count)
        {
            IAlgorithmicColorRamp algorithmicColorRamp = new AlgorithmicColorRamp();
            algorithmicColorRamp.Size = count;
            algorithmicColorRamp.FromColor = fromColor;
            algorithmicColorRamp.ToColor = toColor;
            bool bture = true;
            algorithmicColorRamp.CreateRamp(out bture);
            return algorithmicColorRamp;
        }
    }
}
