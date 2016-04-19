using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShapeFile
{
    public class Polylgon : Gemotry
    {
        List<Polylgon> polylines = new List<Polylgon>();
        public Polylgon[] CollectPolyline(string pathToShapefile)
        {
            string filepath = System.IO.Path.HasExtension(pathToShapefile) ? pathToShapefile.Substring(0, pathToShapefile.Length - (System.IO.Path.GetExtension(pathToShapefile).Length)) : pathToShapefile;
            string shpfilepath = filepath + ".shp";
            FileStream fs = new FileStream(shpfilepath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryFile = new BinaryReader(fs);
            binaryFile.BaseStream.Seek(100, SeekOrigin.Current);
            MainRecord mainrecords = new MainRecord();
            int shapeCount = mainrecords.GetNumRecords(pathToShapefile);
            for (int i = 0; i < shapeCount; i++)
            {
                Polylgon polyline = new Polylgon();
                binaryFile.BaseStream.Seek(12, SeekOrigin.Current);
                polyline.Box[0] = binaryFile.ReadDouble();
                polyline.Box[1] = binaryFile.ReadDouble();
                polyline.Box[2] = binaryFile.ReadDouble();
                polyline.Box[3] = binaryFile.ReadDouble();
                //读取的partcount 为1 pointcount 为149
                int numParts = binaryFile.ReadInt32();
                int numPoints = binaryFile.ReadInt32();


                for (int j = 0; j < numParts; j++)
                {
                    int parts = binaryFile.ReadInt32();
                    polyline.Parts.Add(parts);
                }

                for (int k = 0; k < numPoints; k++)
                {
                    Point tempPoint = new Point();
                    tempPoint.X = binaryFile.ReadDouble();
                    tempPoint.Y = binaryFile.ReadDouble();
                    polyline.Points.Add(tempPoint);
                }
                polylines.Add(polyline);

            }
            return polylines.ToArray();
        }
        

        
                

        public void DisplayPolylines(object[] polylines)
        {
            Polylgon[] polyline = (Polylgon[])polylines;
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
 