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
    public partial class Form2 : Form
    {
        public Form2()
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && textBox4.Text != "")
            {
                using (WWWEntities3 dbConteiner = new WWWEntities3())
                {
                    int v1, v2;//1 обозначает ручной ввод, 2 машинный
                    Random rand = new Random();
                    DateTime dt, dt2= new DateTime();
                    DateTime d = new DateTime(2020,10,24,0,0,0);
                    dt = dateTimePicker1.Value;//1
                    /*v1 = rand.Next(20);//2
                    v2 = rand.Next(59);
                    
                    d=d.AddHours(v1);
                    d=d.AddMinutes(v2);
                    dt = d;
                    v1 =rand.Next( 23 - v1);
                    v2 = rand.Next(59);
                    d=d.AddHours(v1);
                    d=d.AddMinutes(v2);
                    dt2 = d;*/
                    dt2 = dateTimePicker2.Value;//1
                    string s1, s2;
                    s1 = comboBox1.Text;
                    s2 = comboBox2.Text;
                    List<city> list = dbConteiner.city.ToList();
                    int fl1=1, fl2=1;
                    foreach(city i in list)
                    {
                        if(i.city1==s1)
                        {
                            fl1 = 0;
                            break;
                        }
                    }
                    if (fl1 == 1)
                    {
                        comboBox1.Items.Add(s1);
                        comboBox2.Items.Add(s1);
                        dbConteiner.city.Add(new city
                        {
                            Id = dbConteiner.city.ToList().Count,
                            city1 = s1
                        });
                    }
                    foreach (city i in list)
                    {
                        if (i.city1 == s2)
                        {
                            fl2 = 0;
                            break;
                        }
                    }
                    if (fl2 == 1)
                    {
                        comboBox1.Items.Add(s2);
                        comboBox2.Items.Add(s2);
                        dbConteiner.city.Add(new city
                        {
                            Id = dbConteiner.city.ToList().Count,
                            city1 = s2
                        });
                    }
                    Hashtable map2 = new Hashtable();
                    List<Class_serv> list2 = dbConteiner.Class_serv.ToList();
                    for (int i = 0; i < list2.Count; i++)
                    {
                        map2.Add(list2[i].@class, i + 1);
                    }
                    dbConteiner.Case.Add(new Case
                    {
                        Id = dbConteiner.Case.ToList().Count + 1,
                        location_of_dispatch = comboBox1.Text,
                        place_of_arrival = comboBox2.Text,
                        time_of_origin = dt,
                        arrival_time = dt2,
                        price = Convert.ToInt32(textBox4.Lines[0]),//1
                        class_of_service = Convert.ToInt32(map2[comboBox3.Text])//1
                        //price = rand.Next(10000) + 10000,//2
                        //class_of_service = rand.Next(6)+1//2

                    });
                    dbConteiner.SaveChanges();

                }
                textBox4.Clear();
            }

        }
    }
}
