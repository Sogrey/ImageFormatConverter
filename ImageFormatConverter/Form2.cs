using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;                 //添加System.Threading，利用新开线程控制转换，防假死

namespace ImageFormatConverter
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
            
        }
        public FormMain mainForm;                   //公共声明主窗口是FormMain

        string[] path1=null;                 //用于存储选择的文件列表
        string path2="";                    //用于存储保存的路径
        Bitmap bt;                          //声明一个转换图片格式的Bitmap对象
        Thread td;                          //声明一个线程
        int Imgtype1;                       //声明一个变量用于标记ConvertImage方法中转换的类型
        string OlePath;                     //声明一个变量用于存储ConvertImage方法中原始图片的路径
        string path;                        //声明一个变量用于存储ConvertImage方法中转换后图片的保存路径
        int flags;                           //用于标记已转换图片的数量，用于计算转换进度

        int rename = 0;                     //重命名计数标记


        private void Form2_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;       //窗体载入居中显示
            this.Text = "ImageFormatConverter-批量转换";
            tscbType.Text = "BMP格式(*.bmp)";             //设置第一个转换类型被选中
            CheckForIllegalCrossThreadCalls = false;     //屏蔽线程弹出的错误提示
            tsslFileNum.Text = "等待添加文件...";        //状态栏第一项  已添加文件数
            tsslPlan.Text = "未开始转换...";             //状态栏第三项  转换进度
            tsslrename.Text = "";                        //状态栏第四项  重命名计数
        }



        /******************************************************
        添加图像文件↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox1_Click(object sender, EventArgs e)              //添加图像文件
        {
            openFileDialog1.Multiselect = true;                         //启用多选
            openFileDialog1.Filter = "图像文件（*.*）|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff;*.ico;*.emf;*.wmf|BMP文件（*.bmp）|*.bmp|JPG文件（*.jpg;*.jpeg）|*.jpg;*.jpeg|PNG文件（*.png）|*.png|GIF文件(*.gif)|*.gif|TIFF文件（*.tif;*.tiff）|*.tif;*.tiff|ICO格式(*.ico)|*.ico|EMF格式(*.emf)|*.emf|WMF格式(*.wmf)|*.wmf|所有文件(*.*)|*.*";           //文件过滤
            openFileDialog1.Title = "打开图像文件";                      //打开文件对话框标题

            if (openFileDialog1.ShowDialog() == DialogResult.OK)        //判断是否选择文件
            {
                listView1.Items.Clear();                                //清空listView1
                string[] info = new string[7];                          //存储每一行数据
                FileInfo fi;                                            //创建一个FileInfo对象，用于获取图片信息
                path1 = openFileDialog1.FileNames;                      //获取选择的图片集合
                for (int i = 0; i < path1.Length; i++)                  //读取集合中的内容
                {

                    string ImgName = path1[i].Substring(path1[i].LastIndexOf("\\") + 1, path1[i].Length - path1

[i].LastIndexOf("\\") - 1);                 //获取图片名称
                    
                    string ImgType = ImgName.Substring(ImgName.LastIndexOf(".") + 1, ImgName.Length -

ImgName.LastIndexOf(".") - 1);             //获取图片类型
                    fi = new FileInfo(path1[i].ToString());             //实例化FileInfo对象
                    
                    //将每一行数据第一个位置的图标添加到imageList1中
                    //imageList1.Images.Add(ImgName, Properties.Resources.已选择 );
                    int xuhao = i + 1;
                    info[0] = xuhao.ToString();             //序号
                    info[1] = ImgName;                      //图片名称
                    info[2] = ImgType;                      //图片类型
                    info[3] = fi.LastWriteTime.ToShortDateString();//图片最后修改日期
                    info[4] = path1[i].ToString();                  //图片位置
                    info[5] = (fi.Length / 1024) + "KB";                //图片大小
                    info[6] = "未转换";                                //图片是否转换
                    ImgName = ImgName.Remove(ImgName.LastIndexOf("."));                //文件名去扩展名
                    ListViewItem lvi = new ListViewItem(info, ImgName);  //实例化ListViewItem对象
                    listView1.Items.Add(lvi);                              //将信息添加到listView1控件中
                }
                tsslFileNum.Text = "当前共有" + path1.Length.ToString() + "个文件";//状态栏中显示图片数量
            }

        }
        /******************************************************
        设置保存路径↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox2_Click(object sender, EventArgs e)             //设置保存路径
        {
            folderBrowserDialog1.Description = "选择保存路径";               //设置对话框窗体提示语
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)   //判断是否选择保存路径
            {
                path2 = folderBrowserDialog1.SelectedPath;              //获取保存路径
                textBox1.Text = path2;                                  //显示保存路径
            }
        }
        /******************************************************
        清空列表↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox3_Click(object sender, EventArgs e)             //清空列表
        {
            listView1.Items.Clear();                                        //清空列表
            path1 = null;                                                   //清空图片的集合
            tsslFileNum.Text = "当前没有文件";                                 //状态栏中提示
            tsslPlan.Text = "";                                                 //清空进度数字
            tsslrename.Text = "";                                               //清空重命名计数
        }
        /******************************************************
        开始转换（确定目标格式，开启新进程转换）↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            tsslPlan.Text = "";                                                 //清空进度数字

            if (path1 == null)                                              //判断是否选择图片
            {
                MessageBox.Show("请选择图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (path2.Length == 0)                                      //判断是否选择保存位置
                {
                    MessageBox.Show("请选择保存位置！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    flags = 1;                                              //初始化flags变量为1，用于计算进度

                    pictureBox1.Enabled = false;                            //当转换开始时，禁用工具栏
                    pictureBox2.Enabled = false;
                    pictureBox3.Enabled = false;
                    pictureBox4.Enabled = false;
                    tscbType.Enabled = false;

                    string  flag = tscbType.Text ;                      //判断将图片转换为何种格式
                    for (int i = 0; i < path1.Length; i++)
                    {
                        listView1.Items[i].SubItems[6].Text = "未转换";    //图片是否转换(主要用于重复转换，重新显示是为“未转换”)
                    }

                        tsslPlan.Text = "正在转换  0%";               //转换进度
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
                    switch (flag)                                           //根据不同的格式进行转换
                    {
                        case "BMP格式(*.bmp)":
                            Imgtype1 = 0;                                   //如果选择第一项则转换为BMP格式
                            td = new Thread(new ThreadStart(ConvertImage)); //通过线程调用ConvertImage方法进行转换
                            td.Start();
                            break;
                        case "JPG格式(*.jpg)":                                             //如果选择第二项则转换为JPG格式
                            Imgtype1 = 1;
                            td = new Thread(new ThreadStart(ConvertImage));
                            td.Start();
                            break;
                        case "PNG格式(*.png)":                                            //如果选择第三项则转换为PNG格式
                            Imgtype1 = 2;
                            td = new Thread(new ThreadStart(ConvertImage));
                            td.Start();
                            break;
                        case "GIF格式(*.gif)":                                             //如果选择第四项则转换为GIF格式
                            Imgtype1 = 3;
                            td = new Thread(new ThreadStart(ConvertImage));
                            td.Start();
                            break;
                        case "ICO格式(*.ico)":                                             //如果选择第四项则转换为ICO格式
                            Imgtype1 = 4;
                            td = new Thread(new ThreadStart(ConvertImage));
                            td.Start();
                            break;
                        case "TIF格式(*.tif)":                                             //如果选择第四项则转换为TIF格式
                            Imgtype1 = 5;
                            td = new Thread(new ThreadStart(ConvertImage));
                            td.Start();
                            break;
                        case "EMF格式(*.emf)":                                             //如果选择第四项则转换为EMF格式
                            Imgtype1 = 6;
                            td = new Thread(new ThreadStart(ConvertImage));
                            td.Start();
                            break;
                        case "WMF格式(*.wmf)":                                             //如果选择第四项则转换为WMF格式
                            Imgtype1 = 7;
                            td = new Thread(new ThreadStart(ConvertImage));
                            td.Start();
                            break;
                        default: 
                            //td.Abort();
                            MessageBox.Show("列表框内容已被改变，请重新选择！！！");
                            tsslPlan.Text = "未开始转换...";             //状态栏第三项
                            pictureBox1.Enabled = true ;                 //当转换结束时，启用工具栏
                            pictureBox2.Enabled = true ;
                            pictureBox3.Enabled = true ;
                            pictureBox4.Enabled = true ;
                            tscbType.Enabled = true ;
                            break;
                    }
                }
            }
        }
        /******************************************************
        开始转换(批量文件转换到指定格式)↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void ConvertImage()
        {
            flags = 1;
            tsslrename.Text = "";                                               //清空重命名计数
            rename = 0;
            switch (Imgtype1)
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
                case 0:
                    for (int i = 0; i < path1.Length; i++)
                    {
                        string ImgName = path1[i].Substring(path1[i].LastIndexOf("\\") + 1, path1[i].Length - path1[i].LastIndexOf("\\") - 1);
                        ImgName = ImgName.Remove(ImgName.LastIndexOf("."));
                        OlePath = path1[i].ToString();
                        bt = new Bitmap(OlePath);
                        path = path2 + "\\" + ImgName + ".bmp";                            //定义保存文件名变量path，并赋值
                        int n = 0;
                        if (File.Exists(path))
                        {
                            while (File.Exists(path))                                       //判断目标文件是否存在，存在则运行
                            {
                                path = path2 + "\\" + ImgName + "_" + n.ToString() + ".bmp";     //强行重命名 
                                n = n + 1;
                            }
                            rename++;                                           //重命名计数
                        }

                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
                        listView1.Items[flags - 1].SubItems[6].Text = "√";
                        listView1.Items[flags - 1].SubItems[6].ForeColor = Color.Green;
                        tsslPlan.Text = "正在转换  " + flags * 100 / path1.Length + "%";
                        if (rename != 0)
                        {
                            tsslrename.Text = "("+rename.ToString() + "个目标文件名已存在，保存时已被重命名)";
                        }
                        if (flags == path1.Length)
                        {
                            tsslPlan.Text = "图片转换全部完成";
                            pictureBox1.Enabled = true;                            //当转换完成时，启用工具栏
                            pictureBox2.Enabled = true;
                            pictureBox3.Enabled = true;
                            pictureBox4.Enabled = true;
                            tscbType.Enabled = true;
                        }
                        flags++;
                    }
                    break;
                case 1:
                    for (int i = 0; i < path1.Length; i++)
                    {
                        string ImgName = path1[i].Substring(path1[i].LastIndexOf("\\") + 1, path1[i].Length - path1[i].LastIndexOf("\\") - 1);
                        ImgName = ImgName.Remove(ImgName.LastIndexOf("."));
                        OlePath = path1[i].ToString();
                        bt = new Bitmap(OlePath);
                        path = path2 + "\\" + ImgName + ".jpeg";
                        int n = 0;
                        if (File.Exists(path))
                        {
                            while (File.Exists(path))                                       //判断目标文件是否存在，存在则运行
                            {
                                path = path2 + "\\" + ImgName + "_" + n.ToString() + ".jpeg";     //强行重命名 
                                n = n + 1;
                            }
                            rename++;                                           //重命名计数
                        }
                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                        listView1.Items[flags - 1].SubItems[6].Text = "√";
                        listView1.Items[flags - 1].SubItems[6].ForeColor = Color.Green;
                        tsslPlan.Text = "正在转换  " + flags * 100 / path1.Length + "%";
                        if (rename != 0)
                        {
                            tsslrename.Text ="("+ rename.ToString() + "个目标文件名已存在，保存时已被重命名)";
                        }
                        if (flags == path1.Length)
                        {
                            tsslPlan.Text = "图片转换全部完成";
                            pictureBox1.Enabled = true;                            //当转换完成时，启用工具栏
                            pictureBox2.Enabled = true;
                            pictureBox3.Enabled = true;
                            pictureBox4.Enabled = true;
                            tscbType.Enabled = true;
                        }
                        flags++;
                    }
                    break;
                case 2:
                    for (int i = 0; i < path1.Length; i++)
                    {
                        string ImgName = path1[i].Substring(path1[i].LastIndexOf("\\") + 1, path1[i].Length - path1[i].LastIndexOf("\\") - 1);
                        ImgName = ImgName.Remove(ImgName.LastIndexOf("."));
                        OlePath = path1[i].ToString();
                        bt = new Bitmap(OlePath);
                        path = path2 + "\\" + ImgName + ".png";
                        int n = 0;
                        if (File.Exists(path))
                        {
                            while (File.Exists(path))                                       //判断目标文件是否存在，存在则运行
                            {
                                path = path2 + "\\" + ImgName + "_" + n.ToString() + ".png";     //强行重命名 
                                n = n + 1;
                            }
                            rename++;                                           //重命名计数
                        }
                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                        listView1.Items[flags - 1].SubItems[6].Text = "√";
                        listView1.Items[flags - 1].SubItems[6].ForeColor = Color.Green;
                        tsslPlan.Text = "正在转换  " + flags * 100 / path1.Length + "%";
                        if (rename != 0)
                        {
                            tsslrename.Text = "(" + rename.ToString() + "个目标文件名已存在，保存时已被重命名)";
                        }
                        if (flags == path1.Length)
                        {
                            tsslPlan.Text = "图片转换全部完成";
                            pictureBox1.Enabled = true;                            //当转换完成时，启用工具栏
                            pictureBox2.Enabled = true;
                            pictureBox3.Enabled = true;
                            pictureBox4.Enabled = true;
                            tscbType.Enabled = true;
                        }
                        flags++;
                    }
                    break;
                case 3:
                    for (int i = 0; i < path1.Length; i++)
                    {
                        string ImgName = path1[i].Substring(path1[i].LastIndexOf("\\") + 1, path1[i].Length - path1[i].LastIndexOf("\\") - 1);
                        ImgName = ImgName.Remove(ImgName.LastIndexOf("."));
                        OlePath = path1[i].ToString();
                        bt = new Bitmap(OlePath);
                        path = path2 + "\\" + ImgName + ".gif";
                        int n = 0;
                        if (File.Exists(path))
                        {
                            while (File.Exists(path))                                       //判断目标文件是否存在，存在则运行
                            {
                                path = path2 + "\\" + ImgName + "_" + n.ToString() + ".gif";     //强行重命名 
                                n = n + 1;
                            }
                            rename++;                                           //重命名计数
                        }
                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Gif);
                        listView1.Items[flags - 1].SubItems[6].Text = "√";
                        listView1.Items[flags - 1].SubItems[6].ForeColor = Color.Green;
                        tsslPlan.Text = "正在转换  " + flags * 100 / path1.Length + "%";
                        if (rename != 0)
                        {
                            tsslrename.Text = "(" + rename.ToString() + "个目标文件名已存在，保存时已被重命名)";
                        }
                        if (flags == path1.Length)
                        {
                            tsslPlan.Text = "图片转换全部完成";
                            pictureBox1.Enabled = true;                            //当转换完成时，启用工具栏
                            pictureBox2.Enabled = true;
                            pictureBox3.Enabled = true;
                            pictureBox4.Enabled = true;
                            tscbType.Enabled = true;
                        }
                        flags++;
                    }
                    break;

                case 4:
                    for (int i = 0; i < path1.Length; i++)
                    {
                        string ImgName = path1[i].Substring(path1[i].LastIndexOf("\\") + 1, path1[i].Length - path1[i].LastIndexOf("\\") - 1);
                        ImgName = ImgName.Remove(ImgName.LastIndexOf("."));
                        OlePath = path1[i].ToString();
                        bt = new Bitmap(OlePath);
                        path = path2 + "\\" + ImgName + ".ico";
                        int n = 0;
                        if (File.Exists(path))
                        {
                            while (File.Exists(path))                                       //判断目标文件是否存在，存在则运行
                            {
                                path = path2 + "\\" + ImgName + "_" + n.ToString() + ".ico";     //强行重命名 
                                n = n + 1;
                            }
                            rename++;                                           //重命名计数
                        }
                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Icon);
                        listView1.Items[flags - 1].SubItems[6].Text = "√";
                        listView1.Items[flags - 1].SubItems[6].ForeColor = Color.Green;
                        tsslPlan.Text = "正在转换  " + flags * 100 / path1.Length + "%";
                        if (rename != 0)
                        {
                            tsslrename.Text = "(" + rename.ToString() + "个目标文件名已存在，保存时已被重命名)";
                        }
                        if (flags == path1.Length)
                        {
                            tsslPlan.Text = "图片转换全部完成";
                            pictureBox1.Enabled = true;                            //当转换完成时，启用工具栏
                            pictureBox2.Enabled = true;
                            pictureBox3.Enabled = true;
                            pictureBox4.Enabled = true;
                            tscbType.Enabled = true;
                        }
                        flags++;
                    }
                    break;
                case 5:
                    for (int i = 0; i < path1.Length; i++)
                    {
                        string ImgName = path1[i].Substring(path1[i].LastIndexOf("\\") + 1, path1[i].Length - path1[i].LastIndexOf("\\") - 1);
                        ImgName = ImgName.Remove(ImgName.LastIndexOf("."));
                        OlePath = path1[i].ToString();
                        bt = new Bitmap(OlePath);
                        path = path2 + "\\" + ImgName + ".tif";
                        int n = 0;
                        if (File.Exists(path))
                        {
                            while (File.Exists(path))                                       //判断目标文件是否存在，存在则运行
                            {
                                path = path2 + "\\" + ImgName + "_" + n.ToString() + ".tif";     //强行重命名 
                                n = n + 1;
                            }
                            rename++;                                           //重命名计数
                        }
                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Tiff);
                        listView1.Items[flags - 1].SubItems[6].Text = "√";
                        listView1.Items[flags - 1].SubItems[6].ForeColor = Color.Green;
                        tsslPlan.Text = "正在转换  " + flags * 100 / path1.Length + "%";
                        if (rename != 0)
                        {
                            tsslrename.Text = "(" + rename.ToString() + "个目标文件名已存在，保存时已被重命名)";
                        }
                        if (flags == path1.Length)
                        {
                            tsslPlan.Text = "图片转换全部完成";
                            pictureBox1.Enabled = true;                            //当转换完成时，启用工具栏
                            pictureBox2.Enabled = true;
                            pictureBox3.Enabled = true;
                            pictureBox4.Enabled = true;
                            tscbType.Enabled = true;
                        }
                        flags++;
                    }
                    break;
                case 6:
                    for (int i = 0; i < path1.Length; i++)
                    {
                        string ImgName = path1[i].Substring(path1[i].LastIndexOf("\\") + 1, path1[i].Length - path1[i].LastIndexOf("\\") - 1);
                        ImgName = ImgName.Remove(ImgName.LastIndexOf("."));
                        OlePath = path1[i].ToString();
                        bt = new Bitmap(OlePath);
                        path = path2 + "\\" + ImgName + ".emf";
                        int n = 0;
                        if (File.Exists(path))
                        {
                            while (File.Exists(path))                                       //判断目标文件是否存在，存在则运行
                            {
                                path = path2 + "\\" + ImgName + "_" + n.ToString() + ".emf";     //强行重命名 
                                n = n + 1;
                            }
                            rename++;                                           //重命名计数
                        }
                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Emf);
                        listView1.Items[flags - 1].SubItems[6].Text = "√";
                        listView1.Items[flags - 1].SubItems[6].ForeColor = Color.Green;
                        tsslPlan.Text = "正在转换  " + flags * 100 / path1.Length + "%";
                        if (rename != 0)
                        {
                            tsslrename.Text = "(" + rename.ToString() + "个目标文件名已存在，保存时已被重命名)";
                        }
                        if (flags == path1.Length)
                        {
                            tsslPlan.Text = "图片转换全部完成";
                            pictureBox1.Enabled = true;                            //当转换完成时，启用工具栏
                            pictureBox2.Enabled = true;
                            pictureBox3.Enabled = true;
                            pictureBox4.Enabled = true;
                            tscbType.Enabled = true;
                        }
                        flags++;
                    }
                    break;

                case 7:
                    for (int i = 0; i < path1.Length; i++)
                    {
                        string ImgName = path1[i].Substring(path1[i].LastIndexOf("\\") + 1, path1[i].Length - path1[i].LastIndexOf("\\") - 1);
                        ImgName = ImgName.Remove(ImgName.LastIndexOf("."));
                        OlePath = path1[i].ToString();
                        bt = new Bitmap(OlePath);
                        path = path2 + "\\" + ImgName + ".wmf";
                        int n = 0;
                        if (File.Exists(path))
                        {
                            while (File.Exists(path))                                       //判断目标文件是否存在，存在则运行
                            {
                                path = path2 + "\\" + ImgName + "_" + n.ToString() + ".wmf";     //强行重命名 
                                n = n + 1;
                            }
                            rename++;                                           //重命名计数
                        }
                        bt.Save(path, System.Drawing.Imaging.ImageFormat.Wmf);
                        listView1.Items[flags - 1].SubItems[6].Text = "√";
                        listView1.Items[flags - 1].SubItems[6].ForeColor = Color.Green;
                        tsslPlan.Text = "正在转换  " + flags * 100 / path1.Length + "%";
                        if (rename != 0)
                        {
                            tsslrename.Text = "(" + rename.ToString() + "个目标文件名已存在，保存时已被重命名)";
                        }
                        if (flags == path1.Length)
                        {
                            tsslPlan.Text = "图片转换全部完成";
                            pictureBox1.Enabled = true;                            //当转换完成时，启用工具栏
                            pictureBox2.Enabled = true;
                            pictureBox3.Enabled = true;
                            pictureBox4.Enabled = true;
                            tscbType.Enabled = true;
                        }
                        flags++;
                    }
                    break;
                default: bt.Dispose(); break;
            }
        }

        /******************************************************
        退出并显示主窗口↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        *******************************************************/
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            mainForm.Visible = true;           //FormMain显示代码（一般在子窗口关闭前执行）
            this.Close ();                                         //退出系统
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (td != null)                                                 //判断是否存在线程
            {
                if (td.ThreadState == ThreadState.Running)                  //然后判断线程是否正在运行
                {
                    td.Abort();                                             //如果运行则关闭线程
                }
            }
            mainForm.Visible = true;           //FormMain显示代码（一般在子窗口关闭前执行）
        }
    }
}