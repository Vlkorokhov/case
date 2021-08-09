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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> s1, s3;
        string s2;
        List<int> f;
        int k;
        know2 sen1;
        string conclusion;
        public struct ty
        {
            public int gotov, sled, znach;
            public long pos, time;
        }
        public class Counter
        {
            public long x;
            public long y;
        }
        ty[,] a = new ty[200, 11];
        int polus;
        DateTime t1;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                using (WWWEntities dbConteiner = new WWWEntities())
                {
                    dbConteiner.Know.Add(new Know
                    {
                        Id = dbConteiner.Know.ToList().Count + 1,
                        Knowledge = textBox1.Lines[0]
                    });
                    dbConteiner.SaveChanges();
                    textBox1.Clear();
                }
            }
            else
                MessageBox.Show("Поле не заполнено для данного действия");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "")
            {
                using (WWWEntities dbConteiner = new WWWEntities())
                {
                    if (textBox2.Lines[0].Contains("Поставка"))
                    {
                        List<Know> list = dbConteiner.Know.ToList();
                        foreach (Know item in list)
                        {
                            if (item.Knowledge == textBox3.Lines[0])
                            {
                                item.confidence = 1;
                                dbConteiner.know2.Add(new know2
                                {
                                    Id = dbConteiner.know2.ToList().Count + 1,
                                    condition = textBox2.Lines[0],
                                    rule = textBox3.Lines[0],
                                    time = int.Parse(textBox4.Lines[0])
                                });
                                dbConteiner.SaveChanges();
                                textBox2.Clear();
                                textBox3.Clear();
                                textBox4.Clear();
                                break;
                            }
                        }
                    }
                    else
                    {
                        dbConteiner.know2.Add(new know2
                        {
                            Id = dbConteiner.know2.ToList().Count + 1,
                            condition = textBox2.Lines[0],
                            rule = textBox3.Lines[0],
                            time = int.Parse(textBox4.Lines[0])
                        });
                        dbConteiner.SaveChanges();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                    }
                }
            }
            else
                MessageBox.Show("Поле не заполнено для данного действия");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            WWWEntities contextEntities = new WWWEntities();
            List<know2> list = contextEntities.know2.ToList();

            foreach (know2 item in list)
            {
                richTextBox1.Text += Convert.ToString(item.Id) + " Если " + Convert.ToString(item.condition)
                    + ", то " + Convert.ToString(item.rule) + "\n";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            WWWEntities contextEntities = new WWWEntities();
            List<Know> list = contextEntities.Know.ToList();
            foreach (Know item in list)
            {
                richTextBox1.Text += Convert.ToString(item.Id) + " " + Convert.ToString(item.Knowledge)
                    + " " + Convert.ToString(item.confidence) + " " + Convert.ToString(item.time) + "\n";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (WWWEntities dbConteiner = new WWWEntities())
            {
                richTextBox1.Clear();
                List<Know> list = dbConteiner.Know.ToList();
                List<know2> lip = dbConteiner.know2.ToList();
                s1 = new List<string>(0);
                foreach (know2 sen in lip)
                {
                    string ret = sen.condition;
                    int it = ret.IndexOf(',');
                    while (it != -1)
                    {
                        s1.Add(ret.Substring(0, it));
                        ret = ret.Remove(0, it + 2);
                        it = ret.IndexOf(',');
                    }
                    s1.Add(ret);
                    s2 = sen.rule;
                    sen1 = sen;
                    k = Convert.ToInt32(textBox5.Lines[0]);
                    var threads = new Thread[k];
                    for (int i = 0; i < k; ++i)
                    {
                        Thread thread1 = new Thread(new ParameterizedThreadStart(diff1));
                        threads[i] = thread1;
                        thread1.Start(i);

                    }
                    for (int i = 0; i < k; ++i)
                    {
                        threads[i].Join();
                    }
                    s1.Clear();
                }

            }
            richTextBox1.Text = conclusion;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            WWWEntities contextEntities = new WWWEntities();
            List<know2> lip = contextEntities.know2.ToList();
            List<Know> list = contextEntities.Know.ToList();
            Hashtable map = new Hashtable();
            s1 = new List<string>(0);
            s3 = new List<string>(0);
            f = new List<int>(0);
            List<int> znach_ind = new List<int>(0);
            int index = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (!map.ContainsKey(list[i].Knowledge))
                    map.Add(list[i].Knowledge, i);
                if (list[i].Knowledge == "Поставка")
                    polus = i;
            }
            foreach (Know item in list)
            {
                if (item.Knowledge == "автомобиль")
                    break;
                index++;
            }
            int z1, z2;
            foreach (know2 item in lip)
            {

                string ret = item.condition;
                z1 = Convert.ToInt32(map[item.rule]);
                a[z1, 0].sled = 0;
                int it = ret.IndexOf(',');
                //производится разбиение условий правил на части
                while (it != -1)
                {
                    z2 = Convert.ToInt32(map[ret.Substring(0, it)]);
                    a[z1, a[z1, 0].sled + 1].sled = z2;//добавление нового ребра
                    //a[z1, a[z1, 0].sled + 1].pos = 0;
                    a[z1, 0].sled = a[z1, 0].sled + 1;
                    ret = ret.Remove(0, it + 2);
                    it = ret.IndexOf(',');
                }

                z2 = Convert.ToInt32(map[ret]);
                if (z2 == polus)
                {
                    a[z1, 0].gotov = 1;

                }
                else
                    a[z1, 0].gotov = 0;
                a[z1, 0].znach = Convert.ToInt32(item.time);
                a[z1, a[z1, 0].sled + 1].sled = z2;
                //a[z1, a[z1, 0].pos + 1].pos = 0;
                a[z1, 0].sled = a[z1, 0].sled + 1;
                //richTextBox1.Text +=item.rule+" "+ Convert.ToString(a[z1, 0].sled) + "\n";

            }
            Counter z = new Counter();
            z.y = index;
            a[polus, 0].gotov = 2;
            z.x = DateTime.Now.Ticks;
            t1 = DateTime.Now;
            int kol_thread = 1;
            kol_thread = Convert.ToInt32(textBox5.Lines[0]);
            var threads = new Thread[kol_thread];
            for (int i = 0; i < kol_thread; ++i)
            {
                Thread thread1 = new Thread(new ParameterizedThreadStart(diff3));
                threads[i] = thread1;
                thread1.Start(z);

            }
            for (int i = 0; i < kol_thread; ++i)
            {
                threads[i].Join();
            }
            diff4();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            diff4();
        }
        void diff4()
        {
            WWWEntities contextEntities = new WWWEntities();
            List<know2> lip = contextEntities.know2.ToList();
            List<Know> list = contextEntities.Know.ToList();
            for (int i = 0; i < 200; ++i)
            {
                if (a[i, 0].time != 0)
                {
                    list[Convert.ToInt32(i)].time = Convert.ToInt32(a[i, 0].time);
                    contextEntities.SaveChanges();
                }
                a[i, 0].time = 0;
                a[i, 0].gotov = 0;
                a[i, 0].znach = 0;
                a[i, 0].pos = 0;
                for (int j = 0; j < 11; ++j)
                    a[i, j].sled = 0;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox5.Text = "1";
        }
        void diff1(object z)
        {
            WWWEntities dbConteiner = new WWWEntities();
            List<Know> list = dbConteiner.Know.ToList();
            string s;
            int it;
            for (int j = z.GetHashCode(); j < list.Count(); j = j + k)
            {
                int l = 0;
                s = "";
                for (int i = 0; i < s1.Count(); ++i)
                {
                    it = strok(Convert.ToString(s1[i]), Convert.ToString(list[j].Knowledge));
                    if (it == 0)
                    {
                        l = 1;
                        break;
                    }
                    if (it < 3 && it > 0)
                        s += "Если " + Convert.ToString(sen1.condition) + " то " + Convert.ToString(sen1.rule) + " предполагаемая замена - " + Convert.ToString(list[j].Knowledge) + "\n";

                }
                it = strok(Convert.ToString(s2), Convert.ToString(list[j].Knowledge));
                if (it == 0 || l == 1)
                    continue;
                if (it < 3 && it > 0)
                    s += "Если " + Convert.ToString(sen1.condition) + " то " + Convert.ToString(sen1.rule) + " предполагаемая замена - " + Convert.ToString(list[j].Knowledge) + "\n";
                conclusion += s;
            }

        }
        int strok(string s1, string s2)//расстояние левенштейна
        {
            int[,] myArr = new int[s1.Count() + 1, s2.Count() + 1];
            myArr[0, 0] = 0;
            for (int j = 1; j <= s2.Count(); ++j)
                myArr[0, j] = myArr[0, j - 1] + 1;
            for (int i = 1; i <= s1.Count(); ++i)
            {
                myArr[i, 0] = myArr[i - 1, 0] + 1;
                for (int j = 1; j <= s2.Count(); ++j)
                {
                    if (s1[i - 1] != s2[j - 1])
                        myArr[i, j] = Math.Min(Math.Min(myArr[i - 1, j], myArr[i, j - 1]), myArr[i - 1, j - 1]) + 1;
                    else
                        myArr[i, j] = myArr[i - 1, j - 1];
                }
            }
            return myArr[s1.Count(), s2.Count()];
        }
        void diff3(object obj)
        {
            WWWEntities contextEntities = new WWWEntities();
            List<Know> list = contextEntities.Know.ToList();
            Counter c = (Counter)obj;
            Counter z1 = new Counter();
            DateTime t2 = DateTime.Now;
            //if(list[Convert.ToInt32(c.y)].confidence==1)
            if (a[c.y, 0].gotov == 1)
            {
                a[c.y, 0].gotov = 3;
                //list[Convert.ToInt32(c.y)].confidence = 3;
                Thread.Sleep(a[c.y, 0].znach);
                a[c.y, 0].gotov = 2;
                list[Convert.ToInt32(c.y)].confidence = 2;
                int ort = a[c.y, 0].znach;
                a[c.y, 0].time = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond - t1.Second * 1000 - t1.Millisecond;
                list[Convert.ToInt32(c.y)].time = Convert.ToInt32(a[c.y, 0].time);
                contextEntities.SaveChanges();
                //richTextBox1.Text += "Доствавка - " + Convert.ToString(list[Convert.ToInt32(c.y)].Knowledge) + ",  " + Convert.ToString(a[c.y, 0].time) + "\n";
            }
            else
            {
                int kol = 0;
                while (a[c.y, 0].sled != kol)
                {

                    for (int i = 1; i <= a[c.y, 0].sled; ++i)
                    {

                        //z1.x = c.x;
                        long y = a[c.y, i].sled;
                        int o1 = a[y, 0].gotov; long o2 = a[y, 0].pos;
                        a[c.y, 0].pos = 0;
                        //if (list[Convert.ToInt32(y)].confidence != 2 && a[y, 0].pos == 0)
                        if (a[y, 0].gotov != 2 && a[y, 0].pos == 0)
                        {
                            z1.x = c.x;
                            z1.y = y;
                            a[y, 0].pos = 1;
                            diff3(z1);

                        }

                    }
                    kol = 0;
                    for (int i = 1; i <= a[c.y, 0].sled; ++i)
                    {
                        long y = a[c.y, i].sled;
                        //if (list[Convert.ToInt32(y)].confidence == 2)
                        if (a[y, 0].gotov == 2)
                            kol = kol + 1;
                    }
                }
                //if(list[Convert.ToInt32(c.y)].confidence==0)
                if (a[c.y, 0].gotov == 0)
                {
                    a[c.y, 0].gotov = 3;
                    //list[Convert.ToInt32(c.y)].confidence = 3;
                    Thread.Sleep(a[c.y, 0].znach);
                    a[c.y, 0].gotov = 2;
                    list[Convert.ToInt32(c.y)].confidence = 2;
                    if (a[c.y, 0].time == 0)
                    {
                        a[c.y, 0].time = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond - t1.Second * 1000 - t1.Millisecond;
                        //List<Know> list = contextEntities.Know.ToList();
                        list[Convert.ToInt32(c.y)].time = Convert.ToInt32(a[c.y, 0].time);
                        contextEntities.SaveChanges();
                        richTextBox1.Text += "Сборка - " + Convert.ToString(list[Convert.ToInt32(c.y)].Knowledge)
                            + ",  " + Convert.ToString(Convert.ToInt32(a[c.y, 0].time / 5)) + "\n";
                    }
                }
            }
        }
    }
}
