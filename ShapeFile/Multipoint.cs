using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapeFile
{
    public class Multipoint : Gemotry
    {
        public Multipoint() { }

        public Multipoint(Point[] points)
        {
            this.Points = Points;
        }

        public Multipoint CreateMultipoint(Point[] points)
        {
            return new Multipoint(points);
        }

        public Multipoint CreateMultipoint(byte[] buffer)
        {
            if (buffer.Length >= 56)
            {
                int numPoints = BitConverter.ToInt32(buffer, 36);
                Point[] pointsFromBuffer = new Point[numPoints];
                for (int i = 0; i < numPoints; i++)
                {
                    pointsFromBuffer[i] = new Point(BitConverter.ToDouble(buffer, (i * 16) + 40), BitConverter.ToDouble(buffer, (i * 16) + 48));
                }
                return new Multipoint(pointsFromBuffer);
            }
            else
            {
                throw new ArgumentException("Byte buffer was an invalid size to create a multipoint shape. Buffer size provided was " + buffer.Length.ToString());
            }
        }

        public Multipoint[] RecordsToMultipointGeometry(List<MainRecord> records)
        {
            Multipoint[] multipoints = new Multipoint[records.Count];
            for (int i = 0; i < records.Count; i++)
            {
                int numPoints = BitConverter.ToInt32(records[i].RecordContents, 36);
                Point[] points = new Point[numPoints];
                for (int j = 0; j < numPoints; j++)
                {
                    points[j] = new Point(BitConverter.ToDouble(records[i].RecordContents, (j * 16) + 40), BitConverter.ToDouble(records[i].RecordContents, (j * 16) + 48));
                }
                multipoints[i] = new Multipoint(points);
            }
            return multipoints;
        }

        public void DisplayMultipoints(Multipoint[] multipoints)
        {
            for (int i = 0; i < multipoints.Length; i++)
            {
                Console.WriteLine("Multipoint containing {0} points: ", multipoints[i].Points.Count);
                Point[] aa = new Point[multipoints[i].Points.Count];
                multipoints[i].Points.CopyTo(aa);
                Point point = new Point();
                point.DisplayPoint(aa);
            }
        }
    }
}