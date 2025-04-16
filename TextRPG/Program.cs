using System.Diagnostics;

namespace TextRPG
{
    class InGame
    {
        // Field
        public GameManager GameManager { get; private set; }
        public SpawnManager SpawnManager { get; private set; }

        // Constructor
        public InGame(GameManager gameManager, SpawnManager spawnManager)
        {
            GameManager = gameManager;
            SpawnManager = spawnManager;    
        }

        /// <summary>
        /// Player chooses to start the game or exit
        /// </summary>
        /// <returns>Returns true, when player chooses to start the game.</returns>
        private bool InMainMenu()
        {
            return GameManager.SelectGameOption();
        }

        /// <summary>
        /// Player goes to cabin and choose to how much health to be restored
        /// </summary>
        private void InRest()
        {
            bool IsSelected = false;
            int option = 1;
            while(!IsSelected)
            {
                UIManager.CabinUI();

                if(!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); continue; }
                
                switch (Math.Clamp(opt, 1, 3))
                {
                    case 1:
                        if (GameManager.SelectedCharacter.Currency < 20) { Console.WriteLine("| Not enough Money! |"); }
                        else { IsSelected = true; option = 1; }
                        break;
                    case 2:
                        if (GameManager.SelectedCharacter.Currency < 40) { Console.WriteLine("| Not enough Money! |"); }
                        else { IsSelected = true; option = 2; }
                        break;
                    case 3:
                        if (GameManager.SelectedCharacter.Currency < 60) { Console.WriteLine("| Not enough Money! |"); }
                        else { IsSelected = true; option = 3; }
                        break;
                }
            }

            Console.WriteLine("| Have a sweet dream! |");

            GameManager.SelectedCharacter.Currency -= (option * 20);
            GameManager.SelectedCharacter.Health += GameManager.SelectedCharacter.MaxHealth * (0.5f + (0.25f * (option - 1)));
            if (GameManager.GameTime == GameTime.Afternoon) GameManager.GameTime = GameTime.Night;
            else { GameManager.GameTime = GameTime.Afternoon; GameManager.RemoveAllBuffs(); }
        }

        /// <summary>
        /// Player chooses to look at the inventory.
        /// </summary>
        private void InInventory()
        {
            Character character = GameManager.SelectedCharacter;
            
            while (true)
            {
                UIManager.InventoryUI(character);

                // Choose Option what to do in inventory
                Console.WriteLine();
                Console.WriteLine("| ------------- |");
                Console.WriteLine("| Options |");
                Console.WriteLine("| 1. Back |");
                Console.WriteLine("| 2. Select Item |");
                Console.WriteLine("| ------------- |");
                Console.Write("Select Option : ");

                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); continue; }
                
