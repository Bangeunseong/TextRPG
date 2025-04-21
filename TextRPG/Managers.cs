using System.Text;
using System.Text.Json;

namespace TextRPG
{
    /// <summary>
    /// Contains UI Materials
    /// </summary>
    static class UIManager
    {
        public static void StartUI()
        {
            foreach(string line in Miscs.GameStart) Console.WriteLine(line);
            Console.ReadKey();
        }

        public static void JobSelectionUI()
        {
            Console.WriteLine();
            foreach(string line in Miscs.CharacterSelection) Console.WriteLine(line);
            Console.WriteLine("\n| 1. Choose Job |");
            Console.Write("Choose Action : ");
        }

        public static void InventoryUI(Character character)
        {
            Console.WriteLine("\n| ----- Inventory ----- |");
            Console.WriteLine("|\"Armors\" |");
            int i = 1;
            foreach(Armor armor in character.Armors) { Console.WriteLine($"{i++}. {armor}"); }
            Console.WriteLine("\n|\"Weapons\" |");
            i = 1;
            foreach (Weapon weapon in character.Weapons) { Console.WriteLine($"{i++}. {weapon}"); }
            Console.WriteLine("\n|\"Potions\" |");
            i = 1;
            foreach (Consumables potion in character.Consumables) { Console.WriteLine($"{i++}. {potion}"); }
            Console.WriteLine("| ------------- |");
            Console.WriteLine("\n| Actions |");
            Console.WriteLine("| 1. Back |");
            Console.WriteLine("| 2. Select Item |");
            Console.Write("Choose Action : ");
        }

        public static void InventoryUI_Equipment()
        {
            Console.WriteLine("\n| ----- \"Equipment\" ----- |");
            Console.WriteLine("| 1. Back |");
            Console.WriteLine("| 2. Equip |");
            Console.WriteLine("| 3. Unequip |");
            Console.WriteLine("| 4. Drop |");
            Console.Write("Choose Action : ");
        }

        public static void InventoryUI_Consumable()
        {
            Console.WriteLine("\n| ----- \"Consumable\" ----- |");
            Console.WriteLine("| 1. Back |");
            Console.WriteLine("| 2. Use  |");
            Console.Write("Choose Action : ");
        }

        public static void ShopUI(Character character)
        {
            Console.WriteLine("\n| ----- Welcome to Henry's Shop! ----- |");
            foreach(string line in Miscs.Henry) Console.WriteLine(line);
            Console.WriteLine("| ---------------------------------- |");

            Console.WriteLine($"\n| Currency : {character.Currency} |");    
            Console.WriteLine("| Actions |");
            Console.WriteLine("| 1. Back |");
            Console.WriteLine("| 2. Buy |");
            Console.WriteLine("| 3. Sell |");
            Console.Write("\nChoose Action : ");
        }

        public static void ShowShopList()
        {
            int i = 1;
            Console.WriteLine("\n| ----- Buy Items ----- |");
            Console.WriteLine("|\"Armors\" |");
            foreach (Armor armor in ItemLists.Armors) { Console.WriteLine($"{i++}. {armor}"); }
            i = 1;
            Console.WriteLine("|\"Weapons\" |");
            foreach (Weapon weapon in ItemLists.Weapons) { Console.WriteLine($"{i++}. {weapon}"); }
            i = 1;
            Console.WriteLine("|\"Potions\" |");
            foreach (Consumables potion in ItemLists.Consumables) { Console.WriteLine($"{i++}. {potion}"); }
            Console.WriteLine("| --------------------- |");
            Console.Write("\nWhat do you want to buy? ( Type [ Category,Index ] ) : ");
        }

