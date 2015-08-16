using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

namespace SVG2Image
{
    class Program
    {
        static void Main(string[] args)
        {
            // Reference WindowsBase
            // 1. Create conversion options
            WpfDrawingSettings settings = new WpfDrawingSettings();
            settings.IncludeRuntime = true;
            settings.TextAsGeometry = false;

            // 2. Select a file to be converted
            string svgTestFile = @"C:\Users\Administrator\Desktop\svg\chart.svg";
            string imgfilename = @"C:\Users\Administrator\Desktop\svg\chart.jpg";
            // 3. Create a file converter
            ImageSvgConverter converter = new ImageSvgConverter(settings);
            // 4. Perform the conversion to image  
            converter.EncoderType = ImageEncoderType.JpegBitmap;
            converter.Convert(svgTestFile, imgfilename);
        }
    }
}
