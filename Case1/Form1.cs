using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Case1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            WWWEntities3 contextEntities = new WWWEntities3();
            List<city> list = contextEntities.city.ToList();
            foreach (city i in list)
            {
                comboBox1.Items.Add(i.city1);
                comboBox2.Items.Add(i.city1);
            }
            List<Class_serv> list1 = contextEntities.Class_serv.ToList();
            foreach (Class_serv i in list1)
            {
                comboBox3.Items.Add(i.@class);
            }
        }
        
        struct ty
        {
            public DateTime x, y;//дата прибытия и отбытия
            public double p;//цена
            public int c;//класс
        }
        public struct zn
        {
            public double p,c;
            public int o;
        }
        int n;
        ty[,,] a = new ty[50, 50, 100];//массив дерева полетов
        public zn[] rez = new zn[10000];//массив для сохранения результата
        int pl1, pl2,kp;
        void rot(int[,] r,int k,int nach)//составление древа отрезков
        {
            for(int i=0; i<n; ++i)
            {
                if(a[nach,i,0].c>0)
                {
                    if(i==pl2)
                    {
                        DateTime d = new DateTime(2020, 10, 24, 0, 0, 0);
                        r[i, 0] = k + 1;
                        r[nach, 1] = i;
                        znah(r,pl1,d,0,0,0,0);
                    }
                    else
                        if(r[i,0]==0)
                        {
                            r[i,0] = k + 1;
                            r[nach, 1] = i;
                            rot(r, k + 1, i);
                            r[i,0] = 0;
                            r[nach, 1] = 0;
                        }


                }
            }
        }
        void znah(int[,] t,int g,DateTime d,int ti,double pr,int cl,int kol)
        {
            DateTime d2 = d;
            int ti0 = ti;
            double pr0 =pr,l;
            if(g==pl2)
            {
                l =(double)cl / kol;
                //richTextBox1.Text +="Время="+ ti+" Цена="+pr +" Средний класс комфорта="+l+ "\n";
                rez[kp].c = pr;
                rez[kp].o = ti;
                rez[kp].p = l;
                kp++;
            }
            else
                for (int i = 1; i <= a[g, t[g, 1], 0].c; ++i)
                {
                    

                    if (a[g, t[g, 1], i].x>d)
                    {
                        d = a[g, t[g, 1], i].y;
                        TimeSpan s = a[g, t[g, 1], i].y - a[g, t[g, 1], i].x;
                        ti += s.Hours * 60 + s.Minutes ;
                        //ti += Convert.ToInt32(a[t[g, 0], t[g, 1], i].y - a[t[g, 0], t[g, 1], i].x);
                        pr += a[g, t[g, 1], i].p;
                        znah(t, t[g, 1], d, ti, pr,cl+ a[g, t[g, 1], i].c,kol +1);
                        ti = ti0;
                        pr = pr0;
                        d = d2;
                    }
                }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }


        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int z1, z2,l,j3;
            int[,] mas=new int[100,2];
            richTextBox1.Clear();
            Hashtable map = new Hashtable();
            Hashtable map2 = new Hashtable();
            WWWEntities3 contextEntities = new WWWEntities3();
            List<city> list0 = contextEntities.city.ToList();
            List<Class_serv> list2 = contextEntities.Class_serv.ToList();
            n = list0.Count;
            for (int i = 0; i < list0.Count; i++)
            {
                map.Add(list0[i].city1, i);
            }
            for (int i = 0; i < list2.Count; i++)
            {
                map2.Add(list2[i].@class, i+1);
            }
            //map.Add("Питер", 0);
            //map.Add("Екатеринбург", 1);
            //map.Add("Ростов", 2);
            //map.Add("Москва", 3);

            bool r1 = radioButton1.Checked;
            bool r2 = radioButton2.Checked;
            for (int i = 0; i < n; ++i)
            {
                mas[i, 0] = 0;
                mas[i, 1] = 0;
            }
            kp = 0;
            for (int i = 0; i < 10000; ++i)
            {
                rez[i].o = 1000000;
                rez[i].p = 10;
            }
            j3 = Convert.ToInt32(map2[comboBox3.Text]);
            pl1 = Convert.ToInt32(map[comboBox1.Text]);
            pl2 = Convert.ToInt32(map[comboBox2.Text]);
            mas[pl1,0] = 1;
            //WWWEntities2 contextEntities = new WWWEntities2();
            List<Case> list = contextEntities.Case.ToList();
            list.Sort((one, two) => Convert.ToString(one.time_of_origin).CompareTo(Convert.ToString(two.time_of_origin)));
            for (int i=0; i<list.Count; i++)
            {
                l= Convert.ToInt32(list[i].class_of_service);
                if (l <= j3)
                {
                    z1 = Convert.ToInt32(map[list[i].location_of_dispatch]);
                    z2 = Convert.ToInt32(map[list[i].place_of_arrival]);
                    a[z1, z2, a[z1, z2, 0].c + 1].x = Convert.ToDateTime(list[i].time_of_origin);
                    a[z1, z2, a[z1, z2, 0].c + 1].y = Convert.ToDateTime(list[i].arrival_time);
                    a[z1, z2, a[z1, z2, 0].c + 1].p = Convert.ToDouble(list[i].price);
                    a[z1, z2, a[z1, z2, 0].c + 1].c = l;
                    a[z1, z2, 0].c++;
                    //richTextBox1.Text += list[i].location_of_dispatch + "\n";
                }
            }
            rot(mas, 1,pl1);
            if (r1)
            {
                zn g;
                for (int i = 0; i < kp; ++i)
                {
                    int w = i;
                    for (int j = i + 1; j < kp; ++j)
                    {
                        if (rez[w].o > rez[j].o)
                            w = j;
                    }
                    g = rez[i];
                    rez[i] = rez[w];
                    rez[w] = g;
                }
            }
            else
            {
                zn g;
                for (int i = 0; i < kp; ++i)
                {
                    int w = i;
                    for (int j = i + 1; j < kp; ++j)
                    {
                        if (rez[w].p > rez[j].p)
                            w = j;
                    }
                    g = rez[i];
                    rez[i] = rez[w];
                    rez[w] = g;
                }
            }
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    a[i, j, 0].c = 0;
            for (int i = 0; i < kp; ++i)
                richTextBox1.Text += "Время=" + rez[i].o + " Цена=" + rez[i].c + " Средний класс комфорта=" + rez[i].p + "\n";
            //foreach (Case item in list)
        }
    }
}
