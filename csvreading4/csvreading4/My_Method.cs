using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace csvreading4
{
    public class My_Methods 
    {

        int gen, i = 0, j, k, l;
        int kontrol = 0;
        int gencounter = 2;

        int genmember2 = 0;
        int genmember3 = 0;
        int genmember4 = 0;
        int genmember5 = 0;
        int genmember6 = 0;

        Point P1 = new Point(1020, 20);
        Point P2 = new Point(500, 60);
        Point P3 = new Point(500, 100);
        Point tmp1;
        Point tmp2;
        

        public void calcgenmembers(List<person> people)
        {
            foreach (person p in people)
            {
                if (p.generation == 2)
                {
                    this.genmember2 += 1;
                }
                else if (p.generation == 3)
                {
                    this.genmember3 += 1;
                }
                else if (p.generation == 4)
                {
                    this.genmember4 += 1;
                }
                else if (p.generation == 5)
                {
                    this.genmember5 += 1;
                }
                else if (p.generation == 6)
                {
                    this.genmember6 += 1;
                }
                //Console.WriteLine("genmember2: " + this.genmember2);
                //Console.WriteLine("genmember3: " + this.genmember3);
                //Console.WriteLine("genmember4: " + this.genmember4);
                //Console.WriteLine("genmember5: " + this.genmember5);
                //Console.WriteLine("genmember6: " + this.genmember6);
            }
        }
        public int calc_generations(person root)
        {
            if (root == null) return 0;
            int sum = 0;
            foreach (person child in root.personlist)
            {
                sum += 1 + calc_generations(child);
            }
            return sum;
        }


        public void calc_ages(List<person> people)
        {
            foreach (person pers in people)
            {
                if(pers.whichfamily == 1)
                {
                    string str_age = pers.BirthDate.Substring(pers.BirthDate.Length - 4, 4);
                    Console.WriteLine(str_age);
                    int int_age = int.Parse(str_age);
                    pers.age = (2022 - int_age);
                }
                if (pers.whichfamily == 2) 
                { 
                string str_age = pers.BirthDate.Substring(pers.BirthDate.Length - 4, 4);
                int int_age = int.Parse(str_age);
                pers.age = (2022 - int_age);
                }
            }
        }


        public void show_generations(List<person> people)
        {
            for (i = 0; i < people.Count; i++)
            {
                Console.WriteLine($"{people[i].Name}\t{people[i].generation}");
            }
        }


        public void findroots(List<person> people)
        {
            foreach (person pers in people)
            {
                if (pers.parentlist.Count == 0)
                {
                    Console.WriteLine(pers.Id + "\t" + pers.Name + "\t" + pers.LastName);
                    pers.isroot = 1;
                }
            }

        }

        public int findTreeDeepness(person p)
        {
            if (p.personlist.Count == 0)
            {
                return 1;
            }

            int maxDepth = 1;
            foreach (person child in p.personlist)
            {
                int depth = findTreeDeepness(child);
                if (depth > maxDepth)
                {
                    maxDepth = depth;
                }
            }

            return maxDepth + 1;
        }


        public void showtreefromroot(person root)
        {
            if (root == null) return;
            Console.WriteLine(root.Name + "\t" + root.LastName + "\t" + root.generation);
            foreach (person child in root.personlist)
            {
                showtreefromroot(child);
            }
        }

        public void deleteduplicate(List<person> people)
        {
            for (i = 0; i < people.Count; i++)
            {
                for (j = i + 1; j < people.Count; j++)
                {
                    if (people[i].Id == people[j].Id)
                    {
                        people.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        public void locatelabels(List<person> people, int maxdeep, node[] nod)
        {
            int i = 0;
            Console.WriteLine("maxdeep is " + maxdeep);
            for (l = 0; l <= maxdeep; l++)
            {

                foreach (person pers in people)
                {
                    if (pers.generation == l)
                    {
                        Console.WriteLine(i + "===============" + pers.Name + "\t" + pers.generation);
                        nod[i] = new node(pers, l);
                        i++;
                    }

                }
            }
        }


        public void findrootnode(node[] nodes)
        {
            foreach (node nod in nodes)
            {
                if (nod.person_node.isroot == 1)
                {
                    nod.isrootnode = 1;
                }
            }
        }

        public void nodeconnections(node[] node_list)
        {
            for (i = 0; i < node_list.Length; i++)
            {
                for (j = 0; j < node_list.Length; j++)
                {
                    bool check = node_list[i].person_node.Partner.Equals(null);
                    if (node_list[i].person_node.Name.Equals(node_list[j].person_node.MotherName) && (node_list[i].person_node.Partner.Contains(node_list[j].person_node.FatherName)) && (check == false))
                    {
                        if (node_list[j].person_node.Gender.Equals("Kadın") && node_list[j].person_node.OldLastName.Equals(node_list[i].person_node.LastName))
                        {
                            node_list[i].personlist.Add(node_list[j]);
                            node_list[j].parentlist.Add(node_list[i]);
                            //Console.WriteLine(people[i].Name + "\t" + people[j].Name);
                        }
                        else
                        {
                            node_list[i].personlist.Add(node_list[j]);
                            node_list[j].parentlist.Add(node_list[i]);
                            //Console.WriteLine(people[i].Name + "\t" + people[j].Name);
                        }
                    }
                    else if (node_list[i].person_node.Name.Equals(node_list[j].person_node.FatherName) && (node_list[i].person_node.Partner.Contains(node_list[j].person_node.MotherName)) && (check == false))
                    {
                        if (node_list[j].person_node.Gender.Equals("Kadın") && node_list[j].person_node.OldLastName.Equals(node_list[i].person_node.LastName))
                        {
                            node_list[i].personlist.Add(node_list[j]);
                            node_list[j].parentlist.Add(node_list[i]);
                            //Console.WriteLine(people[i].Name + "\t" + people[j].Name);
                        }
                        else
                        {
                            node_list[i].personlist.Add(node_list[j]);
                            node_list[j].parentlist.Add(node_list[i]);
                            //Console.WriteLine(people[i].Name + "\t" + people[j].Name);
                        }
                    }
                }
            }
        }

        Pen pen = new Pen(Color.Blue, 1);
        Control.ControlCollection controls = Form1.ActiveForm.Controls;

        public void layoutnodes2(node rootnode,int x,int y,Graphics g)
        {
           
            controls.Add(rootnode.person_label);
            Point parenttochild = rootnode.drawcouplelines(rootnode, g);
            rootnode.islayouted = 1;
            
            if(rootnode.person_node.Partner != null)  
            {
                person temppartner = new person();
                temppartner.generation = rootnode.generation;
                
                if (rootnode.person_node.Gender.Equals("Erkek")) { temppartner.Name = $"{rootnode.person_node.Partner} {rootnode.person_node.LastName}"; }
                else {temppartner.Name = $"{rootnode.person_node.Partner} {rootnode.person_node.LastName}"; }

                node nodetemppartner = new node(temppartner, rootnode.generation);
                rootnode.partner = nodetemppartner;

                int len=0;
                if(rootnode.generation == 1) { len = 60; }
                if (rootnode.generation == 2) { len = 40; }
                if (rootnode.generation == 3) { len = 20; }
                if(rootnode.generation == 4) { len = 20; }
                if(rootnode.generation == 5) { len = 20; }

                rootnode.partner.person_label.Location = new Point(x + rootnode.person_label.Width + len, y);
                controls.Add(rootnode.partner.person_label);
                            
            }
            

            rootnode.person_label.Location = new Point(x, y);

            int persX = x - rootnode.generation * 50;
            int persY = y + rootnode.person_label.Height + 40;
            foreach (node child in rootnode.personlist)
            {
                if(child.parentlist != null && parenttochild.X !=0 )
                {
                    
                        child.drawparentchild(parenttochild,child,g);
                    
                }
                controls.Add(child.person_label);
                layoutnodes2(child, persX, persY,g);
                persX += child.person_label.Width*4+30;
                //persY += child.person_label.Height + 20;
            }
        }


        //_________________________________________________________________________________________________________________________________________
        public enum Orientation
        {
            TopDown,
            BottomUp,
            LeftToRight,
            RightToLeft
        }


        public void LayoutTree(node root, Orientation orientation, int spacing, int maxDepth , int maxSize)
        {
            // Check if the node has already been laid out
            if (root.islayouted == 1)
            {
                return;
            }

            // Mark the node as laid out
            root.islayouted = 1;

            // Calculate the position for the node based on its generation and the positions of its parent nodes
            Point pos = CalculateNodePosition(root, orientation, spacing);
            if(root.isparent == 1) { pos.X += 50; }
            if(root.person_node.Partner != null && root.person_node.Gender.Equals("Kadın")) { pos.X += 100; }
            // Set the position of the node
            root.nodeloc = pos;
            root.person_label.Location = pos;
            root.person_label.Location = root.nodeloc;
            root.person_label.Size = new Size(100, 30);
            root.person_label.Text = $"{root.person_node.Name} {root.person_node.LastName}";
            Console.WriteLine(root.isparent + root.person_label.Text+"\t"+root.person_label.Location+"\t"+maxDepth);
            controls.Add(root.person_label);
            
            // If the tree is larger than the maximum size, scale the positions of the nodes down to fit within the maximum size
            if (orientation == Orientation.TopDown || orientation == Orientation.BottomUp)
            {
                ScalePositions(root, maxSize, orientation);
            }
            else
            {
                ScalePositions(root, maxSize, orientation);
            }

            // If the generation of the node is equal to the maximum depth, return without calling the function for its children
            if (root.generation == maxDepth)
            {
                return;
            }

            // Iterate through the child nodes and layout them recursively
            foreach (node child in root.personlist)
            {
                LayoutTree(child, orientation, spacing, maxDepth, maxSize);
            }
        }

        private Point CalculateNodePosition(node n, Orientation orientation, int spacing)
        {
            Point pos = new Point();

            // Calculate the position based on the orientation
            if (n.parentlist.Count > 0)
            {

                switch (orientation)
                {
                    case Orientation.TopDown:
                        // Position the node below the highest parent node, with the specified spacing between them
                        pos.Y = n.parentlist.Max(p => p.nodeloc.Y) + spacing;
                        pos.X = (int)n.parentlist.Average(p => p.nodeloc.X);
                        break;
                    case Orientation.BottomUp:
                        // Position the node above the lowest parent node, with the specified spacing between them
                        pos.Y = n.parentlist.Min(p => p.nodeloc.Y) - spacing;
                        pos.X = (int)n.parentlist.Average(p => p.nodeloc.X);
                        break;
                    case Orientation.LeftToRight:
                        // Position the node to the right of the furthest parent node, with the specified spacing between them
                        pos.X = n.parentlist.Max(p => p.nodeloc.X) + spacing;
                        pos.Y = (int)n.parentlist.Average(p => p.nodeloc.Y);
                        break;
                    case Orientation.RightToLeft:
                        // Position the node to the left of the closest parent node, with the specified spacing between them
                        pos.X = n.parentlist.Min(p => p.nodeloc.X) - spacing;
                        pos.Y = (int)n.parentlist.Average(p => p.nodeloc.Y);
                        break;
                }
            }
            
            return pos;
        }
        private void ScalePositions(node root, int maxSize, Orientation orientation)
        {
            // Calculate the size of the tree
            int treeSize = CalculateTreeSize(root, orientation);

            // If the tree is larger than the maximum size, scale the positions of the nodes down to fit within the maximum size
            if (treeSize > maxSize)
            {
                double scaleFactor = (double)maxSize / (double)treeSize;

                ScalePositionsRecursive(root, scaleFactor, orientation);
            }
        }

        private int CalculateTreeSize(node root, Orientation orientation)
        {
            // Calculate the size of the tree based on the orientation
            if (orientation == Orientation.TopDown || orientation == Orientation.BottomUp)
            {
                return CalculateTreeHeight(root);
            }
            else
            {
                return CalculateTreeWidth(root);
            }
        }

        private int CalculateTreeHeight(node root)
        {
            // If the root node is null, return 0
            if (root == null)
            {
                return 0;
            }

            // Initialize the height to the generation of the root node
            int height = root.generation;

            // Iterate through the child nodes and calculate the height of the subtree rooted at each child node
            foreach (node child in root.personlist)
            {
                int childHeight = CalculateTreeHeight(child);

                // Update the height if the subtree rooted at the child node has a greater height
                if (childHeight > height)
                {
                    height = childHeight;
                }
            }

            return height;
        }

        private int CalculateTreeWidth(node root)
        {
            // If the root node is null, return 0
            if (root == null)
            {
                return 0;
            }

            // Initialize the width to the position of the root node
            int width = root.nodeloc.X;

            // Iterate through the child nodes and calculate the width of the subtree rooted at each child node
            foreach (node child in root.personlist)
            {
                int childWidth = CalculateTreeWidth(child);

                // Update the width if the subtree rooted at the child node has a greater width
                if (childWidth > width)
                {
                    width = childWidth;
                }
            }

            // Return the width of the tree
            return width;
        }


        private void ScalePositionsRecursive(node root, double scaleFactor, Orientation orientation)
        {
            // Scale the position of the current node
            root.nodeloc = ScalePosition(root.nodeloc, scaleFactor, orientation);

            // Scale the positions of the child nodes recursively
            foreach (node child in root.personlist)
            {
                ScalePositionsRecursive(child, scaleFactor, orientation);
            }
        }

        private Point ScalePosition(Point pos, double scaleFactor, Orientation orientation)
        {
            // Scale the position based on the orientation
            if (orientation == Orientation.TopDown || orientation == Orientation.BottomUp)
            {
                pos.Y = (int)(pos.Y * scaleFactor);
            }
            else
            {
                pos.X = (int)(pos.X * scaleFactor);
            }

            return pos;
        }








    }


}
