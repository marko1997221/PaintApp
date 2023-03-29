using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace PaintApp.MyClasses
{
    public class OurLine : OurShape
    {
        private Brush original;
        
        public double LineWidth { get; set; } = 1;
        //public static List<OurLine> OurLines = new List<OurLine>();
        private Line CreatedLine;
        public Brush StrokeColor { get; set; } = Brushes.Black;

        public OurLine(int id, int _type, Point start, Point end) : base(id, _type, start, end)
        {
            CreatedLine = new Line();
            LineWidth = 2;
            StrokeColor = Brushes.Black;
        }
        public OurLine(int id, int _type, Point start, Point end,double _lineWidth) : base(id, _type, start, end)
        {
            CreatedLine = new Line();
            LineWidth = _lineWidth;
            StrokeColor= Brushes.Black;
        }
        public override void deleteMe(OurShape ourShape)
        {
            base.deleteMe(ourShape);
        }
        
        public override object drawMe(object container)
        {
            CreatedLine = new Line();
            CreatedLine.X1 = GetStartPoint().X;
            CreatedLine.Y1 = GetStartPoint().Y;
            CreatedLine.X2 = GetEndPoint().X;
            CreatedLine.Y2= GetEndPoint().Y;
            CreatedLine.Stroke = StrokeColor;
            CreatedLine.StrokeThickness=LineWidth;
            CreatedLine.MouseLeftButtonDown += CreatedLine_MouseLeftButtonDown;
            
            var canvasTemp = container as Canvas;
            AddToList(this);
            canvasTemp.Children.Add(CreatedLine);
            original = CreatedLine.Stroke;
            return this;
        }

        private void CreatedLine_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
                        var line = sender as Line;
                        if (line != null)
                        {
                            line.StrokeThickness = 5;
                            line.Stroke = Brushes.DeepSkyBlue;
                            var rez = line.Focus();
                        }
                    }
                }
                catch (Exception)
                {


                }
            }
          

        }

        public override void RestoreOriginalStroke()
        {
            CreatedLine.StrokeThickness = LineWidth;
           CreatedLine.Stroke = original;
        }

        public Line GetLine()
        {
            return CreatedLine;
        }
        public void SetLine(Line line)
        {
            CreatedLine = line;
        }
        public override bool IsFillable()
        {
            return false;
        }

        public override bool IsSelected()
        {
            if (Selected)
            {
                return true;
            }
            else
            {
                return false;   
            }
        }

        public virtual Point Move(Point _NewStartingPoint)
        {
            var originalSP=GetStartPoint();
            var originalEP=GetEndPoint();
            var height= originalEP.Y - originalSP.Y;
            var width=originalEP.X - originalSP.X;
            var newX = _NewStartingPoint.X + width;
            var newY= _NewStartingPoint.Y + height;
            return new Point(newX, newY);
        }

    } 

}

