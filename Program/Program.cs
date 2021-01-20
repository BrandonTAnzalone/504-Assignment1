using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildTest
{
    public static class Program
    {
        #region Global Variables
        public static uint MAX_LEVEL = 60;
        public static uint GEAR_SLOTS = 14;
        public static int MAX_INVENTORY_SIZE = 20;
        public static uint MAX_ILLVL = 360;
        public static uint MAX_PRIMARY = 200;
        public static uint MAX_STAMINA = 275;
        #endregion

        public enum ItemType
        {
            Helmet, Neck, Shoulders, Back, Chest,
            Wrist, Gloves, Belt, Pants, Boots,
            Ring, Trinket
        };

        public enum Race { Orc, Troll, Tauren, Forsaken };

        public class Item : IComparable
        {
            // Private attributes
            private uint id;
            private string name;
            private ItemType type;
            private uint ilvl;
            private uint primary;
            private uint stamina;
            private uint requirement;
            private string flavor;

            public uint Id
            {
                get { return id; }
                private set { id = value; }
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public ItemType Type
            {
                get { return type; }
                set
                {
                    if (value >= (ItemType)0 || value <= (ItemType)11)
                        type = value;
                    else
                    {
                        Console.WriteLine(value + " is not a valid item type");
                    }
                }
            }

            public uint Ilvl
            {
                get { return ilvl; }
                set
                {
                    if (value >= 0 || value <= MAX_ILLVL)
                        ilvl = value;
                    else
                    {
                        Console.WriteLine(value + " is not a valid level");
                    }
                }
            }

            public uint Primary
            {
                get { return primary; }
                set
                {
                    if (value >= 0 || value <= MAX_PRIMARY)
                        primary = value;
                    else
                    {
                        Console.WriteLine(value + " is not a valid Primary");
                    }
                }
            }

            public uint Stamina
            {
                get { return stamina; }
                set
                {
                    if (value >= 0 || value <= MAX_STAMINA)
                        stamina = value;
                    else
                    {
                        Console.WriteLine(value + " is not valid stamina");
                    }
                }
            }

            public uint Requirement
            {
                get { return requirement; }
                set
                {
                    if (value >= 0 || value <= MAX_LEVEL)
                        requirement = value;
                    else
                    {
                        Console.WriteLine(value + "is not a valid requirement");
                    }
                }
            }

            public string Flavor
            {
                get { return flavor; }
                set { flavor = value; }
            }

            // Default Constructor
            public Item()
            {
                Id = 0;
                Name = "";
                Type = 0;
                Ilvl = 0;
                Primary = 0;
                Stamina = 0;
                Requirement = 0;
                Flavor = "";
            }

            // Constructor
            public Item(uint iId, string iName, ItemType iType, uint iIlvl, uint iPrimary, uint iStamina, uint iRequirement, string iFlavor)
            {
                Id = iId;
                Name = iName;
                Type = iType;
                Ilvl = iIlvl;
                Primary = iPrimary;
                Stamina = iStamina;
                Requirement = iRequirement;
                Flavor = iFlavor;
            }

            // ToString Method
            public override string ToString()
            {
                //Displays the item type, name, ilvl, requirement, and flavor
                string output = String.Format("({0}) {1} |{2}| --{3}--\n\t\"{4}\"", this.Type, this.Name, this.Ilvl, this.Requirement, this.Flavor);
                return output;
            }

            // IComparable Method
            public int CompareTo(object alpha)
            {
                //Check for null values
                if (alpha == null) return 1;

                //typecast
                Item rightOp = alpha as Item;

                //Items are compared by name
                if (rightOp != null)
                    return Name.CompareTo(rightOp.Name);
                else
                    throw new ArgumentException("Item CompareTo argument is not an item");
            }

        }

        public static string EquipmentFile = @"..\..\..\Program\equipment.txt";
        public static Dictionary<uint, Item> ItemDictionary = new Dictionary<uint, Item>();

        public static int Main()
        {
            // Title
            Console.WriteLine("Welcome to the World of ConflictCraft: Testing Environment!\n");
            ReadItems(EquipmentFile);

            string s = "";

            do
            {
                Console.WriteLine("\nWelcome to World of ConflictCraft: Testing Environment. Please select an option from the list below:");
                Console.WriteLine("\t1.) Print All Gear");
                Console.WriteLine("\t2.) Quit");

                s = Console.ReadLine();

                switch (s.ToString())
                {
                    case "1":
                        ListItems();
                        break;
                    case "2":
                        s = "2";
                        break;
                    default:
                        Console.WriteLine(s + " is not an option.");
                        break;
                }
            } while (s != "2");

            return 0;
        }

        // Read items
        public static void ReadItems(string input)
        {
            // String to read lines
            string inputLine;

            // Try block to catch filenotfound
            try
            {
                using (StreamReader inFile = new StreamReader(input))
                {
                    inputLine = inFile.ReadLine();

                    while (inputLine != null)
                    {
                        // Separate with tab 
                        string[] inItems = inputLine.Split('\t');
                        // If the correct number of attributes are on the line, create an item from line
                        if (inItems.Length == 8)
                        {
                            // Create the item from the array, convert string to uints and ItemTypes when necessary
                            Item inputItem = new Item(Convert.ToUInt32(inItems[0]), inItems[1], (ItemType)Convert.ToUInt32(inItems[2]), Convert.ToUInt32(inItems[3]), Convert.ToUInt32(inItems[4]),
                                                     Convert.ToUInt32(inItems[5]), Convert.ToUInt32(inItems[6]), inItems[7]);
                            // Add to dictionary of items
                            ItemDictionary.Add(inputItem.Id, inputItem);
                        }

                        // Read next line
                        inputLine = inFile.ReadLine();
                    } // end of while
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(input + "file does not exist");
            }
        }

        // Print items
        public static void ListItems()
        {
            foreach (KeyValuePair<uint, Item> pair in ItemDictionary)
            {
                Console.WriteLine("{0}", pair.Value);
            }
        }
    }
}