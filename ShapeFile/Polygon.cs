using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapeFile
{
    public class Polygon : Gemotry
    {
        public Polygon CreatePolygon(byte[] buffer)
        {
            if (buffer.Length >= 60)
            {
                // return new Polygon(ge);
                return null;
            }
            else
            {
                throw new ArgumentException("Byte buffer was an invalid size to create a polygon shape. Buffer size provided was " + buffer.Length.ToString());
            }
        }

        public void RecordsToPolygon()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayPolygons(object[] polygons)
        {
            Polygon[] polygon = (Polygon[])polygons;
            for (int i = 0; i < polygons.Length; i++)
            {
                Console.WriteLine("Polygon containing {0} part: ", polygon[i].Parts.Count);
                Point[] aa = new Point[polygon[i].Points.Count];
                polygon[i].Points.CopyTo(aa);
                Point point = new Point();
                point.DisplayPoint(aa);
            }
        }
    }
}