using System.Text;
using System.Text.Json.Serialization;

namespace TextRPG
{
    /// <summary>
    /// Base Character Stat.
    /// </summary>
    public class CharacterStat
    {
        // Field
        private string name;
        private float maxHealth;
        private float health;
        private int level;
        private AttackStat attackStat;
        private DefendStat defendStat;
        
        // Property
        public string Name { get { return name; } set { name = value; } }
        public int Level { get { return level; } set { level = Math.Min(100, value); } }
        public AttackStat AttackStat { get { return attackStat; } set { attackStat = value; } }
        public DefendStat DefendStat { get { return defendStat; } set { defendStat = value; } }
        public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public float Health { get { return health; } set { health = value; } }
        
        public CharacterStat()
        {
            Name = "Anon";
            MaxHealth = 0;
            Health = 0;
            Level = 1;
            AttackStat = new(1, 1, 1);
            DefendStat = new(1, 1, 1);
        }
        public CharacterStat(string name, float maxHealth, int level, AttackStat attackStat, DefendStat defendStat)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
            Level = level;
            AttackStat = attackStat;
            DefendStat = defendStat;
        }
        public CharacterStat(CharacterStat characterStat)
        {
            name = characterStat.name;
            maxHealth = characterStat.maxHealth;
            health = characterStat.health; 
            level = characterStat.level;
            attackStat = new(characterStat.attackStat);
            defendStat = new(characterStat.defendStat);
        }
    }

    /// <summary>
    /// Base Character Class
    /// </summary>
    abstract class Character : IDamagable
    {
        // Field
        private CharacterStat characterStat;
        private List<Armor> armors = new List<Armor>();
        private List<Weapon> weapons = new List<Weapon>();
        private List<Consumables> consumables = new List<Consumables>();
        private Armor?[] equippedArmor = new Armor[Enum.GetValues((typeof(ArmorPosition))).Length];
        private Weapon? equippedWeapon = null;
        private int currency;
        private int exp;

        // Property
        [JsonInclude] public CharacterStat CharacterStat { get { return characterStat; } protected set { characterStat = value; } }
        
        [JsonIgnore] public float MaxHealth { get { return characterStat.MaxHealth; } protected set { characterStat.MaxHealth = value; } }
        [JsonIgnore] public float Health { get { return characterStat.Health; } protected set { characterStat.Health = Math.Clamp(value, 0, MaxHealth); } }
        [JsonIgnore] public string Name { get { return characterStat.Name; } protected set { characterStat.Name = value; } }
        [JsonIgnore] public int Level { get { return characterStat.Level; } protected set { characterStat.Level = value; } }
        [JsonIgnore] public AttackStat AttackStat { get { return characterStat.AttackStat; } set { characterStat.AttackStat = value; } }
        [JsonIgnore] public DefendStat DefendStat { get { return characterStat.DefendStat; } set { characterStat.DefendStat = value; } }

        [JsonInclude] public int Currency { get { return currency; } set { currency = Math.Max(0, value); } }
        [JsonInclude] public int Exp { get { return exp; } protected set { exp = Math.Clamp(value, 0, 9999); } }
        [JsonInclude] public bool IsAlive { get; protected set; } = true;

        [JsonInclude] public List<Armor> Armors { get { return armors; } set { armors = value; } }
        [JsonInclude] public List<Weapon> Weapons { get { return weapons; } set { weapons = value; } }
        [JsonInclude] public List<Consumables> Consumables { get { return consumables; } set { consumables = value; } }

        [JsonInclude] public Armor?[] EquippedArmor { get { return equippedArmor; } set { equippedArmor = value; } }
        [JsonInclude] public Weapon? EquippedWeapon { get { return equippedWeapon; } set { equippedWeapon = value; } }

        public event Action? OnDeath;

        // Constructor
        public Character() { CharacterStat = new(); Currency = 0; Exp = 0; }
        public Character(CharacterStat characterStat, int currency, int exp)
        {
            this.characterStat = new(characterStat);
            Currency = currency;
            Exp = exp;
        }

        public void OnEarnExp(int exp)
        {
            int curLvl = Level;

            Exp += exp;
            Level = Exp / 100 + 1;
            if(curLvl != Level) OnLevelUp();
        }

        // Methods
        public void OnHeal(float coef)
        {
            if (!IsAlive) { return; }

            Health += coef;
        }
        
        public void OnDamage(AttackType type, float damage)
        {
            float calculatedDamage = 
                type == AttackType.Close ? (damage * (1f - DefendStat.Defend / 100f)) : 
                (type == AttackType.Long ? damage * (1f - DefendStat.RangeDefend / 100f) : 
                (damage * (1f - DefendStat.MagicDefend / 100f)));

            Console.WriteLine($"| {Name} got {calculatedDamage:F2} damage! |");
            Health -= calculatedDamage;

            if (Health <= 0 && IsAlive) Die();
        }

        public bool OnRevive()
        {
            if (IsAlive) { return false; }

            IsAlive = true;
            if (Currency < 100) return false; 
            Currency -= 100;
            Health = MaxHealth;
            return true;
        }

        public virtual new string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Name: {Name}").AppendLine($"MaxHealth : {MaxHealth}")
              .AppendLine($"Health : {Health}").AppendLine($"Level : {Level}")
              .AppendLine($"AttackStat : {AttackStat}").AppendLine($"DefendStat : {DefendStat}")
              .AppendLine($"Currency : {Currency}").AppendLine($"Exp : {Exp}");
            foreach(Armor armor in Armors) { sb.Append(armor.ToString() + ", "); }
            sb.AppendLine();
            foreach(Weapon weapon in Weapons) { sb.Append(weapon.ToString() + ", "); }
            sb.AppendLine();
            foreach(Consumables consumable in Consumables) { sb.Append(consumable.ToString() + ", "); }
            sb.AppendLine();
            foreach(Armor? equipped in EquippedArmor) { sb.Append(equipped?.ToString() + ", "); }
            sb.AppendLine();
            sb.Append(EquippedWeapon?.ToString());
            return sb.ToString();
        }

        private void OnLevelUp()
        {
            MaxHealth += 25f;

            AttackStat += new AttackStat(AttackStat.Attack * 0.15f, 
                                         AttackStat.RangeAttack * 0.15f, 
                                         AttackStat.MagicAttack * 0.15f);
            DefendStat += new DefendStat(DefendStat.Defend * 0.15f,
                                         DefendStat.RangeDefend * 0.15f,
                                         DefendStat.MagicDefend * 0.15f);
        }

        private void Die()
        {
            IsAlive = false;
            OnDeath?.Invoke();
        }
    }

    /// <summary>
    /// Warrior -> Close Range Attack
    /// </summary>
    class Warrior : Character
    {
        // Field
        private Job job;

        // Property
        public Job Job { get { return job; } protected set { job = value; } }

        // Constructor
        public Warrior() : base() { job = Job.Warrior; }
        public Warrior(CharacterStat characterStat, int currency, int exp) : base(characterStat, currency, exp) { job = Job.Warrior; }
        public Warrior(Warrior warrior) : base(warrior.CharacterStat, warrior.Currency, warrior.Exp) { job = Job.Warrior; }
        public Warrior(CharacterStat characterStat, int currency, int exp, bool isAlive, List<Armor> armors, List<Weapon> weapons, List<Consumables> consumables, Armor?[] equippedArmor, Weapon? equippedWeapon)
            :base(characterStat, currency, exp)
        {
            Job = Job.Warrior;
            IsAlive = isAlive;
            Armors = armors ?? new List<Armor>();
            Weapons = weapons ?? new List<Weapon>();
            Consumables = consumables ?? new List<Consumables>();
            EquippedArmor = equippedArmor ?? new Armor[Enum.GetValues(typeof(ArmorPosition)).Length];
            EquippedWeapon = equippedWeapon;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nJob : {Job}";
        }
    }

    /// <summary>
    /// Wizard -> Magic Attack
    /// </summary>
    class Wizard : Character
    {
        // Field
        private Job job;

        // Property
        public Job Job { get { return job; } protected set { job = value; } }

        // Constructor
        public Wizard() : base() { job = Job.Wizard; }
        public Wizard(CharacterStat characterStat, int currency, int exp) : base(characterStat, currency, exp) { job = Job.Wizard; }
        public Wizard(Wizard wizard) : base(wizard.CharacterStat, wizard.Currency, wizard.Exp) { job = Job.Wizard; }
        public Wizard(CharacterStat characterStat, int currency, int exp, bool isAlive, List<Armor> armors, List<Weapon> weapons, List<Consumables> consumables, Armor?[] equippedArmor, Weapon? equippedWeapon)
            : base(characterStat, currency, exp)
        {
            Job = Job.Wizard;
            IsAlive = isAlive;
            Armors = armors ?? new List<Armor>();
            Weapons = weapons ?? new List<Weapon>();
            Consumables = consumables ?? new List<Consumables>();
            EquippedArmor = equippedArmor ?? new Armor[Enum.GetValues(typeof(ArmorPosition)).Length];
            EquippedWeapon = equippedWeapon;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nJob : {Job}";
        }
    }

    /// <summary>
    /// Archer -> Long Range Attack
    /// </summary>
    class Archer : Character
    {
        // Field
        private Job job;

        // Property
        public Job Job { get { return job; } protected set { job = value; } }

        // Constructor
        public Archer() : base() { job = Job.Archer; }
        public Archer(CharacterStat characterStat, int currency, int exp) : base(characterStat, currency, exp) { job = Job.Archer; }
        public Archer(Archer archer) : base(archer.CharacterStat, archer.Currency, archer.Exp) { job = Job.Archer; }
        public Archer(CharacterStat characterStat, int currency, int exp, bool isAlive, List<Armor> armors, List<Weapon> weapons, List<Consumables> consumables, Armor?[] equippedArmor, Weapon? equippedWeapon)
            : base(characterStat, currency, exp)
        {
            Job = Job.Archer;
            IsAlive = isAlive;
            Armors = armors ?? new List<Armor>();
            Weapons = weapons ?? new List<Weapon>();
            Consumables = consumables ?? new List<Consumables>();
            EquippedArmor = equippedArmor ?? new Armor[Enum.GetValues(typeof(ArmorPosition)).Length];
            EquippedWeapon = equippedWeapon;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nJob : {Job}";
        }
    }
}
