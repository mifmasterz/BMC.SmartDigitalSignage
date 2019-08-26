using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace BMC.SmartDigitalSignage
{
    public class DetectAgeGender
    {
        void getMaxClass(ref Mat probBlob, ref int classId, ref double classProb)
        {
            Mat probMat = probBlob.Reshape(1, 1); //reshape the blob to 1x1000 matrix
            Point classNumber = new Point();

            var tmp = new Point();
            double tmpdouble = 0;
            CvInvoke.MinMaxLoc(probMat, ref tmpdouble, ref classProb, ref tmp, ref classNumber);

            classId = classNumber.X;
        }

        private List<string> readClassNames(string TagList)
        {
            var classNames = Regex.Split(TagList, Environment.NewLine);//File.ReadAllLines(filename).ToList<string>();
            return classNames.ToList();
        }

        public (int index, string desc) clsImg(Image<Bgr, byte> img,byte[] protoFile,byte[] caffeModel,string classList)
        {
            var net = Emgu.CV.Dnn.DnnInvoke.ReadNetFromCaffe(protoFile, caffeModel);
            //Emgu.CV.Dnn.Net net = new Emgu.CV.Dnn.Net();
            //caffe.PopulateNet(net);

            //Emgu.CV.Dnn.Net net = Emgu.CV.Dnn.DnnInvoke.ReadNetFromCaffe("Text.prototxt", "Model.caffemodel");
            //THIS COMMAND CAN BE USED FOR THREE LINES AT THE TOP. For Emgucv 3.4.3 or later versions. Thanks to JacobC. for this statement. 

            Mat prob;
            Mat img_Mat = img.Mat;

            Size size = new Size(227, 227);
            //mean
            MCvScalar scalar = new MCvScalar(78.4263377603, 87.7689143744, 114.895847746);
            Mat blob = Emgu.CV.Dnn.DnnInvoke.BlobFromImage(img_Mat, 1, size, scalar,false);

            net.SetInput(blob);//"data"
            prob = net.Forward();//"prob"
            // var detection_ou = net.Forward("detection_out");
            // var concat = net.Forward("concat");

            int classId = 0;
            double classProb = 0;

            getMaxClass(ref prob, ref classId, ref classProb);

            var classNames = readClassNames(classList);

            var bestClass = classNames[classId];

            Console.WriteLine("Best: id:" + classId + ", val: " + bestClass);
            Console.WriteLine("Prob: " + classProb * 100 + "%");

            return (classId, bestClass);

        }
    }
}
