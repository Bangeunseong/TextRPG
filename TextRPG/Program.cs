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
        /// Player can start the game or end the game.
        /// </summary>
        /// <returns>Returns true, when player chooses to start the game.</returns>
        private bool InMainMenu()
        {
            int option;
            while (true)
            {
                UIManager.GameOptionUI();
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); }
                else if(opt < 1 || opt > Enum.GetValues(typeof(GameOption)).Length) { Console.WriteLine("| Invalid Input! |");  }
                else { option = Math.Clamp(opt, 1, Enum.GetValues(typeof(GameOption)).Length); break; }
            }

            switch ((GameOption)(option - 1))
            {
                case GameOption.NewGame: GameManager.SelectJob(); GameManager.GameState = GameState.Town; break;
                case GameOption.Continue: GameManager.LoadGame(); break;
                case GameOption.Exit: return false;
            }

            return true;
        }

        #region Town Method and Misc. Methods -> Rest, Inventory, Status, Option, Shop
        /// <summary>
        /// Player can go to cabin and choose to how much health to be restored.
        /// </summary>
        private void InRest()
        {
            bool isSelected = false;
            int option = 1;

            while(!isSelected)
            {
                UIManager.CabinUI();

                if(!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); continue; }
                else if(opt < 1 || opt > 4) { Console.WriteLine("| Invalid Input! |"); continue; }
                switch (Math.Clamp(option, 1, 4))
                {
                    case 1: Console.WriteLine("| Have a great day! |"); return;
                    case 2:
                        if (GameManager.SelectedCharacter.Currency < 20) { Console.WriteLine("| Not enough Money! |"); }
                        else { isSelected = true; }
                        break;
                    case 3:
                        if (GameManager.SelectedCharacter.Currency < 40) { Console.WriteLine("| Not enough Money! |"); }
                        else { isSelected = true; }
                        break;
                    case 4:
                        if (GameManager.SelectedCharacter.Currency < 60) { Console.WriteLine("| Not enough Money! |"); }
                        else { isSelected = true; }
                        break;
                }
            }

            Console.WriteLine("| Have a sweet dream! |");

            GameManager.SelectedCharacter.Currency -= (option * 20);
            GameManager.SelectedCharacter.OnHeal(GameManager.SelectedCharacter.MaxHealth * (0.5f + (0.25f * (option - 1))));
            if (GameManager.GameTime == GameTime.Afternoon) GameManager.GameTime = GameTime.Night;
            else { GameManager.GameTime = GameTime.Afternoon; GameManager.RemoveAllBuffs(); }
        }

        /// <summary>
        /// Player can look at the inventory of the character.
        /// </summary>
        private void InInventory()
        {   
            while (true)
            {
                // Prints Inventory UI
                UIManager.InventoryUI(GameManager.SelectedCharacter);

                // Get Input from user
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); continue; }
                else if(opt < 1 || opt > 2) { Console.WriteLine("| Invalid Input! |"); continue; }

                // If "Back" option selected, go back to main game.
                if (opt == 1) return;

                // Select Category and Index of Item
                Console.Write("Type item category and index ( Type [ Category,Index ] ) : ");
                string[] vals = Console.ReadLine().Split(new char[] { ',', ' ', '|' });
                if (!int.TryParse(vals[0].Trim(new char[] { '[', ']', ' ', ',' }), out int cat)) { Console.WriteLine("| Invalid Input! |"); break; }
                if (!int.TryParse(vals[1].Trim(new char[] { '[', ']', ' ', ',' }), out int ind)) { Console.WriteLine("| Invalid Input! |"); break; }
                
                ItemCategory category = (ItemCategory)(Math.Clamp(cat, 1, 3) - 1);
                
                // Get interfaces from selected item to change its state
                InInventory_SelectItem(category, ind, out IWearable? wearable, out IPickable? pickable, out IUseable? useable);

                // Check If there is item in array
                if (wearable == null && useable == null && pickable == null) { Console.WriteLine("| Selected Category is empty! |"); break; }

                // Select Item and Modify
                if(!InInventory_ChangeStateOfItem(category, wearable, useable, pickable)) continue;
            }
        }

        /// <summary>
        /// This method is used to extract selected item's interfaces.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="ind"></param>
        /// <param name="wearable"></param>
        /// <param name="pickable"></param>
        /// <param name="useable"></param>
        private void InInventory_SelectItem(ItemCategory category, int ind, out IWearable? wearable, out IPickable? pickable, out IUseable? useable)
        {
            Character character = GameManager.SelectedCharacter;
            wearable = null; pickable = null; useable = null;

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
        }

        /// <summary>
        /// This Method is used to change selected item state.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="wearable"></param>
        /// <param name="useable"></param>
        /// <param name="pickable"></param>
        /// <returns></returns>
        private bool InInventory_ChangeStateOfItem(ItemCategory category, IWearable? wearable, IUseable? useable, IPickable? pickable)
        {
            // Select Item and Modify
            if (category == ItemCategory.Armor || category == ItemCategory.Weapon)
            {
                UIManager.InventoryUI_Equipment();
                if (!int.TryParse(Console.ReadLine(), out int index)) { Console.WriteLine("| Invalid Input! |"); return false; }

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
                UIManager.InventoryUI_Consumable();
                if (!int.TryParse(Console.ReadLine(), out int index)) { Console.WriteLine("| Invalid Input! |"); return false; }

                switch (Math.Clamp(index, 1, 2))
                {
                    case 1: break;
                    case 2: useable.OnUsed(GameManager.SelectedCharacter); break;
                }
            }
            return true;
        }

        /// <summary>
        /// Player can look at the current status of the character
        /// </summary>
        private void InStatus() 
        {
            UIManager.StatusUI(GameManager.SelectedCharacter);
        }

        /// <summary>
        /// Gives interface that player can save, load, or end game.
        /// </summary>
        private void InOption()
        {
            while (true)
            {
                UIManager.SettingOptionUI();

                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); continue; }
            
                switch ((SettingOptions)(opt - 1))
                {
                    case SettingOptions.Back: return;
                    case SettingOptions.Save: GameManager.SaveGame(); break;
                    case SettingOptions.Load: GameManager.LoadGame(); break;
                    case SettingOptions.EndGame: GameManager.GameState = GameState.MainMenu; Console.WriteLine(); return;
                    default: Console.WriteLine("Invalid Input"); continue;
                }
            }
        }
        
        /// <summary>
        /// Gives interface what player can buy or sell items.
        /// </summary>
        private void InShop()
        {
            Character character = GameManager.SelectedCharacter;

            while (true)
            {
                UIManager.ShopUI(character);
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); continue; }
                else if(opt < 1 || opt > 3) { Console.WriteLine("| Invalid Input! |"); continue; }
                    switch (Math.Clamp(opt, 1, 3))
                    {
                        // Exit from shop
                        case 1: Console.WriteLine("| Have a nice day! |"); return;

                        // Buy Item in shop
                        case 2:
                            UIManager.ShowShopList();
                            string[] vals1 = Console.ReadLine().Split(new char[] { ',', ' ', '|' });
                            if (!int.TryParse(vals1[0].Trim(new char[] { '[', ']', ' ', ',' }), out int cat1)) { Console.WriteLine("| Invalid Input! |"); break; }
                            if (!int.TryParse(vals1[1].Trim(new char[] { '[', ']', ' ', ',' }), out int ind1)) { Console.WriteLine("| Invalid Input! |"); break; }
                            InShop_Buy((ItemCategory)(cat1 - 1), ind1);
                            break;

                        // Sell Item in inventory
                        case 3:
                            UIManager.ShowItemList(character);
                            string[] vals2 = Console.ReadLine().Split(new char[] { ',', ' ', '|' });
                            if (!int.TryParse(vals2[0].Trim(new char[] { '[', ']', ' ', ',' }), out int cat2)) { Console.WriteLine("| Invalid Input! |"); break; }
                            if (!int.TryParse(vals2[1].Trim(new char[] { '[', ']', ' ', ',' }), out int ind2)) { Console.WriteLine("| Invalid Input! |"); break; }
                            InShop_Sell((ItemCategory)(cat2 - 1), ind2);
                            break;
                    }
            }
        }

        /// <summary>
        /// Purchase Mechanism of item in shop.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="ind"></param>
        private void InShop_Buy(ItemCategory category, int ind)
        {
            switch (category)
            {
                case ItemCategory.Armor:
                    if (ItemLists.Armors.Length < ind || ind < 1)
                    {
                        Console.WriteLine("| Item does not exist! |"); break;
                    }
                    ItemLists.Armors[ind - 1].OnPurchased(GameManager.SelectedCharacter); break;
                case ItemCategory.Weapon:
                    if (ItemLists.Weapons.Length < ind || ind < 1)
                    {
                        Console.WriteLine("| Item does not exist! |"); break;
                    }
                    ItemLists.Weapons[ind - 1].OnPurchased(GameManager.SelectedCharacter); break;
                case ItemCategory.Consumable:
                    if (ItemLists.Consumables.Length < ind || ind < 1)
                    {
                        Console.WriteLine("| Item does not exist! |"); break;
                    }
                    ItemLists.Consumables[ind - 1].OnPurchased(GameManager.SelectedCharacter); break;
            }
        }

        /// <summary>
        /// Selling Mechanism of item in player inventory.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="ind"></param>
        private void InShop_Sell(ItemCategory category, int ind)
        {
            switch (category)
            {
                case ItemCategory.Armor:
                    if (GameManager.SelectedCharacter.Armors.Count < ind || ind < 1)
                    {
                        Console.WriteLine("| Item does not exist! |"); break;
                    }
                    GameManager.SelectedCharacter.Armors[ind - 1].OnSold(GameManager.SelectedCharacter); break;
                case ItemCategory.Weapon:
                    if (GameManager.SelectedCharacter.Weapons.Count < ind || ind < 1)
                    {
                        Console.WriteLine("| Item does not exist! |"); break;
                    }
                    GameManager.SelectedCharacter.Weapons[ind - 1].OnSold(GameManager.SelectedCharacter); break;
                case ItemCategory.Consumable:
                    if (GameManager.SelectedCharacter.Consumables.Count < ind || ind < 1)
                    {
                        Console.WriteLine("| Item does not exist! |"); break;
                    }
                    GameManager.SelectedCharacter.Consumables[ind - 1].OnSold(GameManager.SelectedCharacter); break;
            }
        }
        
        // TODO: Implement Quest System
        /// <summary>
        /// Gives interface what player can do in quest.
        /// </summary>
        private void InQuest()
        {

        }
        
        /// <summary>
        /// Gives interface what player can do in town(Shop, Rest, Dungeon, Inventory, Status, Option).
        /// </summary>
        private void InTown()
        {
            UIManager.BaseUI(GameManager.SelectedCharacter, "The Town of Adventurers", typeof(IdleOptions));

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); return; }
            else if(opt < 1 || opt > Enum.GetValues(typeof(IdleOptions)).Length) { Console.WriteLine("| Invalid Input! |"); return; }
                switch ((IdleOptions)(Math.Clamp(opt - 1, 0, Enum.GetValues(typeof(IdleOptions)).Length - 1)))
                {
                    case IdleOptions.Shop: InShop(); break;
                    case IdleOptions.Quest: InQuest(); break;
                    case IdleOptions.Dungeon: GameManager.GameState = GameState.Dungeon; break;
                    case IdleOptions.Rest: InRest(); break;
                    case IdleOptions.Inventory: InInventory(); break;
                    case IdleOptions.Status: InStatus(); break;
                    case IdleOptions.Option: InOption(); break;
                    default: Console.WriteLine("| Something is wrong! |"); break;
                }
        }
        #endregion

        #region Dungeon Methods
        /// <summary>
        /// Gives interface what player can do in dungeon(Path selection, Inventory, Status, BackToTown).
        /// </summary>
        private void InDungeon()
        {
            // Check for Quota completion
            if (SpawnManager.KilledMonsterCount >= GameManager.Quota)
            {
                SpawnManager.ResetKillCount();
                GameManager.GoToNextLevel();
            }

            // Print UI of Kill Count and Player Options
            UIManager.KillCountUI(SpawnManager.KilledMonsterCount, GameManager.Quota);
            UIManager.BaseUI(GameManager.SelectedCharacter, $"The Dungeon Lv{GameManager.GroundLevel}", typeof(DungeonOptions));

            // Try parsing, if successed clamp Parsed Input
            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); return; }
            else if(opt < 1 || opt > Enum.GetValues(typeof(DungeonOptions)).Length) { Console.WriteLine("| Invalid Input! |"); return; }
            opt = Math.Clamp(opt - 1, 0, Enum.GetValues(typeof(DungeonOptions)).Length - 1);

            // Choices
            if (opt >= 0 && opt < 4) { InDungeon_MonsterEncounter((DungeonOptions)opt); }
            else if(opt == 4) { InInventory(); }
            else if(opt == 5) { InStatus();}
            else { InDungeon_ReturnToTown(); }
        }

        /// <summary>
        /// Mechanism of Monster Encounter in dungeon.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="random"></param>
        /// <param name="spawnManager"></param>
        /// <param name="gameManager"></param>
        private void InDungeon_MonsterEncounter(DungeonOptions option)
        {
            int random = new Random().Next(0, 10);
            if (random >= ((int)option*2) && random < ((int)option * 2 + 2 +((int)option % 2)))
            {
                SpawnManager.SpawnMonsters(GameManager.SelectedCharacter, GameManager.GroundLevel);
                UIManager.MonsterEncounterUI(SpawnManager);
                GameManager.GameState = GameState.Battle;
            }
            else { UIManager.NoMonsterFoundUI(); }
        }

        /// <summary>
        /// Exit from dungeon and return to town.
        /// </summary>
        /// <param name="gameManager"></param>
        private void InDungeon_ReturnToTown()
        {
            GameManager.GameState = GameState.Town;
            if (GameManager.GameTime == GameTime.Afternoon) GameManager.GameTime = GameTime.Night;
            else { GameManager.GameTime = GameTime.Afternoon; GameManager.RemoveAllBuffs(); }
        }
        #endregion

        #region Battle Methods
        /// <summary>
        /// Gives interface what player can do in battle(Attack, Inventory, Status, Escape).
        /// </summary>
        private void InBattle()
        {
            UIManager.BaseUI(GameManager.SelectedCharacter, "Kill the monsters", typeof(BattleOptions));

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); return; }

            switch ((BattleOptions)(Math.Clamp(opt - 1, 0, Enum.GetValues(typeof(BattleOptions)).Length - 1)))
            {
                case BattleOptions.Attack: InBattle_Attack(); break;
                case BattleOptions.Inventory: InInventory(); return;
                case BattleOptions.Status: InStatus(); return;
                case BattleOptions.Escape: SpawnManager.RemoveAllMonsters(); GameManager.GameState = GameState.Dungeon; return;
                default: Console.WriteLine("| Something is wrong! |"); return;
            }
            
            if (SpawnManager.GetMonsterCount() <= 0) { GameManager.GameState = GameState.Dungeon; return; }
            
            // Monster Attack Mechanism
            foreach(Monster monster in SpawnManager.spawnedMonsters)
            {
                if(monster.AttackType == AttackType.Close) GameManager.SelectedCharacter.OnDamage(AttackType.Close, monster.AttackStat.Attack);
                else if(monster.AttackType == AttackType.Long) GameManager.SelectedCharacter.OnDamage(AttackType.Long, monster.AttackStat.RangeAttack);
                else GameManager.SelectedCharacter.OnDamage(AttackType.Magic, monster.AttackStat.MagicAttack);
            }
        }

        /// <summary>
        /// Attack Mechanism of player
        /// </summary>
        private void InBattle_Attack()
        {
            UIManager.ShowMonsterList(SpawnManager);
            int opt;
            while(true) { 
                if (!int.TryParse(Console.ReadLine(), out int ind)) Console.WriteLine("| Invalid Input! |"); 
                else { opt = Math.Clamp(ind, 1, SpawnManager.GetMonsterCount()); break; } 
            }
            if (opt > 0 && opt <= SpawnManager.GetMonsterCount())
            {
                AttackType? type = GameManager.SelectedCharacter.EquippedWeapon?.AttackType;
                switch (type)
                {
                    case AttackType.Close: SpawnManager.spawnedMonsters[opt - 1].OnDamage(AttackType.Close, GameManager.SelectedCharacter.AttackStat.Attack); break;
                    case AttackType.Long: SpawnManager.spawnedMonsters[opt - 1].OnDamage(AttackType.Close, GameManager.SelectedCharacter.AttackStat.RangeAttack); break;
                    case AttackType.Magic: SpawnManager.spawnedMonsters[opt - 1].OnDamage(AttackType.Close, GameManager.SelectedCharacter.AttackStat.MagicAttack); break;
                    default: SpawnManager.spawnedMonsters[opt - 1].OnDamage(AttackType.Close, GameManager.SelectedCharacter.AttackStat.Attack); break;
                }
            }
            else { Console.WriteLine("| Invalid Input! |"); return; }
        }
        #endregion

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
            // Game Start UI
            UIManager.StartUI();
            InGame inGame = new(new GameManager(), new SpawnManager());

            // Main Game
            inGame.MainGame();
        }
    }
}