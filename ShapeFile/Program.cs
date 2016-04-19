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
            var geometry = shapefile.CollectionGemotry(@"C:\Users\alinawang\Desktop\testData\lower.shp");
            Gemotry geomotry1 = new Gemotry();
            geomotry1.DisplayGeometryRecords(geometry);
            
            Console.ReadLine();
        }
    }
}
