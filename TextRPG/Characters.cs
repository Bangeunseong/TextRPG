namespace TextRPG
{
    /// <summary>
    /// Base Character Stat.
    /// </summary>
    public struct CharacterStat
    {
        // Field
        private string name;
        private float maxHealth;
        private float health;
        private int level;
        private AttackStat attackStat;
        private DefendStat defendStat;
        
        // Property
        public string Name { get { return name; } set { if (value.Length <= 0) name = "Anon"; else name = value; } }
        public int Level { get { return level; } set { level = Math.Min(100, value); } }
        public AttackStat AttackStat { get { return attackStat; } set { attackStat = value; } }
        public DefendStat DefendStat { get { return defendStat; } set { defendStat = value; } }
        public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public float Health { get { return health; } set { health = value; } }
        
        public CharacterStat(string name, float maxHealth, int level, AttackStat attackStat, DefendStat defendStat)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
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
        private CharacterStat _characterStat;
        private int _currency;
        private int _exp;
        private bool _isAlive;

        // Property
        public float MaxHealth { get { return _characterStat.MaxHealth; } private set { _characterStat.MaxHealth = value; } }
        public float Health { get { return _characterStat.Health; } set { _characterStat.Health = Math.Clamp(value, 0, MaxHealth); } }
        public string Name { get { return _characterStat.Name; } set { _characterStat.Name = value; } }
        public int Level { get { return _characterStat.Level; } set { _characterStat.Level = value; } }
        public AttackStat AttackStat { get { return _characterStat.AttackStat; } set { _characterStat.AttackStat = value; } }
        public DefendStat DefendStat { get { return _characterStat.DefendStat; } set { _characterStat.DefendStat = value; } }
        
        public int Currency { get { return _currency; } set { _currency = Math.Max(0, value); } }
        public int Exp { get { return _exp; } private set { _exp = Math.Clamp(value, 0, 9999); } }
        public bool IsAlive { get { return _isAlive; } private set { _isAlive = value; } }

        public List<Armor> Armors = new List<Armor>();
        public List<Weapon> Weapons = new List<Weapon>();
        public List<Consumables> Consumables = new List<Consumables>();

        public Armor?[] EquippedArmor = new Armor[Enum.GetValues((typeof(ArmorPosition))).Length];
        public Weapon? EquippedWeapon = null;

        public event Action? OnDeath;

        // Constructor
        public Character(CharacterStat characterStat)
        {
            _characterStat = characterStat;
            Currency = 100;
            Exp = 0;
            IsAlive = true;
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
        public Job Job { get { return job; } private set { job = value; } }

        // Constructor
        public Warrior(CharacterStat characterStat) : base(characterStat) 
        { 
            job = Job.Warrior;

            // LINQ
            var basicHelmets = from armor in ItemLists.armors
                               where armor.GetType().Equals(typeof(Helmet)) && armor.Rarity == Rarity.Common
                               select armor;
            var basicChestArmors = from armor in ItemLists.armors
                                   where armor.GetType().Equals(typeof(ChestArmor)) && armor.Rarity == Rarity.Common
                                   select armor;
            var basicSwords = from sword in ItemLists.weapons
                              where sword.GetType().Equals(typeof(Sword)) && sword.Rarity == Rarity.Common
                              select sword;
            var basicHealthPotions = from item in ItemLists.consumables
                                    where item.GetType().Equals(typeof(HealthPotion)) && item.Rarity == Rarity.Common
                                    select item;

            if(basicHelmets.Count() > 0) { Armors.Add(new Helmet((Helmet)basicHelmets.First())); }
            if(basicChestArmors.Count() > 0) { Armors.Add(new ChestArmor((ChestArmor)basicChestArmors.First())); }
            if(basicSwords.Count() > 0) { Weapons.Add(new Sword((Sword)basicSwords.First())); }
            if(basicHealthPotions.Count() > 0) { Consumables.Add(new HealthPotion((HealthPotion)basicHealthPotions.First())); }
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
        public Job Job { get { return job; } private set { job = value; } }

        // Constructor
        public Wizard(CharacterStat characterStat) : base(characterStat)
        { 
            job = Job.Wizard;
            var basicHelmets = from armor in ItemLists.armors
                               where armor.GetType().Equals(typeof(Helmet)) && armor.Rarity == Rarity.Common
                               select armor;
            var basicChestArmors = from armor in ItemLists.armors
                                   where armor.GetType().Equals(typeof(ChestArmor)) && armor.Rarity == Rarity.Common
                                   select armor;
            var basicStaffs = from staff in ItemLists.weapons
                              where staff.GetType().Equals(typeof(Staff)) && staff.Rarity == Rarity.Common
                              select staff;
            var basicHealthPotions = from item in ItemLists.consumables
                                     where item.GetType().Equals(typeof(HealthPotion)) && item.Rarity == Rarity.Common
                                     select item;

            if (basicHelmets.Count() > 0) { Armors.Add(new Helmet((Helmet)basicHelmets.First())); }
            if (basicChestArmors.Count() > 0) { Armors.Add(new ChestArmor((ChestArmor)basicChestArmors.First())); }
            if (basicStaffs.Count() > 0) { Weapons.Add(new Staff((Staff)basicStaffs.First())); }
            if (basicHealthPotions.Count() > 0) { Consumables.Add(new HealthPotion((HealthPotion)basicHealthPotions.First())); }
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
        public Job Job { get { return job; } private set { job = value; } }

        // Constructor
        public Archer(CharacterStat characterStat) : base(characterStat) 
        { 
            job = Job.Archer;
            var basicHelmets = from armor in ItemLists.armors
                               where armor.GetType().Equals(typeof(Helmet)) && armor.Rarity == Rarity.Common
                               select armor;
            var basicChestArmors = from armor in ItemLists.armors
                                   where armor.GetType().Equals(typeof(ChestArmor)) && armor.Rarity == Rarity.Common
                                   select armor;
            var basicBows = from bow in ItemLists.weapons
                              where bow.GetType().Equals(typeof(Bow)) && bow.Rarity == Rarity.Common
                              select bow;
            var basicHealthPotions = from item in ItemLists.consumables
                                     where item.GetType().Equals(typeof(HealthPotion)) && item.Rarity == Rarity.Common
                                     select item;

            if (basicHelmets.Count() > 0) { Armors.Add(new Helmet((Helmet)basicHelmets.First())); }
            if (basicChestArmors.Count() > 0) { Armors.Add(new ChestArmor((ChestArmor)basicChestArmors.First())); }
            if (basicBows.Count() > 0) { Weapons.Add(new Bow((Bow)basicBows.First())); }
            if (basicHealthPotions.Count() > 0) { Consumables.Add(new HealthPotion((HealthPotion)basicHealthPotions.First())); }
        }
    }
}
