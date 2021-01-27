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

        #region Exceptions
        public class ItemException : Exception // Deriving from the "Exception" base class
        {
            public override string ToString()
            {
                return "That item does not exist!";
            }
        }

        public class LevelException : Exception // Deriving from the "Exception" base class
        {
            public override string ToString()
            {
                return "Item level is too high";
            }
        }

        public class InventoryException : Exception // Deriving from the "Exception" base class
        {
            public override string ToString()
            {
                return "The inventory is full";
            }
        }

        public class PlayerException : Exception // Deriving from the "Exception" base class
        {
            public override string ToString()
            {
                return "This player does not exist";
            }
        }

        public class GuildException : Exception // Deriving from the "Exception" base class
        {
            public override string ToString()
            {
                return "This guild does not exist";
            }
        }
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
            public Item(uint newId, string newName, ItemType newType, uint newIlvl, uint newPrimary, uint newStamina, uint newRequirement, string newFlavor)
            {
                Id = newId;
                Name = newName;
                Type = newType;
                Ilvl = newIlvl;
                Primary = newPrimary;
                Stamina = newStamina;
                Requirement = newRequirement;
                Flavor = newFlavor;
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

            private bool ring;
            private bool trinket;

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

            // Private Read/Write Access
            private bool Ring
            {
                get { return ring; }
                set { ring = value; }
            }

            // Private Read/Write Access
            private bool Trinket
            {
                get { return trinket; }
                set { trinket = value; }
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

                        Console.WriteLine((ItemType)emptyGear + ": empty");
                    }
                }
            }

            /**
             * Unequip the user selected item 
             * 
             * Runs through checks that see if the item is part of the ring or trinket
             * gear slot. If they are, unequip both rings or trinkets.
             * 
             * @param slotID - The ID of the gear slot on the player
             ****************************************************************************/
            public void UnequipGear(int slotID)
            {
                try
                {
                    // Unequip the slot they select
                    if (slotID < 10)
                    {
                        //add the item to the inventory 
                        this.AddInventory(this[Convert.ToUInt32(slotID)]);
                        this[Convert.ToUInt32(slotID)] = 0;
                        Console.WriteLine(this.Name + " has successfully unequipped the" + (ItemType)slotID + " slot!");
                    }
                    // If they choose ring, unequip both rings and inform user
                    // Set boolean to true/lower slot
                    else if (slotID < 11)
                    {
                        this.AddInventory(this.gear[10]);
                        this.AddInventory(this.gear[11]);
                        this.gear[10] = 0;
                        this.gear[11] = 0;
                        this.Ring = true;
                        Console.WriteLine(this.Name + " has successfully unequipped " + (ItemType)slotID + "s!");
                    }
                    // If they choose trinket, unequip trinkets and inform user
                    // Set boolean to true/lower slot
                    else if (slotID < 12)
                    {
                        this.AddInventory(this.gear[12]);
                        this.AddInventory(this.gear[13]);
                        this.gear[12] = 0;
                        this.gear[13] = 0;
                        this.Trinket = true;
                        Console.WriteLine(this.Name + " has successfully unequipped " + (ItemType)slotID + "s!");
                    }
                    else
                    {
                        Console.WriteLine(slotID + " is not an option");
                    }
                }
                catch (InventoryException Error)
                {
                    Console.WriteLine(Error);
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine(slotID + " is not an option");
                }
            }

            /**
             * Adds unequipped gear to the player's inventory
             * 
             * Runs through checks to make sure the item exists and
             * the inventory isn't full
             * 
             * @param gearID - The unique ID for the gear about to be added
             ****************************************************************************/
            public void AddInventory(uint gearID)
            {
                if (!ItemDictionary.ContainsKey(gearID))
                {
                    throw new ItemException();
                }
                if (this.inventory.Count >= MAX_INVENTORY_SIZE)
                {
                    throw new InventoryException();
                }

                this.inventory.Add(gearID);
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
            public Player(uint newID, string newName, Race newPlayerRace, uint newLevel, uint newExp, uint newGuildID,
                          uint helmet, uint neck, uint shoulders, uint back, uint chest, uint wrist, uint gloves,
                          uint belt, uint pants, uint boots, uint ring1, uint ring2, uint trinket1, uint trinket2)
            {
                Id = newID;
                Name = newName;
                PlayerRace = newPlayerRace;
                Level = newLevel;
                Exp = newExp;
                GuildID = newGuildID;
                inventory = new List<uint>();
                gear = new uint[GEAR_SLOTS];
                gear = new uint[] { helmet, neck, shoulders, back, chest, wrist, gloves, belt, pants, boots, ring1, ring2, trinket1, trinket2 };

                if (this[10] == 0 || (this[10] != 0 && this[11] != 0))
                {
                    Ring = true;
                }
                else if (this[10] != 0)
                {
                    Ring = false;
                }

                if (this[12] == 0 || (this[12] != 0 && this[13] != 0))
                {
                    Trinket = true;
                }
                else if (this[12] != 0)
                {
                    Trinket = false;
                }
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
        public static string EquipmentFile = @"..\..\..\equipment.txt";
        public static string GuildFile = @"..\..\..\guilds.txt";
        public static string PlayerFile = @"..\..\..\players.txt";

        // Dictionary declarations
        public static Dictionary<uint, Item> ItemDictionary = new Dictionary<uint, Item>();
        public static Dictionary<uint, Player> PlayerDictionary = new Dictionary<uint, Player>();
        public static Dictionary<uint, string> GuildDictionary = new Dictionary<uint, string>();

        public static int Main()
        {
            // Title
            Console.WriteLine("Welcome to the World of ConflictCraft: Testing Environment!\n");

            // Call functions to read each file
            ReadPlayers(PlayerFile);
            ReadGuilds(GuildFile);
            ReadItems(EquipmentFile);

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

                        try
                        {
                            bool playerFound = false;
                            Console.Write("Enter the player name: ");
                            string playerName = Console.ReadLine();


                            foreach (KeyValuePair<uint, Player> pair in PlayerDictionary)
                            {
                                if (pair.Value.Name.Equals(playerName))
                                {
                                    playerFound = true;
                                    PlayerDictionary[pair.Value.Id].PrintGearList();
                                }
                            }

                            if (!playerFound) throw new PlayerException();
                        }
                        catch (PlayerException Error)
                        {
                            Console.WriteLine(Error);
                        }

                        break;
                    case "5":
                        LeaveGuild();
                        break;
                    case "6":
                        JoinGuild();
                        break;
                    case "7":

                        // Equip Gear

                        break;
                    case "8":
                        UnequipGear();
                        break;
                    case "9":
                        AwardExperience();
                        break;
                    case "T":
                        HiddenOption();
                        break;
                    case "10":
                    case "q":
                    case "Q":
                    case "quit":
                    case "Quit":
                    case "exit":
                    case "Exit":
                        s = "10";
                        break;
                    default:
                        Console.WriteLine(s + " is not an option.");
                        break;
                }
            } while (s != "10");

            return 0;
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
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(input + " file does not exist");
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
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(input + "file does not exist");
            }
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
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(input + "file does not exist");
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
         * The user enters a player's name and the player leaves their guild
         * 
         * Uses a foreach loop to cycle through the player dictionary in order
         * to find a matching player with the input provided.
         ****************************************************************************/
        public static void LeaveGuild()
        {
            try
            {
                Console.Write("Enter the player name: ");
                string playerName = Console.ReadLine();
                bool playerFound = false;

                foreach (KeyValuePair<uint, Player> pair in PlayerDictionary)
                {
                    if (pair.Value.Name.Equals(playerName))
                    {
                        playerFound = true;
                        pair.Value.GuildID = 0;
                        Console.WriteLine("{0} has left thier Guild.", playerName);
                        break;
                    }
                }

                if (!playerFound) throw new PlayerException();
            }
            catch (PlayerException Error)
            {
                Console.WriteLine(Error);
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
            try
            {
                Console.Write("Enter the player name: ");
                string playerName = Console.ReadLine();
                bool playerFound = false;
                bool guildFound = false;

                foreach (KeyValuePair<uint, Player> pair in PlayerDictionary)
                {
                    if (pair.Value.Name.Equals(playerName))
                    {
                        playerFound = true;

                        Console.Write("Enter the Guild they will join: ");
                        string guildName = Console.ReadLine();

                        foreach (KeyValuePair<uint, string> pair2 in GuildDictionary)
                        {
                            if (pair2.Value.Equals(guildName))
                            {
                                guildFound = true;
                                pair.Value.GuildID = pair2.Key;
                                Console.WriteLine("{0} has joined {1}!", playerName, guildName);
                                break;
                            }
                        }
                    }
                }

                if (!playerFound) throw new PlayerException();
                if (!guildFound) throw new GuildException();
            }
            catch (PlayerException Error)
            {
                Console.WriteLine(Error);
            }
            catch (GuildException Error)
            {
                Console.WriteLine(Error);
            }
        }

        /**
         * The user enters a player's name and the gear slot they will unequip
         * 
         * Uses an enumerator to list all the gear in order. The user can choose
         * which gear slot to unequip and the function matches it with the proper
         * player.
         ****************************************************************************/
        public static void UnequipGear()
        {
            try
            {
                bool playerFound = false;

                Console.Write("Enter the player name: ");
                string unequipPlayerName = Console.ReadLine();

                foreach (KeyValuePair<uint, Player> pair in PlayerDictionary)
                {
                    if (pair.Value.Name.Equals(unequipPlayerName))
                    {
                        playerFound = true;
                        // Ask for name of item
                        Console.WriteLine("Enter the item slot they will unequip: ");
                        // Show all items
                        for (int i = 0; i < Enum.GetNames(typeof(ItemType)).Length; i++)
                        {
                            Console.WriteLine("\t{0} = {1}", i, (ItemType)i);
                        }

                        string unequipItem = Console.ReadLine();

                        PlayerDictionary[pair.Value.Id].UnequipGear(Convert.ToInt32(unequipItem));
                    }
                }

                if (!playerFound) throw new PlayerException();
            }
            // If the player doesn't exist
            catch (PlayerException Error)
            {
                Console.WriteLine(Error);
            }
            // If the item doesn't exist or is not equipped
            catch (ItemException Error)
            {
                Console.WriteLine(Error);
            }
            // If they did not enter a number correctly
            catch (FormatException)
            {
                Console.WriteLine("Not a valid number");
            }
            // If the number doesn't fit into unsigned 32 bit integer
            catch (OverflowException)
            {
                Console.WriteLine("Number too large or too small");
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
            bool playerFound = false;

            try
            {
                foreach (KeyValuePair<uint, Player> pair in PlayerDictionary)
                {
                    if (pair.Value.Name.Equals(playerName))
                    {
                        playerFound = true;

                        Console.Write("Enter the amount of experience to award: ");
                        string expAmount = Console.ReadLine();
                        PlayerDictionary[pair.Value.Id].awardExp(Convert.ToUInt32(expAmount));
                    }
                }

                if (!playerFound) throw new PlayerException();
            }
            // If player does not exist
            catch (PlayerException Error)
            {
                Console.WriteLine(Error);
            }
            // If user did not enter the experience correctly
            catch (FormatException)
            {
                Console.WriteLine("Not a valid number");
            }
            // If experience doesn't fit into unsigned 32 bit integer
            catch (OverflowException)
            {
                Console.WriteLine("Number too large or too small");
            }
        }

        /**
         * A hidden option used to set the items and players in order
         * 
         * Uses a sorted set to display the player and item dictionaries in
         * an appropriate order.
         ****************************************************************************/
        public static void HiddenOption()
        {
            // Create sorted set of players
            SortedSet<Player> playerSet = new SortedSet<Player>();
            // Create sorted set of items
            SortedSet<Item> itemSet = new SortedSet<Item>();


            // Fill set with players
            foreach (Player p in PlayerDictionary.Values)
            {
                playerSet.Add(p);
            }
            //fill set with items
            foreach (Item i in ItemDictionary.Values)
            {
                itemSet.Add(i);
            }

            Console.WriteLine("Sorted Players\n");
            //Output the sorted set of players
            foreach (Player p in playerSet)
            {
                Console.WriteLine(p);
            }
            Console.WriteLine("\nSorted Items\n");
            //Output the sorted set of items
            foreach (Item i in itemSet)
            {
                Console.WriteLine(i);
            }
        }

    }
}