        public static void ShowItemList(Character character)
        {
            int i = 1;
            Console.WriteLine("\n| ----- Sell Items ----- |");
            Console.WriteLine("|\"Armors\" |");
            foreach (Armor armor in character.Armors) { Console.WriteLine($"{i++}. {armor}"); }
            Console.WriteLine("|\"Weapons\" |");
            i = 1;
            foreach (Weapon weapon in character.Weapons) { Console.WriteLine($"{i++}. {weapon}"); }
            Console.WriteLine("|\"Potions\" |");
            i = 1;
            foreach (Consumables potion in character.Consumables) { Console.WriteLine($"{i++}. {potion}"); }
            Console.WriteLine("| ---------------------- |");
            Console.Write("\nWhat do you want to sell? ( Type [ Category,Index ] ) : ");
        }

        public static void ShowMonsterList(SpawnManager spawnManager)
        {
            Console.WriteLine("\n| ----- Battle ----- |");
            Console.WriteLine("| \"Monsters\" |");
            int i = 1;
            foreach (Monster monster in spawnManager.spawnedMonsters)
                Console.WriteLine($"| {i++}. {monster.Name} | Health : {monster.Health} |");
            Console.WriteLine("| ------------------ |");
            Console.Write("\nChoose Monster to Attack : ");
        }

        public static void CabinUI()
        {
            Console.WriteLine("\n| ----- Welcome to Alby's Cabin! ----- |");
            foreach(string line in Miscs.Alby) Console.WriteLine(line);
            Console.WriteLine("\n| Room Options |");
            Console.WriteLine("| 1. Back |");
            Console.WriteLine("| 2. Normal Room (Heals 50% of your Max Health, 20G) |");
            Console.WriteLine("| 3. Comfy Room (Heals 75% of your Max Health, 40G) |");
            Console.WriteLine("| 4. Emperror Room (Heals 100% of your Max Health, 60G)");
            Console.WriteLine("| ------------------------------------ |");
            Console.Write("\nChoose Room Option : ");
        }

        public static void StatusUI(Character character)
        {
            Console.WriteLine("\n| ----- \"Character Info.\" ----- |");
            Console.WriteLine($"\n| \"Name\" : {character.Name} |");
            Console.WriteLine($"| \"Lv {character.Level:D2}\" |");
            Console.WriteLine($"| \"Experience\" : {character.Exp:F2} |");
            Console.WriteLine($"| \"Health\" : {character.Health:F2} |");
            Console.WriteLine($"| \"Magic Point\" : {character.MagicPoint:F2} |");
            Console.WriteLine($"| \"Currency\" : {character.Currency} |");

            Console.WriteLine("\n| ----- \"Status\" ----- |");
            Console.WriteLine($"| \"Attack\" : {character.AttackStat.Attack} |");
            Console.WriteLine($"| \"Range Attack\" : {character.AttackStat.RangeAttack} |");
            Console.WriteLine($"| \"Magic Attack\" : {character.AttackStat.MagicAttack} |");
            Console.WriteLine($"| \"Defence\" : {character.DefendStat.Defend} |");
            Console.WriteLine($"| \"Range Defence\" : {character.DefendStat.RangeDefend} |");
            Console.WriteLine($"| \"Magic Defence\" : {character.DefendStat.MagicDefend} |");

            Console.WriteLine("\n| ----- \"Armors\" ----- |");
            foreach (Armor armor in character.Armors) { Console.WriteLine($"| {armor} |"); }
            Console.WriteLine("| ----- \"Weapons\" ----- |");
            foreach (Weapon weapon in character.Weapons) { Console.WriteLine($"| {weapon} |"); }
            Console.WriteLine("| ----- \"Potions\" ----- |");
            foreach(Consumables consumable in character.Consumables) { Console.WriteLine($"| {consumable} |"); }
            Console.WriteLine("\n| Press any key to continue... |");
            Console.ReadKey();
        }

        public static void KillCountUI(int KillCount, int Quota)
        {
            Console.WriteLine("\n| --------------------------------------------- |");
            Console.WriteLine($"| Killed Monsters : {KillCount}, Quota : {Quota} |");
            Console.WriteLine("| ----------------------------------------------- |");
        }

