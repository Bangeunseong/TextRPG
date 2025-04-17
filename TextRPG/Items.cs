using System.Text;
using System.Text.Json.Serialization;

namespace TextRPG
{
    /// <summary>
    /// Stat. of Attack
    /// </summary>
    public class AttackStat
    {
        // Field
        private float attack;
        private float rangeAttack;
        private float magicAttack;

        // Property
        public float Attack { get { return attack; } set { attack = Math.Max(0, value); } }
        public float RangeAttack { get { return rangeAttack; } set { rangeAttack = Math.Max(0, value); } }
        public float MagicAttack { get { return magicAttack; } set { magicAttack = Math.Max(0, value); } }
        
        public AttackStat() { Attack = 0; RangeAttack = 0; MagicAttack = 0; }
        public AttackStat(float attack, float rangeAttack, float magicAttack)
        {
            Attack = attack; 
            RangeAttack = rangeAttack; 
            MagicAttack = magicAttack;
        }
        public AttackStat(AttackStat attackStat)
        {
            Attack = attackStat.Attack;
            RangeAttack = attackStat.RangeAttack;
            MagicAttack = attackStat.MagicAttack;
        }

        public static AttackStat operator +(AttackStat stat1, AttackStat stat2)
        {
            return new AttackStat(stat1.Attack + stat2.Attack, stat1.RangeAttack + stat2.RangeAttack, stat1.MagicAttack + stat2.MagicAttack);
        }

        public static AttackStat operator -(AttackStat stat1, AttackStat stat2)
        {
            return new AttackStat(stat1.Attack - stat2.Attack, stat1.RangeAttack - stat2.RangeAttack, stat1.MagicAttack - stat2.MagicAttack);
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"Attack : {Attack}, ").Append($"Range Attack : {RangeAttack}, ").Append($"Magic Attack : {MagicAttack}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Stat. of Defend
    /// </summary>
    public class DefendStat
    {
        // Field
        private float defend;
        private float rangeDefend;
        private float magicDefend;

        // Property
        public float Defend { get { return defend; } set { defend = Math.Max(0, value); } }
        public float RangeDefend { get { return rangeDefend; } set { rangeDefend = Math.Max(0, value); } }
        public float MagicDefend { get { return magicDefend; } set { magicDefend = Math.Max(0, value); } }
        
        public DefendStat() { Defend = 0; RangeDefend = 0; MagicDefend = 0; }
        public DefendStat(float defend, float rangeDefend, float magicDefend) { Defend = defend; RangeDefend = rangeDefend; MagicDefend = magicDefend; }
        public DefendStat(DefendStat defendStat)
        {
            Defend = defendStat.Defend;
            RangeDefend = defendStat.RangeDefend;
            MagicDefend = defendStat.MagicDefend;
        }

        public static DefendStat operator +(DefendStat stat1, DefendStat stat2)
        {
            return new DefendStat(stat1.Defend + stat2.Defend, stat1.RangeDefend + stat2.RangeDefend, stat1.MagicDefend + stat2.MagicDefend);
        }

        public static DefendStat operator -(DefendStat stat1, DefendStat stat2)
        {
            return new DefendStat(stat1.Defend - stat2.Defend, stat1.RangeDefend - stat2.RangeDefend, stat1.MagicDefend - stat2.MagicDefend);
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"Defend : {Defend}, ").Append($"Range Defend : {RangeDefend}, ").Append($"Magic Defend : {MagicDefend}");
            return sb.ToString();
        }
    }

    #region Armor Class
    /// <summary>
    /// Base Armor Class
    /// </summary>
    abstract class Armor : IPurchasable, ISellable, IWearable, IPickable
    {
        // Field
        private string name;
        private Rarity rarity;
        private DefendStat defendStat;
        private bool isEquipped;
        private int price;

