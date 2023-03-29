using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintApp.MyClasses
{
    public abstract class Ashape
    {
        public abstract object drawMe(object container);
        public abstract void deleteMe(OurShape ourShape);
        public abstract bool IsFillable();
        public abstract bool IsSelected();
    }
}