        public static void MonsterEncounterUI(SpawnManager spawnManager)
        {
            StringBuilder sb = new();
            int i = 0;

            Console.WriteLine("\n| ---------------------------------- |");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach(Monster monster in spawnManager.spawnedMonsters)
            {
                if(i != 0) sb.Append(", ");
                sb.Append(monster.Name); i++;

                if(monster.AttackType == AttackType.Close) foreach (string line in Miscs.Goblin) Console.WriteLine(line);
                else if(monster.AttackType == AttackType.Long) foreach (string line in Miscs.GoblinArcher) Console.WriteLine(line);
                else foreach (string line in Miscs.GoblinWizard) Console.WriteLine(line);
            }
            Console.ResetColor();
            Console.WriteLine("| ---------------------------------- |");

            Console.WriteLine($"\n| Warning! : Encountered {sb} |");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void NoMonsterFoundUI()
        {
            int ind = new Random().Next(Miscs.Quotes.Length);
            Console.WriteLine($"\n| Nothing Found, {Miscs.Quotes[ind]}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void GameOverUI()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            foreach(string line in Miscs.GameOver) Console.WriteLine(line);
            Console.ResetColor();

            Console.WriteLine("\nPress any key to revive(Loses 100G)...");
            Console.ReadKey();
        }

        public static void GameOptionUI()
        {
            Console.WriteLine($"\n| ----- Game Options ----- |");
            Console.WriteLine();
            int i = 1;
            foreach (var opt in Enum.GetValues(typeof(GameOption)))
            {
                Console.WriteLine($"| {i++}. {opt} |");
            }
            Console.Write("\nChoose Action : ");
        }

        public static void SettingOptionUI()
        {
            Console.WriteLine($"\n| ----- Options ----- |");
            int i = 1;
            foreach (var opt in Enum.GetValues(typeof(SettingOptions)))
            {
                Console.WriteLine($"| {i++}. {opt} |");
            }
            Console.Write("\nChoose Action : ");
        }

        public static void BaseUI(Character character, string headLine, Type type)
        {
            Console.WriteLine($"\n| ----- {headLine} ----- |");
            Console.WriteLine($"| Current Time : {GameManager.GameTime} |");
            Console.WriteLine($"| Health : {character.Health} |");
            Console.WriteLine($"| Currency : {character.Currency} |");
            Console.WriteLine();
            int i = 1;
            foreach (var opt in Enum.GetValues(type))
            {
                Console.WriteLine($"| {i++}. {opt} |");
            }
            Console.Write("\nChoose Action : ");
        }
    }

    /// <summary>
    /// Contains Quotes and Ascii Arts
    /// </summary>
    static class Miscs
    {
        public static string[] Quotes = { "It's cold and silent.", "My body starts shivering", "I miss the alby's cabin.",
                                          "I should not come down here.", "It feels like someone is watching...",
                                          "I think i'm lost...", "It's dark and moist...", "This place is freaking me out...",
                                          "I hate bats...", "Something feels wrong...", "I miss the henry's shop."};

        public static string[] Goblin = new string[]
        {
            @"                *-|=> ",
            @"       ,      ,       ",
            @"      /(.-""-.)\      ",
            @"  |\  \/      \/  /|  ",
            @"  | \ / =.  .= \ / |  ",
            @"  \( \   o\/o   / )/  ",
            @"   \_, '-/  \-' ,_/   ",
            @"     /   \__/   \     ",
            @"     \ \__/\__/ /     ",
            @"   ___\ \|--|/ /___   ",
            @" /`    \      /    `\ ",
            @" \__.   |    |   .__/ ",
            @"     \  |    |  /     ",
            @"      \_|____|_/      "
        };

        public static string[] GoblinArcher = new string[]
        {
            @"          /\    >--=> ",
            @"         /__\         ",
            @"      \__\  /__/      ",
            @"      /(.-""-.)\      ",
            @"  |\  \/      \/  /|  ",
            @"  | \ / =.  .= \ / |  ",
            @"  \( \   o\/o   / )/  ",
            @"   \_, '-/  \-' ,_/   ",
            @"     /   \__/   \     ",
            @"     \ \__/\__/ /     ",
            @"   ___\ \|--|/ /___   ",
            @" /`    \      /    `\ ",
            @" \__.   |    |   .__/ ",
            @"     \  |    |  /     ",
            @"      \_|____|_/      "
        };

        public static string[] GoblinWizard = new string[]
        {
            @"          /\    *--=* ",
            @"         /__\         ",
            @"        (/()\)        ",
            @"     \_,/\__/\,_/     ",
            @"      /(.-""-.)\      ",
            @"  |\  \/      \/  /|  ",
            @"  | \ / =.  .= \ / |  ",
            @"  \( \   o\/o   / )/  ",
            @"   \_, '-/  \-' ,_/   ",
            @"     /   \__/   \     ",
            @"     \ \__/\__/ /     ",
            @"   ___\ \|--|/ /___   ",
            @" /`    \      /    `\ ",
            @" \__.   |    |   .__/ ",
            @"     \  |    |  /     ",
            @"      \_|____|_/      "
        };

        public static string[] Henry = new string[]
        {
            @"        /     \        ",
            @"       / (/@\) \       ",
            @"   \__/_________\__/   ",
            @"     |  O     O  |     ",
            @"     |     ^     |     ",
            @"     |   \___/   |     ",
            @"      \_________/      ",
            @"   ___/   |||   \___   ",
            @" /`    \       /    `\ ",
            @" \__.   |     |   .__/ ",
            @"     \  |     |  /     ",
            @"      \_|_____|_/      ",
        };

        public static string[] Alby = new string[]
        {
            @"        /     \        ",
            @"       / -(+)- \       ",
            @"    __/_________\__    ",
            @"   / |  O     O  | \   ",
            @"     |     ^     |     ",
            @"     |   '---'   |     ",
            @"      \_________/      ",
            @"   ___/   |||   \___   ",
            @" /`    \       /    `\ ",
            @" \__.   |     |   .__/ ",
            @"     \  |     |  /     ",
            @"      \_|_____|_/      ",
        };

        public static string[] GameStart = new string[]
        {
            @"╔═══════════════════════════════════════════════════════════╗",
            @"║                                                           ║",
            @"║                           /\                              ║",
            @"║                          /  \                             ║",
            @"║                         |    |                            ║",
            @"║                         | [] |                            ║",
            @"║                         |____|                            ║",
            @"║                        /| || |\                           ║",
            @"║                       /_|_||_|_\                          ║",
            @"║                         ||  ||                            ║",
            @"║                         ()  ()                            ║",
            @"║                      __/||  ||\__                         ║",
            @"║                     /___/_  _\___\                        ║",
            @"║                           ||                              ║",
            @"║                           ||                              ║",
            @"║                           ||                              ║",
            @"║                          /__\                             ║",
            @"║                                                           ║",
            @"║                     GOBLIN__SLAYER                        ║",
            @"║                                                           ║",
            @"║                 PRESS ANY KEY TO START!                   ║",
            @"╚═══════════════════════════════════════════════════════════╝",
        };

        public static string[] CharacterSelection = new string[]
        {
            @"╔═══════════════════════════════════════════════════════════════╗",
            @"║                                                               ║",
            @"║    [1. WARRIOR]          [2. WIZARD ]          [3. ARCHER]    ║",
            @"║         /\                  '(**),                            ║",
            @"║        /__\                   ||                 |\           ║",
            @"║        |  |                   ||                 | '\         ║",
            @"║        |  |                   ||                 |   |        ║",
            @"║        |  |                   ||               >-------=>     ║",
            @"║        |__|                   ||                 |   |        ║",
            @"║         ||                    ||                 | ,/         ║",
            @"║        .||.                   ()                 |/           ║",
            @"║                                                               ║",
            @"║                                                               ║",
            @"║                        Select your Job!                       ║",
            @"╚═══════════════════════════════════════════════════════════════╝",
        };

        public static string[] GameOver = new string[]
        {
            @"| ======================================================================== |",
            @"|  ██████   █████  ███    ███ ███████     ██████  ██    ██ ███████ ███████ |",
            @"| ██       ██   ██ ████  ████ ██         ██    ██ ██    ██ ██      ██   ██ |",
            @"| ██   ███ ███████ ██ ████ ██ █████      ██    ██ ██    ██ ███████ █████   |",
            @"| ██    ██ ██   ██ ██  ██  ██ ██         ██    ██ ██    ██ ██      ██   ██ |",
            @"|  ██████  ██   ██ ██      ██ ███████     ██████   ██████  ███████ ██   ██ |",
            @"| ======================================================================== |",
        };
    }

    /// <summary>
    /// Manage spawning monsters
    /// </summary>
    class SpawnManager
    {
        public List<Monster> spawnedMonsters = new();
        public int KilledMonsterCount { get; set; } = 0;

        public SpawnManager() { }

        public void SpawnMonsters(Character character, int groundLevel)
        {
            int count = new Random().Next(1, 5);
            for(int i = 0; i < count; i++)
            {
                int type = new Random().Next(MonsterLists.monsters.Length);

                if (MonsterLists.monsters[type].AttackType == AttackType.Close)
                {
                    GoblinWarrior monster = new GoblinWarrior((GoblinWarrior)MonsterLists.monsters[type]);
                    SetMonster(monster, groundLevel, character, 50);
                    AddMonster(monster);
                }
                else if (MonsterLists.monsters[type].AttackType == AttackType.Long)
                {
                    GoblinArcher monster = new GoblinArcher((GoblinArcher)MonsterLists.monsters[type]);
                    SetMonster(monster, groundLevel, character, 65);
                    AddMonster(monster);
                }
                else if (MonsterLists.monsters[type].AttackType == AttackType.Magic)
                {
                    GoblinMage monster = new GoblinMage((GoblinMage)MonsterLists.monsters[type]);
                    SetMonster(monster, groundLevel, character, 80);
                    AddMonster(monster);
                }
            }
        }
        
        // Public Methods
        public int GetMonsterCount() { return spawnedMonsters.Count; }
        public void ResetKillCount() { KilledMonsterCount = 0; }
        public void RemoveAllMonsters() { spawnedMonsters.Clear(); }

        // Private Methods
        // TODO: Increase monster stat based on ground level
        private void SetMonster(Monster monster, int groundLevel, Character character, int currency)
        {
            monster.Level = character.Level;
            monster.AttackStat += new AttackStat(monster.AttackStat.Attack * 0.1f * monster.Level,
                                                 monster.AttackStat.RangeAttack * 0.1f * monster.Level,
                                                 monster.AttackStat.MagicAttack * 0.1f * monster.Level);
            monster.DefendStat += new DefendStat(monster.DefendStat.Defend * 0.1f * monster.Level,
                                                 monster.DefendStat.RangeDefend * 0.1f * monster.Level,
                                                 monster.DefendStat.MagicDefend * 0.1f * monster.Level);
            monster.OnDeath += () =>
            {
                RemoveMonster(character, monster, currency);
            };
        }
        private void AddMonster(Monster monster) { spawnedMonsters.Add(monster); }
        private void RemoveMonster(Character character, Monster monster, int currency) {
            Console.WriteLine($"| {monster.Name} is dead! |");
            Console.WriteLine($"| {character.Name} gets {currency}G |");
            KilledMonsterCount++;
            character.Currency += currency;
            character.OnEarnExp(monster.Exp);

            // Randomly drop items
            int ind = new Random().Next(0, 3);
            if (ind == 0) GetRandomArmor(monster.Level)?.OnPicked(character);
            else if (ind == 1) GetRandomWeapon(monster.Level)?.OnPicked(character);
            else GetRandomConsumable(monster.Level)?.OnPicked(character);

            spawnedMonsters.Remove(monster);
        }


        private Armor? GetRandomArmor(int level)
        {
            if (new Random().Next(1, 101) % 2 != 0) return null;

            IEnumerable<Armor> filteredItems;
            if (level > 0 && level <= 15)
            {
                filteredItems = from item in ItemLists.Armors
                                where item.Rarity == Rarity.Common || item.Rarity == Rarity.Exclusive
                                select item;
            }
            else if (level > 15 && level <= 30)
            {
                filteredItems = from item in ItemLists.Armors
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare
                                select item;
            }
            else if (level > 30 && level <= 50)
            {
                filteredItems = from item in ItemLists.Armors
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero
                                select item;
            }
            else
            {
                filteredItems = from item in ItemLists.Armors
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero || item.Rarity == Rarity.Legend
                                select item;
            }
            int ind = new Random().Next(filteredItems.Count());
            return filteredItems.ElementAt(ind);
        }
        private Weapon? GetRandomWeapon(int level)
        {
            if (new Random().Next(1, 101) % 2 != 0) return null;
            IEnumerable<Weapon> filteredItems;
            if (level > 0 && level <= 15)
            {
                filteredItems = from item in ItemLists.Weapons
                                where item.Rarity == Rarity.Common || item.Rarity == Rarity.Exclusive
                                select item;
            }
            else if (level > 15 && level <= 30)
            {
                filteredItems = from item in ItemLists.Weapons
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare
                                select item;
            }
            else if (level > 30 && level <= 50)
            {
                filteredItems = from item in ItemLists.Weapons
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero
                                select item;
            }
            else
            {
                filteredItems = from item in ItemLists.Weapons
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero || item.Rarity == Rarity.Legend
                                select item;
            }
            int ind = new Random().Next(filteredItems.Count());
            return filteredItems.ElementAt(ind);
        }
        private Consumables? GetRandomConsumable(int level)
        {
            if (new Random().Next(1, 101) % 2 != 0) return null;

            IEnumerable<Consumables> filteredItems;
            if(level > 0 && level <= 15)
            {
                filteredItems = from item in ItemLists.Consumables
                                where item.Rarity == Rarity.Common || item.Rarity == Rarity.Exclusive
                                select item;
            }
            else if(level > 15 && level <= 30)
            {
                filteredItems = from item in ItemLists.Consumables
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare
                                select item;
            }
            else if(level > 30 && level <= 50)
            {
                filteredItems = from item in ItemLists.Consumables
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero
                                select item;
            }
            else
            {
                filteredItems = from item in ItemLists.Consumables
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero || item.Rarity == Rarity.Legend
                                select item;
            }
            int ind = new Random().Next(filteredItems.Count());
            return filteredItems.ElementAt(ind);
        }
    }

    /// <summary>
    /// Manage controls of overall game statements and functions
    /// </summary>
    class GameManager
    {
        // Static Field
        public static GameState GameState = GameState.MainMenu;
        public static GameTime GameTime = GameTime.Afternoon;
        public static Queue<Consumables> Exposables = new();

        // Property
        public Character SelectedCharacter { get; private set; }
        public int GroundLevel { get; private set; }
        public int Quota { get; private set; } = 10;

        // Constructor
        public GameManager(int groundLevel = 1) { GroundLevel = groundLevel; }

        // Methods
        /// <summary>
        /// Job Selection UI will be displayed.
        /// This method will return true if the job selected successfully.
        /// If not, it will return false.
        /// </summary>
        /// <returns>Returns true, if job selected successfully. If not, returns false.</returns>
        public void SelectJob()
        {
            int option;
            while (true)
            {
                UIManager.JobSelectionUI();
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); }
                else { option = Math.Clamp(opt, 1, Enum.GetValues(typeof(Job)).Length); break; }
            }

            switch ((Job)(option - 1))
            {
                case Job.Warrior:
                    Console.WriteLine("| You selected Warrior! |");
                    Console.Write("Type the name of your warrior : ");
                    SelectedCharacter = new Warrior(new CharacterStat(Console.ReadLine(), 150, 50, 1, new AttackStat(30f, 6f, 1f), new DefendStat(25, 15, 5)), 100, 0);
                    break;
                case Job.Wizard:
                    Console.WriteLine("| You selected Wizard! |");
                    Console.Write("Type the name of your wizard : ");
                    SelectedCharacter = new Wizard(new CharacterStat(Console.ReadLine(), 100, 65, 1, new AttackStat(1f, 6f, 30f), new DefendStat(5, 10, 30)), 100, 0);
                    break;
                case Job.Archer:
                    Console.WriteLine("| You selected Archer! |");
                    Console.Write("Type the name of your archer : ");
                    SelectedCharacter = new Archer(new CharacterStat(Console.ReadLine(), 120, 80, 1, new AttackStat(6f, 30f, 1f), new DefendStat(15, 25, 5)), 100, 0);
                    break;
            }

            SelectedCharacter.OnDeath += GameOver;
            GiveBasicItems(SelectedCharacter);
        }

