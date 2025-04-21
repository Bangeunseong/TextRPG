using System.Text.Json.Serialization;

namespace TextRPG
{
    abstract class Quest : IContractable
    {
        // Field
        private string name;
        private string description;
        private int rewardExp;
        private int rewardGold;

        // Property
        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public int RewardExp { get { return rewardExp; } set { rewardExp = value; } }
        public int RewardGold { get { return rewardGold; } set { rewardGold = value; } }
        [JsonInclude] public bool IsCompleted { get; protected set; } = false;
        [JsonInclude] public bool IsSpecial { get; protected set; } = false;

        // Constructor
        public Quest(string name, string description, int rewardExp, int rewardGold)
        {
            this.name = name;
            this.description = description;
            this.rewardExp = rewardExp;
            this.rewardGold = rewardGold;
        }
        public Quest(Quest quest)
        {
            name = quest.name;
            description = quest.description;
            rewardExp = quest.rewardExp;
            rewardGold = quest.rewardGold;
            IsCompleted = quest.IsCompleted;
            IsSpecial = quest.IsSpecial;
        }

        [JsonConstructor]
        public Quest(string name, string description, int rewardExp, int rewardGold, bool isCompleted, bool isSpecial)
        {
            this.name = name;
            this.description = description;
            this.rewardExp = rewardExp;
            this.rewardGold = rewardGold;
            IsCompleted = isCompleted;
            IsSpecial = isSpecial;
        }

        /// <summary>
        /// Called when the quest is completed.
        /// </summary>
        /// <param name="character"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void OnCompleted(Character character)
        {
            IsCompleted = true;
            Console.WriteLine($"| Quest '{Name}' completed! |");
        }

        /// <summary>
        /// Called when the quest is contracted.
        /// </summary>
        /// <param name="character"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void OnContracted(Character character)
        {
            Console.WriteLine($"| Quest '{Name}' contracted! |");
        }
    }

    class NormalQuest : Quest, ICancelable
    {
        // Constructor
        public NormalQuest(string name, string description, int rewardExp, int rewardGold) : base(name, description, rewardExp, rewardGold) { IsSpecial = false; }
        public NormalQuest(NormalQuest quest) : base(quest) { IsSpecial = false; }
        [JsonConstructor]
        public NormalQuest(string name, string description, int rewardExp, int rewardGold, bool isCompleted, bool isSpecial) : base(name, description, rewardExp, rewardGold, isCompleted, isSpecial) { }

        // Methods
        public override void OnCompleted(Character character)
        {
            base.OnCompleted(character);
            // TODO: Add quest completion logic here
        }

        public override void OnContracted(Character character)
        {
            base.OnContracted(character);
            // TODO: Add quest contracting logic here
        }

        public void OnCanceled(Character character)
        {
            // TODO: Add quest cancellation logic here
        }
    }

    class SpecialQuest : Quest
    {
        // Constructor
        public SpecialQuest(string name, string description, int rewardExp, int rewardGold) : base(name, description, rewardExp, rewardGold) { IsSpecial = true; }
        public SpecialQuest(SpecialQuest quest) : base(quest) { IsSpecial = true; }
        [JsonConstructor]
        public SpecialQuest(string name, string description, int rewardExp, int rewardGold, bool isCompleted, bool isSpecial) : base(name, description, rewardExp, rewardGold, isCompleted, isSpecial) { }
        
        // Methods
        public override void OnCompleted(Character character)
        {
            base.OnCompleted(character);
            // TODO: Add special quest completion logic here
        }
        public override void OnContracted(Character character)
        {
            base.OnContracted(character);
            // TODO: Add special quest contracting logic here
        }
    }
}
