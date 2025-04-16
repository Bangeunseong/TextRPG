using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    /// <summary>
    /// Base Class of Monsters
    /// </summary>
    abstract class Monster : IDamagable
    {
        private CharacterStat _characterStat;
        private bool _isAlive;
        private int _exp;

        // Property
        public float MaxHealth { get { return _characterStat.MaxHealth; } }
        public float Health { get { return _characterStat.Health; } set { _characterStat.Health = Math.Clamp(value, 0, MaxHealth); } }
        public string Name { get { return _characterStat.Name; } set { _characterStat.Name = value; } }
        public int Level { get { return _characterStat.Level; } set { _characterStat.Level = value; } }
        public AttackStat AttackStat { get { return _characterStat.AttackStat; } set { _characterStat.AttackStat = value; } }
        public DefendStat DefendStat { get { return _characterStat.DefendStat; } set { _characterStat.DefendStat = value; } }
        public AttackType AttackType { get; protected set; }
        
        public int Exp { get { return _exp; } set { _exp = value; } }
        public bool IsAlive { get { return _isAlive; } private set { _isAlive = value; } }

        public event Action OnDeath;

        public Monster(CharacterStat characterStat, int exp)
        {
            _characterStat = characterStat;
            IsAlive = true;
            Exp = exp;
        }

        public CharacterStat GetStat() { return _characterStat; }

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
            new GoblinWarrior(new CharacterStat("Normal Goblin Warrior", 150, 1, new AttackStat(10f, 1f, 1f), new DefendStat(25, 10, 1)), 20),
            new GoblinArcher(new CharacterStat("Normal Goblin Archer", 120, 1, new AttackStat(1f, 10f, 1f), new DefendStat(15, 18, 3)), 25),
            new GoblinMage(new CharacterStat("Normal Goblin Mage", 100, 1, new AttackStat(1f, 10f, 1f), new DefendStat(15, 18, 3)), 30),
        };
    }
}
