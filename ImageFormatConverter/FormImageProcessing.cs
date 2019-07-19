using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;        //添加System.Drawing.Imaging引用，用于画图
using System.IO;
using System.Text.RegularExpressions;  //判断输入为数字



namespace ImageFormatConverter
{
    public partial class FormImageProcessing : Form
    {
        public FormImageProcessing()
        {
            InitializeComponent();
        }
        public FormMain mainForm;                   //公共声明主窗口是FormMain
        Bitmap bitmap;

        string path;                  //用于选择的文件全路径
        string ImgName;               //原文件文件名
        string ImgType;               //源文件类型
        string filename;              //目标文件全路径(带扩展名)
        //int width, height;            //源文件的宽度和高度整型
        //string width1, height1;       //源文件的宽度和高度字符串类型
        //int width2, height2;           //目标文件的宽度和高度整型
        //string width3, height3;         //目标文件的宽度和高度字符串类型

        string folder;                              //目标文件夹
        Color fontcolor;                            //定义字体颜色变量
        Font fontfont;                              //定义字体变量
        //添加水印
        int x=20, y=20;         //水印文字位置坐标

        string fontname = "宋体";
        float fontsize = 50;
        //图像旋转
        Bitmap bmp1;

        //图像裁剪
        bool isClip = false;       //标识变量
        Rectangle rec = new Rectangle(new Point(0, 0), new Size(0, 0));      //定义矩形
        Point startp, endp;
        Graphics gra;
        //百叶窗显示图片
        Image myImage;

        int imagewidth,imageheight;   //图片长宽属性

