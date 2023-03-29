using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using PaintApp.MyClasses;

namespace PaintApp.MyClasses
{
    public class OurSerialisationTemplate
    {
        public int id  { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public int Type { get; set; }
        public double LineWidth { get; set; }
        public Brush? StrokeColor { get; set; }
        public Brush? FillColor { get; set; }
        public double? Width { get; set; }
        public double? Heigh { get; set; }
        public PointCollection Points { get; set; }
    }
}
