using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenCvSharp;
using TensorFlow;
using NumSharp;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private string uploadDir;
        protected void Page_Load(object sender, EventArgs e)
        {
            uploadDir = Path.Combine(Request.PhysicalApplicationPath, "uploadFile");
        }

        public static TFTensor ImageToTensorGrayScale(string file)
        {
            using (System.Drawing.Bitmap image = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var matrix = new float[1, image.Size.Height, image.Size.Width, 1];
                for (var iy = 0; iy < image.Size.Height; iy++)
                {
                    for (int ix = 0, index = iy * image.Size.Width; ix < image.Size.Width; ix++, index++)
                    {
                        System.Drawing.Color pixel = image.GetPixel(ix, iy);
                        matrix[0, iy, ix, 0] = pixel.B / 255.0f;
                    }
                }
                TensorFlow.TFTensor tensor = matrix;
                return tensor;
            }
        }

        // 将原始图片裁剪为
        public void crop_rgb(string filename, string path= "C:/Users/ilovewarpig/source/repos/WebApplication1/WebApplication1/arduinoPics/", int newsize=150)
        {
            int start_x, start_y;
            string file = path + filename;

            Mat src = new Mat(@file, ImreadModes.Color);
            Mat dst = new Mat();
            int width = src.Size().Width;
            int height = src.Size().Height;
            // 截取图片中的正方形
            if (width > height)
            {
                start_x = (int)(width - height) / 2;
                start_y = 0;
                width = height;
            }
            else
            {
                start_y = (int)(height - width) / 2;
                start_x = 0;
                height = width;
            }

            Rect rectcrop = new Rect(start_x, start_y, width, height);
            Mat test = new Mat(src, rectcrop);
            // 图像去噪
            //Cv2.BilateralFilter(test, dst, 3, 50, 10); // 双向滤波
            // 增强对比度
            //CLAHE clahe = Cv2.CreateCLAHE();
            //clahe.SetClipLimit(2.0);  // 直方图均衡化
            //clahe.SetTilesGridSize(new Size(3, 3));
            //clahe.Apply(dst, dst);
            // 缩小至标准大小
            Cv2.Resize(test, dst, new Size(newsize, newsize));
            Cv2.ImShow("mach", dst);
            Cv2.WaitKey(0);
            Cv2.ImWrite(file, dst);

        }

        internal static int[] Quantized(float[,] results)
        {
            int[] q = new int[]
            {
        results[0,0]>0.5?1:0,
        results[0,1]>0.5?1:0,
        results[0,2]>0.5?1:0,
        results[0,3]>0.5?1:0,
        results[0,4]>0.5?1:0,
        results[0,5]>0.5?1:0,
        results[0,6]>0.5?1:0,
        results[0,7]>0.5?1:0,
        results[0,8]>0.5?1:0,
        results[0,9]>0.5?1:0,
            };
            return q;
        }

        internal static string Lookup(string id, string table= "arduino_parts", string target="weblink")
        {

            string url = "can not find id";
            try
            {
                string statment = string.Concat("select ", target, " from ", table, " where id = ", id);
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "server=DESKTOP-BT63T7T;database=learningsup;user=sa;pwd=0000";
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.Text;
                com.CommandText = statment;
                SqlDataAdapter sda = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    url = dr[target].ToString();
                }
                sda.Dispose();
                conn.Close();
            }
            catch
            {

            }
            finally
            {

            }
            return url;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int start_x, start_y;
            Mat src = new Mat(@"C:/Users/ilovewarpig/source/repos/WebApplication1/WebApplication1/bin/motor_motor_000030.jpg", ImreadModes.Grayscale);
            Mat dst = new Mat();
            int width = src.Size().Width;
            int height = src.Size().Height;

            // 截取图片中的正方形
            if (width > height)
            {
                start_x = (int)(width - height) / 2;
                start_y = 0;
                width = height;
            }
            else
            {
                start_y = (int)(height - width) / 2;
                start_x = 0;
                height = width;
            }

            Rect rectcrop = new Rect(start_x, start_y, width, height);
            Mat test = new Mat(src, rectcrop);
            // 图像去噪
            Cv2.BilateralFilter(test, dst, 3, 50, 10); // 双向滤波
            // 增强对比度
            CLAHE clahe = Cv2.CreateCLAHE();
            clahe.SetClipLimit(2.0);  // 直方图均衡化
            clahe.SetTilesGridSize(new Size(3, 3));
            clahe.Apply(dst, dst);
            // 缩小至标准大小
            Cv2.Resize(dst, dst, new Size(150, 150));
            //Cv2.ImShow("mach", dst);
            //Cv2.WaitKey(0);
            Cv2.ImWrite(@"C:/Users/ilovewarpig/source/repos/WebApplication1/WebApplication1/bin/motor.jpg", dst);
            

            byte[] buffer = System.IO.File.ReadAllBytes(@"C:/Users/ilovewarpig/source/repos/WebApplication1/WebApplication1/bin/arduino13.pb");
            Console.WriteLine("loaded");
            using (var graph = new TensorFlow.TFGraph())
            {
                graph.Import(buffer);
                float[][][][] outfloats;
                using (var session = new TensorFlow.TFSession(graph))
                {
                    Part part1 = new Part();
                    Part part2 = new Part();
                    Part part3 = new Part();

                    var file = @"C:/Users/ilovewarpig/source/repos/WebApplication1/WebApplication1/bin/motor.jpg";
                    var runner = session.GetRunner();
                    var tensor = ImageToTensorGrayScale(file);
                    runner.AddInput(graph["conv2d_input_1"][0], tensor);
                    runner.Fetch(graph["output_1"][0]);
                    

                    var output = runner.Run();
                    var vecResults = output[0].GetValue();
                    var result2 = new float[22];
                    output[0].GetValue(result2);
                    float[] a = new float[3];
                    a[0] = 0.1f;
                    a[1] = 0.2f;
                    a[2] = 0.3f;
                    var num = result2.Max();
                    int predict = Array.IndexOf(result2, num);
                    //Label1.Text = (result2[predict]*100).ToString() + " %";
                    
                    var cname = Lookup(id: predict.ToString(), target: "cn_name");
                    var url = Lookup(id: predict.ToString(), target: "weblink");
                    var subpath = Lookup(id: predict.ToString(), target: "name");
                    var prob = num * 100;
                    part1.set(url:url, id:predict.ToString(), subpath:subpath, prob:prob, name:cname);
                    Session["pre"] = part1;
                    
                    result2[predict] = 0;
                    num = result2.Max();
                    predict = Array.IndexOf(result2, num);
                    //Label2.Text = (result2[predict] * 100).ToString() + " %";
                    cname = Lookup(id: predict.ToString(), target: "cn_name");
                    url = Lookup(id: predict.ToString(), target: "weblink");
                    subpath = Lookup(id: predict.ToString(), target: "name");
                    prob = num * 100;
                    part2.set(url: url, id: predict.ToString(), subpath: subpath, prob: prob, name: cname);
                    Session["pre2"] = part2;                   
                    result2[predict] = 0;

                    num = result2.Max();
                    predict = Array.IndexOf(result2, num);
                    //Label3.Text = (result2[predict] * 100).ToString() + " %";
                    cname = Lookup(id: predict.ToString(), target: "cn_name");
                    url = Lookup(id: predict.ToString(), target: "weblink");
                    subpath = Lookup(id: predict.ToString(), target: "name");
                    prob = num * 100;
                    part3.set(url: url, id: predict.ToString(), subpath: subpath, prob: prob, name: cname);
                    Session["pre3"] = part3;
                    result2[predict] = 0;
                    

                    //Part part1 = new Part();
                    //part1.set(id: "1", name: "主板", subpath: "mainchip", prob: 0.95f * 100, url: "https://www.baidu.com");
                    //Session["pre"] = part1;
                    //Session["pre"] = 1;
                }
            }
            //Session["pre"] = 1;
            
            Response.Redirect("showResult.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //Label2.Text =  Lookup("6");
            Part part1 = new Part();
            part1.set(id: "1", name: "主板", subpath: "mainchip", prob: 0.95f*100, url:"https://www.baidu.com");
            Session["pre"] = part1;
            Response.Redirect("showResult.aspx");

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            crop_rgb(filename: "ultrasonic.png", newsize:300);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            string fullpath = "";
            if (FileUpload1.PostedFile.FileName == "")
            {
                alert.Text = "无文件上传";
            }
            else
            {
                string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                switch (extension.ToLower())
                {
                    case ".png":
                    case ".jpg":
                        break;
                    default:
                        alert.Text = "文件必须是png或者jpg格式";
                        return;
                }
                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);

                fullpath = Path.Combine(uploadDir, fileName);
                try
                {
                    FileUpload1.PostedFile.SaveAs(fullpath);
                    alert.Text = "文件"+ fileName + "成功上传! 请稍等";
                }
                catch(Exception ee)
                {
                    alert.Text = ee.Message;
                }
            }
            int start_x, start_y;
            Mat src = new Mat(fullpath, ImreadModes.Grayscale);
            Mat dst = new Mat();
            int width = src.Size().Width;
            int height = src.Size().Height;

            // 截取图片中的正方形
            if (width > height)
            {
                start_x = (int)(width - height) / 2;
                start_y = 0;
                width = height;
            }
            else
            {
                start_y = (int)(height - width) / 2;
                start_x = 0;
                height = width;
            }

            Rect rectcrop = new Rect(start_x, start_y, width, height);
            Mat test = new Mat(src, rectcrop);
            // 图像去噪
            Cv2.BilateralFilter(test, dst, 3, 50, 10); // 双向滤波
            // 增强对比度
            CLAHE clahe = Cv2.CreateCLAHE();
            clahe.SetClipLimit(2.0);  // 直方图均衡化
            clahe.SetTilesGridSize(new Size(3, 3));
            clahe.Apply(dst, dst);
            // 缩小至标准大小
            Cv2.Resize(dst, dst, new Size(150, 150));
            //Cv2.ImShow("mach", dst);
            //Cv2.WaitKey(0);
            Cv2.ImWrite(@"C:/Users/ilovewarpig/source/repos/WebApplication1/WebApplication1/bin/temp.jpg", dst);


            byte[] buffer = System.IO.File.ReadAllBytes(@"C:/Users/ilovewarpig/source/repos/WebApplication1/WebApplication1/bin/arduino13.pb");
            Console.WriteLine("loaded");
            using (var graph = new TensorFlow.TFGraph())
            {
                graph.Import(buffer);
                float[][][][] outfloats;
                using (var session = new TensorFlow.TFSession(graph))
                {
                    Part part1 = new Part();
                    Part part2 = new Part();
                    Part part3 = new Part();

                    var file = @"C:/Users/ilovewarpig/source/repos/WebApplication1/WebApplication1/bin/temp.jpg";
                    var runner = session.GetRunner();
                    var tensor = ImageToTensorGrayScale(file);
                    runner.AddInput(graph["conv2d_input_1"][0], tensor);
                    runner.Fetch(graph["output_1"][0]);


                    var output = runner.Run();
                    var vecResults = output[0].GetValue();
                    var result2 = new float[22];
                    output[0].GetValue(result2);
                    float[] a = new float[3];
                    a[0] = 0.1f;
                    a[1] = 0.2f;
                    a[2] = 0.3f;
                    var num = result2.Max();
                    int predict = Array.IndexOf(result2, num);
                    //Label1.Text = (result2[predict]*100).ToString() + " %";

                    var cname = Lookup(id: predict.ToString(), target: "cn_name");
                    var url = Lookup(id: predict.ToString(), target: "weblink");
                    var subpath = Lookup(id: predict.ToString(), target: "name");
                    var prob = num * 100;
                    part1.set(url: url, id: predict.ToString(), subpath: subpath, prob: prob, name: cname);
                    Session["pre"] = part1;

                    result2[predict] = 0;
                    num = result2.Max();
                    predict = Array.IndexOf(result2, num);
                    //Label2.Text = (result2[predict] * 100).ToString() + " %";
                    cname = Lookup(id: predict.ToString(), target: "cn_name");
                    url = Lookup(id: predict.ToString(), target: "weblink");
                    subpath = Lookup(id: predict.ToString(), target: "name");
                    prob = num * 100;
                    part2.set(url: url, id: predict.ToString(), subpath: subpath, prob: prob, name: cname);
                    Session["pre2"] = part2;
                    result2[predict] = 0;

                    num = result2.Max();
                    predict = Array.IndexOf(result2, num);
                    //Label3.Text = (result2[predict] * 100).ToString() + " %";
                    cname = Lookup(id: predict.ToString(), target: "cn_name");
                    url = Lookup(id: predict.ToString(), target: "weblink");
                    subpath = Lookup(id: predict.ToString(), target: "name");
                    prob = num * 100;
                    part3.set(url: url, id: predict.ToString(), subpath: subpath, prob: prob, name: cname);
                    Session["pre3"] = part3;
                    result2[predict] = 0;


                    //Part part1 = new Part();
                    //part1.set(id: "1", name: "主板", subpath: "mainchip", prob: 0.95f * 100, url: "https://www.baidu.com");
                    //Session["pre"] = part1;
                    //Session["pre"] = 1;
                }
            }
            //Session["pre"] = 1;

            Response.Redirect("showResult.aspx");

        }
    }
}