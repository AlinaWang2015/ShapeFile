using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFile
{
    public class Point : Gemotry
    {
        private double x;
        private double y;
        public List<Point> points = new List<Point>();

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

        public Point[] CollectPoint(string pathToShapefile)
        {
            string filepath = System.IO.Path.HasExtension(pathToShapefile) ? pathToShapefile.Substring(0, pathToShapefile.Length - (System.IO.Path.GetExtension(pathToShapefile).Length)) : pathToShapefile;
            string shpfilepath = filepath + ".shp";
            FileStream fs = new FileStream(shpfilepath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryFile = new BinaryReader(fs);
            binaryFile.BaseStream.Seek(100, SeekOrigin.Current);
            binaryFile.ReadBytes(100);
            MainRecord mainrecords = new MainRecord();
            int shapeCount = mainrecords.GetNumRecords(pathToShapefile);
            for (int i = 0; i < shapeCount; i++)
            {
                Point point = new Point();
                //记录头8个字节和一个int(4个字节)的shapetype
                //binaryFile.BaseStream.Seek(12,fs);
                binaryFile.ReadBytes(12);
                point.X = binaryFile.ReadDouble();
                point.Y = binaryFile.ReadDouble();
                points.Add(point);
            }
            return points.ToArray();
        }

        public void DisplayPoint(Point[] points)
        {
            Console.WriteLine("points number are :" + points.Count());
            Console.WriteLine("Points: ");
            for (int i = 0; i < points.Length; i++)
            {
                Console.WriteLine("\tX: {0}, Y: {1}", points[i].X, points[i].Y);
                
            }
        }

        
    }
}