        // Property
        [JsonInclude] public string Name { get { return name; } protected set { name = value; } }
        [JsonInclude] public DefendStat DefendStat { get { return defendStat; } protected set { defendStat = value; } }
        [JsonInclude] public Rarity Rarity { get { return rarity; } protected set { rarity = value; } }
        [JsonInclude] public ArmorPosition ArmorPosition { get; protected set; }
        [JsonInclude] public ItemCategory Category { get; protected set; } = ItemCategory.Armor;
        [JsonInclude] public int Price { get { return price; } protected set { price = value; } }
        public bool IsEquipped { get { return isEquipped; } set { isEquipped = value; } }

        // Constructor
        public Armor() { Name = "Unknown"; DefendStat = new(1, 1, 1); Price = 0; Rarity = Rarity.Common; }
        public Armor(string name = "Unknown", DefendStat? defendStat = null, int price = 0, Rarity rarity = Rarity.Common)
        {
            Name = name;
            Price = price;
            Rarity = rarity;
            if (defendStat != null)
            {
                if ((int)rarity > (int)Rarity.Common)
                {
                    DefendStat newStat = new(
                        defendStat.Defend + defendStat.Defend * ((int)rarity - (int)Rarity.Common) * 0.3f,
                        defendStat.RangeDefend + defendStat.RangeDefend * ((int)rarity - (int)Rarity.Common) * 0.3f,
                        defendStat.MagicDefend + defendStat.MagicDefend * ((int)rarity - (int)Rarity.Common) * 0.3f);
                    DefendStat = newStat;
                }
                else DefendStat = defendStat;
            }
            else DefendStat = new(1, 1, 1);
        }

        // Methods
        public void OnEquipped(Character character)
        {
            if (IsEquipped) { Console.WriteLine($"| {Name} is already equipped! |"); return; }
            if (character.EquippedArmor[(int)(ArmorPosition)] != null) { character.EquippedArmor[(int)(ArmorPosition)]?.OnUnequipped(character); }
            character.EquippedArmor[(int)(ArmorPosition)] = this;
            IsEquipped = true;
            character.DefendStat += DefendStat;
            Console.WriteLine($"| {name} equipped! |");
        }

        public void OnUnequipped(Character character)
        {
            if (!IsEquipped) { Console.WriteLine($"| {Name} is not equipped! |"); return; }
            character.EquippedArmor[(int)ArmorPosition] = null;
            IsEquipped = false;
            character.DefendStat -= DefendStat;
            Console.WriteLine($"| {name} unequipped! |");
        }
        
        public virtual void OnPurchased(Character character) 
        {
            if (character.Currency < Price) { Console.WriteLine("| Not enough Money! |"); return; }
            character.Currency -= Price;
            Console.WriteLine($"| {name} is purchased! |"); 
        }
        
        public void OnSold(Character character)
        {
            if(IsEquipped) { Console.WriteLine($"| Not possible to sell!, {Name} is equipped! |"); return; }
            character.Currency += Price;
            character.Armors.Remove(this);
            Console.WriteLine($"| {Name} is sold! |");
        }
        
        public virtual void OnPicked(Character character)
        {
            Console.WriteLine($"| Picked {name}! |");
        }

        public void OnDropped(Character character)
        {
            if (IsEquipped) { Console.WriteLine($"| Not possible to drop!, {Name} is equipped! |"); return; }
            character.Armors.Remove(this);
            Console.WriteLine($"| Dropped {name}! |");
        }
        
