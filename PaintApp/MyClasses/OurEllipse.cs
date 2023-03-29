using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace PaintApp.MyClasses
{
    public class OurEllipse : OurShape
    {

        public double LineWidth { get; set; } = 1;
        //public static List<OurLine> OurLines = new List<OurLine>();
        public Brush StrokeColor { get; set; } = Brushes.Black;
        private Brush _thisStroke;
        public Brush Fill { get; set; } = Brushes.Transparent;
        public Ellipse Circle { get; set; }
        public OurEllipse(int id, int type, Point start, Point end) : base(id, type, start, end)
        {
            Circle = new Ellipse();
            LineWidth = 3;
            StrokeColor = Brushes.Black;
            _thisStroke = Brushes.Black;
            Fill = Brushes.Transparent;
        }
        public override void deleteMe(OurShape index)
        {
            base.deleteMe(index);
        }
        public override object drawMe(object container)
        {
            Point rez = CaclulateCircle(GetEndPoint());
            Circle.Width = rez.X;
            Circle.Height = rez.X;
            Circle.Fill = Fill;
            Circle.Stroke=StrokeColor; 
            Circle.StrokeThickness = LineWidth;
            Circle.MouseLeftButtonDown += Circle_MouseLeftButtonDown;
            var canvasTemp = container as Canvas;
            AddToList(this);
            canvasTemp.Children.Add(Circle);
            return this;
        }

        private void Circle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ApprovedSelection==true)
            {
                try
                {
                    foreach (var item in OurShape.ourShapes)
                    {
                        switch (item.GetOurType())
                        {
                            case (int)OurType.Line:
                                {
                                    var line = item as OurLine;
                                    if (line != null)
                                    {
                                        line.RestoreOriginalStroke();
                                        line.Selected = false;
                                    }
                                }
                                break;
                            case (int)OurType.Rectangle:
                                {
                                    var rectangle = item as OurRectangle;
                                    if (rectangle != null)
                                    {
                                        rectangle.RestoreOriginalStroke();
                                        rectangle.Selected = false;
                                    }
                                }
                                break;
                            case (int)OurType.Circle:
                                {
                                    var circle = item as OurEllipse;
                                    if (circle != null)
                                    {
                                        circle.RestoreOriginalStroke();
                                        circle.Selected = false;
                                    }
                                }
                                break;
                            case (int)OurType.Triangle:
                                {
                                    var triangle = item as OurTriangle;
                                    if (triangle != null)
                                    {
                                        triangle.RestoreOriginalStroke();
                                        triangle.Selected = false;
                                    }
                                }
                                break;
                            default:
                                break;
                        }


                    }
                    Selected = true;
                    if (Selected)
                    {
                        var circle = sender as Ellipse;
                        if (circle != null)
                        {
                            circle.StrokeThickness = 5;
                            circle.Stroke = Brushes.DeepSkyBlue;
                            var rez = circle.Focus();
                        }
                    }
                }
                catch (Exception)
                {


                }
            }
            

        }

        public virtual void RestoreOriginalStroke()
        {
            Circle.StrokeThickness = LineWidth;
            if (StrokeColor!=null)
            {
                Circle.Stroke = StrokeColor;
            }
            else
            {
                Circle.Stroke = _thisStroke;
            }
           
        }
        //finish
        public override bool IsFillable()
        {
            return true;
        }
        public Point CaclulateCircle( Point Endpoint)
        {
            return new Point(Math.Abs(Endpoint.X-GetStartPoint().X), Math.Abs(Endpoint.Y-GetStartPoint().Y));
        }
       
    }
}
