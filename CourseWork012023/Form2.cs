using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork012023
{
    public partial class Form2 : Form
    {
        protected GraphData G;
        public Form2(ref GraphData gr)
        {
            G = gr;
            InitializeComponent();
            try
            {
                this.textBox1.Text = G.GetData().Replace("\n", "\r\n");
            }
            catch { };
        }

        //Сохранить
        private void button1_Click(object sender, EventArgs e)
        {
            string adjlist = this.textBox1.Text;
            
            G.Intersection(adjlist);
            this.Close();
        }
        //Отмена
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
