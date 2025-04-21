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
        private float maxMagicPoint;
        private float magicPoint;
        private int level;
        private AttackStat attackStat;
        private DefendStat defendStat;
        
        // Property
        public string Name { get { return name; } set { name = value; } }
        public int Level { get { return level; } set { level = Math.Min(100, value); } }
        public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public float Health { get { return health; } set { health = value; } }
        public float MaxMagicPoint { get { return maxMagicPoint; } set { maxMagicPoint = value; } }
        public float MagicPoint { get { return magicPoint; } set { magicPoint = value; } }
        public AttackStat AttackStat { get { return attackStat; } set { attackStat = value; } }
        public DefendStat DefendStat { get { return defendStat; } set { defendStat = value; } }

        // Constructor
        public CharacterStat(string name, float maxHealth, float maxMagicPoint, int level, AttackStat attackStat, DefendStat defendStat)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
            MaxMagicPoint = maxMagicPoint;
            MagicPoint = maxMagicPoint;
            Level = level;
            AttackStat = attackStat;
            DefendStat = defendStat;
        }

        public CharacterStat(CharacterStat characterStat)
        {
            Name = characterStat.Name ?? "Unknown";
            MaxHealth = characterStat.MaxHealth;
            Health = characterStat.Health;
            MaxMagicPoint = characterStat.MaxMagicPoint;
            MagicPoint = characterStat.MagicPoint;
            Level = characterStat.Level;
            AttackStat = new(characterStat.AttackStat);
            DefendStat = new(characterStat.DefendStat);
        }

        [JsonConstructor]
        public CharacterStat(string name, float maxHealth, float health, float maxMagicPoint, float magicPoint, int level, AttackStat attackStat, DefendStat defendStat)
        {
            Name = name ?? "Unknown";
            MaxHealth = maxHealth;
            Health = health;
            MaxMagicPoint = maxMagicPoint;
            MagicPoint = magicPoint;
            Level = level;
            AttackStat = attackStat;
            DefendStat = defendStat;
        }
    }

    /// <summary>
    /// Base Character Class
    /// </summary>
    abstract class Character : IDamagable
    {
        // Field
        private CharacterStat characterStat;
        private int currency;
        private int exp;

        // Property
        [JsonIgnore] public CharacterStat CharacterStat { get { return characterStat; } protected set { characterStat = value; } }

        [JsonInclude] public string Name { get { return characterStat.Name; } protected set { characterStat.Name = value; } }
        [JsonInclude] public float MaxHealth { get { return characterStat.MaxHealth; } protected set { characterStat.MaxHealth = value; } }
        [JsonInclude] public float Health { get { return characterStat.Health; } protected set { characterStat.Health = Math.Clamp(value, 0, MaxHealth); } }
        [JsonInclude] public float MaxMagicPoint { get { return characterStat.MaxMagicPoint; } protected set { characterStat.MaxMagicPoint = value; } }
        [JsonInclude] public float MagicPoint { get { return characterStat.MagicPoint; } protected set { characterStat.MagicPoint = Math.Clamp(value,0, MaxMagicPoint); } }
        [JsonInclude] public int Level { get { return characterStat.Level; } protected set { characterStat.Level = value; } }
        [JsonInclude] public AttackStat AttackStat { get { return characterStat.AttackStat; } set { characterStat.AttackStat = value; } }
        [JsonInclude] public DefendStat DefendStat { get { return characterStat.DefendStat; } set { characterStat.DefendStat = value; } }

        public int Currency { get { return currency; } set { currency = Math.Max(0, value); } }
        [JsonInclude] public int Exp { get { return exp; } protected set { exp = Math.Clamp(value, 0, 9999); } }
        [JsonInclude] public bool IsAlive { get; protected set; } = true;

        public List<Armor> Armors { get; set; } = new List<Armor>();
        public List<Weapon> Weapons { get; set; } = new List<Weapon>();
        public List<Consumables> Consumables { get; set; } = new List<Consumables>();

        public Armor?[] EquippedArmor { get; set; } = new Armor[Enum.GetValues((typeof(ArmorPosition))).Length];
        public Weapon? EquippedWeapon { get; set; } = null;

        public event Action? OnDeath;

        // Constructor
        public Character(CharacterStat characterStat, int currency, int exp)
        {
            CharacterStat = new(characterStat);
            Currency = currency;
            Exp = exp;
        }

        [JsonConstructor]
        public Character(string name, float maxHealth, float health, float maxMagicPoint, float magicPoint, int level, AttackStat attackStat, DefendStat defendStat, int currency, int exp, bool isAlive)
        {
            CharacterStat = new(name, maxHealth, health, maxMagicPoint, magicPoint, level, attackStat, defendStat);
            Currency = currency;
            Exp = exp;
            IsAlive = isAlive;
        }

        // Methods
        /// <summary>
        /// Character levels up when exp is over 100.
        /// </summary>
        /// <param name="exp"></param>
        public void OnEarnExp(int exp)
        {
            int curLvl = Level;

            Exp += exp;
            Level = Exp / 100 + 1;
            if(curLvl != Level) OnLevelUp();
        }

        /// <summary>
        /// Heal the character.
        /// </summary>
        /// <param name="coef"></param>
        public void OnHeal(float coef)
        {
            if (!IsAlive) { return; }

            Health += coef;
        }

        /// <summary>
        /// Heal the character's magic point.
        /// </summary>
        /// <param name="coef"></param>
        public void OnMagicPointHeal(float coef)
        {
            if (!IsAlive) { return; }

            MagicPoint += coef;
        }

        /// <summary>
        /// Level up the character.
        /// </summary>
        private void OnLevelUp()
        {
            MaxHealth += 25f;
            MaxMagicPoint += 15f;

            AttackStat += new AttackStat(AttackStat.Attack * 0.15f, 
                                         AttackStat.RangeAttack * 0.15f, 
                                         AttackStat.MagicAttack * 0.15f);
            DefendStat += new DefendStat(DefendStat.Defend * 0.15f,
                                         DefendStat.RangeDefend * 0.15f,
                                         DefendStat.MagicDefend * 0.15f);
        }

        // TODO: Add Avoid Mechanism
        /// <summary>
        /// Calculate the damage taken by the monster.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        public void OnDamage(AttackType type, float damage)
        {
            float calculatedDamage = 
                type == AttackType.Close ? Math.Min(1f, damage * (1f - DefendStat.Defend / 100f)) : 
                (type == AttackType.Long ? Math.Min(1f, damage * (1f - DefendStat.RangeDefend / 100f)) :
                Math.Min(1f, damage * (1f - DefendStat.MagicDefend / 100f)));

            Console.WriteLine($"| {Name} got {calculatedDamage:F2} damage! |");
            Health -= calculatedDamage;

            if (Health <= 0.9f && IsAlive) Die();
        }

        /// <summary>
        /// Revive the character.
        /// </summary>
        /// <returns></returns>
        public bool OnRevive()
        {
            if (IsAlive) { return false; }

            IsAlive = true;
            if (Currency < 100) return false; 
            Currency -= 100;
            Health = MaxHealth;
            return true;
        }

        /// <summary>
        /// Die the character.
        /// </summary>
        private void Die()
        {
            IsAlive = false;
            OnDeath?.Invoke();
        }

        /// <summary>
        /// Return the character's information.
        /// </summary>
        /// <returns></returns>
        public virtual new string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Name: {Name}").AppendLine($"MaxHealth : {MaxHealth}")
              .AppendLine($"Health : {Health}").AppendLine($"MagicPoint : {MagicPoint}")
              .AppendLine($"Level : {Level}")
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
    }

    /// <summary>
    /// Warrior -> Close Range Attack
    /// </summary>
    class Warrior : Character
    {
        // Field
        private Job job;

        // Property
        [JsonIgnore] public Job Job { get { return job; } protected set { job = value; } }

        // Constructor
        public Warrior(CharacterStat characterStat, int currency, int exp) : base(characterStat, currency, exp) { job = Job.Warrior; }
        public Warrior(Warrior warrior) : base(warrior.CharacterStat, warrior.Currency, warrior.Exp) { job = Job.Warrior; }

        [JsonConstructor]
        public Warrior(string name, float maxHealth, float health, float maxMagicPoint, float magicPoint, int level, AttackStat attackStat, DefendStat defendStat, int currency, int exp, bool isAlive, List<Armor> armors, List<Weapon> weapons, List<Consumables> consumables, Armor?[] equippedArmor, Weapon? equippedWeapon)
            : base(name, maxHealth, health, maxMagicPoint, magicPoint, level, attackStat, defendStat, currency, exp, isAlive)
        {
            Job = Job.Warrior;
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
        public Wizard(CharacterStat characterStat, int currency, int exp) : base(characterStat, currency, exp) { job = Job.Wizard; }
        public Wizard(Wizard wizard) : base(wizard.CharacterStat, wizard.Currency, wizard.Exp) { job = Job.Wizard; }

        [JsonConstructor]
        public Wizard(string name, float maxHealth, float health, float maxMagicPoint, float magicPoint, int level, AttackStat attackStat, DefendStat defendStat, int currency, int exp, bool isAlive, List<Armor> armors, List<Weapon> weapons, List<Consumables> consumables, Armor?[] equippedArmor, Weapon? equippedWeapon)
            : base(name, maxHealth, health, maxMagicPoint, magicPoint, level, attackStat, defendStat, currency, exp, isAlive)
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
        public Archer(CharacterStat characterStat, int currency, int exp) : base(characterStat, currency, exp) { job = Job.Archer; }
        public Archer(Archer archer) : base(archer.CharacterStat, archer.Currency, archer.Exp) { job = Job.Archer; }

        [JsonConstructor]
        public Archer(string name, float maxHealth, float health, float maxMagicPoint, float magicPoint, int level, AttackStat attackStat, DefendStat defendStat, int currency, int exp, bool isAlive, List<Armor> armors, List<Weapon> weapons, List<Consumables> consumables, Armor?[] equippedArmor, Weapon? equippedWeapon)
            : base(name, maxHealth, health, maxMagicPoint, magicPoint, level, attackStat, defendStat, currency, exp, isAlive)
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
