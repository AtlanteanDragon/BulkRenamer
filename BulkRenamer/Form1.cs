using System;
using System.Windows.Forms;

namespace BulkRenamer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            new Renamer().RenameFiles(textBox2.Text, textBox1.Text);
        }
    }
}
