using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using PaintApp.MyClasses;
using System.Windows.Ink;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace PaintApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum OurType
        {
            Bucket=1,
            Pencil=2,
            Rubber=3,
            Circle=4,
            Line=5,
            Clear=6,
            Triangle=7,
            Rectangle=8,
            Select=9
        }
        private int _id=0;
        private int IdIncrement()
        {
            return _id++;
        }
        public MainWindow()
        {
            InitializeComponent();
            LineWidth = 1;
           
        }

        private Image CursorImage;
        private bool SelectionOn;
        public System.Windows.Media.SolidColorBrush SelectedBgColor { get; set; } 
        public System.Windows.Media.Brush SelectedStrokeColor { get; set; } 
        public int SelectedObjectToCreate { get; set; }
        public int LineWidth { get; set; } = 0;
        public object CreatingObject{ get; set; }
        public Key IsEqualLenght { get; set; }

        private List<Button> myBtnLinewidth = new List<Button>();
        private List<Button> myBtnPencilSp = new List<Button>();
        private List<System.Windows.Shapes.Rectangle> myRecBorder = new List<System.Windows.Shapes.Rectangle>();
        private List<System.Windows.Shapes.Rectangle> myRecStoke = new List<System.Windows.Shapes.Rectangle>();


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddBgColors();
            AddLineWidth();
            AddDrawindObjects();
            BitmapCanvas.Width = this.Width/10.6 * 8;
            BitmapCanvas.Height = this.Height/ 20.8 * 18;
            PencilDraw.Width = this.Width / 10.6 * 8;
            PencilDraw.Height = this.Height / 20.8 * 18;
            OurRectangle rectangle = new OurRectangle(1,4,new Point(1,2), new Point(1,2));
            
           
        }
       
        private List<BgColors> ReadColors(string path)
        {
            List<BgColors> bgColors = new List<BgColors>();
            var file= File.ReadAllText(path);
            var lines = file.Split("\n");
            foreach (var line in lines)
            {
                var colors = line.Split(",");
                BgColors color1= new BgColors();
                color1.Red = int.Parse(colors[0]);
                color1.Green = int.Parse(colors[1]);
                color1.Blue = int.Parse(colors[2]);
                bgColors.Add(color1);
            }
            return bgColors;
        }
        private void AddBgColors()
        {
            //in this line i am reading colors from file 
            StringBuilder path= new StringBuilder();
            path.Append(Directory.GetCurrentDirectory());
            path.Append(@"\BgColors.txt");
            List<BgColors> colors = ReadColors(path.ToString());
            //setting counter to got thrue list that i read from file
            int counter = 0;
            //starting to create first lines of colors (row)
            for (int j = 0; j < 2; j++)
            {
                //define the container that will store first row of req
                StackPanel spRow1 = new StackPanel();
                spRow1.Orientation = Orientation.Horizontal;
                spRow1.VerticalAlignment = VerticalAlignment.Center;
                //starting a loop to create req
                for (int i = 0; i < 7; i++)
                {
                    //createing new req
                    System.Windows.Shapes.Rectangle rec1 = new System.Windows.Shapes.Rectangle();
                    //staret adding prop
                    rec1.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(colors[counter].Red), Convert.ToByte(colors[counter].Green), Convert.ToByte(colors[counter].Blue)));
                    rec1.Width = 50;
                    rec1.Height = 50;
                    rec1.Stroke = System.Windows.Media.Brushes.White;
                    rec1.StrokeThickness = 1;
                    //end defining prop
                    //add the created req into the list of req which i will use afterwards to change some prop
                    myRecBorder.Add(rec1);
                    // registring event that will handle the click on the req
                    rec1.MouseLeftButtonDown += Rec1_MouseLeftButtonUp;
                    //adding req to the cointainer created befour
                    spRow1.Children.Add(rec1);
                    //going to the next color of the list of bgColorw
                    counter++;
                }
                //adeding the whol cointainer to the stackpanel created into the designe tools
                SpBack.Children.Add(spRow1);
            }
            counter = 0;
            //adding the text to the stackpanel created into the designe tools
            TextBlock textForegroud = new TextBlock();
            textForegroud.Text = "Border colors:";
            SpBack.Children.Add(textForegroud);
            //reapeating the whole process from line 68
            for (int j = 0; j < 2; j++)
            {
                StackPanel spRow1 = new StackPanel();
                spRow1.Orientation = Orientation.Horizontal;
                spRow1.VerticalAlignment = VerticalAlignment.Center;
                for (int i = 0; i < 7; i++)
                {
                    System.Windows.Shapes.Rectangle rec1Stroke = new System.Windows.Shapes.Rectangle();
                    rec1Stroke.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(colors[counter].Red), Convert.ToByte(colors[counter].Green), Convert.ToByte(colors[counter].Blue)));
                    rec1Stroke.Width = 50;
                    rec1Stroke.Height = 50;
                    rec1Stroke.Stroke = System.Windows.Media.Brushes.White;
                    rec1Stroke.StrokeThickness = 1;
                    myRecStoke.Add(rec1Stroke);
                    rec1Stroke.MouseLeftButtonDown += Rec1Stroke_MouseLeftButtonDown; ;
                    spRow1.Children.Add(rec1Stroke);
                    counter++;
                }
                SpBack.Children.Add(spRow1);
            }


        }
        private void AddLineWidth()
        {
            var sp1 = Directory.GetFiles(Directory.GetCurrentDirectory()+ @"\icons\LineWidth");
            StackPanel sp = new StackPanel();            
            sp.Orientation = Orientation.Horizontal;
            sp.VerticalAlignment = VerticalAlignment.Center;
            int counter = 0;
            foreach (var item in sp1)
            {
                BitmapImage img = new BitmapImage(new Uri(item));
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = img;
                image.Height = 50;
                Button button = new Button();
                button.Content = image;
                button.Background = System.Windows.Media.Brushes.White;
                button.BorderBrush = System.Windows.Media.Brushes.White;
                button.Width = 120;
                button.Height = 120;
                button.Padding = new Thickness(5);
                button.Click += Button_Click;
                button.Tag = counter +1;
                myBtnLinewidth.Add(button);
                sp.Children.Add(button);
                counter++;
            }
           
            SpBrush.Children.Add(sp);
           

        }

        private void AddDrawindObjects()
        {
            var sp1 = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\icons\Pencil");
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.VerticalAlignment = VerticalAlignment.Center;
            int counter = 0;
            foreach (var item in sp1)
            {
                BitmapImage img = new BitmapImage(new Uri(item));
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = img;
                image.Height = 50;
                Button button1 = new Button();
                button1.Content = image;
                button1.Background = System.Windows.Media.Brushes.White;
                button1.BorderBrush = System.Windows.Media.Brushes.White;
                button1.Width = 120;
                button1.Height = 120;
                button1.Padding = new Thickness(5);
                button1.Click += Button1_Click; ;
                button1.Tag = counter + 1;
                myBtnPencilSp.Add(button1);
                sp.Children.Add(button1);
                counter++;
            }
            SpPencil.Children.Add(sp);
            var sp2 = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\icons\ButtomObjectPanle");
            StackPanel sp22 = new StackPanel();
            sp22.Orientation = Orientation.Horizontal;
            sp22.VerticalAlignment = VerticalAlignment.Center;
            foreach (var item in sp2)
            {
                BitmapImage img = new BitmapImage(new Uri(item));
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = img;
                image.Height = 50;
                Button button1 = new Button();
                button1.Content = image;
                button1.Background = System.Windows.Media.Brushes.White;
                button1.BorderBrush = System.Windows.Media.Brushes.White;
                button1.Width = 120;
                button1.Height = 120;
                button1.Padding = new Thickness(5);
                button1.Click += Button1_Click; ;
                button1.Tag = counter + 1;
                myBtnPencilSp.Add(button1);
                sp22.Children.Add(button1);
                counter++;
            }
            BottomBtnPanel.Children.Add(sp22);
            var sp3 = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\icons\TopObjectPanel");
            StackPanel sp33 = new StackPanel();
            sp33.Orientation = Orientation.Horizontal;
            sp33.VerticalAlignment = VerticalAlignment.Center;
            foreach (var item in sp3)
            {
                BitmapImage img = new BitmapImage(new Uri(item));
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = img;
                image.Height = 50;
                Button button1 = new Button();
                button1.Content = image;
                button1.Background = System.Windows.Media.Brushes.White;
                button1.BorderBrush = System.Windows.Media.Brushes.White;
                button1.Width = 120;
                button1.Height = 120;
                button1.Padding = new Thickness(5);
                button1.Click += Button1_Click; ;
                button1.Tag = counter + 1;
                myBtnPencilSp.Add(button1);
                sp33.Children.Add(button1);
                counter++;
            }
           TopBtnPanel.Children.Add(sp33);

        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            ResetBtnPencilGroup();
            if (CursorImage!=null)
            {
                BitmapCanvas.Children.Remove(CursorImage);
            }
            Button b1 = sender as Button;
            b1.BorderBrush = System.Windows.Media.Brushes.Black;
            b1.BorderThickness = new Thickness(1);
            if (b1.Tag != null)
            {
                SelectionOn = false;
                switch (b1.Tag)
                {
                    case (int)OurType.Pencil:
                        PencilDraw.IsEnabled = true;
                        PencilDraw.EditingMode = InkCanvasEditingMode.Ink;
                        PencilDraw.DefaultDrawingAttributes.Color = System.Windows.Media.Color.FromRgb(0, 0, 0);
                        SelectedObjectToCreate = (int)b1.Tag;
                        break;
                    case (int)OurType.Rubber:
                        {
                            PencilDraw.IsEnabled = true;
                            PencilDraw.EditingMode = InkCanvasEditingMode.EraseByPoint;
                          
                            PencilDraw.DefaultDrawingAttributes = new System.Windows.Ink.DrawingAttributes
                            {
                                StylusTip = System.Windows.Ink.StylusTip.Ellipse,

                            };
                            PencilDraw.DefaultDrawingAttributes.Color = System.Windows.Media.Color.FromRgb(255, 255, 255);
                            SelectedObjectToCreate = (int)b1.Tag;
                        }
                        break;
                    case (int)OurType.Circle:                     
                    case (int)OurType.Rectangle:                    
                    case (int)OurType.Triangle:                    
                    case (int)OurType.Line:
                        {
                            PencilDraw.IsEnabled = false;
                            SelectedObjectToCreate = (int)b1.Tag;
                            SelectionOn = false;
                            foreach (var item in OurShape.ourShapes)
                            {
                                item.ApprovedSelection = false;
                                item.RestoreOriginalStroke();
                            }

                        }
                        break;
                    case (int)OurType.Select:
                        {
                            PencilDraw.IsEnabled = false;
                            SelectedObjectToCreate = 0;
                            SelectionOn = true;
                            foreach (var item in OurShape.ourShapes)
                            {
                                item.ApprovedSelection = true;
                            }
                        }
                        break;
                    case (int)OurType.Bucket:
                        {
                            PencilDraw.IsEnabled = false;
                            SelectedObjectToCreate = (int)b1.Tag;
                            SelectionOn = true;
                            foreach (var item in OurShape.ourShapes)
                            {
                                item.ApprovedSelection = true;
                            }
                            var contetn = b1.Content;
                            var _image = contetn as Image;
                            CursorImage = new Image();
                            CursorImage.Source = _image.Source;
                            if (CursorImage!=null)
                            {
                                CursorImage.Width = 30;
                                CursorImage.Height = 30;
                            }
                            BitmapCanvas.Children.Add(CursorImage);

                        }
                        break;
                    case (int)OurType.Clear:
                        {
                            OurShape.ourShapes.Clear();
                            BitmapCanvas.Children.Clear();
                            PencilDraw.Strokes.Clear();
                            BitmapCanvas.Children.Add(PencilDraw);
                        }
                        break;
                    default:
                        break;
                }
                

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResetLineWidht();
            Button b1 = sender as Button;
            b1.BorderBrush = System.Windows.Media.Brushes.Black;
            b1.BorderThickness = new Thickness(1);
            if (b1.Tag!=null)
            {
                LineWidth = (int)b1.Tag*3;
            }
            PencilDraw.DefaultDrawingAttributes.Width = LineWidth;
            PencilDraw.DefaultDrawingAttributes.Height = LineWidth;

        }

        private void Rec1Stroke_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ResertStroke();
            System.Windows.Shapes.Rectangle rec1 = sender as System.Windows.Shapes.Rectangle;
            SelectedStrokeColor = rec1.Fill;
            rec1.StrokeThickness = 5;
            BtnLoad.Background = SelectedStrokeColor;
        }

        
        private void ResertBorder()
        {
            foreach (System.Windows.Shapes.Rectangle item in myRecBorder)
            {
                item.StrokeThickness = 1;
            }
        }
        private void ResertStroke()
        {
            foreach (System.Windows.Shapes.Rectangle item in myRecStoke)
            {
                item.StrokeThickness = 1;
            }
        }
        private void ResetLineWidht()
        {
            foreach (Button item in myBtnLinewidth)
            {
                item.BorderThickness = new Thickness(0);
                item.BorderBrush = System.Windows.Media.Brushes.White;
            }
        }
        private void ResetBtnPencilGroup()
        {
            foreach (Button item in myBtnPencilSp)
            {
                item.BorderThickness = new Thickness(0);
                item.BorderBrush = System.Windows.Media.Brushes.White;
            }
        }

        private void Rec1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ResertBorder();
            System.Windows.Shapes.Rectangle rec1= sender as System.Windows.Shapes.Rectangle;
            SelectedBgColor = rec1.Fill as SolidColorBrush;
            rec1.StrokeThickness = 5;
            PencilDraw.DefaultDrawingAttributes.Color = SelectedBgColor.Color;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider= sender as Slider;
            int _value=(int)slider.Value;
            TextBlockZoom.Text= (100+(_value*10)).ToString()+"%";
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlockNewProject.Visibility = Visibility.Hidden;
            TextBoxNewProject.Visibility=Visibility.Visible;
            TextBoxNewProject.Text=TextBlockNewProject.Text.Trim();
            TextBoxNewProject.Focus();
        }

        private void TextBoxNewProject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                TextBlockNewProject.Text = TextBoxNewProject.Text.Trim();
                TextBlockNewProject.Visibility = Visibility.Visible;
                TextBoxNewProject.Visibility = Visibility.Hidden;
            }
        }

        private void BitmapCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            switch (SelectedObjectToCreate)
            {
                case 3:
                    break;
                case (int)OurType.Circle:
                    {
                        OurEllipse ourEllipse = new OurEllipse(IdIncrement(), (int)OurType.Circle, e.GetPosition(BitmapCanvas), e.GetPosition(BitmapCanvas));
                        ourEllipse.ApprovedSelection = false;
                        if (LineWidth != 0)
                        {
                            ourEllipse.LineWidth = LineWidth;
                        }
                        if (SelectedStrokeColor != null)
                        {

                            ourEllipse.StrokeColor = SelectedStrokeColor;

                        }
                        if (SelectedBgColor != null)
                        {
                            if (ourEllipse.IsFillable())
                            {
                                ourEllipse.Fill = SelectedBgColor;
                            }
                        }
                        CreatingObject = ourEllipse.drawMe(BitmapCanvas);

                    }
                    break;
                case (int)OurType.Line:
                    {
                        
                        OurLine ourLine = new OurLine(IdIncrement(), (int)OurType.Line, e.GetPosition(BitmapCanvas), e.GetPosition(BitmapCanvas));
                        ourLine.ApprovedSelection = false;
                        if (LineWidth!=0)
                        {
                            ourLine.LineWidth = LineWidth;
                        }
                        if (SelectedStrokeColor!=null)
                        {
                            if (!ourLine.IsFillable())
                            {
                                ourLine.StrokeColor = SelectedStrokeColor;
                            }
                        }
                        CreatingObject = ourLine.drawMe(BitmapCanvas);

                    }
                    break;
                case (int)OurType.Triangle:
                    {
                        try
                        {
                            OurTriangle ourTriangle = new OurTriangle(IdIncrement(), (int)OurType.Triangle, e.GetPosition(BitmapCanvas), e.GetPosition(BitmapCanvas));
                            ourTriangle.ApprovedSelection = false;
                            if (LineWidth != 0)
                            {
                                ourTriangle.LineWidth = LineWidth;
                            }
                            if (SelectedStrokeColor != null)
                            {

                                ourTriangle.StrokeColor = SelectedStrokeColor;

                            }
                            if (SelectedBgColor != null)
                            {
                                if (ourTriangle.IsFillable())
                                {
                                    ourTriangle.FillColor = SelectedBgColor;
                                }
                            }
                            CreatingObject = ourTriangle.drawMe(BitmapCanvas);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    break;
                case (int)OurType.Rectangle:
                    {

                        OurRectangle ourRecq = new OurRectangle(IdIncrement(), (int)OurType.Rectangle, e.GetPosition(BitmapCanvas), e.GetPosition(BitmapCanvas));
                        ourRecq.ApprovedSelection=false;
                        if (LineWidth != 0)
                        {
                            ourRecq.LineWidth = LineWidth;
                        }
                        if (SelectedStrokeColor != null)
                        {
                           
                          ourRecq.StrokeColor = SelectedStrokeColor;
                            
                        }
                        if (SelectedBgColor!=null)
                        {
                            if (ourRecq.IsFillable())
                            {
                                ourRecq.FillColor = SelectedBgColor;
                            }
                        }

                        CreatingObject = ourRecq.drawMe(BitmapCanvas);
                        Canvas.SetTop(ourRecq.GetRectangle(), e.GetPosition(BitmapCanvas).Y);
                        Canvas.SetLeft(ourRecq.GetRectangle(), e.GetPosition(BitmapCanvas).X);
                    }
                    break;
                default:
                    break;
            }
            if (SelectionOn == true)
            {
                if (SelectedObjectToCreate == (int)OurType.Bucket)
                {
                    try
                    {
                        var element = OurShape.ourShapes.Find(a => a.Selected == true);
                        if (element != null)
                        {
                            switch (element.GetOurType())
                            {
                                case (int)OurType.Rectangle:
                                    {
                                        var rectangle = element as OurRectangle;
                                        rectangle.FillColor = SelectedBgColor;
                                        rectangle.GetRectangle().Fill = SelectedBgColor;
                                    }
                                    break;
                                case (int)OurType.Circle:
                                    {
                                        var circle = element as OurEllipse;
                                        circle.Fill = SelectedBgColor;
                                        circle.Circle.Fill = SelectedBgColor;
                                    }
                                    break;
                                case (int)OurType.Line:
                                    {
                                        var line = element as OurLine;
                                        line.StrokeColor = SelectedStrokeColor;
                                        line.GetLine().Stroke = SelectedStrokeColor;
                                    }
                                    break;
                                case (int)OurType.Triangle:
                                    {
                                        var triangle = element as OurTriangle;
                                        triangle.FillColor = SelectedBgColor;
                                        triangle.Poly.Fill = SelectedBgColor;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                    catch (Exception)
                    {

                    }

                }

            }
        }
        private void BitmapCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //we can do serialization here
            var canvas = sender as Canvas;
            if (SelectionOn==true)
            {
                var shape = OurShape.ourShapes.Find(a => a.Selected == true);
                if (shape != null)
                {
                    switch (shape.GetOurType())
                    {
                        case (int)OurType.Rectangle:
                            {
                                var ourRecq = shape as OurRectangle;
                                ourRecq.SetStartPoint(e.GetPosition(canvas));
                                var endpoint = new Point(ourRecq.GetStartPoint().X+ourRecq.GetRectangle().Width, ourRecq.GetStartPoint().Y+ ourRecq.GetRectangle().Height);
                                shape.SetEndPoint(endpoint);

                            }
                            break;
                        case (int)OurType.Circle:
                            {
                                var circle = shape as OurEllipse;
                                circle.SetStartPoint(e.GetPosition(canvas));
                                var endpoint=new Point(circle.GetStartPoint().X+circle.Circle.Width, circle.GetStartPoint().Y+circle.Circle.Height);
                                circle.SetEndPoint(endpoint);
                            }
                            break;
                        case (int)OurType.Line:
                            {
                                var line = shape as OurLine;
                                var width = line.GetLine().Width;
                                var height = line.GetLine().Height;
                                line.SetStartPoint(e.GetPosition((canvas)));
                                line.SetEndPoint(new Point(e.GetPosition(canvas).X+width, e.GetPosition(canvas).Y+height));
                                
                            }
                            break;
                        case (int)OurType.Triangle:
                            {
                                var triangle = shape as OurTriangle;
                                triangle.EndPoint = e.GetPosition(canvas);
                                var p1 = triangle.MoveP1(triangle.EndPoint);
                                var p2 = triangle.MoveP2(triangle.EndPoint);
                                if (p1.X > BitmapCanvas.Width || triangle.EndPoint.X > BitmapCanvas.Width)
                                {
                                    p1.X = BitmapCanvas.Width;

                                }
                                if (p2.X > BitmapCanvas.Width)
                                {
                                    p2.X = BitmapCanvas.Width;
                                }
                                if (p2.Y > BitmapCanvas.Height || triangle.EndPoint.Y > BitmapCanvas.Height)
                                {
                                    p2.Y = BitmapCanvas.Height;
                                }
                                if (p1.Y > BitmapCanvas.Height)
                                {
                                    p1.Y = BitmapCanvas.Height;
                                }
                                triangle.points.Clear();
                                triangle.points.Add(p1);
                                triangle.points.Add(p2);
                                triangle.points.Add(triangle.EndPoint);
                            }
                            break;
                        default:
                            break;
                    }
                }
               
            }
            else
            {
                switch (SelectedObjectToCreate)
                {
                    case 3:
                        break;
                    case (int)OurType.Line:
                        {
                            try
                            {
                                var ourLine = CreatingObject as OurLine;
                                var creating = ourLine.GetLine();
                                creating.X2 = e.GetPosition(canvas).X;
                                creating.Y2 = e.GetPosition(canvas).Y;
                                var element = OurShape.ourShapes.Find((b) => b.GetId() == ourLine.GetId());
                                element.SetEndPoint(new Point(creating.X2, creating.Y2));
                                var line = element as OurLine;
                                line.LineWidth = LineWidth;
                                line.StrokeColor = SelectedStrokeColor;
                                CreatingObject = null;
                            }
                            catch (Exception)
                            {

                            }
                        }
                        break;
                    case (int)OurType.Circle:
                        {
                            try
                            {
                                var ourEllipse = CreatingObject as OurEllipse;
                                var circle = ourEllipse.Circle;
                                Point rez = ourEllipse.CaclulateCircle(e.GetPosition(canvas));
                                circle.Height = rez.X;
                                circle.Width = rez.X;
                                var element = OurShape.ourShapes.Find(b => b.GetId() == ourEllipse.GetId());
                                element.SetEndPoint(e.GetPosition((canvas)));
                                var circle1 = element as OurEllipse;
                                circle1.LineWidth = LineWidth;
                                circle1.StrokeColor = SelectedStrokeColor;
                                circle1.Fill = SelectedBgColor;
                                CreatingObject = null;
                            }
                            catch (Exception)
                            {

                            }

                        }
                        break;
                    case (int)OurType.Triangle:
                        {
                            try
                            {
                                CreatingObject = null;
                            }
                            catch (Exception)
                            {

                            }
                        }
                        break;
                    case (int)OurType.Rectangle:
                        {
                            try
                            {
                                if (IsEqualLenght == Key.LeftShift)
                                {
                                    var ourRecq = CreatingObject as OurRectangle;
                                    var creating = ourRecq.GetRectangle();
                                    Point rez = CalculateSquare(ourRecq.GetStartPoint(), e.GetPosition(canvas));
                                    if (creating != null)
                                    {
                                        creating.Width = rez.X;
                                        creating.Height = rez.Y;
                                    }
                                    var element = OurShape.ourShapes.Find((b) => b.GetId() == ourRecq.GetId());
                                    element.SetEndPoint(new Point(element.GetStartPoint().X + rez.X, element.GetStartPoint().Y + rez.Y));
                                    var rec = element as OurRectangle;
                                    rec.LineWidth = LineWidth;
                                    rec.StrokeColor = SelectedStrokeColor;
                                    rec.FillColor = SelectedBgColor;
                                    CreatingObject = null;
                                    IsEqualLenght = 0;
                                }
                                else
                                {
                                    var ourRecq = CreatingObject as OurRectangle;
                                    var creating = ourRecq.GetRectangle();
                                    Point rez = CalculateRect(ourRecq.GetStartPoint(), e.GetPosition(canvas));
                                    if (creating != null)
                                    {
                                        creating.Width = rez.X;
                                        creating.Height = rez.Y;
                                    }
                                    var element = OurShape.ourShapes.Find((b) => b.GetId() == ourRecq.GetId());
                                    var rec = element as OurRectangle;
                                    rec.LineWidth = LineWidth;
                                    rec.StrokeColor = SelectedStrokeColor;
                                    rec.FillColor = SelectedBgColor;
                                    element.SetEndPoint(e.GetPosition(canvas));

                                    CreatingObject = null;
                                }

                            }
                            catch (Exception)
                            {

                            }
                        }
                        break;
                    default:
                        break;
                }
            }
           
        }

        private void BitmapCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var canvas = sender as Canvas;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                switch (SelectedObjectToCreate)
                {
                    case 3:
                        break;
                    case (int)OurType.Line:
                        {
                            if (CreatingObject!=null)
                            {
                                var ourLine = CreatingObject as OurLine;
                                var creating = ourLine.GetLine();
                                if (creating != null)
                                {
                                    creating.X2 = e.GetPosition(canvas).X;
                                    creating.Y2 = e.GetPosition(canvas).Y;
                                }
                            }
                           
                        }
                        break;
                    case (int)OurType.Circle:
                        {
                            if (CreatingObject!=null)
                            {
                                var ourEllipse = CreatingObject as OurEllipse;
                                var cicle = ourEllipse.Circle;
                                Point rez = ourEllipse.CaclulateCircle(e.GetPosition(canvas));
                                if (cicle != null)
                                {
                                    cicle.Width = rez.X;
                                    cicle.Height = rez.X;
                                }
                                Canvas.SetTop(cicle, ourEllipse.GetStartPoint().Y);
                                Canvas.SetLeft(cicle, ourEllipse.GetStartPoint().X);
                            }
                        }
                        break;
                    case (int)OurType.Triangle:
                        {
                            if (CreatingObject != null)
                            {
                                var ourTriangle = CreatingObject as OurTriangle;
                                //var index = canvas.Children.IndexOf(ourTriangle.Poly);
                                ourTriangle.EndPoint=e.GetPosition(canvas);
                                var p1 = ourTriangle.CalculateP1(ourTriangle.EndPoint);
                                var p2 = ourTriangle.CalculateP2(ourTriangle.EndPoint);
                                if (p1.X>BitmapCanvas.Width ||  ourTriangle.EndPoint.X > BitmapCanvas.Width)
                                {
                                    p1.X = BitmapCanvas.Width;

                                }
                                if (p2.X > BitmapCanvas.Width )
                                {
                                    p2.X = BitmapCanvas.Width;
                                }
                                if (p2.Y>BitmapCanvas.Height  || ourTriangle.EndPoint.Y > BitmapCanvas.Height)
                                {
                                    p2.Y= BitmapCanvas.Height;
                                }
                                if ( p1.Y > BitmapCanvas.Height)
                                {
                                    p1.Y = BitmapCanvas.Height;
                                }
                                ourTriangle.points.Clear();
                                ourTriangle.points.Add(p1);
                                ourTriangle.points.Add(p2);
                                ourTriangle.points.Add(ourTriangle.EndPoint);
                                
                            }
                        }
                        break;
                    case (int)OurType.Rectangle:
                        {
                            if (IsEqualLenght==Key.LeftShift)
                            {
                                
                                if (CreatingObject != null)
                                {
                                    var ourRecq = CreatingObject as OurRectangle;
                                    var creating = ourRecq.GetRectangle();
                                    Point rez = CalculateSquare(ourRecq.GetStartPoint(), e.GetPosition(canvas));
                                    if (creating != null)
                                    {
                                        creating.Width = rez.X;
                                        creating.Height = rez.Y;
                                    }

                                }
                                IsEqualLenght= 0;

                            }
                            else
                            {
                                if (CreatingObject != null)
                                {
                                    var ourRecq = CreatingObject as OurRectangle;
                                    var creating = ourRecq.GetRectangle();
                                    Point rez = CalculateRect(ourRecq.GetStartPoint(), e.GetPosition(canvas));
                                    if (creating != null)
                                    {
                                        creating.Width = rez.X;
                                        creating.Height = rez.Y;
                                    }
                                }
                            }
                            

                        }
                        break;              
                    default:
                        break;
                }
                if (SelectionOn==true)
                {
                    try
                    {
                        if (SelectedObjectToCreate!=(int)OurType.Bucket)
                        {
                            var element = OurShape.ourShapes.Find(a => a.Selected == true);
                            if (element != null)
                            {
                                switch (element.GetOurType())
                                {
                                    case (int)OurType.Rectangle:
                                        {
                                            var rectangle = element as OurRectangle;
                                            Canvas.SetTop(rectangle.GetRectangle(), e.GetPosition(canvas).Y);
                                            Canvas.SetLeft(rectangle.GetRectangle(), e.GetPosition(canvas).X);
                                        }
                                        break;
                                    case (int)OurType.Circle:
                                        {
                                            var circle = element as OurEllipse;
                                            Canvas.SetTop(circle.Circle, e.GetPosition(canvas).Y);
                                            Canvas.SetLeft(circle.Circle, e.GetPosition(canvas).X);
                                        }
                                        break;
                                    case (int)OurType.Line:
                                        {
                                            var line = element as OurLine;
                                            line.GetLine().X1 = e.GetPosition(canvas).X;
                                            line.GetLine().Y1 = e.GetPosition(canvas).Y;
                                            line.GetLine().X2 = line.Move(e.GetPosition(canvas)).X;
                                            line.GetLine().Y2 = line.Move(e.GetPosition(canvas)).Y;
                                        }
                                        break;
                                    case (int)OurType.Triangle:
                                        {
                                            var triangle = element as OurTriangle;
                                            triangle.EndPoint = e.GetPosition(canvas);
                                            var p1 = triangle.MoveP1(triangle.EndPoint);
                                            var p2 = triangle.MoveP2(triangle.EndPoint);
                                            if (p1.X > BitmapCanvas.Width || triangle.EndPoint.X > BitmapCanvas.Width)
                                            {
                                                p1.X = BitmapCanvas.Width;

                                            }
                                            if (p2.X > BitmapCanvas.Width)
                                            {
                                                p2.X = BitmapCanvas.Width;
                                            }
                                            if (p2.Y > BitmapCanvas.Height || triangle.EndPoint.Y > BitmapCanvas.Height)
                                            {
                                                p2.Y = BitmapCanvas.Height;
                                            }
                                            if (p1.Y > BitmapCanvas.Height)
                                            {
                                                p1.Y = BitmapCanvas.Height;
                                            }
                                            triangle.points.Clear();
                                            triangle.points.Add(p1);
                                            triangle.points.Add(p2);
                                            triangle.points.Add(triangle.EndPoint);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        
                       
                    }
                    catch (Exception)
                    {

                    }
                    
                }
            }
            if (SelectedObjectToCreate!=null)
            {
                if ((int)SelectedObjectToCreate == (int)OurType.Bucket)
                {
                    BitmapCanvas.Cursor = Cursors.None;
                    Canvas.SetTop(CursorImage, e.GetPosition(canvas).Y);
                    Canvas.SetLeft(CursorImage, e.GetPosition(canvas).X);
                }
                else
                {
                    BitmapCanvas.Cursor = Cursors.Arrow;
                }
            }
            else
            {
                BitmapCanvas.Cursor = Cursors.Arrow;
            }
            
           
              
            
        }
        private Point CalculateRect(Point _start,Point _current)
        {
            double _width = 0;
            double _height = 0;
            if (_current.X>_start.X)
            {
                _width = _current.X - _start.X;
            }
            else
            {
                _width = Math.Abs(_current.X - _start.X);
            }
            if (_current.Y > _start.Y)
            {
                _height = _current.Y - _start.Y;
            }
            else
            {
                _height =Math.Abs(_current.Y - _start.Y);
            }
            return new Point(_width, _height);
        }
        private Point CalculateSquare(Point _start, Point _current)
        {
            double _width = 0;
            if (_current.X > _start.X)
            {
                _width = _current.X - _start.X;
            }
            else
            {
                _width = Math.Abs(_current.X - _start.X);
            }
           
            return new Point(_width, _width);
        }

        private void BitmapCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            IsEqualLenght = e.Key;
            if (e.Key == Key.Delete)
            {
                var element = OurLine.ourShapes.Find(a => a.Selected == true);
                if (element != null)
                {
                    switch (element.GetOurType())
                    {
                        case (int)OurType.Line:
                            {
                                var obj = element as OurLine;
                                BitmapCanvas.Children.Remove(obj.GetLine());
                            }
                            break;
                            case (int)OurType.Rectangle:
                            {
                                var obj = element as OurRectangle;
                                BitmapCanvas.Children.Remove(obj.GetRectangle());
                            }
                            break;
                        case (int)OurType.Circle:
                            {
                                var obj = element as OurEllipse;
                                BitmapCanvas.Children.Remove(obj.Circle);
                            }
                            break;
                        case (int)OurType.Triangle:
                            {
                                var obj = element as OurTriangle;
                                BitmapCanvas.Children.Remove(obj.Poly);
                            }
                            break;
                        default:
                            break;
                    }
                    element.deleteMe(element);
                }
                
               
              
            }
            
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //Save logic
            List<OurSerialisationTemplate> serialisationTemplates = new List<OurSerialisationTemplate>();
            foreach (var item in OurShape.ourShapes)
            {
                OurSerialisationTemplate ourSerialisation = new OurSerialisationTemplate
                {
                    id = item.GetId(),
                    EndPoint = item.GetEndPoint(),
                    StartPoint = item.GetStartPoint(),
                    Type = item.GetOurType(),
                };
                switch (item.GetOurType())
                {
                    case (int)OurType.Line:
                        {
                            var line = item as OurLine;
                            ourSerialisation.LineWidth = line.LineWidth;
                            ourSerialisation.StrokeColor = line.StrokeColor;
                            serialisationTemplates.Add(ourSerialisation);
                        }
                        break;
                    case (int)OurType.Rectangle:
                        {
                            var rectangle = item as OurRectangle;
                            ourSerialisation.LineWidth = rectangle.LineWidth;
                            ourSerialisation.StrokeColor = rectangle.StrokeColor;
                            ourSerialisation.FillColor = rectangle.FillColor;
                            var calculated = CalculateRect(item.GetStartPoint(), item.GetEndPoint());
                            ourSerialisation.Width = calculated.X;
                            ourSerialisation.Heigh = calculated.Y;
                            serialisationTemplates.Add(ourSerialisation);
                        }
                        break;
                        case (int)OurType.Circle:
                        {
                            var circle = item as OurEllipse;
                            ourSerialisation.LineWidth = circle.LineWidth;
                            ourSerialisation.StrokeColor= circle.StrokeColor;
                            ourSerialisation.FillColor = circle.Fill;
                            serialisationTemplates.Add(ourSerialisation);
                        }
                        break;
                    case (int)OurType.Triangle:
                        {
                            var triangle = item as OurTriangle;
                            ourSerialisation.LineWidth = triangle.LineWidth;
                            ourSerialisation.FillColor= triangle.FillColor;
                            ourSerialisation.StrokeColor=triangle.StrokeColor;
                            ourSerialisation.Points = triangle.points;
                            serialisationTemplates.Add(ourSerialisation);
                        }
                        break;
                    default:
                        break;
                }
            }
            var json = JsonConvert.SerializeObject(serialisationTemplates);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = TextBoxNewProject.Text+".ourPaint";
            if (saveFileDialog.ShowDialog()==true)
            {
                File.WriteAllText(saveFileDialog.FileName, json);
            }
            
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
           
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Certan files (*.ourPaint)|*.ourPaint";
            if (openFileDialog.ShowDialog()==true)
            {
                
                var name = openFileDialog.FileName.Split("\\");
                var subname=name[name.Length - 1].Split(".");
                TextBlockNewProject.Text = subname[0];
                var loaded=File.ReadAllText(openFileDialog.FileName);
                
                var rez=JsonConvert.DeserializeObject<List<OurSerialisationTemplate>>(loaded);
                BitmapCanvas.Children.Clear();
                PencilDraw.Strokes.Clear();
                BitmapCanvas.Children.Add(PencilDraw);
                foreach (var item in rez)
                {
                    _id = item.id;
                    switch (item.Type)
                    {
                        case (int)OurType.Line:
                            {
                                OurLine ourLine = new OurLine(item.id,item.Type,item.StartPoint,item.EndPoint);
                                ourLine.LineWidth = item.LineWidth;
                                if (item.StrokeColor == null)
                                {
                                    ourLine.StrokeColor = Brushes.Black;
                                }
                                else
                                {
                                    ourLine.StrokeColor = item.StrokeColor;
                                }
                               
                                ourLine.drawMe(BitmapCanvas);
                          
                            }
                            break;
                        case (int)OurType.Circle:
                            {
                                OurEllipse ourEllipse = new OurEllipse(item.id, item.Type, item.StartPoint, item.EndPoint);
                                if (item.FillColor == null)
                                {
                                    ourEllipse.Fill = Brushes.Transparent;
                                }
                                else
                                {
                                    ourEllipse.Fill = item.FillColor;
                                }
                                if (item.StrokeColor == null)
                                {
                                    ourEllipse.StrokeColor = Brushes.Black;
                                }
                                else
                                {
                                    ourEllipse.StrokeColor = item.StrokeColor;
                                }
                                ourEllipse.LineWidth = item.LineWidth;
                                ourEllipse.drawMe(BitmapCanvas);
                                Canvas.SetTop(ourEllipse.Circle, ourEllipse.GetStartPoint().Y);
                                Canvas.SetLeft(ourEllipse.Circle, ourEllipse.GetStartPoint().X);
                            }
                            break;
                        case (int)OurType.Rectangle:
                            {
                                OurRectangle ourRect = new OurRectangle(item.id, item.Type, item.StartPoint, item.EndPoint);
                                if (item.FillColor==null)
                                {
                                    ourRect.FillColor = Brushes.Transparent;
                                }
                                else
                                {
                                    ourRect.FillColor=item.FillColor;
                                }
                                if (item.StrokeColor == null)
                                {
                                    ourRect.StrokeColor = Brushes.Black;
                                }
                                else
                                {
                                    ourRect.StrokeColor = item.StrokeColor;
                                }

                                ourRect.LineWidth = item.LineWidth;
                                ourRect.drawMe(BitmapCanvas);
                                var rec1=ourRect.GetRectangle();
                                rec1.Width=(double)item.Width;
                                rec1.Height = (double)item.Heigh;
                                Canvas.SetTop(ourRect.GetRectangle(), ourRect.GetStartPoint().Y);
                                Canvas.SetLeft(ourRect.GetRectangle(), ourRect.GetStartPoint().X);
                            }
                            break;
                        case (int)OurType.Triangle:
                            {
                                OurTriangle ourTriangle = new OurTriangle(item.id, item.Type, item.StartPoint, item.EndPoint);
                                if (item.FillColor == null)
                                {
                                    ourTriangle.FillColor = Brushes.Transparent;
                                }
                                else
                                {
                                    ourTriangle.FillColor = item.FillColor;
                                }
                                if (item.StrokeColor == null)
                                {
                                    ourTriangle.StrokeColor = Brushes.Black;
                                }
                                else
                                {
                                    ourTriangle.StrokeColor = item.StrokeColor;
                                }
                                if (item.Points==null)
                                {
                                    break;
                                }
                                else
                                {
                                    ourTriangle.EndPoint = item.Points[0];
                                    ourTriangle.P1 = item.Points[1];
                                    ourTriangle.P2= item.Points[2];
                                }
                                ourTriangle.LineWidth = item.LineWidth;
                                ourTriangle.drawMe(BitmapCanvas);
                            }
                            break;
                        default:
                            break;
                    }
                }
                _id++;
               
            }
        }
    }
}