        /// <summary>
        /// Remove all buffs applied to character when night passed.
        /// </summary>
        public void RemoveAllBuffs()
        {
            if (Exposables.Count <= 0) return;

            while(Exposables.Count > 0)
            {
                Consumables consumable = Exposables.Dequeue();
                if (consumable == null) continue;

                consumable.OnDeBuffed(SelectedCharacter);
            }
        }

        /// <summary>
        /// Increase Dungeon level when character reached the quota.
        /// </summary>
        public void GoToNextLevel()
        {
            Console.WriteLine("| Quota reached. Moving to next level! |");
            Console.WriteLine("| Press any key to continue... |");
            Console.ReadKey();
            Quota = 10 + (GroundLevel - 1) * 5;
        }

        /// <summary>
        /// Save the character and game data to JSON files.
        /// </summary>
        public void SaveGame()
        {
            if (!Directory.Exists("data")) Directory.CreateDirectory("data");
            

            var characterOptions = new JsonSerializerOptions
            {
                Converters = { 
                    new CharacterConverter(), new ArmorConverter(), 
                    new WeaponConverter(), new ConsumableConverter(),
                },
                WriteIndented = true
            };

            string characterJson = JsonSerializer.Serialize(SelectedCharacter, characterOptions);
            File.WriteAllText("data/character.json", characterJson, new UTF8Encoding(true));

            var gameData = new GameData {
                GroundLevel = GroundLevel,
                Quota = Quota,
                GameState = GameState,
                GameTime = GameTime,
                
                // TODO : Add Quest list

                Exposables = Exposables.ToList()
            };

            var gameOptions = new JsonSerializerOptions
            {
                Converters = {
                    new ConsumableConverter(),
                    new QuestConverter(),
                },
                WriteIndented = true
            };
            string gameJson = JsonSerializer.Serialize(gameData, gameOptions);
            File.WriteAllText("data/game.json", gameJson, new UTF8Encoding(true));

            Console.WriteLine("| Game Saved Successfully! |");
        }

