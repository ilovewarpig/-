using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Data;


namespace WebApplication1
{
    public partial class showResult : System.Web.UI.Page
    {
        string url1 = "";
        string url2 = "";
        string url3 = "";
        // 读取数据库, 最终没有使用这个函数
        internal static string Lookup(string id, string table = "arduino_parts", string target = "weblink")
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

        protected void Page_Load(object sender, EventArgs e)
        {
            // 检查session
            if ((Session["pre"] != null) && (Session["pre2"] != null) && (Session["pre3"] != null))
            {
                
                string path = ".\\arduinoPics\\";
                
                // 输出三个预测结果
                Part part1 = (Part)Session["pre"];
                im1.Text = part1.get_name();
                ib1.ImageUrl = path + part1.get_subpath()+".png";
                lb1.Text = "匹配度: " + part1.get_prob().ToString() + "%";
                url1 = part1.get_url();

                Part part2 = (Part)Session["pre2"];
                im2.Text = part2.get_name();
                ib2.ImageUrl = path + part2.get_subpath() + ".png";
                lb2.Text = "匹配度: " + part2.get_prob().ToString() + "%";
                url2 = part2.get_url();

                Part part3 = (Part)Session["pre3"];
                im3.Text = part3.get_name();
                ib3.ImageUrl = path + part3.get_subpath() + ".png";
                lb3.Text = "匹配度: " + part3.get_prob().ToString() + "%";
                url3 = part3.get_url();
                
            }
            else
            {
                im1.Text = "fail";
                im3.Text = Session.Contents.Count.ToString();
                for (int i = 0; i < Session.Contents.Count; i++)
                {
                    Response.Write(Session.Keys[i] + "-" + Session[i] + "<br/>");
                }
                
            }
        }

        // 点击图片重定向到网络数字资源
        protected void ib1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(url1);
        }

        protected void ib2_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(url2);
        }

        protected void ib3_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(url3);
        }

        // 返回文件上传页面
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("WebForm1.aspx");
        }
    }
}
