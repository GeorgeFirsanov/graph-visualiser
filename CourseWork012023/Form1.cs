using System.Globalization;

namespace CourseWork012023
{
    public partial class Form1 : Form
    {
        protected  GraphData G;
        public Form1(ref GraphData gr)
        {
            G = gr;
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            while (numericUpDown1.Value > G.nodecounts)
            {
                G.NewNode();
            }
            while (numericUpDown1.Value < G.nodecounts)
            {
                G.PopNode();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           

        }
        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            Form2 newform = new Form2(ref G);
            newform.Show();
            this.numericUpDown1.Value = G.nodecounts == 0 ? 1: G.nodecounts;

        }
    }
    // Class for working with TableLayoutPanel
    public static class TableLayoutHelper
    {
        public static void RemoveArbitraryRow(TableLayoutPanel panel, int rowIndex)
        {
            if (rowIndex > panel.RowCount)
            {
                return;
            }

            // delete all controls of row that we want to delete
            for (int i = 0; i < panel.ColumnCount; i++)
            {
                var control = panel.GetControlFromPosition(i, rowIndex);
                panel.Controls.Remove(control);
            }

            // move up row controls that comes after row we want to remove
            for (int i = rowIndex + 1; i < panel.RowCount; i++)
            {
                for (int j = 0; j < panel.ColumnCount; j++)
                {
                    var control = panel.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        panel.SetRow(control, i - 1);
                    }
                }
            }

            var removeStyle = panel.RowCount - 1;

            if (panel.RowStyles.Count > removeStyle)
                panel.RowStyles.RemoveAt(removeStyle);

            panel.RowCount--;
        }
    }
}