        /// <summary>
        /// Load the character and game data from JSON files.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void LoadGame()
        {
            if(!File.Exists("data/character.json") || !File.Exists("data/game.json"))
            {
                Console.WriteLine("| No saved data found! |");
                return;
            }

            var options = new JsonSerializerOptions
            {
                Converters = {
                    new CharacterConverter(), new ArmorConverter(),
                    new WeaponConverter(), new ConsumableConverter(),
                },
                WriteIndented = true
            };

            string characterJson = File.ReadAllText("data/character.json", Encoding.UTF8);
            var obj = JsonSerializer.Deserialize<Character>(characterJson, options);
            // Console.WriteLine(obj?.ToString());
            SelectedCharacter = obj ?? throw new InvalidOperationException("Failed to load character data.");

            var gameOptions = new JsonSerializerOptions
            {
                Converters = {
                    new ConsumableConverter(),
                    new QuestConverter(),
                },
                WriteIndented = true
            };

            string gameJson = File.ReadAllText("data/game.json", Encoding.UTF8);
            var gameObj = JsonSerializer.Deserialize<GameData>(gameJson, gameOptions);
            
            if(gameObj == null) throw new InvalidOperationException("Failed to load game data.");
            
            /*
            Console.Write(gameObj.GroundLevel + ", " + gameObj.Quota + ", " + gameObj.GameState + ", " + gameObj.GameTime + "\n");
            foreach(var item in gameObj.Exposables)
            {
                Console.Write(item.ToString() + ", ");
            }
            Console.WriteLine();
            */

            GroundLevel = gameObj.GroundLevel;
            Quota = gameObj.Quota;
            GameState = gameObj.GameState;
            GameTime = gameObj.GameTime;
            Exposables = new Queue<Consumables>(gameObj.Exposables);

            Console.WriteLine("| Game Loaded Successfully! |");
        }

