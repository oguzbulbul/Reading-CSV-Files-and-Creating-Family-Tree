using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csvreading4
{

    public class person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Partner { get; set; }
        public string PartnerId { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public string BloodType { get; set; }
        public string Job { get; set; }
        public string CivilStatus { get; set; }
        public string OldLastName { get; set; }
        public string Gender { get; set; }

        public int whichfamily = 0;
        public int isLabeled=0;
        public int generation=-1;
        public int isroot = 0;
        public int age;

        public List<person> parentlist = new List<person>();

        public List<person> personlist = new List<person>();


        public void addkid(person child)
        {
            this.personlist.Add(child);
        }
        public void addparent(person parent)
        {
            this.parentlist.Add(parent);
        }


        public void showkids()
        {
            for(int i = 0; i < personlist.Count; i++)
            {
                Console.WriteLine(personlist[i].Name + "\t" + personlist[i].LastName);
            }
        }

    }
}
