//***************************************************************************
//
//  Troy DeClerck       - Z1877438
//  Brandon Anzalone    - Z-------
//  CSCI 473/504
//
//  We certify that this is our own work and where appropriate an extension
//  of the starter code provided for the assignment
//***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
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

        // The Specific item types
        public enum ItemType
        {
            Helmet, Neck, Shoulders, Back, Chest,
            Wrist, Gloves, Belt, Pants, Boots,
            Ring, Trinket
        };

        // Each unique race
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

            // Read Only Access
            public uint Id
            {
                get { return id; }
                private set { id = value; }
            }

            // Read/Write Access
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            // Read/Write Access
            // Must EXCLUDE 0 and 12
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

            // Read/Write Access
            // Must be between 0 and max level
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

            // Read/Write Access
            // Must be between 0 and max primary
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

            // Read/Write Access
            // Must be between 0 and max stamina
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

            // Read/Write Access
            // Must be between 0 and max level
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

            // Read/Write Access
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

        public class Player : IComparable
        {
            // Private attributes
            private uint id;
            private string name;
            private Race playerRace;
            private uint level;
            private uint exp;
            private uint guildID;
            private uint[] gear;
            private List<uint> inventory;

            // Read Only Access
            public uint Id
            {
                get { return id; }
                private set { id = value; }
            }

            // Read Only Access
            public string Name
            {
                get { return name; }
                private set { name = value; }
            }

            // Read Only Access
            // Must be between 0 and 3
            public Race PlayerRace
            {
                get { return playerRace; }
                private set
                {
                    if (value >= (Race)0 || value <= (Race)3)
                        playerRace = value;
                    else
                    {
                        Console.WriteLine(value + " is not a valid race id");
                    }


                }
            }

            // Read/Write Access
            public uint Level
            {
                get { return level; }
                set
                {
                    if (value >= 0 || value <= MAX_LEVEL)
                        level = value;
                    else
                    {
                        Console.WriteLine(value + " is not a valid level");
                    }
                }
            }

            // Read Access, Write Access Increases Current Level
            public uint Exp
            {
                get { return exp; }
                set
                {
                    exp = exp + value;
                }
            }

            // Read/Write Access
            public uint GuildID
            {
                get { return guildID; }
                set { guildID = value; }
            }

            // Indexer for gear
            public uint this[uint index]
            {
                get { return gear[index]; }
                set { gear[index] = value; }
            }

            /**
             * Print out the gear for a specified player.
             * 
             * Take the player's name from the user's input and print any matching
             * gear from the item's dictionary.
             * 
             * @note If a match is not found, set a variable to the current iteration
             *       and print an 'Empty' message.
             ****************************************************************************/
            public void PrintGearList()
            {
                Console.Write("\n");
                Console.WriteLine(this);

                for (uint i = 0; i < GEAR_SLOTS; i++)
                {
                    // Print if item is in Player's gear
                    if (ItemDictionary.ContainsKey(this[i]))
                    {
                        Console.WriteLine(ItemDictionary[this[i]]);
                    }
                    else
                    {
                        uint emptyGear = i;
                        
                        // Ensure that both ring and trinket slots print properly
                        switch (emptyGear)
                        {
                            case 11:
                                emptyGear--;
                                break;
                            case 12:
                                emptyGear--;
                                break;
                            case 13:
                                emptyGear = emptyGear - 2;
                                break;
                        }
                        
                        Console.WriteLine("(" + (ItemType)emptyGear + ") Empty\n");
                    }
                }
            }

            /**
             * Computes the amount of experience given to a specific player
             * 
             * Uses a while loop to continuously level up a player and accumulate
             * the new experience given.
             * 
             * @param newExp - The experience value entered in by the user
             ****************************************************************************/
            public void awardExp(uint newExp)
            {
                while (newExp > this.Level * 1000 && this.Level != MAX_LEVEL) 
                {
                    // Subtract current level from the amount of experience given
                    newExp = newExp - this.Level * 1000;

                    // Increase player level
                    this.Level++;

                    // Player levels up successfully
                    Console.WriteLine("Ding!");
                }
            }


            // Default Constructor
            public Player()
            {
                Id = 0;
                Name = "";
                PlayerRace = 0;
                Level = 1;
                Exp = 0;
                GuildID = 0;
                inventory = new List<uint>();
                gear = new uint[GEAR_SLOTS];
            }

            // Constructor
            public Player(uint iID, string iName, Race iPlayerRace, uint iLevel, uint iExp, uint iGuildID,
                          uint helmet, uint neck, uint shoulders, uint back, uint chest, uint wrist, uint gloves,
                          uint belt, uint pants, uint boots, uint ring1, uint ring2, uint trinket1, uint trinket2)
            {
                Id = iID;
                Name = iName;
                PlayerRace = iPlayerRace;
                Level = iLevel;
                Exp = iExp;
                GuildID = iGuildID;
                inventory = new List<uint>();
                gear = new uint[GEAR_SLOTS];
                gear = new uint[] { helmet, neck, shoulders, back, chest, wrist, gloves, belt, pants, boots, ring1, ring2, trinket1, trinket2 };
            }

            // ToString Method
            public override string ToString()
            {
                // Displays the player name, race, level, and guild
                string raceString = "" + (Race)this.PlayerRace;
                string levelString = "" + this.Level;
                // If the player is in a guild it will say which guild, if not then nothing
                string guildString = "";
                if (Program.GuildDictionary.ContainsKey(this.guildID))
                {
                    guildString = "Guild: " + Program.GuildDictionary[this.guildID];
                }

                string output = String.Format("Name: {0}Race: {1} Level: {2}{3}", this.Name.PadRight(20), raceString.PadRight(10), levelString.PadRight(10), guildString);

                return output;
            }

            // IComparable Method
            public int CompareTo(object alpha)
            {
                //Check for null values
                if (alpha == null) return 1;

                //typecast
                Player rightOp = alpha as Player;

                //Players are compared by name
                if (rightOp != null)
                    return Name.CompareTo(rightOp.Name);
                else
                    throw new ArgumentException("Item CompareTo argument is not an item");
            }

        }

        // File paths
        public static string EquipmentFile = @"..\..\..\Program\equipment.txt";
        public static string GuildFile = @"..\..\..\Program\guilds.txt";
        public static string PlayerFile = @"..\..\..\Program\players.txt";

        // Dictionary declarations
        public static Dictionary<uint, Item> ItemDictionary = new Dictionary<uint, Item>();
        public static Dictionary<uint, Player> PlayerDictionary = new Dictionary<uint, Player>();
        public static Dictionary<uint, string> GuildDictionary = new Dictionary<uint, string>();

        public static int Main()
        {
            // Title
            Console.WriteLine("Welcome to the World of ConflictCraft: Testing Environment!\n");
            
            // Call functions to read each file
            ReadItems(EquipmentFile);
            ReadPlayers(PlayerFile);
            ReadGuilds(GuildFile);

            string s = "";

            do
            {
                // Menu
                Console.WriteLine("\nWelcome to World of ConflictCraft: Testing Environment. Please select an option from the list below:");
                Console.WriteLine("\t1.) Print All Players");
                Console.WriteLine("\t2.) Print All Guilds");
                Console.WriteLine("\t3.) List All Gear");
                Console.WriteLine("\t4.) Print Gear List for Player");
                Console.WriteLine("\t5.) Leave Guild");
                Console.WriteLine("\t6.) Join Guild");
                Console.WriteLine("\t7.) Equip Gear");
                Console.WriteLine("\t8.) Unequip Gear");
                Console.WriteLine("\t9.) Award Experience");
                Console.WriteLine("\t10.) Quit");

                s = Console.ReadLine();

                switch (s.ToString())
                {
                    case "1":
                        ListPlayers();
                        break;
                    case "2":
                        ListGuilds();
                        break;
                    case "3":
                        ListItems();
                        break;
                    case "4":
                        Console.Write("Enter the player name: ");
                        string playerName = Console.ReadLine();

                        foreach (KeyValuePair<uint, Player> pair in PlayerDictionary)
                        {
                            if (pair.Value.Name.Equals(playerName))
                            {
                                PlayerDictionary[pair.Value.Id].PrintGearList();
                            }
                        }

                        break;
                    case "5":
                        LeaveGuild();
                        break;
                    case "6":
                        JoinGuild();
                        break;
                    case "7":
                        // Equip Gear Method
                        break;
                    case "8":
                        // Unequip Gear Method
                        break;
                    case "9":
                        AwardExperience();
                        break;
                    case "10":
                        // Quit
                        break;
                    default:
                        Console.WriteLine(s + " is not an option.");
                        break;
                }
            } while (s != "10");

            return 0;
        }

        /**
         * Reads the equipment file and stores them into a dictionary.
         * 
         * Converts each input separated by tabs into a new Item and adds them to 
         * a previously defined item dictionary.
         * 
         * @param input - The name of the input file to be read.
         ****************************************************************************/
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

        /**
         * Lists all items in the item dictionary.
         * 
         * Uses a foreach loop to cycle through the items and prints each 
         * of their values.
         ****************************************************************************/
        public static void ListItems()
        {
            foreach (KeyValuePair<uint, Item> pair in ItemDictionary)
            {
                Console.WriteLine("{0}", pair.Value);
            }
        }

        /**
         * Reads the Player file and stores them into a dictionary.
         * 
         * Converts each input separated by tabs into a new Player and adds them to 
         * a previously defined player dictionary.
         * 
         * @param input - The name of the input file to be read.
         ****************************************************************************/
        public static void ReadPlayers(string input)
        {
            // String to read lines into
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
                        string[] inPlayers = inputLine.Split('\t');
                        // If the correct number of attributes are on the line, create a player from line
                        if (inPlayers.Length == 20)
                        {

                            //Create the player from the array, convert string to uints and other attributes when necessary
                            Player inputPlayer = new Player(Convert.ToUInt32(inPlayers[0]), inPlayers[1], (Race)Convert.ToUInt32(inPlayers[2]), Convert.ToUInt32(inPlayers[3]), Convert.ToUInt32(inPlayers[4]), Convert.ToUInt32(inPlayers[5]),
                                Convert.ToUInt32(inPlayers[6]), Convert.ToUInt32(inPlayers[7]), Convert.ToUInt32(inPlayers[8]), Convert.ToUInt32(inPlayers[9]), Convert.ToUInt32(inPlayers[10]), Convert.ToUInt32(inPlayers[11]), Convert.ToUInt32(inPlayers[12]), Convert.ToUInt32(inPlayers[13]), Convert.ToUInt32(inPlayers[14]), Convert.ToUInt32(inPlayers[15]), Convert.ToUInt32(inPlayers[16]), Convert.ToUInt32(inPlayers[17]), Convert.ToUInt32(inPlayers[18]), Convert.ToUInt32(inPlayers[19]));

                            // Add to dictionary of players
                            PlayerDictionary.Add(inputPlayer.Id, inputPlayer);

                        }

                        // Read next line
                        inputLine = inFile.ReadLine();
                    } // end of while
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(input + " file does not exist");
            }
        }

        /**
         * Lists all players in the player dictionary.
         * 
         * Uses a foreach loop to cycle through the players and prints each 
         * of their values.
         ****************************************************************************/
        public static void ListPlayers()
        {
            foreach (KeyValuePair<uint, Player> pair in PlayerDictionary)
            {
                Console.WriteLine("{0}", pair.Value);
            }
        }

        /**
         * Reads the Guild file and stores them into a dictionary.
         * 
         * Converts each input separated by tabs and adds them to 
         * a previously defined guild dictionary.
         * 
         * @param input - The name of the input file to be read.
         ****************************************************************************/
        public static void ReadGuilds(string input)
        {
            // String to read lines into
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
                        string[] inGuilds = inputLine.Split('\t');
                        // If the correct number of attributes are on the line, create a guild from line
                        if (inGuilds.Length == 2)
                        {
                            // Add to dictionary of items
                            GuildDictionary.Add(Convert.ToUInt32(inGuilds[0]), inGuilds[1]);
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

        /**
         * Lists all guilds in the guild dictionary.
         * 
         * Uses a foreach loop to cycle through the guilds and prints each 
         * of their values.
          ****************************************************************************/
        public static void ListGuilds()
        {
            foreach (KeyValuePair<uint, string> pair in GuildDictionary)
            {
                Console.WriteLine("{0}", pair.Value);
            }
        }

        /**
         * The user enters a player's name and the player leaves their guild
         * 
         * Uses a foreach loop to cycle through the player dictionary in order
         * to find a matching player with the input provided.
         ****************************************************************************/
        public static void LeaveGuild()
        {
            Console.Write("Enter the player name: ");
            string playerName = Console.ReadLine();

            foreach (KeyValuePair<uint, Player> pair in PlayerDictionary)
            {
                if (pair.Value.Name.Equals(playerName))
                {
                    pair.Value.GuildID = 0;
                    Console.WriteLine("{0} has left thier Guild.", playerName);
                    break;
                }
            }
        }

        /**
         * The user enters a player's name and a guild they would like to join.
         * 
         * Uses a foreach loop to cycle through the player dictionary in order
         * to find a matching player with the input provided. Next, a second foreach
         * loop is used to find a proper guild. Once both are found, the player's 
         * Guild ID is linked to the new Guild.
         ****************************************************************************/
        public static void JoinGuild()
        {
            Console.Write("Enter the player name: ");
            string playerName = Console.ReadLine();

            Console.Write("Enter the Guild they will join: ");
            string guildName = Console.ReadLine();

            foreach (KeyValuePair<uint, Player> pair in PlayerDictionary)
            {
                if (pair.Value.Name.Equals(playerName))
                {
                    foreach (KeyValuePair<uint, string> pair2 in GuildDictionary)
                    {
                        if (pair2.Value.Equals(guildName))
                        {
                            pair.Value.GuildID = pair2.Key;
                            Console.WriteLine("{0} has joined {1}!", playerName, guildName);
                            break;
                        }
                    }
                }
            }
        }

        /**
         * The user enters a player's name and the amount of experience awarded.
         * 
         * Uses a foreach loop to cycle through the player dictionary in order
         * to find a matching player with the input provided. Next, the player's
         * id gets paired with the award experience functiion and the experience
         * amount.
         ****************************************************************************/
        public static void AwardExperience()
        {
            Console.Write("Enter the player name: ");
            string playerName = Console.ReadLine();

            Console.Write("Enter the amount of experience to award: ");
            string expAmount = Console.ReadLine();

            try
            {
                foreach (KeyValuePair<uint, Player> pair in PlayerDictionary)
                {
                    if (pair.Value.Name.Equals(playerName))
                    {
                        PlayerDictionary[pair.Value.Id].awardExp(Convert.ToUInt32(expAmount));
                    }
                }
            }
            // If user did not enter the experience correctly
            catch (FormatException)
            {
                Console.WriteLine("");
            }
            // If experience doesn't fit into unsigned 32 bit integer
            catch (OverflowException)
            {
                Console.WriteLine("");
            }
        }

    }
}
 