        /// <summary>
        /// Give basic items to the character.
        /// </summary>
        /// <param name="character"></param>
        private void GiveBasicItems(Character character)
        {
            // LINQ
            var basicHelmets = from armor in ItemLists.Armors
                               where armor.GetType().Equals(typeof(Helmet)) && armor.Rarity == Rarity.Common
                               select armor;
            var basicChestArmors = from armor in ItemLists.Armors
                                   where armor.GetType().Equals(typeof(ChestArmor)) && armor.Rarity == Rarity.Common
                                   select armor;
            
            var basicHealthPotions = from item in ItemLists.Consumables
                                     where item.GetType().Equals(typeof(HealthPotion)) && item.Rarity == Rarity.Common
                                     select item;
            var basicMagicPotions = from item in ItemLists.Consumables
                                    where item.GetType().Equals(typeof(MagicPotion)) && item.Rarity == Rarity.Common
                                    select item;

            if (basicHelmets.Count() > 0) { character.Armors.Add(new Helmet((Helmet)basicHelmets.First())); }
            if (basicChestArmors.Count() > 0) { character.Armors.Add(new ChestArmor((ChestArmor)basicChestArmors.First())); }
            
            if (character.GetType().Equals(typeof(Warrior)))
            {
                var basicSwords = from sword in ItemLists.Weapons
                              where sword.GetType().Equals(typeof(Sword)) && sword.Rarity == Rarity.Common
                              select sword;
                if (basicSwords.Count() > 0) { character.Weapons.Add(new Sword((Sword)basicSwords.First())); }
            }
            else if (character.GetType().Equals(typeof(Wizard)))
            {
                var basicStaffs = from staff in ItemLists.Weapons
                                  where staff.GetType().Equals(typeof(Staff)) && staff.Rarity == Rarity.Common
                                  select staff;
                if (basicStaffs.Count() > 0) { character.Weapons.Add(new Staff((Staff)basicStaffs.First())); }
            }
            else
            {
                var basicBows = from bow in ItemLists.Weapons
                                where bow.GetType().Equals(typeof(Bow)) && bow.Rarity == Rarity.Common
                                select bow;
                if (basicBows.Count() > 0) { character.Weapons.Add(new Bow((Bow)basicBows.First())); }
            }

            if (basicHealthPotions.Count() > 0) { character.Consumables.Add(new HealthPotion((HealthPotion)basicHealthPotions.First())); }
            if (basicMagicPotions.Count() > 0) { character.Consumables.Add(new MagicPotion((MagicPotion)basicMagicPotions.First())); }
        }

        /// <summary>
        /// Game Over UI will be displayed.
        /// </summary>
        private void GameOver()
        {
            UIManager.GameOverUI();
            GameState = GameState.GameOver;

            if (!SelectedCharacter.OnRevive()) { ResetGame(); return; }
            GameState = GameState.Town;
            GameTime = GameTime.Afternoon;
        }

        /// <summary>
        /// Reset the game to initial state.
        /// </summary>
        private void ResetGame()
        {
            GameState = GameState.MainMenu;
            GameTime = GameTime.Afternoon;

            Exposables.Clear();
            GroundLevel = 1;
            Quota = 10;
        }
    }
}
