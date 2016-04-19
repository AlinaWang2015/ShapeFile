using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShapeFile
{
    public class Polygon : Gemotry
    {
        List<Polylgon> polygons = new List<Polylgon>();
        public Polylgon[] CollectPolygon(string pathToShapefile)
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
                Polylgon polygon = new Polylgon();
                binaryFile.ReadBytes(12);
                polygon.Box[0] = binaryFile.ReadDouble();
                polygon.Box[1] = binaryFile.ReadDouble();
                polygon.Box[2] = binaryFile.ReadDouble();
                polygon.Box[3] = binaryFile.ReadDouble();
                
                int numParts = binaryFile.ReadInt32();
                int numPoints = binaryFile.ReadInt32();


                for (int j = 0; j < numParts; j++)
                {
                    int parts = binaryFile.ReadInt32();
                    polygon.Parts.Add(parts);
                }

                for (int k = 0; k < numPoints; k++)
                {
                    Point tempPoint = new Point();
                    tempPoint.X = binaryFile.ReadDouble();
                    tempPoint.Y = binaryFile.ReadDouble();
                    polygon.Points.Add(tempPoint);
                }
                polygons.Add(polygon);

            }
            return polygons.ToArray();
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