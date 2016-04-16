using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapeFile
{
    public class Polyline : Gemotry
    {

        public Polyline CreatePolyline(byte[] buffer)
        {
            if (buffer.Length >= 60)
            {
                //); 
                return null;
            }
            else
            {
                throw new ArgumentException("Byte buffer was an invalid size to create a polyline shape. Buffer size provided was " + buffer.Length.ToString());
            }
        }

        /* public Polyline[] RecordsToPolyline(List<MainRecord> records)
         {
             Polyline[] polyline = new Polyline[records.Count];
             for (int i = 0; i < records.Count; i++)
             {

             }
             return polyline;
         }*/

        public void DisplayPolylines(object[] polylines)
        {
            Polyline[] polyline = (Polyline[])polylines;
            for (int i = 0; i < polylines.Length; i++)
            {
                Console.WriteLine("Polyline containing {0} part: ", polyline[i].Parts.Count);
                Point point = new Point ();
                Point[] aa = new Point[polyline[i].Points.Count];
                polyline[i].Points.CopyTo(aa);
                point.DisplayPoint(aa);
            }
        }
    }
}
 