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
        /// if the file is vaild
        /// </summary>
        /// <param name="shapeFilePath"></param>
        /// <returns></returns>
        public bool ISValidPath(string shapeFilePath)
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
        /// read the file header
        /// </summary>
        /// <param name="pathToShapefile"></param>
        /// <returns></returns>
        public int ReadShpFile(string pathToShapefile)
        {
            string filepath = System.IO.Path.HasExtension(pathToShapefile) ? pathToShapefile.Substring(0, pathToShapefile.Length - (System.IO.Path.GetExtension(pathToShapefile).Length)) : pathToShapefile;
            string shpfilepath = filepath + ".shp";
            if (!ISValidPath(pathToShapefile))
            {
                Console.WriteLine("Missing shapefile components at " +pathToShapefile);
            }
            else
            {
                FileStream fs = new FileStream(shpfilepath, FileMode.Open, FileAccess.Read);
                BinaryReader binaryFile = new BinaryReader(fs);
                //open the   binary file
                binaryFile = new BinaryReader(fs);
                binaryFile.BaseStream.Seek(24, SeekOrigin.Current);
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
            return ShapeType;
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
                        return typeof(Polylgon);
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

        public object[] CollectionGemotry(string shpfilepath)
        {
            int shapetype = ReadShpFile(shpfilepath);
            var geometryType = Enum.GetName(typeof(ShapeTypes), shapetype);
            switch (geometryType)
            {
                case "Point":
                    Point points = new Point();
                    return points.CollectPoint(shpfilepath);
                case "PolyLine":
                    Polylgon polylines = new Polylgon();
                    return polylines.CollectPolyline(shpfilepath);
                case "Polygone":
                    Polygon polygon = new Polygon();
                    return polygon.CollectPolygon(shpfilepath);
                case "Null":
                    return null;
                default:
                    throw new Exception("Unknown geometry type \"" + geometryType + "\" provided for shapefile  " );
            }
        }

        public void CombineFile()
        {

        }

    }
}