using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;        //System.IO.File判断文件是否存在

namespace ImageFormatConverter
{
    public partial class FormPictureShow : Form
    {
        public FormPictureShow()
        {
            InitializeComponent();
        }

        private void FormPictureShow_Load(object sender, EventArgs e)
        {

                int DeskWidth = Screen.PrimaryScreen.WorkingArea.Width;         //获取屏幕宽度
                int DeskHeight = Screen.PrimaryScreen.WorkingArea.Height;       //获取屏幕高度

                if (CodeString.imagewidth <= DeskWidth && CodeString.imageheight <= DeskHeight)
                {
                    this.Size = new Size(CodeString.imagewidth, CodeString.imageheight);//如果图片分辨率小于屏幕的设置窗体大小适应图片大小
                }
                else
                {
                    this.Size = new Size(DeskWidth, DeskHeight);             //如果图片分辨率大于屏幕的设置窗体大小适应屏幕大小
                }
                if (CodeString.tmp == 1)
                {
                    string filenametmp = "C:\\IFCtmp.bmp";                  //定义文件名变量filename，并赋值
                    pictureBox1.Image = Image.FromFile(filenametmp);
                }
                if (CodeString.tmp == 0)
                {
                    pictureBox1.Image = Image.FromFile(CodeString.imagepath);       //点击预览缩略图在pictureBox1中显示大图
                }

                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;                 //大图显示自动适应画布缩放
                this.CenterToScreen();                                          //窗体居中显示

                //this.BackColor = Color.Transparent;   //窗体背景不支持透明
                //if (CodeString.imagewidth <= DeskWidth && CodeString.imageheight <= DeskHeight)
                //{
                //    this.Size = new Size(CodeString.imagewidth, CodeString.imageheight);//如果图片分辨率小于屏幕的设置窗体大小适应图片大小
                //}
                //else if (pictureBox1.Width / DeskWidth > pictureBox1.Height / DeskHeight)  //pictureBox1.Width / this.Width ==pictureBox1.Height/ this.Height
                //{
                //    //this.Width = DeskWidth;
                //    //this.Height = pictureBox1.Height * DeskWidth/ pictureBox1.Width;             //如果图片分辨率大于屏幕的设置窗体大小适应屏幕大小
                //    this.Size = new Size(DeskWidth, pictureBox1.Height * this.Width / pictureBox1.Width);
                //}
                //else if (pictureBox1.Width / DeskWidth < pictureBox1.Height / DeskHeight)
                //{
                //    //this.Height = DeskHeight;
                //    //this.Width = pictureBox1.Width * DeskHeight / pictureBox1.Height;
                //    this.Size = new Size(pictureBox1.Width * this.Height / pictureBox1.Height, DeskHeight);
                //}


                pictureBox1.Left = 0;
                pictureBox1.Top = 0;
                pictureBox1.Width = this.Width;
                pictureBox1.Height = this.Height;

                string  ImgName = CodeString.imagepath.Substring(CodeString.imagepath.LastIndexOf("\\") + 1, CodeString.imagepath.Length - CodeString.imagepath.LastIndexOf("\\") - 1);                 //获取图片名称

                label1.Visible = true ;                                         //设置标签可视
                label1.Left =20;
                label1.Top = 20;                                                //设置标签在窗体中的位置
                label1.Font = new Font("黑体", 20);                             //设置标签字体效果
                label1.ForeColor = Color.Red;                                   //设置标签字体颜色
                label1.Text = "预览" + Environment.NewLine + ImgName + Environment.NewLine + CodeString.imagewidth.ToString() + "*" + CodeString.imageheight.ToString();
                                                                                //设置标签文本内容
                label1.Parent = pictureBox1;                                    //设置标签的父容器为图片框
                label1.BackColor = Color.Transparent;                           //设置标签背景透明（相对于父容器）
            }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //if (CodeString.tmp == 1)
            //{
            //    if (File.Exists("C:\\tmp.bmp"))                                       //判断目标文件是否存在，存在则运行
            //    {
            //        File.Delete("C:\\tmp.bmp");                   //存在就删除（为用户清除缓存）
            //    }
            //}
                    this.Close();                 //单击图片关闭预览窗口
        }
    }
}

