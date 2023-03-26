using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csvreading4
{
    public class node
    {
        public person person_node = new person();
        public Label person_label = new Label();
        public int isrootnode=0;
        public int isparent = 0;
        public int generation;
        public int islayouted=0;
        public Point nodeloc;

        public node partner;
            
        public List<node> parentlist = new List<node>();

        public List<node> personlist = new List<node>();

        public node(person person1,int gen) 
        {
            person_node = person1;
            generation = gen;
            
            person_label.AutoSize = true;
            person_label.Font = new System.Drawing.Font("Inter V", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            person_label.Location = new System.Drawing.Point(1020, 20);
            person_label.Name = person_node.Id;
            person_label.Size = new System.Drawing.Size(46, 21);
            person_label.TabIndex = 2;
            person_label.Text = $"{person_node.Name} {person_node.LastName}";
            person_label.BackColor = Color.PowderBlue;
            if (this.person_node.personlist.Count > 0 || this.person_node.isroot == 1)
            {
                this.isparent = 1;
                //Console.WriteLine("parent" + this.person_node.Name);
            }
        }


        public Point drawcouplelines(node nod,Graphics g)
        {
            Point p = new Point();
           
            if (nod.person_node.Partner != null && nod.generation > 1)
            {
                Pen pen = new Pen(Color.Blue, 3);
                int len = 0;
                if (nod.generation == 1) { len = 60; }
                if (nod.generation == 2) { len = 40; }
                if (nod.generation == 3) { len = 20; }
                if (nod.generation == 4) { len = 20; }
                if (nod.generation == 5) { len = 20; }

                Point temp1 = new Point(nod.person_label.Location.X + nod.person_label.Width, nod.person_label.Location.Y + nod.person_label.Height / 2);
                Point temp2 = new Point(temp1.X + len, temp1.Y);
                if (nod.person_node.Name.Equals("Zeynep"))
                {
                    temp1 = new Point(nod.person_label.Location.X - nod.person_label.Width, nod.person_label.Location.Y + nod.person_label.Height / 2);
                    temp2 = new Point(temp1.X - len, temp1.Y);
                    g.DrawLine(pen, temp1, temp2);
                    Point temp4 = new Point((temp1.X + temp2.X) / 2, (temp1.Y + temp2.Y) / 2);
                    return temp4;
                }
                else
                    g.DrawLine(pen, temp1, temp2);



                Point temp3 = new Point((temp1.X + temp2.X) / 2, (temp1.Y + temp2.Y) / 2);

                return temp3;
                p = temp3;
            }
            else
                return p;
        }

        public void drawparentchild(Point parentloc, node nod, Graphics g)
        {
            Pen pen = new Pen(Color.Blue, 1);
            g.DrawLine(pen, parentloc, nod.person_label.Location);
        }
        
    }
}
