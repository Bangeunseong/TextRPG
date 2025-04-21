namespace TextRPG
{
    /// <summary>
    /// Base Class of Monsters
    /// </summary>
    abstract class Monster : IDamagable
    {
        // Field
        private CharacterStat characterStat;
        private bool isAlive;
        private int exp;

        // Property
        public float MaxHealth { get { return characterStat.MaxHealth; } }
        public float Health { get { return characterStat.Health; } set { characterStat.Health = Math.Clamp(value, 0, MaxHealth); } }
        public float MaxMagicPoint { get { return characterStat.MaxMagicPoint; } }
        public float MagicPoint { get { return characterStat.MagicPoint; } set { characterStat.MagicPoint = Math.Clamp(value, 0, MaxMagicPoint); } }
        public string Name { get { return characterStat.Name; } set { characterStat.Name = value; } }
        public int Level { get { return characterStat.Level; } set { characterStat.Level = value; } }
        public AttackStat AttackStat { get { return characterStat.AttackStat; } set { characterStat.AttackStat = value; } }
        public DefendStat DefendStat { get { return characterStat.DefendStat; } set { characterStat.DefendStat = value; } }
        public AttackType AttackType { get; protected set; }
        
        public int Exp { get { return exp; } set { exp = value; } }
        public bool IsAlive { get { return isAlive; } private set { isAlive = value; } }

        public event Action OnDeath;

        // Constructor
        public Monster(CharacterStat characterStat, int exp)
        {
            this.characterStat = new(characterStat);
            isAlive = true;
            this.exp = exp;
        }

        // Methods
        public CharacterStat GetStat() { return characterStat; }

        public void OnDamage(AttackType type, float damage)
        {
            float calculatedDamage =
                type == AttackType.Close ? Math.Min(1f, (damage * (1f - DefendStat.Defend / 100f))) :
                (type == AttackType.Long ? Math.Min(1f, damage * (1f - DefendStat.RangeDefend / 100f)) :
                Math.Min(1f, (damage * (1f - DefendStat.MagicDefend / 100f))));

            Console.WriteLine($"| {Name} got {calculatedDamage:F2} damage! |");
            Health -= calculatedDamage;

            if (Health <= 0 && IsAlive) Die();
        }

        private void Die()
        {
            IsAlive = false;
            OnDeath?.Invoke();
        }
    }

    /// <summary>
    /// GoblinWarrior Class -> Close Range Attack Monster
    /// </summary>
    class GoblinWarrior : Monster
    {
        public GoblinWarrior(CharacterStat characterStat, int exp) : base(characterStat, exp)
        {
            AttackType = AttackType.Close;
        }
        public GoblinWarrior(GoblinWarrior warrior) : base(warrior.GetStat(), warrior.Exp)
        {
            AttackType = warrior.AttackType;
        }
    }

    /// <summary>
    /// GoblinArcher Class -> Long Range Attack Monster
    /// </summary>
    class GoblinArcher : Monster
    {
        public GoblinArcher(CharacterStat characterStat, int exp) : base(characterStat, exp)
        {
            AttackType = AttackType.Long;
        }
        public GoblinArcher(GoblinArcher archer) : base(archer.GetStat(), archer.Exp)
        {
            AttackType = archer.AttackType;
        }
    }

    /// <summary>
    /// GoblinMage Class -> Magic Attack Monster
    /// </summary>
    class GoblinMage : Monster
    {
        public GoblinMage(CharacterStat characterStat, int exp) : base (characterStat, exp)
        {
            AttackType = AttackType.Magic;
        }
        public GoblinMage(GoblinMage mage) : base(mage.GetStat(), mage.Exp)
        {
            AttackType = mage.AttackType;
        }
    }

    static class MonsterLists
    {
        public static Monster[] monsters = {
            new GoblinWarrior(new CharacterStat("Normal Goblin Warrior", 150, 10, 1, new AttackStat(20f, 1f, 1f), new DefendStat(18, 15, 3)), 20),
            new GoblinArcher(new CharacterStat("Normal Goblin Archer", 120, 30, 1, new AttackStat(1f, 20f, 1f), new DefendStat(15, 18, 3)), 25),
            new GoblinMage(new CharacterStat("Normal Goblin Mage", 100, 50, 1, new AttackStat(1f, 1f, 20f), new DefendStat(3, 15, 18)), 30),
        };
    }
}
