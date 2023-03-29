using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PaintApp.MyClasses
{
    public class OurShape : Ashape
    {
        public  enum OurType
        {
            Pencil = 2,
            Rubber = 3,
            Circle = 4,
            Line = 5,
            Arrow = 6,
            Triangle = 7,
            Rectangle = 8,
            Select = 9
        }
        public bool ApprovedSelection { get; set; }=false;
        public bool Selected { get; set; } = false;
        private int _id;
        private Point _start = new Point(0, 0);
        private Point _end = new Point(0, 0);
        private int _type;
        public static List<OurShape> ourShapes = new List<OurShape>();
        public OurShape(int id, int type, Point start, Point end)
        {
            _id = id;
            _type = type;
            _start = start;
            _end = end;
        }
        public Point GetStartPoint()
        {
            return _start;
        }
        public Point GetEndPoint()
        {
            return _end;
        }
        public void SetStartPoint(Point p)
        {
            if (p != null)
            {
                _start = p;
            }
        }
        public void SetEndPoint(Point p)
        {
            if (p != null)
            {
                _end = p;
            }
        }
        public int GetId()
        {
            return this._id;
        }
        public int GetOurType()
        {
            return _type;
        }
        public override void deleteMe(OurShape ourShape)
        {
            ourShapes.Remove(ourShape);
        }
        public virtual void RestoreOriginalStroke()
        {

        }

        public override bool IsFillable()
        {
            return false;
        }

        public override object drawMe(object container)
        {
            throw new NotImplementedException();
        }
        public void AddToList( OurShape element)
        {
            ourShapes.Add(element);
        }

        public override bool IsSelected()
        {
            return false;
        }
    }
}