        public override string ToString()
        {
            StringBuilder sb = new();
            _ = IsEquipped == true ? sb.Append("[E]") : sb.Append("[ ]");
            sb.Append($"{Name} | Defense : {DefendStat.Defend}, ")
              .Append($"RangeDefense : {DefendStat.RangeDefend}, ")
              .Append($"MagicDefense : {DefendStat.MagicDefend} | ")
              .Append($"Price : {Price} | Rarity : {Rarity}");
            return sb.ToString();
        }
    }
    /// <summary>
    /// Helmet
    /// </summary>
    class Helmet : Armor
    {
        // Constructor
        public Helmet() : base() { ArmorPosition = ArmorPosition.Head; }
        public Helmet(string name, DefendStat defendStat, int price, Rarity rarity) : base(name, defendStat, price, rarity) {
            ArmorPosition = ArmorPosition.Head;
        }
        public Helmet(Helmet helmet) : base(helmet.Name, helmet.DefendStat, helmet.Price, helmet.Rarity) { 
            ArmorPosition = helmet.ArmorPosition; 
        }
        

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Armors.Add(new Helmet(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Armors.Add(new Helmet(this));
        }
    }
    /// <summary>
    /// Chest
    /// </summary>
    class ChestArmor : Armor
    {
        // Constructor
        public ChestArmor() : base() { ArmorPosition = ArmorPosition.Torso; }
        public ChestArmor(string name, DefendStat defendStat, int price, Rarity rarity) : base(name, defendStat, price, rarity) {
            ArmorPosition = ArmorPosition.Torso;
        }
        public ChestArmor(ChestArmor chestArmor) : base(chestArmor.Name, chestArmor.DefendStat, chestArmor.Price, chestArmor.Rarity) {
            ArmorPosition = ArmorPosition.Torso;
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Armors.Add(new ChestArmor(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Armors.Add(new ChestArmor(this));
        }
    }
    /// <summary>
    /// Leg
    /// </summary>
    class LegArmor : Armor
    {
        // Constructor
        public LegArmor() : base() { ArmorPosition = ArmorPosition.Leg; }
        public LegArmor(string name, DefendStat defendStat, int price, Rarity rarity) : base(name, defendStat, price, rarity) {
            ArmorPosition = ArmorPosition.Leg;
        }
        public LegArmor(LegArmor legArmor) : base(legArmor.Name, legArmor.DefendStat, legArmor.Price, legArmor.Rarity) {
            ArmorPosition = ArmorPosition.Leg;
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Armors.Add(new LegArmor(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Armors.Add(new LegArmor(this));
        }
    }
    /// <summary>
    /// Foot
    /// </summary>
    class FootArmor : Armor
    {
        // Constructor
        public FootArmor() : base() { ArmorPosition = ArmorPosition.Foot; }
        public FootArmor(string name, DefendStat defendStat, int price, Rarity rarity) : base(name, defendStat, price, rarity) {
            ArmorPosition = ArmorPosition.Foot;
        }
        public FootArmor(FootArmor footArmor) : base(footArmor.Name, footArmor.DefendStat, footArmor.Price, footArmor.Rarity) {
            ArmorPosition = ArmorPosition.Foot;
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Armors.Add(new FootArmor(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Armors.Add(new FootArmor(this));
        }
    }
    /// <summary>
    /// Arm
    /// </summary>
    class Guntlet : Armor
    {
        // Constructor
        public Guntlet() : base() { ArmorPosition = ArmorPosition.Arm; }
        public Guntlet(string name, DefendStat defendStat, int price, Rarity rarity) : base(name, defendStat, price, rarity) {
            ArmorPosition = ArmorPosition.Arm;
        }
        public Guntlet(Guntlet guntlet) : base(guntlet.Name, guntlet.DefendStat, guntlet.Price, guntlet.Rarity) {
            ArmorPosition = ArmorPosition.Arm;
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Armors.Add(new Guntlet(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Armors.Add(new Guntlet(this));
        }
    }
    #endregion

    #region Weapon Class
    /// <summary>
    /// Base Weapon Class
    /// </summary>
    abstract class Weapon : IPurchasable, ISellable, IWearable, IPickable
    {
        // Field
        private string name;
        private Rarity rarity;
        private AttackStat attackStat;
        private bool isEquipped;
        private int price;

        // Property
        [JsonInclude] public string Name { get { return name; } protected set { name = value; } }
        [JsonInclude] public AttackStat AttackStat { get { return attackStat; } protected set { attackStat = value; } }
        [JsonInclude] public Rarity Rarity { get { return rarity; } protected set { rarity = value; } }
        [JsonInclude] public ItemCategory Category { get; protected set; } = ItemCategory.Weapon;
        [JsonInclude] public AttackType AttackType { get; protected set; }
        [JsonInclude] public int Price { get { return price; } protected set { price = value; } }
        public bool IsEquipped { get { return isEquipped; } set { isEquipped = value; } }

        public Weapon() { Name = "Unknown"; AttackStat = new(); Price = 0; Rarity = Rarity.Common; }
        public Weapon(string name, AttackStat attackStat, int price, Rarity rarity)
        {
            Name = name;
            Price = price;
            Rarity = rarity;
            if (attackStat != null)
            {
                if ((int)rarity > (int)Rarity.Common)
                {
                    AttackStat newStat = new()
                    {
                        Attack = attackStat.Attack + attackStat.Attack * ((int)rarity - (int)Rarity.Common) * 0.3f,
                        RangeAttack = attackStat.RangeAttack + attackStat.RangeAttack * ((int)rarity - (int)Rarity.Common) * 0.3f,
                        MagicAttack = attackStat.MagicAttack + attackStat.MagicAttack * ((int)rarity - (int)Rarity.Common) * 0.3f
                    };
                    AttackStat = newStat;
                }
                else AttackStat = attackStat;
            }
            else AttackStat = new(1, 1, 1);
        }

        public void OnEquipped(Character character)
        {
            if (IsEquipped) { Console.WriteLine($"| {Name} is already equipped! |"); return; }
            if (character.EquippedWeapon != null) { character.EquippedWeapon?.OnUnequipped(character); }
            character.EquippedWeapon = this;
            IsEquipped = true;
            character.AttackStat += AttackStat;
            Console.WriteLine($"| {name} equipped! |");
        }

        public void OnUnequipped(Character character)
        {
            if (!IsEquipped) { Console.WriteLine($"| {Name} is not equipped! |"); return; }
            character.EquippedWeapon = null;
            IsEquipped = false;
            character.AttackStat -= AttackStat;
            Console.WriteLine($"| {name} unequipped! |");
        }

        public virtual void OnPurchased(Character character) 
        { 
            if(character.Currency < Price) { Console.WriteLine("| Not enough Money! |"); return; }
            character.Currency -= Price; 
            Console.WriteLine($"| {name} is purchased |"); 
        }
        
        public void OnSold(Character character)
        {
            character.Currency += Price;
            character.Weapons.Remove(this);
            Console.WriteLine($"| {Name} is sold! |");
        }
        
        public virtual void OnPicked(Character character)
        {
            Console.WriteLine($"| Picked {name}! |");
        }

        public void OnDropped(Character character)
        {
            if (IsEquipped) { Console.WriteLine($"| {Name} is equipped! |"); return; }
            character.Weapons.Remove(this);
            Console.WriteLine($"| Dropped {name}! |");
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            _ = IsEquipped == true ? sb.Append("[E]") : sb.Append("[ ]");
            sb.Append($"{Name} | Attack : {AttackStat.Attack}, ")
              .Append($"RangeAttack : {AttackStat.RangeAttack}, ")
              .Append($"MagicAttack : {AttackStat.MagicAttack} | ")
              .Append($"Price : {Price} | Rarity : {Rarity}");
            return sb.ToString();
        }
    }
    /// <summary>
    /// Sword -> Close Range Attack Stat
    /// </summary>
    class Sword : Weapon
    {
        public Sword() : base() { AttackType = AttackType.Close; }
        public Sword(string name, AttackStat attackStat, int price, Rarity rarity) : base(name, attackStat, price, rarity) {
            AttackType = AttackType.Close;
        }
        public Sword(Sword sword) : base(sword.Name, sword.AttackStat, sword.Price, sword.Rarity) { 
            AttackType = sword.AttackType; 
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Weapons.Add(new Sword(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Weapons.Add(new Sword(this));
        }
    }
    /// <summary>
    /// Bow -> Range Attack Stat
    /// </summary>
    class Bow : Weapon
    {
        public Bow() : base() { AttackType = AttackType.Long; }
        public Bow(string name, AttackStat attackStat, int price, Rarity rarity) : base(name, attackStat, price, rarity ) {
            AttackType = AttackType.Long;
        }
        public Bow(Bow bow) : base(bow.Name, bow.AttackStat, bow.Price, bow.Rarity) {
            AttackType = bow.AttackType;
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Weapons.Add(new Bow(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Weapons.Add(new Bow(this));
        }
    }
    /// <summary>
    /// Staff -> Magic Attack Stat
    /// </summary>
    class Staff : Weapon
    {
        public Staff() : base() { AttackType= AttackType.Magic; }
        public Staff(string name, AttackStat attackStat, int price, Rarity rarity) : base(name, attackStat, price, rarity) {
            AttackType = AttackType.Magic;
        }
        public Staff(Staff staff) : base(staff.Name, staff.AttackStat, staff.Price, staff.Rarity) {
            AttackType = staff.AttackType;
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Weapons.Add(new Staff(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Weapons.Add(new Staff(this));
        }
    }
    #endregion

    #region Consumable Class
    /// <summary>
    /// Base Consumable Items Class
    /// </summary>
    abstract class Consumables : IUseable, IPurchasable, ISellable, IPickable
    {
        // Field
        private string _name;
        private float _coefficient;
        private int _price;
        private Rarity _rarity;
        private ConsumableCategory _consumableCategory;

        // Property
        [JsonInclude] public string Name { get { return _name; } protected set { _name = value; } }
        [JsonInclude] public float Coefficient { get { return _coefficient; } protected set { _coefficient = value; } }
        [JsonInclude] public int Price { get { return _price; } protected set { _price = value; } }
        [JsonInclude] public ItemCategory Category { get; protected set; } = ItemCategory.Consumable;
        [JsonInclude] public Rarity Rarity { get { return _rarity; } protected set { _rarity = value; } }
        public ConsumableCategory ConsumableCategory { get { return _consumableCategory; } protected set { _consumableCategory = value; } }
        
        // Constructor
        public Consumables() { Name = "Unknown"; Coefficient = 0; Price = 0; ConsumableCategory = ConsumableCategory.IncreaseHealth; Rarity = Rarity.Common; }
        public Consumables(string name, float coefficient, int price, ConsumableCategory consumableCategory, Rarity rarity)
        {
            Name = name; Coefficient = coefficient; Price = price; ConsumableCategory = consumableCategory; Rarity = rarity;
        }

        // Methods
        public virtual void OnUsed(Character character)
        {
            character.Consumables.Remove(this);
        }

        public virtual void OnDeBuffed(Character character)
        {
            Console.WriteLine("| All Buffs Removed! |");
        }
        
        public virtual void OnPurchased(Character character)
        {
            if (character.Currency < Price) { Console.WriteLine("| Not enough Money! |"); return; }
            character.Currency -= Price;
            Console.WriteLine($"| {_name} is purchased |");
        }
        
        public void OnSold(Character character)
        {
            character.Currency += Price;
            character.Consumables.Remove(this);
            Console.WriteLine($"| {Name} is sold! |");
        }

        public virtual void OnPicked(Character character)
        {
            Console.WriteLine($"| Picked {_name}! |");
        }

        public void OnDropped(Character character)
        {
            character.Consumables.Remove(this);
            Console.WriteLine($"| Dropped {_name}! |");
        }
    }

    /// <summary>
    /// Health Potion -> Restores health partially
    /// </summary>
    class HealthPotion : Consumables
    {
        // Constructor
        public HealthPotion() : base() { }
        public HealthPotion(string name, float coefficient, int price, Rarity rarity = Rarity.Common) : base(name, coefficient, price, ConsumableCategory.IncreaseHealth, rarity) { }
        public HealthPotion(HealthPotion potion) : base(potion.Name, potion.Coefficient, potion.Price, potion.ConsumableCategory, potion.Rarity) { }

        // Methods
        public override void OnUsed(Character character)
        {
            if(character.Health >= character.MaxHealth) { Console.WriteLine("| Health is already full! |"); return; }
            
            base.OnUsed(character);
            character.OnHeal(Coefficient + (Coefficient * (int)Rarity * 0.5f));
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Consumables.Add(new HealthPotion(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Consumables.Add(new HealthPotion(this));
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"{Name} | Category : {ConsumableCategory} | Restore Health : {Coefficient} | Price : {Price} | Rarity : {Rarity}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Attack Buff Potion -> Buffs Attack Parameters until the day passes.
    /// </summary>
    class AttackBuffPotion : Consumables
    {
        private AttackStat attackStat;

        public AttackStat AttackStat { get { return attackStat; } protected set { attackStat = value; } }

        // Constructor
        public AttackBuffPotion() : base() { }
        public AttackBuffPotion(string name, float coefficient, int price, Rarity rarity = Rarity.Common) 
            : base(name, coefficient, price, ConsumableCategory.IncreaseAttack, rarity)
        {
            AttackStat = new AttackStat(2 + (int)Rarity * 4, 2 + (int)Rarity * 4, 2 + (int)Rarity * 4);
        }
        public AttackBuffPotion(AttackBuffPotion potion) : base(potion.Name, potion.Coefficient, potion.Price, potion.ConsumableCategory, potion.Rarity)
        {
            AttackStat = potion.AttackStat;
        }

        // Methods
        public override void OnUsed(Character character)
        {
            GameManager.Exposables.Enqueue(this);

            base.OnUsed(character);
            character.AttackStat += AttackStat;
        }

        public override void OnDeBuffed(Character character)
        {
            base.OnDeBuffed(character);
            character.AttackStat -= AttackStat;
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Consumables.Add(new AttackBuffPotion(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Consumables.Add(new AttackBuffPotion(this));
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"{Name} | Category : {ConsumableCategory} | ")
              .Append($"Attack Buff : {AttackStat.Attack} | ")
              .Append($"Range Attack Buff : {AttackStat.RangeAttack} | ")
              .Append($"Magic Attack Buff : {AttackStat.MagicAttack} | ")
              .Append($"Price : {Price} | Rarity : {Rarity}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Defend Buff Potion -> Buffs Defend Parameters until the day passes.
    /// </summary>
    class DefendBuffPotion : Consumables
    {
        // Field
        private DefendStat defendStat;

        // Property
        public DefendStat DefendStat { get { return defendStat; } protected set { defendStat = value; } }

        // Constructor
        public DefendBuffPotion() : base() { }
        public DefendBuffPotion(string name, float coefficient, int price, Rarity rarity = Rarity.Common)
            : base(name, coefficient, price, ConsumableCategory.IncreaseDefence, rarity)
        {
            DefendStat = new DefendStat(1.2f + (int)Rarity * 3, 1.2f + (int)Rarity * 3, 1.2f + (int)Rarity * 3);
        }
        public DefendBuffPotion(DefendBuffPotion potion) : base(potion.Name, potion.Coefficient, potion.Price, potion.ConsumableCategory, potion.Rarity)
        {
            DefendStat = potion.DefendStat;
        }

        // Methods
        public override void OnUsed(Character character)
        {
            GameManager.Exposables.Enqueue(this);

            base.OnUsed(character);
            character.DefendStat += DefendStat;
        }

        public override void OnDeBuffed(Character character)
        {
            base.OnDeBuffed(character);
            character.DefendStat -= DefendStat;
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Consumables.Add(new DefendBuffPotion(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Consumables.Add(new DefendBuffPotion(this));
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"{Name} | Category : {ConsumableCategory} | ")
              .Append($"Defence Buff : {DefendStat.Defend} | ")
              .Append($"Range Defence Buff : {DefendStat.RangeDefend} | ")
              .Append($"Magic Defence Buff : {DefendStat.MagicDefend} | ")
              .Append($"Price : {Price} | Rarity : {Rarity}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// All Parameter Buff Potion -> Buffs All Parameters until the day passes.
    /// </summary>
    class AllBuffPotion : Consumables
    {
        // Field
        private AttackStat attackStat;
        private DefendStat defendStat;

        public AttackStat AttackStat { get { return attackStat; } protected set { attackStat = value; } }
        public DefendStat DefendStat { get { return defendStat; } protected set { defendStat = value; } }

        // Constructor
        public AllBuffPotion() : base() { }
        public AllBuffPotion(string name, float coefficient, int price, Rarity rarity = Rarity.Common) 
            : base(name, coefficient, price, ConsumableCategory.IncreaseAllStat, rarity)
        {
            AttackStat = new AttackStat(2 + (int)Rarity * 4, 2 + (int)Rarity * 4, 2 + (int)Rarity * 4);
            DefendStat = new DefendStat(1.2f + (int)Rarity * 3, 1.2f + (int)Rarity * 3, 1.2f + (int)Rarity * 3);
        }
        public AllBuffPotion(AllBuffPotion potion) : base(potion.Name, potion.Coefficient, potion.Price, potion.ConsumableCategory, potion.Rarity)
        {
            AttackStat = potion.AttackStat;
            DefendStat = potion.DefendStat;
        }

        // Methods
        public override void OnUsed(Character character)
        {
            GameManager.Exposables.Enqueue(this);

            base.OnUsed(character);
            character.AttackStat += AttackStat;
            character.DefendStat += DefendStat;
        }

        public override void OnDeBuffed(Character character)
        {
            base.OnDeBuffed(character);
            character.AttackStat -= AttackStat;
            character.DefendStat -= DefendStat;
        }

        public override void OnPurchased(Character character)
        {
            base.OnPurchased(character);
            character.Consumables.Add(new AllBuffPotion(this));
        }

        public override void OnPicked(Character character)
        {
            base.OnPicked(character);
            character.Consumables.Add(new AllBuffPotion(this));
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"{Name} | Category : {ConsumableCategory} | ")
              .Append($"All Attack Stat. Buff : {AttackStat.Attack} | ")
              .Append($"All Defence Stat. Buff : {DefendStat.Defend} | ")
              .Append($"Price : {Price} | Rarity : {Rarity}");
            return sb.ToString();
        }
    }
    #endregion

    /// <summary>
    /// Lists of items
    /// </summary>
    static class ItemLists
    {
        public static readonly Armor[] Armors = {
            new Helmet("Steel Helmet", new DefendStat(3.1f, 2.4f, 1.2f), 15, Rarity.Common),
            new ChestArmor("Steel ChestArmor", new DefendStat(5.1f, 4.2f, 3.3f), 30, Rarity.Common),
            new LegArmor("Steel LegArmor", new DefendStat(1.5f, 1.3f, 0f), 8, Rarity.Common),
            new FootArmor("Steel FootArmor", new DefendStat(0.5f, 0.3f, 0.1f), 4, Rarity.Common),
            new Guntlet("Steel Guntlet", new DefendStat(1.5f, 1.3f, 0f), 8, Rarity.Common),
        };

        public static readonly Weapon[] Weapons = {
            new Sword("Steel Sword", new AttackStat(10f, 0f, 0f), 20, Rarity.Common),
            new Bow("Wooden Bow", new AttackStat(0f, 10f, 0f), 20, Rarity.Common),
            new Staff("Steel Staff", new AttackStat(0f,0f,10f), 20, Rarity.Common),
        };

        public static readonly Consumables[] Consumables =
        {
            new HealthPotion("Small Health Potion", 20, 5, Rarity.Common),
            new HealthPotion("Medium Health Potion", 40, 10, Rarity.Exclusive),
            new HealthPotion("Large Health Potion", 60, 20, Rarity.Rare),
            new AttackBuffPotion("Common Attack Potion", 5, 10, Rarity.Common),
            new AttackBuffPotion("Exclusive Attack Potion", 10, 30, Rarity.Exclusive),
            new AttackBuffPotion("Rare Attack Potion", 15, 50, Rarity.Rare),
            new DefendBuffPotion("Common Defend Potion", 5, 10, Rarity.Common),
            new DefendBuffPotion("Exclusive Defend Potion", 10, 30, Rarity.Exclusive),
            new DefendBuffPotion("Rare Defend Potion", 15, 50, Rarity.Rare),
            new AllBuffPotion("Rare Universal Potion", 10, 150, Rarity.Rare),
            new AllBuffPotion("Hero Universal Potion", 30, 350, Rarity.Hero),
            new AllBuffPotion("Legendary Universal Potion", 100, 1000, Rarity.Legend),
        };
    }
}
