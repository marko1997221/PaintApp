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
    public class OurRectangle : OurLine
    {

        //public static List<OurLine> OurLines = new List<OurLine>();
        private Rectangle CreatedRect;
        public Brush FillColor { get; set; } = Brushes.Transparent;
        private Brush _thisStroke;
        public OurRectangle(int id, int type, Point start, Point end) : base(id, type, start, end)
        {
            CreatedRect = new Rectangle();
            LineWidth = 2;
            StrokeColor = Brushes.Black;
            _thisStroke = Brushes.Black;
            FillColor = Brushes.Transparent;
        }
        public override void deleteMe(OurShape index)
        {
            base.deleteMe(index);
        }
        public override object drawMe(object container)
        {
            CreatedRect = new Rectangle();
            CreatedRect.StrokeThickness = LineWidth;
            CreatedRect.Fill = FillColor;
            CreatedRect.Stroke = StrokeColor;
            CreatedRect.Height = 0;
            CreatedRect.Width = 0;
            CreatedRect.MouseLeftButtonDown += CreatedRect_MouseLeftButtonDown;
            var canvasTemp = container as Canvas;
            AddToList(this);
            canvasTemp.Children.Add(CreatedRect);
            return this;
        }
        public override void RestoreOriginalStroke()
        {
            CreatedRect.StrokeThickness = LineWidth;
            if (StrokeColor!=null)
            {
                CreatedRect.Stroke = StrokeColor;
            }
            else
            {
                CreatedRect.Stroke = _thisStroke;
            }
            CreatedRect.Fill=FillColor;
        }

        private void CreatedRect_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
                        var rectangle = sender as Rectangle;
                        if (rectangle != null)
                        {
                            rectangle.StrokeThickness = 5;
                            rectangle.Stroke = Brushes.DeepSkyBlue;
                            var rez = rectangle.Focus();
                        }
                    }
                }
                catch (Exception)
                {


                }

            }


        }

        public Rectangle GetRectangle()
        {
            return CreatedRect;
        }
        //finish
        public override bool IsFillable()
        {
            return true;
        }
    }
}
