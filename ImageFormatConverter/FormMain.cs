using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;               //添加System.Drawing.Imaging类
using System.IO;
using System.Threading;                      //添加System.Threading类线程处理类



namespace ImageFormatConverter
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        Bitmap bitmap;                      //定义System.Drawing.Bitmap类变量bitmap 
        string PictureName = "";              //定义字符串变量PictureName且初始化为空
        string name = "";                   //定义字符串变量name且初始化为空  

        string path;                  //用于选择的文件全路径
        string path2;                 //用于存储的文件全路径
        string folder;                //存储的文件的文件夹
        string ImgName;               //原文件文件名
        string ImgName2;              //目标文件文件名
        string ImgType;               //源文件类型
        string ImgType2;              //目标文件类型
        string filename;              //目标文件全路径(带扩展名)
        int width, height;            //源文件的宽度和高度整型
        string  width1, height1;       //源文件的宽度和高度字符串类型
        int width2, height2;           //目标文件的宽度和高度整型
        //string width3, height3;         //目标文件的宽度和高度字符串类型
        Image MyImage;                  //原图片

        int bmpn = 0;                    //判断重名情况初始化
        int jpgn=0;
        int gifn=0;
        int pngn=0;
        int icon=0;
        int emfn = 0;
        int wmfn = 0;
        int tifn = 0;


        /******************************************************
        窗口启动初始化↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void FormMain_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;       //窗体载入居中显示
            this.Text = "ImageFormatConverter-单文件转换";
            textBox6.Text = "...";
            label8.Text = "...";
            label9.Text = "...";
            label10.Text = "...";
            textBox7.Text = "...";
            label12.Text = "...";
            label13.Text = "...";
            label14.Text = "...";
            label22.Text = "...";
            label23.Text = "...";
            label25.Text = "";
            label21.Text = "版本：1.2.0.0";

            //label21.Parent = pictureBox1;          //设置标签的父容器为图片框
            //label21.BackColor = Color.Transparent; //设置标签背景透明（相对于父容器）
            pictureBox6.Visible = false;           //【重命名】复选框没有选中，重命名标志隐藏
            pictureBox7.Visible = false;           //【更改尺寸】复选框没有选中，更改尺寸标志隐藏

            this.timer1.Interval = 1000;           //设置时间间隔为1ms
            this.timer1.Start();                   //计时器启动
            label7.Text = "";                      //label7.Text清空

        }

        /******************************************************
        显示当前系统时间↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;                                   //定义时间变量，并初始化为获取的当前系统时间         
            label7.Font = new Font("黑体", 12);
            this.label7.Text = time.ToString();                             //显示当前系统时间
        }

        /******************************************************
        点击菜单栏【文件】中【打开源文件】选项↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void 打开源文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            选择将要转换的源文件();       //调用选择将要转换的源文件(子程序)
        }

        /******************************************************
        点击菜单栏【文件】中【设置保存路径】选项↓↓↓↓↓↓↓↓
        *******************************************************/
        private void 设置保存路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            设置保存路径();              //调用设置保存路径，目标文件保存路径子程序
        }

        /******************************************************
        点击菜单栏【文件】中【开始转换】选项↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void 开始转换ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            开始转换();                //调用开始转换（子程序）

        }

        /******************************************************
        点击菜单栏【文件】选项【退出】关闭窗口↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            退出软件();               //调用退出子程序
        }


        /******************************************************
        点击菜单栏【格式转换】选项【批量转换】加载【批量转换】窗口
        *******************************************************/
        private void 批量转换_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();       //实例化Form2
            frm.mainForm = this;
            frm.Show();                    //显示Form2
            this.Visible = false;
        }

        /******************************************************
        点击菜单栏【图片处理与显示特效】选项【添加水印】↓↓↓↓
        *******************************************************/
        private void 添加水印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeString.tabpageNO = 1;
            FormImageProcessing FormImageProcessing = new FormImageProcessing();       //实例化FormImageProcessing
            FormImageProcessing.mainForm = this;
            FormImageProcessing.Show();                    //显示FormImageProcessing
            this.Visible = false;
        }

        /******************************************************
        点击菜单栏【图片处理与显示特效】选项【旋转】选中↓↓↓↓
        *******************************************************/
        private void 旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeString.tabpageNO =2;
            FormImageProcessing ImageProcess = new FormImageProcessing();        //实例化窗口
            ImageProcess.mainForm = this;
            ImageProcess.Show();                             //显示窗口
            this.Visible = false;
        }
        /******************************************************
        点击菜单栏【图片处理与显示特效】选项【翻转】选中↓↓↓↓
        *******************************************************/
        private void 翻转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeString.tabpageNO = 2;
            FormImageProcessing ImageProcess = new FormImageProcessing();        //实例化窗口
            ImageProcess.mainForm = this;
            ImageProcess.Show();                             //显示窗口
            this.Visible = false;
        }


        /******************************************************
        点击菜单栏【图片处理与显示特效】选项【裁剪】↓↓↓↓↓↓
        *******************************************************/
        private void 修改图片尺寸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeString.tabpageNO = 3;
            FormImageProcessing FormImageProcessing = new FormImageProcessing();       //实例化FormImageProcessing
            FormImageProcessing.mainForm = this;
            FormImageProcessing.Show();                    //显示FormImageProcessing
            this.Visible = false;
        }
        /******************************************************
        点击菜单栏【图片处理与显示特效】选项【反相】选中↓↓↓↓
        *******************************************************/
        private void 反相ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeString.tabpageNO = 5;
            FormImageProcessing ImageProcess = new FormImageProcessing();        //实例化窗口
            ImageProcess.mainForm = this;
            ImageProcess.Show();                             //显示窗口
            this.Visible = false;
        }
        /******************************************************
        点击菜单栏【图片处理与显示特效】选项【黑白】选中↓↓↓↓
        *******************************************************/
        private void 黑白ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeString.tabpageNO = 5;
            FormImageProcessing ImageProcess = new FormImageProcessing();        //实例化窗口
            ImageProcess.mainForm = this;
            ImageProcess.Show();                             //显示窗口
            this.Visible = false;
        }

        /******************************************************
        点击菜单栏【图片处理与显示特效】选项【图片显示特效】选中
        *******************************************************/
        private void 图片显示特效DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeString.tabpageNO = 4;
            FormImageProcessing ImageProcess = new FormImageProcessing();        //实例化窗口
            ImageProcess.mainForm = this;
            ImageProcess.Show();                             //显示窗口
            this.Visible = false;
        }

        /******************************************************
        点击菜单栏【图片处理与显示特效】选项【浮雕】选中↓↓↓↓
        *******************************************************/
        private void 浮雕ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeString.tabpageNO =5;
            FormImageProcessing ImageProcess = new FormImageProcessing();        //实例化窗口
            ImageProcess.mainForm = this;
            ImageProcess.Show();                             //显示窗口
            this.Visible = false;
        }
        /******************************************************
        点击菜单栏【图片处理与显示特效】选项【消隐】选中↓↓↓↓
        *******************************************************/
        private void 消隐ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeString.tabpageNO = 6;
            FormImageProcessing ImageProcess = new FormImageProcessing();        //实例化窗口
            ImageProcess.mainForm = this;
            ImageProcess.Show();                             //显示窗口
            this.Visible = false;
        }
        /******************************************************
        点击菜单栏【关于】中【帮助】选项加载【帮助】窗口↓↓↓↓
        *******************************************************/
        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("正在加紧施工...");
            string chm = "IFCHelp.chm";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            if (File.Exists(chm))                                       //判断帮助文件IFCHelp.chm是否存在，存在则运行
            {
                p.StartInfo.FileName = "IFCHelp.chm";                   //设置要调用的文件名
                //p.StartInfo.Arguments = "-x sourceFile.Arj c:\temp";   //启动参数
                p.Start();
                if (p.HasExited)                                        //判断是否运行结束
                    p.Kill();
            }
            else                                                        //帮助文件不存在弹出提示
                MessageBox.Show("找不到帮助文件IFCHelp.chm！");
        }

        /******************************************************
        点击菜单栏【关于】中【升级信息】选项↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void 升级信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("updata.txt"))
            {
                FormUpdata Updata = new FormUpdata();        //实例化【关于】窗口
                Updata.Show();                             //显示【关于】窗口
            }
            else
                MessageBox.Show("找不到\"updata.txt\"文件");
        }

        /******************************************************
        点击菜单栏【关于】中【关于本软件】选项加载【关于】窗口↓
        *******************************************************/
        private void 关于本软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();        //实例化【关于】窗口
            about.Show();                             //显示【关于】窗口
        }

        /******************************************************
        点击菜单栏【退出软件】选项关闭窗口↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void 退出软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            退出软件();                              //调用退出子程序
        }  

        /******************************************************
        点击“打开”图标，选择将要转换的源文件↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            选择将要转换的源文件();                  //调用选择将要转换的源文件(子程序)
        }

        /******************************************************
        点击“保存”图标，设置保存路径↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            设置保存路径();                           //调用设置保存路径，目标文件保存路径子程序
        }

        /******************************************************
        点击“转换”图标，开始转换↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            开始转换();                             //调用开始转换（子程序）
        }

        /******************************************************
        点击预览图片缩略图查看原图片（原尺寸）↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (CodeString.imagepath != "")
            {
                FormPictureShow picture = new FormPictureShow();       //实例化FormPictureShow窗口
                picture.Show();                                        //显示
            }
            else
            {
                MessageBox.Show("选择好图片可以点我预览大图哦！");
            }
        }

        /******************************************************
        选择将要转换的源文件的文本框被单击↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void textBox1_Click(object sender, EventArgs e)
        {
            选择将要转换的源文件();       //调用选择将要转换的源文件(子程序)
        }

        /******************************************************
        设置保存路径的文本框被单击↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void textBox2_Click(object sender, EventArgs e)
        {
            设置保存路径();               //调用设置保存路径，目标文件保存路径子程序
        }

        /******************************************************
        点击【重命名】复选框↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {
                textBox5.Enabled = true;               //【重命名】复选框选中，重命名文本框使能为真
                button1.Enabled = true;                //【重命名】复选框选中，【ok】按钮使能为真
            }
            else
            {
                textBox5.Enabled = false;               //【重命名】复选框没有选中，重命名文本框使能为假
                button1.Enabled = false;                //【重命名】复选框没有选中，【ok】按钮使能为假
                name = "";                              //【重命名】复选框没有选中，name清空
                pictureBox6.Visible = false;            //【重命名】复选框没有选中，重命名标志隐藏
            }
        }
        /******************************************************
        点击【ok】重命名↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox5.Text == "")
            {
                MessageBox.Show("文件名不能为空！！！");        //重命名不能为空

            }
            else
            {
                name = textBox5.Text;                       //点击【ok】重命名
                MessageBox.Show("重命名成功，命名为：" + name + ",后缀名会在转换成功后自动加上！");         //显示重命名
                pictureBox6.Visible = true;          //【重命名】复选框选中且重命名启用，重命名标志显示
            }
        }
        /******************************************************
        点击【更改尺寸】复选框↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox2.Checked == true)
            {
                groupBox4.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                button2.Enabled = true;                //【更改尺寸】复选框选中，分组框，尺寸X文本框，尺寸Y文本框，【锁定比例】复选框，按钮【ok】使能为真

            }
            else
            {
                groupBox4.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                button2.Enabled = false;                //【更改尺寸】复选框没有选中，分组框，尺寸X文本框，尺寸Y文本框，【锁定比例】，按钮【ok】使复选框使能为假
                pictureBox7.Visible = false;
            }
        }


        /******************************************************
        点击【更改尺寸】复选框【ok】按钮↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("请先添加源图片！");
            else
            {
                if (radioButton1.Checked == true)               ////按实际给定尺寸缩放
                {
                    pictureBox7.Visible = false;          //【更改尺寸】复选框选中且启用，更改尺寸标志显示

                    if (textBox3.Text != "" && textBox4.Text != "" && textBox3.Text != "0" && textBox4.Text != "0")
                    {
                        //************************************************************                         //修改尺寸


                        int n;
                        if (int.TryParse(textBox3.Text, out n) && int.TryParse(textBox4.Text, out n))          //判断输入角度是否为数字，是否有效，有效则。。。。
                        {
                            string w = textBox3.Text;
                            width2 = int.Parse(textBox3.Text);           //字符串型转整型
                            string h = textBox4.Text;
                            height2 = int.Parse(textBox4.Text);           //字符串型转整型
                            if (width2 == 0 || height2 == 0)
                            {
                                MessageBox.Show("尺寸不能为空或0！");
                                textBox3.Text = width.ToString();          //显示原始尺寸
                                textBox4.Text = height.ToString();
                            }
                            else
                            {
                                pictureBox7.Visible = true;          //【更改尺寸】复选框选中且启用，更改尺寸标志显示
                                MessageBox.Show("修改尺寸成功，新改为：" + textBox3.Text + "*" + textBox4.Text);      //显示新修改的尺寸
                                bitmap = KiResizeImage(bitmap, width2, height2);                  //调用修改尺寸子程序
                            }
                        }
                        else                                          //判断输入角度是否为数字，是否有效，无效则。。。。
                        {
                            MessageBox.Show("尺寸输入有误！请重新输入！");
                            textBox3.Text = width.ToString();          //显示原始尺寸
                            textBox4.Text = height.ToString();
                        }

                    }
                    else
                        MessageBox.Show("尺寸不能为空或0！");
                }
                if (radioButton2.Checked == true)               //按比例缩放
                {
                    pictureBox7.Visible = false;          //【更改尺寸】复选框选中且启用，更改尺寸标志显示
                    textBox3.Text = width.ToString();          //显示原始尺寸
                    textBox4.Text = height.ToString();
                    //以任意比例缩放显示图像
                    if (MyImage != null)
                    {
                        int fx = (int)(this.numericUpDown1.Value);
                        int fy = (int)(this.numericUpDown1.Value);
                        //int width, height;            //源文件的宽度和高度整型
                        //int width2, height2;           //目标文件的宽度和高度整型
                        width2 = MyImage.Width * fx / 10;
                        height2 = MyImage.Height * fy / 10;
                        textBox3.Text = width2.ToString();          //显示原始尺寸
                        textBox4.Text = height2.ToString();
                        if (width2 == 0 || height2 == 0)
                        {
                            MessageBox.Show("尺寸不能为空或0！");
                        }
                        else
                        {
                            pictureBox7.Visible = true;          //【更改尺寸】复选框选中且启用，更改尺寸标志显示
                            MessageBox.Show("修改尺寸成功，新改为：" + width2.ToString() + "*" + height2.ToString());      //显示新修改的尺寸
                            //************************************************************                         //修改目标尺寸

                            bitmap = KiResizeImage(bitmap, width2, height2);                  //调用修改尺寸子程序

                        }
                    }
                }

                pictureBox1.Image = bitmap;
                string filenametmp = "C:\\IFCtmp.bmp";                  //定义文件名变量filename，并赋值
                if (File.Exists(filenametmp))                                       //判断目标文件是否存在，存在则运行
                {
                    File.Delete(filenametmp);                   //存在就删除（有点霸道哦）
                }
                filenametmp = "C:\\IFCtmp.bmp";                  //定义文件名变量filename，并赋值


                bitmap.Save(filenametmp, ImageFormat.Bmp);                    //文件转换并保存
                CodeString.tmp = 1;
                CodeString.imagewidth = width2;
                CodeString.imageheight = height2;
            }
        }

        //更改尺寸子程序
        ///   <summary> 
        ///   Resize图片 
        ///   </summary> 
        ///   <param   name= "bmp "> 原始Bitmap </param> 
        ///   <param   name= "newW "> 新的宽度 </param> 
        ///   <param   name= "newH "> 新的高度 </param> 
        ///   <returns> 处理以后的Bitmap </returns> 
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                
                return b;
            }
            catch
            {
                return null;
            }
        } 

        //按实际尺寸缩放
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                label15.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                pictureBox5.Visible = false;
                pictureBox7.Visible = false;
                numericUpDown1.Enabled = false;
            }
        }
        //按比例缩放
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                label15.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                pictureBox5.Visible = true;
                pictureBox7.Visible = false;
                numericUpDown1.Enabled = true;
            }
        }

        /////////////////////////////////////////子程序\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        /******************************************************
        选择将要转换的源文件(子程序)↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void 选择将要转换的源文件()
        {
            label25.Text = "";
            label18.Visible = false;
            openFileDialog1.Filter = "图像文件（*.*）|*.bmp;*.jpg;*.png;*.gif;*.tif;*.ico;*.emf;*.wmf|BMP文件（*.bmp）|*.bmp|JPG文件（*.jpg;*.jpeg）|*.jpg;*.jpeg|PNG文件（*.png）|*.png|GIF文件(*.gif)|*.gif|TIFF文件（*.tif;*.tiff）|*.tif;*.tiff|ICO格式(*.ico)|*.ico|EMF格式(*.emf)|*.emf|WMF格式(*.wmf)|*.wmf|所有文件(*.*)|*.*";           //文件过滤
            openFileDialog1.Title = "打开图像文件*.bmp;*.jpg;*.png;*.gif;*.tif;*.ico;*.emf;*.wmf";                      //打开文件对话框标题
            openFileDialog1.Multiselect = false;                         //多选为假，不可多选

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);       //打开文件并在pictureBox1中显示
                bitmap = new Bitmap(pictureBox1.Image);                            //创建Bitmap对象
                label16.Text = "预览";
                label21.Visible = false;

                textBox1.Text = openFileDialog1.FileName;                              //显示图片路径

                ///////////以下获取并显示源图片属性/////////////
                FileInfo fi;                                            //创建一个FileInfo对象，用于获取图片信息
                path = openFileDialog1.FileName;                      //获取选择的图片路径

                ImgName = path.Substring(path.LastIndexOf("\\") + 1, path.Length - path.LastIndexOf("\\") - 1);                 //获取图片名称
                ImgType = ImgName.Substring(ImgName.LastIndexOf(".") + 1, ImgName.Length - ImgName.LastIndexOf(".") - 1);             //获取图片类型
                
                fi = new FileInfo(path.ToString());             //实例化FileInfo对象

                textBox6.Text = ImgName;                      //图片名称
                label8.Text = ImgType;                      //图片类型
                MyImage = System.Drawing.Image.FromFile(path);
                label9.Text = MyImage.Width + "*" + MyImage.Height;//分辨率
                label10.Text = (fi.Length / 1024) + "KB";                //图片大小
                label23.Text = fi.LastWriteTime.ToShortDateString();//图片最后修改日期
                width = MyImage.Width;
                height = MyImage.Height;
                textBox3.Text = width.ToString();          //显示原始尺寸
                textBox4.Text = height.ToString();
                width1 = textBox3.Text;                     //存储原始尺寸
                height1 = textBox4.Text;

                ImgName = ImgName.Remove(ImgName.LastIndexOf("."));                //文件名去扩展名

                CodeString.imagepath = openFileDialog1.FileName;     //全局变量传递源图片全路径
                CodeString.imagewidth = MyImage.Width;               //全局变量传递源图片宽度
                CodeString.imageheight = MyImage.Height;             //全局变量传递源图片高度
            }
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
        开始转换（子程序）↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void 开始转换()
        {
            int converted = 1;                                           //转换成功与否标记，默认成功，如果真成功，转换完成其值不变，否则置零
            if (textBox1.Text == "")
            {
                MessageBox.Show(" 请选择要转换的源图片！");              //如果没有选择图片，提示选择               
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show(" 请选择保存路径！");                     //如果没有选择图片，提示选择
            }
            else if (comboBox1.Text == "选择要转换成的目标格式")
            {
                MessageBox.Show(" 请选择要转换的目标格式！");             //没有选择目标格式，提示选择
            }
            else
            {
                if (name == "")
                {
                    PictureName = textBox2.Text + "\\" + ImgName;                             //如果重命名为空，即不重命名，将源文件的文件名（包括后缀名）赋给PictureName作为目标文件名（不含后缀名，转换完成自动加上新后缀名）
                }
                else
                {
                    PictureName = textBox2.Text + "\\" + name;               //如果重命名不为空，即要重命名，将目标路径加上反斜杠“\”再加上重命名的文件名赋给PictureName，转换完成自动加上新后缀名
                }

                switch (comboBox1.Text)
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
                    case "BMP格式(*.bmp)":                                         //选择BMP格式(*.bmp)
                        if (ImgType == "gif")
                        {
                            label18.Visible = true;
                            label18.Text = "选择动态Gif图片转换至bmp文件，只保留第一帧！";
                        }
                        filename = PictureName + ".bmp";                  //定义文件名变量filename，并赋值
                        while (File.Exists(filename))                                       //判断目标文件是否存在，存在则运行
                        {
                            bmpn = bmpn + 1;
                            filename = PictureName + "_" + bmpn.ToString()  + ".bmp";     //强行重命名 
                        }
                        bitmap.Save(filename, ImageFormat.Bmp);                    //文件转换并保存
                        bmpn = 0;
                        break;
                    case "JPG格式(*.jpg)":                                         //选择JPG格式(*.jpg)
                        if (ImgType == "gif")
                        {
                            label18.Visible = true;
                            label18.Text = "选择动态Gif图片转换至jpg文件，只保留第一帧！";
                        }
                        filename = PictureName + ".jpg";                          //引用文件名变量filename，并赋值
                        while (File.Exists(filename))                                       //判断目标文件是否存在，存在则运行
                        {
                            jpgn = jpgn + 1;
                            filename = PictureName + "_" + jpgn.ToString() + ".jpg";      //强行重命名
                        }
                        bitmap.Save(filename, ImageFormat.Jpeg);
                        jpgn =0;
                        break;
                    case "PNG格式(*.png)":
                        if (ImgType == "gif")
                        {
                            label18.Visible = true;
                            label18.Text = "选择动态Gif图片转换至png文件，只保留第一帧！";
                        }
                        filename = PictureName + ".png";
                        while (File.Exists(filename))                                       //判断目标文件是否存在，存在则运行
                        {
                            pngn = pngn + 1;
                            filename = PictureName + "_" + pngn.ToString() + ".png";      //强行重命名

                        }
                        bitmap.Save(filename, ImageFormat.Png);
                        pngn =0;
                        break;
                    case "GIF格式(*.gif)":                                         //选择GIF格式(*.gif)
                        if (ImgType != "gif")
                        {
                            label18.Visible = true;
                            label18.Text = "选择静态图片转换至Gif文件，还是静态图片！";
                        }
                        else if (ImgType == "gif")
                        {
                            label18.Visible = true;
                            label18.Text = "选择动态Gif图片转换至Gif文件，只保留第一帧！";
                        }
                        filename = PictureName + ".gif";
                        while (File.Exists(filename))                                       //判断目标文件是否存在，存在则运行
                        {
                            gifn = gifn + 1;
                            filename = PictureName + "_" + gifn.ToString() +  ".gif";      //强行重命名
                        }
                        bitmap.Save(filename, ImageFormat.Gif);
                        gifn = 0;
                        break;
                    case "TIF格式(*.tif)":                                         //选择TIF格式(*.tif)
                        if (ImgType == "gif")
                        {
                            label18.Visible = true;
                            label18.Text = "选择动态Gif图片转换至tif文件，只保留第一帧！";
                        }
                        filename = PictureName + ".tif";
                        while (File.Exists(filename))                                       //判断目标文件是否存在，存在则运行
                        {
                            tifn = tifn + 1;
                            filename = PictureName + "_" + tifn.ToString() + ".tif";      //强行重命名
                        }
                        bitmap.Save(filename, ImageFormat.Tiff);
                        tifn = 0;
                        break;
                    case "ICO格式(*.ico)":                                         //选择ICO格式(*.ico)
                        if (ImgType == "gif")
                        {
                            label18.Visible = true;
                            label18.Text = "选择动态Gif图片转换至ico文件，只保留第一帧！";
                        }
                        filename = PictureName + ".ico";
                        while (File.Exists(filename))                                       //判断目标文件是否存在，存在则运行
                        {
                            icon = icon + 1;
                            filename = PictureName + "_" + icon.ToString() + ".ico";      //强行重命名
                        }
                        bitmap.Save(filename, ImageFormat.Icon);
                        icon =0;
                        break;
                    case "EMF格式(*.emf)":                                         //选择EMF格式(*.emf)
                        if (ImgType == "gif")
                        {
                            label18.Visible = true;
                            label18.Text = "选择动态Gif图片转换至emf文件，只保留第一帧！";
                        }
                        filename = PictureName + ".emf";
                        while (File.Exists(filename))                                       //判断目标文件是否存在，存在则运行
                        {
                            emfn = emfn + 1;
                            filename = PictureName + "_" + emfn.ToString() +  ".emf";      //强行重命名
                        }
                        bitmap.Save(filename, ImageFormat.Emf);
                        emfn = 0;
                        break;
                    case "WMF格式(*.wmf)":                                         //选择WMF格式(*.wmf)
                        if (ImgType == "gif")
                        {
                            label18.Visible = true;
                            label18.Text = "选择动态Gif图片转换至wmf文件，只保留第一帧！";
                        }
                        filename = PictureName + ".wmf";
                        while (File.Exists(filename))                                       //判断目标文件是否存在，存在则运行
                        {
                            wmfn = wmfn + 1;
                            filename = PictureName + "_" + wmfn.ToString() + ".wmf";      //强行重命名
                        }
                        bitmap.Save(filename, ImageFormat.Wmf);
                        break;
                    default:
                        converted = 0;        //转换失败标记
                        MessageBox.Show("列表框内容已被改变，请重新选择！！！");
                        break;

                }
                if (converted == 1)                             //转换成功时执行
                {
                    //以下获取并显示源图片属性
                    FileInfo fi2;                                                //创建一个FileInfo对象，用于获取图片信息
                    path2 = filename;                                            //不重命名获取存储的图片路径
                    ImgName2 = path2.Substring(path2.LastIndexOf("\\") + 1, path2.Length - path2.LastIndexOf("\\") - 1);                 //获取图片名称
                    ImgType2 = ImgName2.Substring(ImgName2.LastIndexOf(".") + 1, ImgName2.Length - ImgName2.LastIndexOf(".") - 1);             //获取图片类型
                    fi2 = new FileInfo(path2.ToString());                       //实例化FileInfo对象
                    textBox7.Text = ImgName2;                                   //图片名称
                    label12.Text = ImgType2;                                    //图片类型
                    Image MyImage2 = System.Drawing.Image.FromFile(path2);
                    label13.Text = MyImage2.Width + "*" + MyImage2.Height;       //分辨率
                    label14.Text = (fi2.Length / 1024) + "KB";                   //图片大小
                    label22.Text = fi2.LastWriteTime.ToShortDateString();        //图片最后修改日期

                    if (label12.Text != label8.Text)                             //判断目标文件属性是否与源文件属性相同，不同则用红色表示
                    {
                        label12.ForeColor = Color.Red;
                        label25.Text = "TIPS：其中红色字体的属性项表示与源文件属性不同！" ;
                    }
                    if (label13.Text != label9.Text)
                    {
                        label13.ForeColor = Color.Red;
                        label25.Text = "TIPS：其中红色字体的属性项表示与源文件属性不同！" ;
                    }
                    if (label14.Text != label10.Text)
                    {
                        label14.ForeColor = Color.Red;
                        label25.Text = "TIPS：其中红色字体的属性项表示与源文件属性不同！" ;
                    }
                    if (label22.Text != label23.Text)
                    {
                        label22.ForeColor = Color.Red;
                        label25.Text = "TIPS：其中红色字体的属性项表示与源文件属性不同！" ;
                    }
                    bitmap = null;
                    bitmap = new Bitmap(pictureBox1.Image);                            //创建Bitmap对象
                    MessageBox.Show(" 转换完成！！");
                    pictureBox6.Visible = false;
                    pictureBox7.Visible = false;
                    textBox3.Text = width.ToString();          //显示原始尺寸
                    textBox4.Text = height.ToString();
                    name = "";
                    textBox5 .Text = "";
                }
            }
        }

        /******************************************************
        自定义退出子程序↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void 退出软件()
        {
            string filenametmp = "C:\\IFCtmp.bmp";                  //定义文件名变量filename，并赋值
            if (File.Exists(filenametmp))                                       //判断目标文件是否存在，存在则运行
            {
                File.Delete(filenametmp);                   //存在就删除（有点霸道哦）
            }
            this.Close();
        }
    }
}