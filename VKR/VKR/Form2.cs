using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace VKR
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public static int cost;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && richTextBox1.Text != "" && richTextBox2.Text != "")
            {
                try
                {
                    cost = int.Parse(textBox1.Text);
                }
                catch
                {
                    MessageBox.Show("Неверный формат данных в поле Стоимость!");
                    return;
                }
                int y = Convert.ToInt32(textBox1.Text);
                WWWEntities contextEntities = new WWWEntities();
                List<know2> list = contextEntities.know2.ToList();
                foreach (know2 item in list)
                {
                    if (Convert.ToInt32(item.Id) == y)
                    {
                        item.condition = richTextBox1.Lines[0];
                        item.rule = richTextBox2.Lines[0];
                        contextEntities.SaveChanges();
                    }
                }
                richTextBox1.Clear();
                richTextBox2.Clear();
                textBox1.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && richTextBox1.Text != "")
            {
                try
                {
                    cost = int.Parse(textBox1.Text);
                }
                catch
                {
                    MessageBox.Show("Неверный формат данных в поле Индекс!");
                    return;
                }
                int y = Convert.ToInt32(textBox1.Text);
                WWWEntities contextEntities = new WWWEntities();
                List<Know> list = contextEntities.Know.ToList();
                foreach (Know item in list)
                {
                    if (Convert.ToInt32(item.Id) == y)
                    {
                        item.Knowledge = richTextBox1.Lines[0];
                        contextEntities.SaveChanges();
                    }
                }
                richTextBox1.Clear();
                textBox1.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            Form1 f1 = new Form1();
            f1.ShowDialog();
        }
    }
}
