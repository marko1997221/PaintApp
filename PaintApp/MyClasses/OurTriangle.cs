using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace PaintApp.MyClasses
{
    public class OurTriangle : OurLine
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public Point EndPoint { get; set; }
        public Brush FillColor { get; set; } = Brushes.Transparent;
        public PointCollection points = new PointCollection();
        public Polygon Poly { get; set; }
        //public Line Line2 { get; set; }
        //public Line Line3 { get; set; }
        private double a;
        private double h;
        public OurTriangle(int id, int type, Point start, Point end) : base(id, type, start, end)
        {
            LineWidth = 3;
            StrokeColor= Brushes.Black;
            FillColor = Brushes.Transparent;
            P1 = CalculateP1(GetEndPoint());
            P2 = CalculateP2(GetEndPoint());
            EndPoint = GetEndPoint();
        }
        public override void deleteMe(OurShape index)
        {
           base.deleteMe(index);
        }
        public override object drawMe(object container)
        {
            var canvasTemp = container as Canvas;
            Polygon polygon = new Polygon();
            points.Add(EndPoint);
            points.Add(P1);
            points.Add(P2);
            polygon.Points = points;
            polygon.Fill = FillColor;
            polygon.Stroke=StrokeColor;
            polygon.StrokeThickness = LineWidth;
            Poly = polygon;
            Poly.MouseLeftButtonDown += Poly_MouseLeftButtonDown;
            AddToList(this);
            canvasTemp.Children.Add(polygon);
            return this;
        }

        private void Poly_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
                        var triangle = sender as Polygon;
                        if (triangle != null)
                        {
                            triangle.StrokeThickness = 5;
                            triangle.Stroke = Brushes.DeepSkyBlue;
                            var rez = triangle.Focus();
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
            Poly.StrokeThickness = LineWidth;
            Poly.Stroke = StrokeColor;
        }
        //finish
        public override bool IsFillable()
        {
            return true;
        }
        private double CalculateH(Point End)
        {
            h= 3 * Math.Sqrt(Math.Pow((End.Y - GetStartPoint().Y), 2) + Math.Pow((End.X - GetStartPoint().X), 2)) / 2;
            return h;
        }
        private double CalculateA(Point point)
        {
            a = 2*CalculateH(point)*Math.Sqrt(3)/3;
            return a;
        }
        public Point CalculateP1(Point Endpoint)
        {
            return new Point(Endpoint.X+CalculateA(Endpoint), Endpoint.Y);
        }
        public Point CalculateP2(Point Endpoint)
        {
            return new Point(Endpoint.X + CalculateA(Endpoint)/2, Endpoint.Y-CalculateH(Endpoint));
        }
        public Point MoveP1(Point EndPoint)
        {
            var EndOld = GetEndPoint();
            var p1old = new Point(GetEndPoint().X+a,GetEndPoint().Y);
            var absX= p1old.X-EndOld.X;
            var absY= p1old.Y-EndOld.Y;
            P1= new Point(EndPoint.X + absX, EndPoint.Y + absY);
            return P1; 

        }
        public Point MoveP2(Point EndPoint)
        {
            var EndOld = GetEndPoint();
            var p2old = new Point(GetEndPoint().X + a / 2, GetEndPoint().Y - h);
            var absX = p2old.X - EndOld.X;
            var absY = p2old.Y - EndOld.Y;
            P2 = new Point(EndPoint.X + absX, EndPoint.Y +absY);
            return P2;
        }
        
    }
}
