using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFile
{
    public class Point : Gemotry
    {
        private double x;
        private double y;

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public Point () { }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Point CreatePoint(double x, double y)
        {
            return new Point(x, y);
        }

        public Point CreatePoint(byte[] buffer)
        {
            if (buffer.Length >= 20)
            {
                return new Point(BitConverter.ToDouble(buffer, 4), BitConverter.ToDouble(buffer, 12));
            }
            else
            {
                throw new ArgumentException("Byte buffer was an invalid size to create a point shape. Buffer size provided was " + buffer.Length.ToString());
            }
        }

        public Point[] RecordsToPointGeometry(List<MainRecord> records)
        {
            Point[] points = new Point[records.Count];
            for (int i = 0; i < records.Count; i++)
            {
                points[i] = new Point(BitConverter.ToDouble(records[i].RecordContents, 4), BitConverter.ToDouble(records[i].RecordContents, 12));
            }
            return points;
        }

        public void DisplayPoint(Point[] points)
        {
            Console.WriteLine("Points: ");
            for (int i = 0; i < points.Length; i++)
            {
                Console.WriteLine("\tX: {0}, Y: {1}", points[i].X, points[i].Y);
            }
        }
    }
}