                switch (Math.Clamp(opt, 1, 2))
                {
                    case 1: return;
                    case 2:
                        // Select Category
                        Console.Write("Type item category and index ( Type [ Category,Index ] ) : ");
                        string[] vals = Console.ReadLine().Split(new char[] { ',', ' ', '|' });
                        if (!int.TryParse(vals[0].Trim(new char[] { '[', ']', ' ', ',' }), out int cat)) { Console.WriteLine("| Invalid Input! |"); break; }
                        if (!int.TryParse(vals[1].Trim(new char[] { '[', ']', ' ', ',' }), out int ind)) { Console.WriteLine("| Invalid Input! |"); break; }
                        
                        ItemCategory category = (ItemCategory)(Math.Clamp(cat, 1, 3) - 1);
                        IWearable? wearable = null;
                        IPickable? pickable = null;
                        IUseable? useable = null;
                        switch (category)
                        {
                            case ItemCategory.Armor:
                                if (character.Armors.Count < 1) break;
                                wearable = character.Armors[Math.Clamp(ind - 1, 0, character.Armors.Count - 1)];
                                pickable = character.Armors[Math.Clamp(ind - 1, 0, character.Armors.Count - 1)];
                                break;
                            case ItemCategory.Weapon:
                                if (character.Weapons.Count < 1) break;
                                wearable = character.Weapons[Math.Clamp(ind - 1, 0, character.Weapons.Count - 1)];
                                pickable = character.Weapons[Math.Clamp(ind - 1, 0, character.Weapons.Count - 1)];
                                break;
                            case ItemCategory.Consumable:
                                if (character.Consumables.Count < 1) break;
                                useable = character.Consumables[Math.Clamp(ind - 1, 0, character.Consumables.Count - 1)];
                                break;
                        }

                        // Check If there is item in array
                        if (wearable == null && useable == null && pickable == null) { Console.WriteLine("| Selected Category is empty! |"); break; }

                        // Select Item and Modify
                        if (category == ItemCategory.Armor || category == ItemCategory.Weapon)
                        {
                            Console.WriteLine();
                            Console.WriteLine("| ------------- |");
                            Console.WriteLine("| Options |");
                            Console.WriteLine("| 1. Back |");
                            Console.WriteLine("| 2. Equip |");
                            Console.WriteLine("| 3. Unequip |");
                            Console.WriteLine("| 4. Drop |");
                            Console.WriteLine("| ------------- |");
                            Console.Write("Select Option : ");
                            if (!int.TryParse(Console.ReadLine(), out int index)) { Console.WriteLine("| Invalid Input! |"); break; }
                            
                            switch (Math.Clamp(index, 1, 4))
                            {
                                case 1: break;
                                case 2: wearable?.OnEquipped(GameManager.SelectedCharacter); break;
                                case 3: wearable?.OnUnequipped(GameManager.SelectedCharacter); break;
                                case 4: pickable?.OnDropped(GameManager.SelectedCharacter); break;
                            }
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("| ------------- |");
                            Console.WriteLine("| Options |");
                            Console.WriteLine("| 1. Back |");
                            Console.WriteLine("| 2. Use  |");
                            Console.WriteLine("| ------------- |");
                            Console.Write("Select Option : ");
                            if (!int.TryParse(Console.ReadLine(), out int index)) { Console.WriteLine("| Invalid Input! |"); break; }
                            
                            switch (Math.Clamp(index, 1, 2))
                            {
                                case 1: break;
                                case 2: useable.OnUsed(GameManager.SelectedCharacter); break;
                            }
                        }
                        break;
                    default: Console.WriteLine("Something is wrong!"); break;
                }
            }
        }

        /// <summary>
        /// Player chooses to look at the current status of the character
        /// </summary>
        private void InStatus() 
        {
            UIManager.StatusUI(GameManager.SelectedCharacter);
        }
        
        // TODO : Save and Load -> Optional
        /// <summary>
        /// Player chooses to save, load, or end game.
        /// </summary>
        private void InOption()
        {
            while (true)
            {
                UIManager.BaseUI("Options", typeof(SettingOptions));

                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); continue; }
            
                switch ((SettingOptions)(opt - 1))
                {
                    case SettingOptions.Back: return;
                    case SettingOptions.Save: break;
                    case SettingOptions.Load: break;
                    case SettingOptions.EndGame: GameManager.GameState = GameState.MainMenu; Console.WriteLine(); return;
                    default: Console.WriteLine("Invalid Input"); break;
                }
            }
        }
        
        /// <summary>
        /// Gives interface what player can buy or sell items
        /// </summary>
        private void InShop()
        {
            Character character = GameManager.SelectedCharacter;

            while (true)
            {
                UIManager.ShopUI();
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); continue; }
                switch (Math.Clamp(opt, 1, 3))
                {
                    // Exit from shop
                    case 1: Console.WriteLine("| Have a nice day! |"); return;
                    
                    // Buy Item in shop
                    case 2:
                        UIManager.ShowShopList();
                        string[] vals1 = Console.ReadLine().Split(new char[] { ',', ' ', '|' });
                        if (!int.TryParse(vals1[0].Trim(new char[] {'[',']',' ',','}),out int cat1)) { Console.WriteLine("| Invalid Input! |"); break; }
                        if (!int.TryParse(vals1[1].Trim(new char[] {'[',']',' ',','}),out int ind1)) { Console.WriteLine("| Invalid Input! |"); break; }
                        
                        switch(Math.Clamp(cat1, 1, 3))
                        {
                            case 1:
                                if(ItemLists.armors.Length < ind1 || ind1 < 1) 
                                { 
                                    Console.WriteLine("| Item does not exist! |"); break; 
                                }
                                ItemLists.armors[ind1 - 1].OnPurchased(character); break;
                            case 2:
                                if (ItemLists.weapons.Length < ind1 || ind1 < 1)
                                {
                                    Console.WriteLine("| Item does not exist! |"); break;
                                }
                                ItemLists.weapons[ind1 - 1].OnPurchased(character); break;
                            case 3:
                                if (ItemLists.consumables.Length < ind1 || ind1 < 1)
                                {
                                    Console.WriteLine("| Item does not exist! |"); break;
                                }
                                ItemLists.consumables[ind1 - 1].OnPurchased(character); break;
                        }
                        break;

                    // Sell Item in inventory
                    case 3:
                        UIManager.ShowItemList(character);
                        string[] vals2 = Console.ReadLine().Split(new char[] { ',', ' ', '|' });
                        if (!int.TryParse(vals2[0].Trim(new char[] { '[', ']', ' ', ',' }), out int cat2)) { Console.WriteLine("| Invalid Input! |"); break; }
                        if (!int.TryParse(vals2[1].Trim(new char[] { '[', ']', ' ', ',' }), out int ind2)) { Console.WriteLine("| Invalid Input! |"); break; }

                        switch (Math.Clamp(cat2, 1, 3))
                        {
                            case 1:
                                if (character.Armors.Count < ind2 || ind2 < 1)
                                {
                                    Console.WriteLine("| Item does not exist! |"); break;
                                }
                                character.Armors[ind2 - 1].OnSold(character); break;
                            case 2:
                                if (character.Weapons.Count < ind2 || ind2 < 1)
                                {
                                    Console.WriteLine("| Item does not exist! |"); break;
                                }
                                character.Weapons[ind2 - 1].OnSold(character); break;
                            case 3:
                                if (character.Consumables.Count < ind2 || ind2 < 1)
                                {
                                    Console.WriteLine("| Item does not exist! |"); break;
                                }
                                character.Consumables[ind2 - 1].OnSold(character); break;
                        }
                        break;
                }
            }
        }
        
        /// <summary>
        /// Gives interface what player should do in town(Shop, Rest, Dungeon, Inventory, Status, Option).
        /// </summary>
        private void InTown()
        {
            UIManager.BaseUI("The Town of Adventurers", typeof(IdleOptions));

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); return; }
            
            switch ((IdleOptions)(Math.Clamp(opt - 1, 0, Enum.GetValues(typeof(IdleOptions)).Length - 1)))
            {
                case IdleOptions.Shop: InShop(); break;
                case IdleOptions.Dungeon: GameManager.GameState = GameState.Dungeon; break;
                case IdleOptions.Rest: InRest(); break;
                case IdleOptions.Inventory: InInventory(); break;
                case IdleOptions.Status: InStatus(); break;
                case IdleOptions.Option: InOption(); break;
                default: Console.WriteLine("| Something is wrong! |"); break;
            }
        }
        
        // TODO: Monster Encounter
        private void InDungeon()
        {
            if (SpawnManager.KilledMonsterCount >= GameManager.Quota)
            {
                SpawnManager.ResetKillCount();
                GameManager.GoToNextLevel();
            }
            Console.WriteLine();
            Console.WriteLine("| ------------------------------------------------------------------------------- |");
            Console.WriteLine($"| Killed Monsters : {SpawnManager.KilledMonsterCount}, Quota : {GameManager.Quota} |");
            Console.WriteLine("| ------------------------------------------------------------------------------- |");
            UIManager.BaseUI($"The Dungeon Lv{GameManager.GroundLevel}", typeof(DungeonOptions));

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); return; }
            
            int random = new Random().Next(0, 10);
            switch ((DungeonOptions)(Math.Clamp(opt - 1, 0, Enum.GetValues(typeof(DungeonOptions)).Length - 1)))
            {
                case DungeonOptions.Forward:
                    if(random >= 0 && random < 2)
                    {
                        Console.WriteLine("| Warning! : Encountered Monsters |");
                        SpawnManager.SpawnMonsters(GameManager.SelectedCharacter);
                        GameManager.GameState = GameState.Battle;
                    }
                    else { Console.WriteLine("| Nothing found, it's cold and silent. |"); }
                    break;
                case DungeonOptions.Left:
                    if (random >= 2 && random < 5)
                    {
                        Console.WriteLine("| Warning! : Encountered Monsters |");
                        SpawnManager.SpawnMonsters(GameManager.SelectedCharacter);
                        GameManager.GameState = GameState.Battle;
                    }
                    else { Console.WriteLine("| Nothing found, it's cold and silent. |"); }
                    break;
                case DungeonOptions.Right:
                    if (random >= 5 && random < 8)
                    {
                        Console.WriteLine("| Warning! : Encountered Monsters |");
                        SpawnManager.SpawnMonsters(GameManager.SelectedCharacter);
                        GameManager.GameState = GameState.Battle;
                    }
                    else { Console.WriteLine("| Nothing found, it's cold and silent. |"); }
                    break;
                case DungeonOptions.Backward:
                    if (random >= 8 && random < 10)
                    {
                        Console.WriteLine("| Warning! : Encountered Monsters |");
                        SpawnManager.SpawnMonsters(GameManager.SelectedCharacter);
                        GameManager.GameState = GameState.Battle;
                    }
                    else { Console.WriteLine("| Nothing found, it's cold and silent. |"); }
                    break;
                case DungeonOptions.Inventory: InInventory(); break;
                case DungeonOptions.Status: InStatus(); break;
                case DungeonOptions.BackToTown:
                    GameManager.GameState = GameState.Town;
                    if (GameManager.GameTime == GameTime.Afternoon) GameManager.GameTime = GameTime.Night;
                    else { GameManager.GameTime = GameTime.Afternoon; GameManager.RemoveAllBuffs(); }
                    break;
                default: Console.WriteLine("| Something is wrong! |"); break;
            }
        }

        // TODO : Attack and Defend, Monster Attack Mech.
        private void InBattle()
        {
            Console.WriteLine();
            Console.WriteLine("| ------------------------------------------------------------------------------- |");
            Console.WriteLine($"| Killed Monsters : {SpawnManager.KilledMonsterCount}, Quota : {GameManager.Quota} |");
            Console.WriteLine("| ------------------------------------------------------------------------------- |");

            UIManager.BaseUI("Kill the monsters", typeof(BattleOptions));

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); return; }

            switch ((BattleOptions)(Math.Clamp(opt - 1, 0, Enum.GetValues(typeof(BattleOptions)).Length - 1)))
            {
                case BattleOptions.Attack:
                    UIManager.ShowMonsterList(SpawnManager);
                    if(!int.TryParse(Console.ReadLine(), out int ind)) { Console.WriteLine("| Invalid Input! |"); return; }
                    if (ind > 0 && ind <= SpawnManager.GetMonsterCount()) {
                        AttackType? type = GameManager.SelectedCharacter.EquippedWeapon?.AttackType;
                        switch (type)
                        {
                            case AttackType.Close: SpawnManager.spawnedMonsters[ind - 1].OnDamage(AttackType.Close, GameManager.SelectedCharacter.AttackStat.Attack); break;
                            case AttackType.Long: SpawnManager.spawnedMonsters[ind - 1].OnDamage(AttackType.Close, GameManager.SelectedCharacter.AttackStat.RangeAttack); break;
                            case AttackType.Magic: SpawnManager.spawnedMonsters[ind - 1].OnDamage(AttackType.Close, GameManager.SelectedCharacter.AttackStat.MagicAttack); break;
                            default: SpawnManager.spawnedMonsters[ind - 1].OnDamage(AttackType.Close, GameManager.SelectedCharacter.AttackStat.Attack); break;
                        }
                    } else { Console.WriteLine("| Invalid Input! |"); return; }
                    break;
                case BattleOptions.Inventory: InInventory(); return;
                case BattleOptions.Status: InStatus(); return;
                case BattleOptions.Escape: SpawnManager.RemoveAllMonsters(); GameManager.GameState = GameState.Dungeon; return;
                default: Console.WriteLine("| Something is wrong! |"); return;
            }
            
            if (SpawnManager.GetMonsterCount() <= 0) { GameManager.GameState = GameState.Dungeon; return; }
            
            foreach(Monster monster in SpawnManager.spawnedMonsters)
            {
                if(monster.AttackType == AttackType.Close) GameManager.SelectedCharacter.OnDamage(AttackType.Close, monster.AttackStat.Attack);
                else if(monster.AttackType == AttackType.Long) GameManager.SelectedCharacter.OnDamage(AttackType.Long, monster.AttackStat.RangeAttack);
                else GameManager.SelectedCharacter.OnDamage(AttackType.Magic, monster.AttackStat.MagicAttack);
            }
        }

        /// <summary>
        /// Runs the game.
        /// </summary>
        public void MainGame()
        {
            while (true)
            {
                switch (GameManager.GameState)
                {
                    case GameState.MainMenu: if(!InMainMenu()) return; break;
                    case GameState.Town: InTown(); break;
                    case GameState.Dungeon: InDungeon(); break;
                    case GameState.Battle: InBattle(); break;
                    case GameState.GameOver: break;
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Network Connection -> Not actually...
            while(NetworkManager.LoginOrLogup()) 
            {
                // Game Start UI
                UIManager.StartUI("Welcome aboard to the Journey of Spartan");
                InGame inGame = new(new GameManager(), new SpawnManager());
                
                // Main Game
                inGame.MainGame();
            }
        }
    }
}