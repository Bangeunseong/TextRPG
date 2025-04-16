namespace TextRPG
{
    static class UIManager
    {
        public static void LoginUI()
        {
            Console.WriteLine("| ------------- |");
            Console.WriteLine("|\"Login Page\" |");
            Console.WriteLine("| 1. Login |");
            Console.WriteLine("| 2. Logup |");
            Console.WriteLine("| 3. Exit |");
            Console.WriteLine("| ------------- |");
            Console.Write("Select Option : ");
        }

        public static void StartUI(string Title)
        {
            Console.WriteLine();
            Console.WriteLine("| ---------------------------------------- |");
            Console.WriteLine($"| {Title} |");
            Console.WriteLine("| ---------------------------------------- |");
            Console.WriteLine("| Press enter key to start... |");
            Console.ReadLine();
        }

        public static void Before_JobSelectionUI()
        {
            Console.WriteLine("| ------------- |");
            Console.WriteLine("| 1. Choose Job |");
            Console.WriteLine("| 2. Back to Login Page |");
            Console.WriteLine("| ------------- |");
            Console.Write("Select Option : ");
        }

        public static void InventoryUI(Character character)
        {
            Console.WriteLine();
            Console.WriteLine("| ------------- |");
            Console.WriteLine("|\"Armors\" |");
            int i = 1;
            foreach(Armor armor in character.Armors) { Console.WriteLine($"{i++}. {armor}"); }
            Console.WriteLine("|\"Weapons\" |");
            i = 1;
            foreach (Weapon weapon in character.Weapons) { Console.WriteLine($"{i++}. {weapon}"); }
            Console.WriteLine("|\"Potions\" |");
            i = 1;
            foreach (Consumables potion in character.Consumables) { Console.WriteLine($"{i++}. {potion}"); }
            Console.WriteLine("| ------------- |");
        }

        public static void ShopUI()
        {
            Console.WriteLine();
            Console.WriteLine("| ------------- |");
            Console.WriteLine("| Welcome to Henry's Shop! |");
            
            Console.WriteLine("| ------------- |");
            Console.WriteLine();
            Console.WriteLine("| Options |");
            Console.WriteLine("| 1. Back |");
            Console.WriteLine("| 2. Buy |");
            Console.WriteLine("| 3. Sell |");
            Console.Write("Select Option : ");
        }

        public static void ShowShopList()
        {
            Console.WriteLine();
            int i = 1;
            Console.WriteLine("| ------------- |");
            Console.WriteLine("|\"Armors\" |");
            foreach (Armor armor in ItemLists.armors) { Console.WriteLine($"{i++}. {armor}"); }
            i = 1;
            Console.WriteLine("|\"Weapons\" |");
            foreach (Weapon weapon in ItemLists.weapons) { Console.WriteLine($"{i++}. {weapon}"); }
            i = 1;
            Console.WriteLine("|\"Potions\" |");
            foreach (Consumables potion in ItemLists.consumables) { Console.WriteLine($"{i++}. {potion}"); }
            Console.WriteLine("| ------------- |");
            Console.Write("What do you want to buy? ( Type [ Category,Index ] ) : ");
        }

        public static void ShowItemList(Character character)
        {
            Console.WriteLine();
            Console.WriteLine("| ------------- |");
            Console.WriteLine("|\"Armors\" |");
            int i = 1;
            foreach (Armor armor in character.Armors) { Console.WriteLine($"{i++}. {armor}"); }
            Console.WriteLine("|\"Weapons\" |");
            i = 1;
            foreach (Weapon weapon in character.Weapons) { Console.WriteLine($"{i++}. {weapon}"); }
            Console.WriteLine("|\"Potions\" |");
            i = 1;
            foreach (Consumables potion in character.Consumables) { Console.WriteLine($"{i++}. {potion}"); }
            Console.WriteLine("| ------------- |");
            Console.Write("What do you want to sell? ( Type [ Category,Index ] ) : ");
        }

        public static void ShowMonsterList(SpawnManager spawnManager)
        {
            Console.WriteLine();
            Console.WriteLine("| ------------- |");
            Console.WriteLine("| \"Monsters\" |");
            int i = 1;
            foreach (Monster monster in spawnManager.spawnedMonsters)
                Console.WriteLine($"| {i++}. {monster.Name} | Health : {monster.Health} |");
            Console.Write("Select Monster : ");
        }

        public static void CabinUI()
        {
            Console.WriteLine();
            Console.WriteLine("| ------------- |");
            Console.WriteLine("| Welcome to Alby's Cabin! |");
            Console.WriteLine("| 1. Normal Room (Heals 50% of your Max Health, 20G) |");
            Console.WriteLine("| 2. Comfy Room (Heals 75% of your Max Health, 40G) |");
            Console.WriteLine("| 3. Emperror Room (Heals 100% of your Max Health, 60G)");
            Console.WriteLine("| ------------- |");
            Console.Write("Select Option : ");
        }

        public static void StatusUI(Character character)
        {
            Console.WriteLine();
            Console.WriteLine("| ------------- |");
            Console.WriteLine("| \"Character Info.\" |");
            Console.WriteLine($"| \"Name\" : {character.Name} |");
            Console.WriteLine($"| \"Lv {character.Level:D2}\" |");
            Console.WriteLine($"| \"Experience\" : {character.Exp} |");
            Console.WriteLine($"| \"Health\" : {character.Health} |");
            Console.WriteLine($"| \"Currency\" : {character.Currency} |");
            Console.WriteLine("| ------------- |");
            Console.WriteLine("| \"Status\" |");
            Console.WriteLine($"| \"Attack\" : {character.AttackStat.Attack} |");
            Console.WriteLine($"| \"Range Attack\" : {character.AttackStat.RangeAttack} |");
            Console.WriteLine($"| \"Magic Attack\" : {character.AttackStat.MagicAttack} |");
            Console.WriteLine($"| \"Defence\" : {character.DefendStat.Defend} |");
            Console.WriteLine($"| \"Range Defence\" : {character.DefendStat.RangeDefend} |");
            Console.WriteLine($"| \"Magic Defence\" : {character.DefendStat.MagicDefend} |");
            Console.WriteLine("| ------------- |");
            Console.WriteLine("| \"Items\" |");
            Console.WriteLine("|\"Armors\" |");
            foreach (Armor armor in character.Armors) { Console.WriteLine($"| {armor} |"); }
            Console.WriteLine("|\"Weapons\" |");
            foreach (Weapon weapon in character.Weapons) { Console.WriteLine($"| {weapon} |"); }
            Console.WriteLine("| \"Potions\" |");
            foreach(Consumables consumable in character.Consumables) { Console.WriteLine($"| {consumable} |"); }
            Console.WriteLine("| ------------- |");
            Console.WriteLine("Press enter key to continue...");
            Console.ReadLine();
        }

        public static void BaseUI(string headLine, Type type)
        {
            Console.WriteLine();
            Console.WriteLine("| ------------- |");
            Console.WriteLine($"| {headLine} |");
            Console.WriteLine($"| Current Time : {GameManager.GameTime} |");
            int i = 1;
            foreach (var opt in Enum.GetValues(type))
            {
                Console.WriteLine($"| {i++}. {opt} |");
            }
            Console.WriteLine("| ------------- |");
            Console.Write("Select Option : ");
        }
    }

    static class NetworkManager
    {
        private static string id;
        private static string pass;

        public static bool IsGuest { get; private set; } = true;
        public static string Rank { get; private set; } = "Bronze";
        public static bool IsNetworkActive { get; private set; } = true;

        /// <summary>
        /// Prints Login and Logup UI
        /// </summary>
        /// <returns>Returns true, when successfully logged in.</returns>
        public static bool LoginOrLogup()
        {
            while (true)
            {
                UIManager.LoginUI();

                if (int.TryParse(Console.ReadLine(), out int opt))
                {
                    if (opt >= 3) { IsNetworkActive = false; return false; }
                    switch (opt)
                    {
                        case 1: if (Login()) { IsGuest = false; return true; } Console.WriteLine("Wrong Id or Password!"); break;
                        case 2:
                            if (Logup())
                            {
                                if (Login()) { IsGuest = false; return true; }
                                Console.WriteLine("Wrong Id or Password!"); break;
                            }
                            Console.WriteLine("Failed to SignUp!"); break;
                        default: Console.WriteLine("Something is wrong!"); break;
                    }
                }
            }
        }

        private static bool Logup()
        {
            Console.WriteLine();

            if (IsGuest)
            {
                Console.WriteLine("| Sign Up |");
                Console.Write("Id : "); id = Console.ReadLine();
                Console.Write("Password : "); pass = Console.ReadLine();
                return true;
            }
            else return false;
        }

        private static bool Login()
        {
            Console.WriteLine();

            string Id, Pass;
            Console.WriteLine("| Sign In |");
            Console.Write("Id : "); Id = Console.ReadLine();
            Console.Write("Password : "); Pass = Console.ReadLine();
            return Id == NetworkManager.id && Pass == NetworkManager.pass;
        }
    }

    class SpawnManager
    {
        public List<Monster> spawnedMonsters = new();
        public int KilledMonsterCount { get; set; } = 0;

        public SpawnManager() { }

        public void SpawnMonsters(Character character)
        {
            int count = new Random().Next(1, 3);
            for(int i = 0; i < count; i++)
            {
                int type = new Random().Next(MonsterLists.monsters.Length);

                if (MonsterLists.monsters[type].AttackType == AttackType.Close)
                {
                    GoblinWarrior monster = new GoblinWarrior((GoblinWarrior)MonsterLists.monsters[type]);
                    monster.OnDeath += () =>
                    {
                        character.Currency += 50;
                        character.OnEarnExp(monster.Exp);
                        KilledMonsterCount++;
                        RemoveMonster(monster);
                        Console.WriteLine($"| {monster.Name} is dead! |");
                    };
                    AddMonster(monster);
                }
                else if (MonsterLists.monsters[type].AttackType == AttackType.Long)
                {
                    GoblinArcher monster = new GoblinArcher((GoblinArcher)MonsterLists.monsters[type]);
                    monster.OnDeath += () =>
                    {
                        character.Currency += 65;
                        character.OnEarnExp(monster.Exp);
                        KilledMonsterCount++;
                        RemoveMonster(monster);
                        Console.WriteLine($"| {monster.Name} is dead! |");
                    };
                    AddMonster(monster);
                }
                else if (MonsterLists.monsters[type].AttackType == AttackType.Magic)
                {
                    GoblinMage monster = new GoblinMage((GoblinMage)MonsterLists.monsters[type]);
                    monster.OnDeath += () =>
                    {
                        character.Currency += 80;
                        character.OnEarnExp(monster.Exp);
                        KilledMonsterCount++;
                        RemoveMonster(monster);
                        Console.WriteLine($"| {monster.Name} is dead! |");
                    };
                    AddMonster(monster);
                }
            }
        }

        public void ResetKillCount() { KilledMonsterCount = 0; }

        public int GetMonsterCount() { return spawnedMonsters.Count; }  
        public void RemoveAllMonsters() { spawnedMonsters.Clear(); }

        private void AddMonster(Monster monster) { spawnedMonsters.Add(monster); }
        private void RemoveMonster(Monster monster) { spawnedMonsters.Remove(monster); }
    }

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

        /// <summary>
        /// Prints Job Selection Interface
        /// </summary>
        /// <returns>Returns true, if job is selected.</returns>
        public bool SelectGameOption()
        {
            UIManager.Before_JobSelectionUI();

            if(int.TryParse(Console.ReadLine(), out int opt))
            {
                switch(opt)
                {
                    case 1: if(Switch_Job()) GameState = GameState.Town; break;
                    case 2: return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Job Selection UI will be displayed.
        /// This method will return true if the job selected successfully.
        /// If not, it will return false.
        /// </summary>
        /// <returns>Returns true, if job selected successfully. If not, returns false.</returns>
        public bool Switch_Job()
        {
            Console.WriteLine();
            Console.WriteLine("| 1: Warrior | 2: Wizard | 3. Archer |");
            Console.Write("Select Job : ");
            if (int.TryParse(Console.ReadLine(), out int job))
            {
                switch ((Job)(job - 1))
                {
                    case Job.Warrior: 
                        Console.WriteLine("| You selected Warrior! |");
                        Console.Write("Type the name of your warrior : ");
                        SelectedCharacter = new Warrior(new CharacterStat(Console.ReadLine(), 150, 1, new AttackStat(30f, 6f, 1f), new DefendStat(25, 15, 5)));
                        SelectedCharacter.OnDeath += () => GameState = GameState.GameOver;
                        break;
                    case Job.Wizard: 
                        Console.WriteLine("| You selected Wizard! |");
                        Console.Write("Type the name of your wizard : ");
                        SelectedCharacter = new Wizard(new CharacterStat(Console.ReadLine(), 100, 1, new AttackStat(1f, 6f, 30f), new DefendStat(5, 10, 30)));
                        SelectedCharacter.OnDeath += () => GameState = GameState.GameOver;
                        break;
                    case Job.Archer: 
                        Console.WriteLine("| You selected Archer! |");
                        Console.Write("Type the name of your archer : ");
                        SelectedCharacter = new Archer(new CharacterStat(Console.ReadLine(), 120, 1, new AttackStat(6f, 30f, 1f), new DefendStat(15, 25, 5)));
                        SelectedCharacter.OnDeath += () => GameState = GameState.GameOver;
                        break;
                    default: return false;
                }
            }
            else { return false; }
            return true;
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

        public void GoToNextLevel()
        {
            Console.WriteLine("| Quota reached. Moving to next level! |");
            Quota = 10 + (GroundLevel - 1) * 5;
        }
    }
}
