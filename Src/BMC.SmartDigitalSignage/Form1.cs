using BMC.SmartDigitalSignage.Properties;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMC.SmartDigitalSignage
{
    public partial class Form1 : Form
    {
        static int DetectorInterval;
        static int SlideInterval;
        Dictionary<int, int> ImageCounter;
        List<DetectionResult> Datas;
        List<string> ImagePath;
        string AgeStr = "";
        string GenderStr = "";
        private Mat _frame;
        private VideoCapture _capture = null;
        DetectAgeGender detector;
        public Form1()
        {
            InitializeComponent();

            Setup();
        }
        void Setup()
        {
            try
            {
                SlideInterval = int.Parse(ConfigurationManager.AppSettings["SlideInterval"]);
                DetectorInterval = int.Parse(ConfigurationManager.AppSettings["DetectorInterval"]);
                TimerDetector.Interval = DetectorInterval;
                SlideTimer.Interval = SlideInterval;
                if (ImagePath == null) ImagePath = new List<string>();
                if (ImageCounter == null) ImageCounter = new Dictionary<int, int>();
                var mapFile = ConfigurationManager.AppSettings["MappingFile"];
                if (File.Exists(mapFile))
                {
                    var AllLines = File.ReadAllLines(mapFile);
                    foreach(var line in AllLines)
                    {
                        var path = line.Split(',')[1];
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                            Directory.CreateDirectory(path+"\\Male");
                            Directory.CreateDirectory(path + "\\Female");
                        }
                        ImagePath.Add(path);
                    }
                }
               
                if (Datas == null) Datas = new List<DetectionResult>();
                _frame = new Mat();
                detector = new DetectAgeGender();
                _capture = new VideoCapture();
                _capture.ImageGrabbed += _capture_ImageGrabbed;
                _capture.Start();
                TimerDetector.Start();
                SlideTimer.Start();
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }

        }

        void Detect()
        {
            Datas.Clear();
            IImage image = _frame;

            long detectionTime;
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();
            var RootPath  = System.IO.Path.GetDirectoryName(Application.ExecutablePath); 
            DetectFace.Detect(
              image, RootPath+@"\\haarcascade_frontalface_default.xml",RootPath+ @"\\haarcascade_eye.xml",
              faces, eyes,
              out detectionTime);

            foreach (Rectangle face in faces)
            {
                CvInvoke.Rectangle(image, face, new Bgr(Color.Red).MCvScalar, 2);
                //crop face
                Image<Bgr, Byte> myImage = new Image<Bgr, Byte>(image.Bitmap);
                myImage.ROI = face;
                var croppedFace = myImage.Copy();
                //croppedFace = croppedFace.Resize(227, 227, Inter.Linear);
                //Image<Bgr, byte> croppedFace2 = null;
                //CvInvoke.CvtColor(croppedFace, croppedFace, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray);
                //var croppedFace2 = croppedFace.Convert<Gray, Byte>();
                int AgeIndex=-1,GenderIndex=-1;
                (AgeIndex, AgeStr) = detector.clsImg(croppedFace, Resources.deploy_age, Resources.age_net, Resources.age);//@"C:\experiment\Training\ujicoba\deploy_age.prototxt", @"C:\experiment\Training\ujicoba\age_net.caffemodel", @"C:\experiment\Training\ujicoba\age.txt");
                (GenderIndex, GenderStr) = detector.clsImg(croppedFace, Resources.deploy_gender, Resources.gender_net, Resources.gender);//@"C:\experiment\Training\ujicoba\deploy_gender.prototxt", @"C:\experiment\Training\ujicoba\gender_net.caffemodel", @"C:\experiment\Training\ujicoba\gender.txt");
                CvInvoke.Rectangle(image, face, new Bgr(Color.LightYellow).MCvScalar, 2);
                Datas.Add(new DetectionResult() { FaceRect = face, AgeIndex = AgeIndex, AgeDesc = AgeStr, GenderDesc = GenderStr, GenderIndex = GenderIndex });
            }
            foreach (Rectangle eye in eyes)
                CvInvoke.Rectangle(image, eye, new Bgr(Color.Blue).MCvScalar, 2);
            /*
            //display the image 
            using (InputArray iaImage = image.GetInputArray())
                ImageViewer.Show(image, String.Format(
                   "Completed face and eye detection using {0} in {1} milliseconds",
                   (iaImage.Kind == InputArray.Type.CudaGpuMat && CudaInvoke.HasCuda) ? "CUDA" :
                   (iaImage.IsUMat && CvInvoke.UseOpenCL) ? "OpenCL"
                   : "CPU",
                   detectionTime));
            */
            using (InputArray iaImage = image.GetInputArray())
            {
                Invoke(new Action(() =>
                {
                    DetectedFacePic.Image = image.Bitmap;
                    StatusLbl.Text = $"Age : {AgeStr}, Gender :{GenderStr} => " + String.Format(
                           "Completed face and eye detection using {0} in {1} milliseconds",
                           (iaImage.Kind == InputArray.Type.CudaGpuMat && CudaInvoke.HasCuda) ? "CUDA" :
                           (iaImage.IsUMat && CvInvoke.UseOpenCL) ? "OpenCL"
                           : "CPU",
                           detectionTime);
                }));

            }
        }

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);

                //DetectedFacePic.Image = _frame.Bitmap;



            }
        }

      
        private void TimerDetector_Tick(object sender, EventArgs e)
        {
            Detect();
        }

        private void SlideTimer_Tick(object sender, EventArgs e)
        {
            if (Datas == null || Datas.Count<=0) return;
            bool IsRandom = true;
            foreach(var item in Datas)
            {
                var pathStr = ImagePath[item.AgeIndex] + (item.GenderIndex == 0 ? "\\Male" : "\\Female");
                var ImagesList = Directory.GetFiles(pathStr, "*.*", SearchOption.AllDirectories)
                            .Where(s => s.EndsWith(".png") || s.EndsWith(".bmp") || s.EndsWith(".jpg") || s.EndsWith(".gif")).ToList();
                if (ImagesList.Count > 0)
                {
                    if (!ImageCounter.ContainsKey(item.AgeIndex))
                    {
                        ImageCounter.Add(item.AgeIndex, 0);
                    }
                    else
                    {
                        if (ImageCounter[item.AgeIndex]+1 >= ImagesList.Count)
                        {
                            ImageCounter[item.AgeIndex] = 0;
                        }
                        else
                        {
                            ImageCounter[item.AgeIndex] = ImageCounter[item.AgeIndex] + 1;
                        }
                    }
                    SlidePicture.Image = Image.FromFile(ImagesList[ImageCounter[item.AgeIndex]]);
                    IsRandom = false;
                    break;
                }
            }
            if (IsRandom)
            {
                Random rnd = new Random(Environment.TickCount);
                while (true)
                {
                    var RootPath = ImagePath[rnd.Next(0, 7)];
                    var ImagesList = Directory.GetFiles(RootPath, "*.*", SearchOption.AllDirectories)
                               .Where(s => s.EndsWith(".png") || s.EndsWith(".bmp") || s.EndsWith(".jpg") || s.EndsWith(".gif")).ToList();
                    if (ImagesList.Count > 0)
                    {
                        var selImage = ImagesList[rnd.Next(0, ImagesList.Count - 1)];
                        SlidePicture.Image = Image.FromFile(selImage);
                        break;
                    }
                }
                
            }
        }
    }
}