        /******************************************************
        窗体初始化↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void FormImageProcessing_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(676, 468);                   //设定窗体大小最小值676*468
            pictureBox1.MinimumSize = new Size(308, 286);            //设定图片框1大小最小值308*286
            pictureBox4.MinimumSize = new Size(308, 286);            //设定图片框2大小最小值308*286

            switch (CodeString.tabpageNO )
            {
                case 1:
                    tabControl1.SelectedTab = tabPage1;
                    break;
                case 2:
                    tabControl1.SelectedTab = tabPage2;
                    break;
                case 3:
                    tabControl1.SelectedTab = tabPage3;
                    break;
                case 4:
                    tabControl1.SelectedTab = tabPage4;
                    break;
                case 5:
                    tabControl1.SelectedTab = tabPage5;
                    break;
                case 6:
                    tabControl1.SelectedTab = tabPage6;
                    break;
            }

            label1.Text = "原图：";
            label2.Text = "效果图：";

            tabControl1.Enabled = false;

            tabPage1.Text = "添加水印";
            tabPage2.Text = "旋转和翻转";
            tabPage3.Text = "裁剪";
            tabPage4.Text = "图片显示特效";
            tabPage5.Text = "图片处理特效";
            tabPage6.Text = "消隐";
            ////////添加水印////////
            label3.Text = "输入要添加的水印文字";
            button5.Text = "预览效果";
            button5.Enabled = false;
            button4.Text = "OK";
            button4.Enabled = false;
            pictureBox6.BackColor = Color.Red;
            label6.Text = "宋体,50";
            label7.Text = "(20,20)";
            ////////图像旋转///////////
            comboBox1.Text = "90度顺指针旋转";
            comboBox2.Text = "水平翻转";

            radioButton1.Checked = true;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            button6.Enabled = false;
            button8.Enabled = false;
            textBox6.Enabled = false;
            button7.Enabled = false;
        }

        /******************************************************
        添加源文件↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            添加源文件();
        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            添加源文件();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (isClip == false)
            {
                添加源文件();
            }
        }
        /******************************************************
        添加源文件（子程序）↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void 添加源文件()
        {
            openFileDialog1.Filter = "图像文件（*.*）|*.bmp;*.jpg;*.png;*.gif;*.tif;*.ico;*.emf;*.wmf|BMP文件（*.bmp）|*.bmp|JPG文件（*.jpg;*.jpeg）|*.jpg;*.jpeg|PNG文件（*.png）|*.png|GIF文件(*.gif)|*.gif|TIFF文件（*.tif;*.tiff）|*.tif;*.tiff|ICO格式(*.ico)|*.ico|EMF格式(*.emf)|*.emf|WMF格式(*.wmf)|*.wmf|所有文件(*.*)|*.*";           //文件过滤
            openFileDialog1.Title = "打开图像文件*.bmp;*.jpg;*.png;*.gif;*.tif;*.ico;*.emf;*.wmf";                      //打开文件对话框标题
            openFileDialog1.Multiselect = false;                         //多选为假，不可多选

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);       //打开文件并在pictureBox1中显示
                pictureBox4.Image = Image.FromFile(openFileDialog1.FileName);       //打开文件并在pictureBox4中显示

                tabControl1.Enabled = true ;

                textBox1.Text = openFileDialog1.FileName;                              //显示图片路径
                //////////添加水印\\\\\\\\\
                button5.Enabled = true ;
                button4.Enabled = true;
                //////////图像旋转\\\\\\\\\
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                button6.Enabled = true;
                button8.Enabled = true;
                textBox6.Enabled = false;
                button7.Enabled = false;
                //////////百叶窗显示图片\\\\\\\\\
                myImage = System.Drawing.Image.FromFile(openFileDialog1.FileName);	//根据文件的路径实例化Image类

                ///////////以下获取并显示源图片属性/////////////
                FileInfo fi;                                            //创建一个FileInfo对象，用于获取图片信息
                path = openFileDialog1.FileName;                      //获取选择的图片路径

                ImgName = path.Substring(path.LastIndexOf("\\") + 1, path.Length - path.LastIndexOf("\\") - 1);                 //获取图片名称
                ImgType = ImgName.Substring(ImgName.LastIndexOf(".") + 1, ImgName.Length - ImgName.LastIndexOf(".") - 1);             //获取图片类型
                fi = new FileInfo(path.ToString());             //实例化FileInfo对象

                string filedata = (fi.Length / 1024) + "KB";                //图片大小
                imagewidth = myImage.Width;
                imageheight = myImage.Height;
                string imagewidthstring = myImage.Width.ToString();          //显示原始尺寸
                string imageheightstring = myImage.Height.ToString();

                label1.Text = "原图：" + ImgName + "，" + imagewidthstring + "*" + imageheightstring + "，" + filedata;
            }
        }
        /******************************************************
        设置保存路径↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            设置保存路径();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            设置保存路径();
        }

        /******************************************************
        设置保存路径，目标文件保存路径（子程序）↓↓↓↓↓↓↓
        *******************************************************/
        private void 设置保存路径()
        {
            folderBrowserDialog1.Description = "选择保存路径";               //设置对话框窗体提示语
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folder = folderBrowserDialog1.SelectedPath;                  //选择保存路径，并赋给变量folder
                textBox2.Text = folder;　　　　　　　　　　　　　　　　　　　//显示保存路径
            }
        }

        /******************************************************
        保存↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button1_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox4.Image);                            //创建Bitmap对象
            if (textBox1.Text == "")
            {
                MessageBox.Show(" 请选择要转换的源图片！");              //如果没有选择图片，提示选择               
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show(" 请选择保存路径！");                     //如果没有选择图片，提示选择
            }
            switch (ImgType )
            {

                /*
                BMP格式(*.bmp)
                JPG格式(*.jpg)
                PNG格式(*.png)
                GIF格式(*.gif)
                ICO格式(*.ico)
                TIF格式(*.tif)
                EMF格式(*.emf)
                WMF格式(*.wmf)
                 */

