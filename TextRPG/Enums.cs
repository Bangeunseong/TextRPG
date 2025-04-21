namespace TextRPG
{
    #region InGame Options
    public enum IdleOptions
    {
        Shop, Quest, Dungeon, Rest, Inventory, Status, Option,
    }
    
    public enum DungeonOptions
    {
        Forward, Left, Right, Backward, Inventory, Status, BackToTown, 
    }
    
    public enum BattleOptions
    {
        Attack, Inventory, Status, Escape,
    }

    public enum SettingOptions
    {
        Back, Save, Load, EndGame,
    }
    #endregion

    #region Category and Rarity of Items
    public enum AttackType
    {
        Close, Long, Magic,
    }

    public enum ItemCategory
    {
        Armor, Weapon, Consumable
    }

    public enum ConsumableCategory
    {
        IncreaseHealth,
        IncreaseMagicPoint,
        IncreaseAttack,
        IncreaseDefence,
        IncreaseAllStat,
    }

    public enum Rarity
    {
        Common,
        Exclusive,
        Rare,
        Hero,
        Legend,
    }
    #endregion

    #region Armor Position
    public enum ArmorPosition
    {
        Head,
        Torso,
        Leg,
        Foot,
        Arm,
    }
    #endregion

    #region Quest Difficulty
    public enum QuestDifficulty
    {
        Easy, Normal, Hard,
    }
    #endregion

    #region Game Mechanism Sources
    public enum GameOption
    {
        NewGame,
        Continue,
        Exit,
    }

    public enum GameState
    {
        MainMenu,
        Town,
        Dungeon,
        Battle,
        GameOver,
    }

    public enum GameTime
    {
        Afternoon, Night,
    }
    
    public enum Job
    {
        Warrior, Wizard, Archer,
    }
    #endregion
}
