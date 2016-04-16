using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapeFile
{
    public class ShxRecord : ShapeFile
    {
        private int offset;

        public int Offset
        {
            get
            {
                return offset;
            }

            private set
            {
                offset = value;
            }
        }
    }
}