                case "bmp":                                         //选择BMP格式(*.bmp)
                case "BMP": 
                    filename = textBox2 .Text  +"\\"+ "IFC_"+ImgName ;                  //定义文件名变量filename，并赋值
                    bitmap.Save(filename, ImageFormat.Bmp);                    //文件转换并保存
                    break;
                case "jpg":                                         //选择JPG格式(*.jpg)
                case "JPG":
                case "jpeg":
                case "JPEG":
                    filename = textBox2.Text + "\\" + "IFC_" + ImgName;                          //引用文件名变量filename，并赋值
                    bitmap.Save(filename, ImageFormat.Jpeg);
                    break;
                case "png":
                case "PNG":
                    filename = textBox2.Text + "\\" + "IFC_" + ImgName;
                    bitmap.Save(filename, ImageFormat.Png);
                    break;
                case "gif":                                         //选择GIF格式(*.gif)
                case "GIF":
                    filename = textBox2.Text + "\\" + "IFC_" + ImgName;
                    bitmap.Save(filename, ImageFormat.Gif);
                    break;
                case "tif":                                         //选择TIF格式(*.tif)
                case "TIF":
                case "tiff":
                case "TIFF":
                    filename = textBox2.Text + "\\" + "IFC_" + ImgName;
                    bitmap.Save(filename, ImageFormat.Tiff);
                    break;
                case "ico":                                         //选择ICO格式(*.ico)
                case "ICO":
                    filename = textBox2.Text + "\\" + "IFC_" + ImgName;
                    bitmap.Save(filename, ImageFormat.Icon);
                    break;
                case "emf":                                         //选择EMF格式(*.emf)
                case "EMF":
                    filename = textBox2 .Text  +"\\"+ "IFC_"+ImgName ;
                    bitmap.Save(filename, ImageFormat.Emf);
                    break;
                case "wmf":                                         //选择WMF格式(*.wmf)
                case "WMF":
                    filename = textBox2.Text + "\\" + "IFC_" + ImgName;
                    bitmap.Save(filename, ImageFormat.Wmf);
                    break;
                default:
                    break;

            }
        }

        /******************************************************
        取消↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /******************************************************
        窗体大小发生变化时，图片框也随之变化↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void FormImageProcessing_SizeChanged(object sender, EventArgs e)
        {
            int newwidth = this.Width;                               //获取窗口新的宽度
            int newheight = this.Height;                             //获取窗口新的高度
            this.MinimumSize = new Size(676, 468);                   //设定窗体大小最小值676*468
            pictureBox1.MinimumSize = new Size(308, 286);            //设定图片框1大小最小值308*286
            pictureBox1.Width = 308 + (newwidth - 676)/2;              //设置图片框的宽度
            pictureBox1.Height = 286 + (newheight - 468);            //设置图片框的高度
            pictureBox4.Left = pictureBox1.Left + pictureBox1.Width + 20;
            pictureBox4.Width = 308 + (newwidth - 676) / 2;              //设置图片框的宽度
            pictureBox4.Height = 286 + (newheight - 468);            //设置图片框的高度

            label2 .Left =pictureBox4.Left+36;

            label12.Top = 0;
            label12.Left = 0;
            label12.Width = 338 + pictureBox1.Width - 308;
            label12.Height = 324 + pictureBox1.Height - 286;

            //以下代码保证添加水印编辑部分组件居中
            pictureBox2.Top = 326 + newheight - 468;
            pictureBox3.Top = 353 + newheight - 468;
            textBox1.Top = 329 + newheight - 468;
            textBox2.Top = 356 + newheight - 468;
            button1.Top = 397 + newheight - 468;
            button3.Top = 397 + newheight - 468;
            tabControl1.Top = 326 + newheight - 468;

            pictureBox2.Left = 15 + (newwidth - 676) / 2;
            pictureBox3.Left = 15 + (newwidth - 676) / 2;
            textBox1.Left = 48 + (newwidth - 676) / 2;
            textBox2.Left = 48 + (newwidth - 676) / 2;
            button1.Left = 48 + (newwidth - 676) / 2;
            button3.Left = 146 + (newwidth - 676) / 2;
            tabControl1.Left = 235 + (newwidth - 676) / 2;
        }
//////////////////////////添加水印\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        /******************************************************
        添加水印预览效果↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button5_Click(object sender, EventArgs e)
        {
            水印预览效果();
        }

        private void 水印预览效果()
        {
            重置图像();
           if (textBox3.Text != "")
            {
                Bitmap bmp = new Bitmap(pictureBox4.Image);//创建Bitmap对象
                Graphics g = Graphics.FromImage(bmp);//创建画板

                //SolidBrush brush = new SolidBrush(pictureBox6.BackColor);//创建笔刷
                int n;
                if (int.TryParse(textBox7.Text, out n))          //判断输入角度是否为数字，是否有效，有效则。。。。
                {
                    int alpha0 = int.Parse(textBox7.Text);
                    if (alpha0 > 100)
                    {
                        alpha0 = 100;
                        textBox7.Text = "100";
                    }
                    else if (alpha0 < 0)
                    {
                        alpha0 = 0;
                        textBox7.Text = "0";
                    }
                    int alpha = alpha0 * 255 / 100;               //透明度alpha值0<alpha<255
                    SolidBrush brush = new SolidBrush(Color.FromArgb(alpha, pictureBox6.BackColor));//创建笔刷
                    // Pen mypen = new Pen(Color.FromArgb(alpal, R, G, B), width);

                    PointF p = new PointF(x, y);//位置
                    Font font;
                    font = new Font(fontname, fontsize);//设置后的字体

                    g.DrawString(textBox3.Text, font, brush, p);//绘制文字
                    pictureBox4.Image = (Bitmap)bmp.Clone();//显示

                    font.Dispose();
                    g.Dispose();
                }
                else                                          //判断输入角度是否为数字，是否有效，无效则。。。。
                {
                    MessageBox.Show("alpha输入有误！请重新输入！");
                    textBox7.Text = "100";          //显示原alpha
                }
            }
            else
            {
                MessageBox.Show("请输入水印文字！");
            }
        }
        /******************************************************
        添加水印文字颜色↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void label5_Click(object sender, EventArgs e)
        {

            字体颜色();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            字体颜色();
        }
        //添加水印文字颜色(子程序)
        private void 字体颜色()
        {
            colorDialog1.ShowDialog();                          //打开颜色多画框
            fontcolor = this.colorDialog1.Color;          //
            pictureBox6.BackColor = fontcolor;
        }
        /******************************************************
        添加水印文字字体↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void label4_Click(object sender, EventArgs e)
        {
            字体设置();
        }


        private void label6_Click(object sender, EventArgs e)
        {
            字体设置();
        }

        private void 字体设置()
        {
            if (this.fontDialog1.ShowDialog() == DialogResult.OK)
            {
                fontfont = this.fontDialog1.Font;
                label6.Text = fontfont.Name + "," + fontfont.Size.ToString();
                                                            //获取并显示字体名称和字号大小
                fontname = fontfont.Name;
                fontsize = fontfont.Size;
            }
        }
        /******************************************************
        添加水印文字位置↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "" && textBox5.Text != "")
            {
                string xstr = textBox4.Text;
                x = int.Parse(xstr);           //字符串型转整型
                string ystr = textBox5.Text;
                y = int.Parse(ystr);           //字符串型转整型
                label7.Text = "("+xstr +","+ystr +")";
                水印预览效果();
                //MessageBox.Show("水印文字的位置被修改为（" + xstr + "," + ystr + "）");
            }
            else
            {
                MessageBox.Show("请输入有效坐标！");
            }
        }

//////////////////////////图像旋转\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        /******************************************************
        图像旋转↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button6_Click(object sender, EventArgs e)
        {
            bmp1 = new Bitmap(pictureBox4.Image);//使用图片创建Bitmap对象

            /*
            90度顺指针旋转Rotate90FlipNone
            180度顺指针旋转Rotate180FlipNone
            270度顺指针旋转Rotate270FlipNone
             */
            if (comboBox1.Text == "90度顺指针旋转" || comboBox1.Text == "180度顺指针旋转" || comboBox1.Text == "270度顺指针旋转")
            {
                switch (comboBox1.SelectedItem.ToString())
                {
                    case "90度顺指针旋转":
                        bmp1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case "180度顺指针旋转":
                        bmp1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case "270度顺指针旋转":
                        bmp1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
                pictureBox4.Image = bmp1;
            }
            else
                MessageBox.Show("不要随意修改【旋转角度】列表内容！！！");
        }
        /******************************************************
        图像翻转↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button8_Click(object sender, EventArgs e)
        {
            bmp1 = new Bitmap(pictureBox4.Image);//使用图片创建Bitmap对象
            /*
            RotateNoneFlipX水平翻转
            RotateNoneFlipY垂直翻转
            RotateNoneFlipXY水平翻转+垂直翻转
             */
            if (comboBox2.Text == "水平翻转" || comboBox2.Text == "垂直翻转" || comboBox2.Text == "水平翻转+垂直翻转")
            {
                switch (comboBox2.SelectedItem.ToString())
                {
                    case "水平翻转":
                        bmp1.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case "垂直翻转":
                        bmp1.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        break;
                    case "水平翻转+垂直翻转":
                        bmp1.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                        break;
                }
                pictureBox4.Image = bmp1;
            }
            else
                MessageBox.Show("不要随意修改【翻转角度】列表内容！！！");
        }
        /******************************************************
        一般角度旋转↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button7_Click(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox6.Text, out n))          //判断输入角度是否为数字，是否有效，有效则。。。。
            {
                label15.Text = "";
                //调用图片旋转子函数，按任意角度旋转picturebox1中图片，并在picturebox4中显示出来
                RotateImg(System.Drawing.Image.FromFile(openFileDialog1.FileName), int.Parse(textBox6.Text));
            }
            else                                          //判断输入角度是否为数字，是否有效，无效则。。。。
                label15.Text ="角度输入有误！";
        }
        /// <summary>
        /// 图片旋转子函数
        /// </summary>
        /// <param name="b">源图片</param>
        /// <param name="angle">角度</param>
        /// <returns>（无返回值）</returns>
　　　　public Image RotateImg(Image b, int angle)
　　　　   {
　　            angle = angle % 360;
　　            //弧度转换
　　            double radian = angle *Math.PI / 180.0;
　　            double cos = Math.Cos(radian);
　　            double sin = Math.Sin(radian);
　　            //原图的宽和高
　　            int w = b.Width;
　　            int h = b.Height;
　　            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin),Math.Abs(w * cos + h * sin)));
　　            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos),Math.Abs(w * sin + h * cos)));
　　            //目标位图
　　            Bitmap dsImage = new Bitmap(W, H);
　　            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);
　　            g.InterpolationMode =System.Drawing.Drawing2D.InterpolationMode.Bilinear;
　　            g.SmoothingMode =System.Drawing.Drawing2D.SmoothingMode.HighQuality;
　　            //计算偏移量
　　            Point Offset = new Point((W - w) / 2, (H - h) / 2);
　　            //构造图像显示区域：让图像的中心与窗口的中心点一致
　　            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
　　            Point center = new Point(rect.X + rect.Width / 2, rect.Y +rect.Height / 2);
　　            g.TranslateTransform(center.X, center.Y);
　　            g.RotateTransform(360 -angle);
　　            //恢复图像在水平和垂直方向的平移
　　            g.TranslateTransform(-center.X, -center.Y);
　　            g.DrawImage(b, rect);
　　            //重至绘图的所有变换
　　            g.ResetTransform();
　　            g.Save();
　　            g.Dispose();
　　            //显示旋转后的图片
                b.Dispose();
                pictureBox4.Image = dsImage;
                return dsImage;
　　        }

        /******************************************************
        特殊角度旋转、翻转组件使能↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                button6.Enabled = true;
                button8.Enabled =true;
                textBox6.Enabled = false;
                button7.Enabled = false;
            }
        }
        /******************************************************
        一般角度旋转组件使能↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                comboBox1.Enabled = false ;
                comboBox2.Enabled = false ;
                button6.Enabled = false ;
                button8.Enabled = false ;
                textBox6.Enabled = true ;
                button7.Enabled = true ;
            }
        }
        /******************************************************
        重置↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button9_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = pictureBox1.Image;
        }
//////////////////////////图像裁剪\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        /******************************************************
        图像裁剪↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        ////////////开始\\\\\\\\\\\
        private void button2_Click_1(object sender, EventArgs e)
        {
            isClip = true;                                        //打开截图标识
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Cursor = Cursors.Cross;                   //更改鼠标样式为Cross
        }
        ////////鼠标左键点下\\\\\\\\
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage3)
            {
                if (e.Button == MouseButtons.Left)                              //鼠标左键
                {
                    if (isClip == true)                             //是否开始操作
                    {
                        startp = new Point(e.X, e.Y);
                        endp = new Point(e.X, e.Y);
                    }

                }
            }
        }
        //////////鼠标移动\\\\\\\\\\
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage3)
            {
                Graphics g = this.CreateGraphics();                     //画板
                Pen mypen = new Pen(Color.Black, 1);                      //画笔
                if (isClip == true)
                {
                    g.DrawRectangle(mypen, startp.X, startp.Y, e.X - startp.X, e.Y - startp.Y);         //绘制矩形区域
                }
            }
        }
        ////////鼠标左键释放\\\\\\\\
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage3)
            {
                isClip = false;                                                     //关闭标识
                pictureBox1.Cursor = Cursors.Hand;                                  //更改鼠标样式为Hand
                pictureBox4.Image = null;
                gra = pictureBox1.CreateGraphics();                                 //画板
                gra.DrawRectangle(new Pen(Color.Black, 1), startp.X, startp.Y, e.X - startp.X, e.Y - startp.Y);
                rec = new Rectangle(startp.X, startp.Y, e.X - startp.X, e.Y - startp.Y);        //创建矩形
            }
        }
        ////////////裁剪\\\\\\\\\\\
        private void button10_Click(object sender, EventArgs e)
        {
            gra = pictureBox4.CreateGraphics();                         //画板
            Bitmap bmp = new Bitmap(pictureBox1.Image);                 //图像
            Bitmap bmpcopy = bmp.Clone(rec, PixelFormat.DontCare);      //复制图像
            gra.DrawImage(bmpcopy, rec);                                //绘制图像 
            pictureBox4.Image = bmpcopy;                                //显示裁剪图
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom ;
        }

        /******************************************************
        百叶窗显示图片↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button11_Click(object sender, EventArgs e)
        {

            try
            {
                Bitmap myBitmap = (Bitmap)this.pictureBox1.Image.Clone();		//用pictureBox1的复本实例化Bitmap类
                int intWidth = myBitmap.Width;							        //记录图片的宽度
                int intHeight = myBitmap.Height / 20;				    		//记录图片的指定高度
                Graphics myGraphics = this.CreateGraphics();	    			//创建窗体的Graphics类
                myGraphics.Clear(Color.WhiteSmoke);				        		//用指定的颜色清除窗体背景
                Point[] myPoint = new Point[30];				    			//定义数组
                for (int i = 0; i < 30; i++)									//记录百叶窗各节点的位置
                {
                    myPoint[i].X = 0;
                    myPoint[i].Y = i * intHeight;
                }
                Bitmap bitmap = new Bitmap(myBitmap.Width, myBitmap.Height);	//实例化Bitmap类
                //通过调用Bitmap对象的SetPixel方法重新设置图像的像素点颜色，从而实现百叶窗效果
                for (int m = 0; m < intHeight; m++)
                {
                    for (int n = 0; n < 20; n++)
                    {
                        for (int j = 0; j < intWidth; j++)
                        {
                            bitmap.SetPixel(myPoint[n].X + j, myPoint[n].Y + m, myBitmap.GetPixel(myPoint[n].X + j, myPoint[n].Y + m));//获取当前象素颜色值
                        }
                    }
                    this.Refresh();								        	//绘制无效
                    this.pictureBox4.Image = bitmap;						//显示百叶窗体的效果
                    System.Threading.Thread.Sleep(100);						//线程挂起
                }
            }
            catch { }
        }

        /******************************************************
        图像拉幕效果↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/

        ////////////由上到下\\\\\\\\\\\
        private void button12_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = null;
            try
            {
                Bitmap myBitmap = (Bitmap)this.pictureBox1.Image.Clone();		//用pictureBox1的复本实例化Bitmap类
                int intWidth = myBitmap.Width;						        	//记录图片的宽度
                int intHeight = myBitmap.Height;					        	//记录图片的指定高度

                Bitmap bitmap = new Bitmap(myBitmap.Width, myBitmap.Height);	//实例化Bitmap类
                //通过调用Bitmap对象的SetPixel方法重新设置图像的像素点颜色，从而实现效果
                for (int j =0; j < intHeight; j++)
                {
                        for (int i = 0; i < intWidth; i++)
                        {
                            bitmap.SetPixel( i,  j, myBitmap.GetPixel( i,  j));//获取当前象素颜色值
                        }
                    this.Refresh();									        //绘制无效
                    this.pictureBox4.Image = bitmap;						//显示效果
                    System.Threading.Thread.Sleep(2);						//线程挂起
                }
            }
            catch { }
            pictureBox4.Image = pictureBox1.Image;
        }
        ////////////由下到上\\\\\\\\\\\
        private void button15_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = null;

                Bitmap myBitmap = (Bitmap)this.pictureBox1.Image.Clone();		//用pictureBox1的复本实例化Bitmap类
                int intWidth = myBitmap.Width;						        	//记录图片的宽度
                int intHeight = myBitmap.Height;					        	//记录图片的指定高度

                Bitmap bitmap = new Bitmap(myBitmap.Width, myBitmap.Height);	//实例化Bitmap类
                //通过调用Bitmap对象的SetPixel方法重新设置图像的像素点颜色，从而实现效果
                for (int j = intHeight-1; j > 0; j--)
                {
                    for (int i = 0; i < intWidth; i++)
                    {
                        bitmap.SetPixel( i, j, myBitmap.GetPixel(i,j));//获取当前象素颜色值
                    }
                    this.Refresh();									        //绘制无效
                    this.pictureBox4.Image = bitmap;						//显示效果
                    System.Threading.Thread.Sleep(2);						//线程挂起
                }
            pictureBox4.Image = pictureBox1.Image;
        }
        ////////////由左到右\\\\\\\\\\\
        private void button13_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = null;

                Bitmap myBitmap = (Bitmap)this.pictureBox1.Image.Clone();		//用pictureBox1的复本实例化Bitmap类
                int intWidth = myBitmap.Width;						        	//记录图片的宽度
                int intHeight = myBitmap.Height;					        	//记录图片的高度

                Bitmap bitmap = new Bitmap(myBitmap.Width, myBitmap.Height);	//实例化Bitmap类
                //通过调用Bitmap对象的SetPixel方法重新设置图像的像素点颜色，从而实现效果
                for (int i = 0;i<intWidth; i++)
                {
                    for (int j = 0; j < intHeight; j++)
                    {
                        bitmap.SetPixel(i, j, myBitmap.GetPixel(i, j));//获取当前象素颜色值
                    }
                    this.Refresh();									        //绘制无效
                    this.pictureBox4.Image = bitmap;						//显示效果
                    System.Threading.Thread.Sleep(2);						//线程挂起
                }
            pictureBox4.Image = pictureBox1.Image;

        }
        ////////////由右到左\\\\\\\\\\\
        private void button16_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = null;

            Bitmap myBitmap = (Bitmap)this.pictureBox1.Image.Clone();		//用pictureBox1的复本实例化Bitmap类
            int intWidth = myBitmap.Width;						        	//记录图片的宽度
            int intHeight = myBitmap.Height;					        	//记录图片的指定高度

            Bitmap bitmap = new Bitmap(myBitmap.Width, myBitmap.Height);	//实例化Bitmap类
            //通过调用Bitmap对象的SetPixel方法重新设置图像的像素点颜色，从而实现效果
            for (int i = intWidth  - 1; i> 0; i--)
            {
                for (int j = 0; j < intHeight ; j++)
                {
                    bitmap.SetPixel(i, j, myBitmap.GetPixel(i, j));//获取当前象素颜色值
                }
                this.Refresh();									        //绘制无效
                this.pictureBox4.Image = bitmap;						//显示效果
                System.Threading.Thread.Sleep(2);						//线程挂起
            }
            pictureBox4.Image = pictureBox1.Image;
        }
        /******************************************************
        窗体被关闭时返回主窗体↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void FormImageProcessing_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Visible = true;           //FormMain显示代码（一般在子窗口关闭前执行）
        }
        /******************************************************
        反相处理↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button18_Click(object sender, EventArgs e)
        {
            Bitmap bmp1 = new Bitmap(pictureBox4.Image);       //创建Bitmap对象
            Bitmap bmp2 = (Bitmap)bmp1.Clone();                //创建Bitmap对象
            Color color = new Color();                         //色彩
            label11.Text = "正在处理，请稍侯...";              //如果像素数太大，处理时间太长，会出现假死状态
            
            int fbl = bmp1.Width * bmp1.Height;
            if (fbl > 1049088)     //1366*768
            {
                MessageBox.Show("源图片像素数太大，处理时间会比较长，出现假死状态，稍等即好。点击【确定】继续...");
            }
            //遍历像素，反色处理
            for (int i = 0; i < bmp1.Width; i++)
            {
                for (int j = 0; j < bmp1.Height; j++)
                {
                    color = bmp1.GetPixel(i, j);

                    int r = 255 - color.R;
                    int g = 255 - color.G;
                    int b = 255 - color.B;
                    Color newcolor = Color.FromArgb(r, g, b);
                    bmp2.SetPixel(i, j, newcolor);              //设置像素色彩信息
                }
                pictureBox4.Image = bmp2;                       //显示图像
            }
            label11.Text = "处理完成！";
        }
        /******************************************************
        黑白处理↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button19_Click(object sender, EventArgs e)
        {
            Bitmap Bitmap1 = new Bitmap(this.pictureBox4.Image);//创建Bitmap对象
            Bitmap Bitmap2 = new Bitmap(pictureBox4.Image);

            int Height = Bitmap2.Height;//图片高
            int Width = Bitmap2.Width;//图片宽

            Color color;
            int r, g, b, Result = 0;

            label11.Text = "正在处理，请稍侯...";              //如果像素数太大，处理时间太长，会出现假死状态

            int fbl = Bitmap1.Width * Bitmap1.Height;
            if (fbl > 1049088)     //1366*768
            {
                MessageBox.Show("源图片像素数太大，处理时间会比较长，出现假死状态，稍等即好。点击【确定】继续...");
            }
            //遍历像素，重新设置颜色值
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    color = Bitmap1.GetPixel(i, j);

                    r = color.R;
                    g = color.G;
                    b = color.B;
                    //实例程序以加权平均值法产生黑白图像

                    Result = ((int)(0.7 * r) + (int)(0.2 * g) + (int)(0.1 * b));
                    Bitmap2.SetPixel(i, j, Color.FromArgb(Result, Result, Result));

                }
                this.pictureBox4.Image = Bitmap2;
            }
            label11.Text = "处理完成！";
        }
        /******************************************************
        浮雕处理↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button20_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox4.Image);//创建Bitmap对象

            label11.Text = "正在处理，请稍侯...";              //如果像素数太大，处理时间太长，会出现假死状态

            int fbl = bmp.Width * bmp.Height;
            if (fbl > 1049088)     //像素大于1366*768
            {
                MessageBox.Show("源图片像素数太大，处理时间会比较长，出现假死状态，稍等即好。点击【确定】继续...");
            }

            for (int i = 0; i < bmp.Width - 1; i++)
            {
                for (int j = 0; j < bmp.Height - 1; j++)
                {
                    //获取相邻两个像素的R、G、B值
                    Color color = bmp.GetPixel(i, j);
                    Color colorLeft = bmp.GetPixel(i + 1, j + 1);
                    //67用来控制图片的最低灰度
                    int r = Math.Max(67, Math.Min(255, Math.Abs(color.R - colorLeft.R + 128)));
                    int g = Math.Max(67, Math.Min(255, Math.Abs(color.G - colorLeft.G + 128)));
                    int b = Math.Max(67, Math.Min(255, Math.Abs(color.B - colorLeft.B + 128)));
                    Color colorResult = Color.FromArgb(255, r, g, b);//颜色值
                    bmp.SetPixel(i, j, colorResult);//设置像素的颜色值
                }
                pictureBox4.Image = bmp;
                label11.Text = "处理完成！";
            }
        }
        /******************************************************
        重置↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button21_Click(object sender, EventArgs e)
        {
            重置图像();
        }

        private void button22_Click(object sender, EventArgs e)
        {
           
            重置图像();
        }

        private void 重置图像()
        {
            pictureBox4.Image = pictureBox1.Image;
        }
    }
}