using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFile
{
    public class Gemotry
    {
        private double[] box = new double[4];
        private List<int> parts = new List<int>();
        private List<Point> points = new List<Point>();
        

        public double[] Box
        {
            get
            {
                return box;
            }

            set
            {
                box = value;
            }
        }

        public List<int> Parts
        {
            get
            {
                return parts;
            }

            set
            {
                parts = value;
            }
        }

        public List<Point> Points
        {
            get
            {
                return points;
            }

            set
            {
                points = value;
            }
        }

        

        public void GetPerimeter()
        {
            throw new System.NotImplementedException();
        }

        public void GetBox()
        {
            throw new System.NotImplementedException();
        }

        public void GetArea()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayGeometryRecords(object[] records)
        {
            if (records.GetType() == typeof(Point[]))
            {
                Point point = new Point();
                point.DisplayPoint((Point[])records);
            }
            else if (records.GetType() == typeof(Multipoint[]))
            {
                Multipoint multipoint = new Multipoint();
                //multipoint.DisplayMultipoints((Multipoint[])records);
            }
            else if (records.GetType() == typeof(Polyline[]))
            {
                Polyline polyline = new Polyline();
                polyline.DisplayPolylines(records);
            }
            else if (records.GetType() == typeof(Polygon[]))
            {
                Polygon polygon = new Polygon();
                polygon.DisplayPolygons((Polygon[])records);
            }
            else
            {
                Console.WriteLine("Type \"{0}\" not recognized.", records.GetType());
            }
        }
    }
}
