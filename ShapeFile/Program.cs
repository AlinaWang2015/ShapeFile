using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFile
{
    class Program
    {
        static void Main(string[] args)
        {
            ShapeFile shapefile = new ShapeFile();
            shapefile.ReadShpFile(@"E:\Project\Shape\lower.shp");
            var geometry = shapefile.CollectionGemotry(@"E:\Project\Shape\lower.shp");
            Gemotry geomotry1 = new Gemotry();
            geomotry1.DisplayGeometryRecords(geometry);
            Console.ReadLine();
        }
    }
}
