using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace lb1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Zoo zoo = new Zoo();
            //zoo.readFromFile();

            //zoo.showData();

            //zoo.writeToFile();

            //zoo.createReport();

            Menu.showMenu();
        }
    }

    class Menu
    {
        static public void showMenu()
        {
            Zoo zoo = new Zoo();

            while (true)
            {
                Console.WriteLine("1 - Read from XML");
                Console.WriteLine("2 - Show data");
                Console.WriteLine("3 - Write to XML");
                Console.WriteLine("4 - Create report");
                Console.WriteLine("5 - Add new animal");
                Console.WriteLine("6 - Delete animal by name");
                Console.WriteLine("7 - Exit\n");

                switch (Console.ReadLine())
                {
                    case "1":
                        zoo.readFromFile();
                        Console.WriteLine("Read succesfully!");
                        break;
                    case "2": zoo.showData(); break;
                    case "3":
                        zoo.writeToFile();
                        Console.WriteLine("Write succesfully!");
                        break;
                    case "4":
                        zoo.createReport();
                        Console.WriteLine("Report has created");
                        break;
                    case "5":
                        Console.WriteLine("Enter new name:");
                        string name = Console.ReadLine();

                        Console.WriteLine("Enter count of new animal:");
                        int count = Int32.Parse(Console.ReadLine());

                        zoo.addAnimal(new Animal(name, count));
                        break;
                    case "6":
                        Console.WriteLine("Enter name of animal for deleting");
                        int resultOfDeleting = zoo.deleteAnimal(Console.ReadLine());
                        if(resultOfDeleting < 0)
                        {
                            Console.WriteLine("Not found!");
                        } else
                        {
                            Console.WriteLine("Delete succesfully");
                        }
                        break;
                    case "7": return;

                }

                Console.ReadLine();
                Console.Clear();
            }

        }
    }

    class Animal
    {
        private string name;
        private int count;

        public Animal(string name, int count)
        {
            this.name = name;
            this.count = count;
        }


        //public string Name => name; 
        public string Name
        {
            get { return name; }
        }

        public int Count
        {
            get { return count; }
        }
    }

    class Zoo
    {
        private List<Animal> animals;

        public Zoo()
        {
            animals = new List<Animal>();
        }

        public Zoo(string path)
        {
            readFromFile(path);
        }

        public void addAnimal(Animal animal)
        {
            animals.Add(animal);
        }

        public int deleteAnimal(string name)
        {
            for(int i = 0; i < animals.Count; i++)
            {
                if (animals.ElementAt(i).Name.Equals(name))
                {
                    animals.RemoveAt(i);
                    return i;
                }
            }
            return -1;
        }

        public void showData()
        {
            Console.WriteLine("Vivod data");

            foreach (Animal animal in animals)
            {
                Console.WriteLine("Name: {0}", animal.Name);
                Console.WriteLine("Count: {0}", animal.Count);
            }
            Console.WriteLine();
        }

        public void readFromFile(string path = "dataXml.xml")
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {
                string name = null;
                int count = -1;
                foreach (XmlNode childnode in xnode.ChildNodes)
                {

                    if (childnode.Name == "name")
                    {
                        name = childnode.InnerText;
                    }
                    if (childnode.Name == "count")
                    {
                        count = Int32.Parse(childnode.InnerText);

                    }
                    if (name != null && count != -1)
                    {
                        addAnimal(new Animal(name, count));

                        name = null;
                        count = -1;
                    }
                }
            }
        }

        public void writeToFile(string path = "dataXmlTo.xml")
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlElement elementZoo = doc.CreateElement("zoo");
            doc.AppendChild(elementZoo);

            foreach (Animal animal in animals)
            {
                XmlElement elementAnimal = doc.CreateElement("animal");
                elementZoo.AppendChild(elementAnimal);

                XmlElement elementName = doc.CreateElement("name");
                XmlText textName = doc.CreateTextNode(animal.Name);
                elementName.AppendChild(textName);
                elementAnimal.AppendChild(elementName);

                XmlElement elementCount = doc.CreateElement("count");
                XmlText textCount = doc.CreateTextNode(animal.Count.ToString());
                elementCount.AppendChild(textCount);
                elementAnimal.AppendChild(elementCount);
            }

            doc.Save(path);
        }

        public void createReport(string path = "dataTxt.txt")
        {
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            string head = "Zoo report \nDate: " + DateTime.Now.ToString("yyyy - MM - dd HH: mm\n") + "We have:\n";

            streamWriter.WriteLine(head);

            int totalAnimalCount = 0;

            foreach (Animal animal in animals)
            {
                string data = string.Empty;

                data += animal.Count + " of " + animal.Name;
                totalAnimalCount += animal.Count;

                streamWriter.WriteLine(data);
            }

            string footer = "\nTotal we have: " + totalAnimalCount.ToString() + " animals.";

            streamWriter.WriteLine(footer);

            streamWriter.Close();
            fileStream.Close();
        }
    }


}
