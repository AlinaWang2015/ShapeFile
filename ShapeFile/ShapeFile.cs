using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShapeFile
{
    public class ShapeFile
    {
        private double[] box = new double[4];
        private int fileLength;
        private int version;
        private int shapeType;
        private int contentLength;
        private FileStream readStream;
        private List<ShxRecord> index;


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

        public int FileLength
        {
            get
            {
                return fileLength;
            }

            set
            {
                fileLength = value;
            }
        }

        public int Version
        {
            get
            {
                return version;
            }

            set
            {
                version = value;
            }
        }

        public int ShapeType
        {
            get
            {
                return shapeType;
            }

            set
            {
                shapeType = value;
            }
        }

        public int ContentLength
        {
            get
            {
                return contentLength;
            }

            set
            {
                contentLength = value;
            }
        }

        public FileStream ReadStream
        {
            get
            {
                return readStream;
            }

            set
            {
                readStream = value;
            }
        }

        private List<ShxRecord> Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }


        /// <summary>
        /// 判断文件路径是否有效,以及路径中的文件是否有效
        /// </summary>
        /// <param name="shapeFilePath"></param>
        /// <returns></returns>
        public bool ValidPath(string shapeFilePath)
        {
            string extention = System.IO.Path.GetExtension(shapeFilePath);
            string filenameNoExtension = System.IO.Path.GetFileNameWithoutExtension(shapeFilePath);

            if(extention ==""||extention ==".shp"||extention==".shx")
            {
                string directory = System.IO.Path.GetDirectoryName(shapeFilePath);
                if(Directory.Exists(directory))
                {
                    string[] allMathingFiles = Directory.GetFiles(directory, filenameNoExtension + ".*");
                    string[] allMathingExtentions = new string[allMathingFiles.Length];
                    for(int i =0;i<allMathingFiles.Length;i++)
                    {
                        allMathingExtentions[i] = System.IO.Path.GetExtension(allMathingFiles[i]);
                    }
                    if(allMathingExtentions.Contains(".shp")&&allMathingExtentions.Contains(".shx"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// read shape file
        /// </summary>
        /// <param name="pathToShapefile"></param>
        /// <returns></returns>
        public void ReadShpFile(string pathToShapefile)
        {
            string filepath = System.IO.Path.HasExtension(pathToShapefile) ? pathToShapefile.Substring(0, pathToShapefile.Length - (System.IO.Path.GetExtension(pathToShapefile).Length)) : pathToShapefile;
            string shxfilepath = filepath + ".shx";
            string shpfilepath = filepath + ".shp";
            if (!ValidPath(pathToShapefile))
            {
                Console.WriteLine("Missing shapefile components at " +pathToShapefile);
            }
            else
            {
                FileStream fs = new FileStream(shpfilepath, FileMode.Open, FileAccess.Read);
                BinaryReader binaryFile = new BinaryReader(fs);
                //打开二进制文件  
                binaryFile = new BinaryReader(fs);
                //先读出36个字节,紧接着是Box边界合  
                binaryFile.ReadBytes(24);
                FileLength = binaryFile.ReadInt32();
                Version = binaryFile.ReadInt32();
                ShapeType = binaryFile.ReadInt32();
                
                for(int i=0;i<4;i++)
                {
                    Box[i] = binaryFile.ReadDouble();
                }
                binaryFile.Close();
                fs.Close();
            }
        }

        public Type GetGemotryType()
        {
            if (ShapeType != 0)
            {
                string geometryType= Enum.GetName(typeof(ShapeTypes), ShapeType);
                Console.WriteLine(geometryType);
                //var geometryType = ShapeType.ToString();
                switch (geometryType)
                {
                    case "Point":
                        return typeof(Point);
                    case "Multipoint":
                        return typeof(Multipoint);
                    case "PolyLine":
                        return typeof(Polyline);
                    case "Polygon":
                        return typeof(Polygon);
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<int> GetParts(string shpfilepath)
        {
            FileStream fs = new FileStream(shpfilepath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryFile = new BinaryReader(fs);
            binaryFile.ReadBytes(144);
            int numParts = binaryFile.ReadInt32();
            List<int> parts = new List<int>();
            for (int i = 0; i < numParts; i++)
            {
                parts.Add(binaryFile.ReadInt32()) ;
            }
            return parts;
        }

        public List<Point> GetPoints(string shpfilepath)
        {
            FileStream fs = new FileStream(shpfilepath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryFile = new BinaryReader(fs);
            binaryFile.ReadBytes(148);
            int numPoints = binaryFile.ReadInt32();
            List<Point> points = new List<Point>() ;
            for (int i = 0; i < numPoints; i++)
            {
                Point tempoint = new Point();
                tempoint.X = binaryFile.ReadDouble();
                tempoint.Y = binaryFile.ReadDouble();
                points.Add(tempoint);
            }
            return points;
        }

        public object[] CollectionGemotry(string shpfilepath)
        {
            var geometryType = Enum.GetName(typeof(ShapeTypes), ShapeType);
            FileStream fs = new FileStream(shpfilepath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryFile = new BinaryReader(fs);
            binaryFile.ReadBytes(100);
            switch (geometryType)
            {
                //case "Point":
                    //List<Point> points = new List<Point>();
                    /*for (int i = 0; i < ShapeCount; i++)
                    {
                        Point point = new Point();
                        //记录头8个字节和一个int(4个字节)的shapetype
                        binaryFile.ReadBytes(12);
                        point.X = binaryFile.ReadDouble();
                        point.Y = binaryFile.ReadDouble();
                        points.Add(point);
                    }
                    return records;*/
                case "PolyLine":
                    List<Polyline> polylines = new List<Polyline>();
                    MainRecord mainrecord = new MainRecord();
                    for(int i=0;i<mainrecord.GetNumRecords(shpfilepath);i++)
                    {
                        binaryFile.ReadBytes(44);
                        Polyline polyline = new Polyline();
                        polyline.Parts = GetParts(shpfilepath);
                        polyline.Points = GetPoints(shpfilepath);
                        polylines.Add(polyline);
                    }
                    Polyline[] aa = new Polyline[polylines.Count];
                    polylines.CopyTo(aa);
                    return aa;
                case "Null":
                    return null;
                default:
                    throw new Exception("Unknown geometry type \"" + geometryType + "\" provided for shapefile  " );
            }
        }

    }
}