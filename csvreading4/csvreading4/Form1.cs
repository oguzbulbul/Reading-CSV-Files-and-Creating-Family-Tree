using CsvHelper;
using csvreading4;
using LINQtoCSV;
using System;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using CsvContext = LINQtoCSV.CsvContext;
using System.IO;
using System.Reflection.Emit;
using System.Net;
using System.Xml.Linq;
using System.Drawing;


namespace csvreading4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            

            InitializeComponent();
        }
        int i,j;
        List<person> roots = new List<person>();
        node[] node_list = new node[50];     // <---------------------
        List<person> people = new List<person>();

        List<List<person>> all_people = new List<List<person>>();



        int maxdeep;
        int famnum = 0;
        public enum Orientation
        {
            TopDown,
            BottomUp,
            LeftToRight,
            RightToLeft
        }


        DataTableCollection dtc;
        string path;
        private void button1_Click(object sender, EventArgs e)
        {
            famnum += 1;
            List<person> people = new List<person>();

            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "CSV Dosyalarý|*.csv|Excel Dosyalarý 97-2000|*.xls|Excel Dosyalarý|*.xlsx", Title = "DOSYALARI" })
            {

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    path = ofd.FileName;
                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {

                    }
                    Console.WriteLine(ofd.FileName);
                    using (var reader = new StreamReader(ofd.FileName))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {

                    }
                }

                var csvFileDescription = new CsvFileDescription
                {
                    FirstLineHasColumnNames = true,
                    IgnoreUnknownColumns = true,
                    SeparatorChar = ',',
                    UseFieldIndexForReadingData = false,
                };

                //var people = new List<person>();
                using (var reader = new StreamReader(path))
                {
                    string line;
                    //var people = new List<person>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        var person = new person
                        {
                            Id = values[0],
                            Name = values[1],
                            LastName = values[2],
                            BirthDate = values[3],
                            Partner = values[4],
                            PartnerId = values[5],
                            MotherName = values[6],
                            FatherName = values[7],
                            BloodType = values[8],
                            Job = values[9],
                            CivilStatus = values[10],
                            OldLastName = values[11],
                            Gender = values[12],
                        };
                        person.whichfamily = famnum;
                        people.Add(person); Console.WriteLine($"{person.Id} | {person.Name} | {person.LastName} | {person.BirthDate} | " +
                        $"{person.Partner} | {person.MotherName} | {person.FatherName} | {person.BloodType} | {person.Job} | " +
                        $"{person.CivilStatus} | {person.OldLastName} | {person.Gender} | ");

                    }       
                }
                people.RemoveAt(0);

                this.people = people;
                this.all_people.Add(people);
                Console.WriteLine("all people count : " + all_people.Count );
                for (i = 0; i <people.Count; i++)
                {
                    for (j = 0; j < people.Count; j++)
                    {
                        bool check = people[i].Partner.Equals(null);
                        if (people[i].Name.Equals(people[j].MotherName) && (people[i].Partner.Contains(people[j].FatherName)) && (check == false))
                        {
                            if (people[j].Gender.Equals("Kadýn") && people[j].OldLastName.Equals(people[i].LastName))
                            {
                                people[i].addkid(people[j]);
                                people[j].addparent(people[i]);
                                //Console.WriteLine(people[i].Name + "\t" + people[j].Name);

                            }
                            else
                            {
                                people[i].addkid(people[j]);
                                people[j].addparent(people[i]);
                                //Console.WriteLine(people[i].Name + "\t" + people[j].Name);


                            }
                        }
                        else if (people[i].Name.Equals(people[j].FatherName) && (people[i].Partner.Contains(people[j].MotherName)) && (check == false))
                        {
                            if (people[j].Gender.Equals("Kadýn") && people[j].OldLastName.Equals(people[i].LastName))
                            {
                                people[i].addkid(people[j]);
                                people[j].addparent(people[i]);
                                //Console.WriteLine(people[i].Name + "\t" + people[j].Name);
                            }
                            else
                            {
                                people[i].addkid(people[j]);
                                people[j].addparent(people[i]);
                                //Console.WriteLine(people[i].Name + "\t" + people[j].Name);

                            }
                        }
                    }
                }
                //_____________________________parent-child control done ____________________________
                foreach(person pers in people)
                {
                    Console.WriteLine(pers.Name);
                    pers.showkids();
                }


                //____________________________root bulma_____________________________________

                My_Methods met = new My_Methods();
                met.calc_ages(people);
                met.deleteduplicate(people);
                met.findroots(people);

                //List<person> roots = new List<person>();
                foreach (person pers in people)
                {
                    if (pers.isroot == 1 )
                    {
                        roots.Add(pers);
                    }
                }
                maxdeep = met.findTreeDeepness(roots[0]);

                
                Console.WriteLine("deepness of tree is " + maxdeep);
                Console.WriteLine($"roots \t {roots.Count} : {roots[0].Name}\t{roots[0].Partner}");
                Console.WriteLine("____________________");

                foreach(person pers in people)
                {
                    int tempdeep = met.findTreeDeepness(pers);
                    pers.generation = maxdeep - tempdeep + 1;
                    if (pers.Name.Equals("Mehmet") || pers.Name.Equals("Mustafa"))
                    {
                        pers.generation -= 1;
                    }
                }

                met.calcgenmembers(people);


                node_list = new node[people.Count];     // <---------------------
                met.locatelabels(people, maxdeep, node_list);
                met.findrootnode(node_list);
                met.nodeconnections(node_list);


                for (int v=0; v<node_list.Length; v++)
                {
                    if (node_list[v] != null)
                        Console.WriteLine(node_list[v].person_label.Text);
                }

                //met.layout_Nodes(node_list);

                Console.WriteLine("____________________");
                Console.WriteLine("____________________");


                IntPtr handle = this.Handle;
                PaintEventArgs paint = new PaintEventArgs(Graphics.FromHwnd(handle), this.ClientRectangle);

                Graphics g = this.CreateGraphics();
                Graphics g2 = this.CreateGraphics();
                Pen p2 = new Pen(Color.RosyBrown,4);
                foreach (node nod in node_list)
                {
                    if (nod.isrootnode == 1)
                    {
                        g2.DrawLine(p2,new Point(0, 20 + (famnum - 1) * 250 - 7), new Point(2400, 20 + (famnum - 1) * 250 - 7));
                        met.layoutnodes2(nod, 800,20+(famnum-1)*250,g);
                       //met.LayoutTree(nod, (My_Methods.Orientation)Orientation.TopDown, 50, maxdeep, 1000);
                    }
                }
            }

        }

       

        private void mission1button_Click_1(object sender, EventArgs e)
        {
            liste.Items.Clear();
            // çocuðu olmayanlarý bul
            List<person> childless = new List<person>();
            
            foreach(List<person> person_list in all_people)
            {
                foreach(person pers in person_list)
                {
                    if (pers.personlist.Count == 0)
                    {
                        childless.Add(pers);
                    }
                }
            }
            Console.WriteLine("\n\n" + "childless people sorted by age");
            liste.Items.Add("childless people sorted by age: ");
            //sort childless by person age
            childless.Sort((x, y) => x.age.CompareTo(y.age));
            foreach (person pers in childless)
            {
                Console.WriteLine(pers.Name + "\t" + pers.age);
                liste.Items.Add($"{pers.Name} - {pers.age}");
            }
        }
       

        private void mission3button_Click(object sender, EventArgs e)
        {
            liste.Items.Clear();

            // find blood type users input
            string bloodtype = mission3textbox.Text;
            Console.WriteLine("\n\n"+"people with blood type " + bloodtype);
            liste.Items.Add($"{bloodtype} kan gruplu insanlar : ");
            List<person> bloodtypepeople = new List<person>();
            
            foreach(List<person> person_list in all_people)
            {
                foreach(person pers in person_list)
                {
                    if (pers.BloodType.Equals(bloodtype))
                    {
                        Console.WriteLine(pers.Name);
                        bloodtypepeople.Add(pers);
                        liste.Items.Add($"{pers.Name}");
                    }
                }
            }
        }


        private void mission4button_Click(object sender, EventArgs e)
        {
            liste.Items.Clear();

            //find the persons who doing same jobs with parents or elders
            List<person> samejobpeople = new List<person>();
            foreach (List<person> person_list in all_people)
            {
                foreach (person pers1 in person_list)
                {
                    foreach (person pers2 in people)
                    {
                        if (pers1.Job.Equals(pers2.Job) && pers1.Job != "" && pers2.Job != "" && !pers1.Id.Equals(pers2.Id) && pers1.whichfamily.Equals(pers2.whichfamily))
                        {
                            if (pers1.generation < pers2.generation)
                            {
                                samejobpeople.Add(pers1);
                            }
                            else if (pers1.generation > pers2.generation)
                            {
                                samejobpeople.Add(pers1);
                            }
                            else if (pers1.generation == pers2.generation)
                            {
                                samejobpeople.Add(pers1);
                            }


                        }

                    }
                }
            }
            //delete duplicates samejobpeople   
            samejobpeople = samejobpeople.Distinct().ToList();
            Console.WriteLine("\n\n" + "people who doing same jobs with parents or elders");
            liste.Items.Add("atalarýyla ayný iþi yapanlar : ");
            foreach (person pers in samejobpeople)
            {
                Console.WriteLine(pers.Name + "\t" + pers.Job);
                liste.Items.Add($"{pers.Name} - {pers.Job}");
            }
        }

       

        private void mission5button_Click(object sender, EventArgs e)
        {
            liste.Items.Clear();

            //find persons in same age
            List<person> samenames = new List<person>();
            foreach (List<person> person_list in all_people)
            {
                foreach (person pers1 in person_list)
                {
                    foreach (person pers2 in people)
                    {
                        if (!pers1.Id.Equals(pers2.Id) && pers1.Name.Equals(pers2.Name) )
                        {
                            samenames.Add(pers1);
                            liste.Items.Add($"{pers1.Name} - {pers1.age}");
                        }

                    }
                }
            }
            samenames.Sort((x, y) => x.age.CompareTo(y.age));
            Console.WriteLine("\n\n" + "Ayný isme sahip kiþiler ve yaþlarý :");
            liste.Items.Add("Ayný isme sahip kiþiler ve yaþlarý : ");
            foreach (person pers in samenames)
            {
                Console.WriteLine(pers.Name + "\t" + pers.age);
                liste.Items.Add($"{pers.Name} - {pers.age}");
            }

        }


        private void mission6button_Click(object sender, EventArgs e)
        {

        }


        private void mission7button_Click(object sender, EventArgs e)
        {
            
        }



        private void mission8button_Click(object sender, EventArgs e)
        {
            liste.Items.Clear();

            My_Methods m = new My_Methods();
            foreach (List<person> person_list in all_people)
            {
                foreach (person p in person_list)
                {
                    if (p.isroot == 1)
                    {
                        int deepness = m.findTreeDeepness(p);
                        Console.WriteLine("\n\n" +p.LastName+ " Aðacýn maksimum derinliði : " + deepness);
                        liste.Items.Add($"{p.LastName} aðacýnýn maksimum derinliði {deepness}");
                        break;
                    }
                }
            }
        }


        private void mission9button_Click(object sender, EventArgs e)
        {
            liste.Items.Clear();

            My_Methods m = new My_Methods();
            string name = mission9textbox.Text;
            foreach (List<person> person_list in all_people)
            {
                foreach (person p in person_list)
                {
                    if (p.Name.Equals(name))
                    {
                        int subdeep = m.findTreeDeepness(p);
                        Console.WriteLine("\n\n" + $"{name} isminden sonra gelen aðacýn derinliði : " + subdeep);
                        liste.Items.Add($"{name} isminden sonra gelen aðacýn derinliði : {subdeep}");
                        break;
                    }
                }
            }

        }
    }
}