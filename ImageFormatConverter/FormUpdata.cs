using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;





namespace ImageFormatConverter
{
    public partial class FormUpdata : Form
    {
        public FormUpdata()
        {
            InitializeComponent();
        }

        private void FormUpdata_Load(object sender, EventArgs e)
        {

            richTextBox1.ReadOnly = true;                   //设置超长文本框为只读
            richTextBox1.Text ="" ;                         //清空
            StreamReader UpdataReader = new StreamReader("updata.txt", UnicodeEncoding.GetEncoding("GB2312"));
            //Implements a System.IO.TextReader that reads characters from a byte stream in a particular encoding.
            //实现一个System.IO.TextReader以特定编码（GB2312）从（updata.txt）以字节流读取字符
            string sLine = UpdataReader.ReadToEnd();        //新建字符串变量，赋予读入的字符 
            if (sLine != null)
            {
                richTextBox1.Text = sLine;                  //长文本框显示读入的数据
                UpdataReader.Close();                       //关闭字符读入
            }
            else
                MessageBox.Show("读取信息失败！");          
        }
    }
}
