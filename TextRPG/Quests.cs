using System.Text.Json.Serialization;

namespace TextRPG
{
    abstract class Quest : IContractable
    {
        // Field
        private string name;
        private string description;
        private QuestDifficulty difficulty;
        private int rewardExp;
        private int rewardGold;

        // Property
        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public QuestDifficulty Difficulty { get { return difficulty; } set { difficulty = value; } }
        public int RewardExp { get { return rewardExp; } set { rewardExp = value; } }
        public int RewardGold { get { return rewardGold; } set { rewardGold = value; } }
        [JsonInclude] public bool IsCompleted { get; protected set; } = false;
        [JsonInclude] public bool IsSpecial { get; protected set; } = false;
        public bool IsCompletable { get; protected set; } = false;

        // Constructor
        public Quest(string name, string description, QuestDifficulty difficulty, int rewardExp, int rewardGold)
        {
            this.name = name;
            this.description = description;
            this.difficulty = difficulty;
            this.rewardExp = rewardExp;
            this.rewardGold = rewardGold;
        }
        public Quest(Quest quest)
        {
            name = quest.name;
            description = quest.description;
            difficulty = quest.difficulty;
            rewardExp = quest.rewardExp;
            rewardGold = quest.rewardGold;
            IsCompleted = quest.IsCompleted;
            IsSpecial = quest.IsSpecial;
            IsCompletable = quest.IsCompletable;
        }

        [JsonConstructor]
        public Quest(string name, string description, QuestDifficulty difficulty, int rewardExp, int rewardGold, bool isCompleted, bool isSpecial, bool isCompletable)
        {
            this.name = name;
            this.description = description;
            this.difficulty = difficulty;
            this.rewardExp = rewardExp;
            this.rewardGold = rewardGold;
            IsCompleted = isCompleted;
            IsSpecial = isSpecial;
            IsCompletable = isCompletable;
        }

        /// <summary>
        /// Called when the quest is completed.
        /// </summary>
        /// <param name="character"></param>
        public virtual void OnCompleted(Character character)
        {
            IsCompleted = true;
            Console.WriteLine($"| Quest '{Name}' completed! |");
        }

        /// <summary>
        /// Called when the quest is contracted.
        /// </summary>
        /// <param name="character"></param>
        public virtual void OnContracted(Character character)
        {
            Console.WriteLine($"| Quest '{Name}' contracted! |");
        }

        /// <summary>
        /// Called when the quest is completable.
        /// </summary>
        public void OnCompletable()
        {
            IsCompletable = true;
        }

        /// <summary>
        /// Called when the quest is not completable.
        /// </summary>
        public void OnNotCompletable()
        {
            IsCompletable = false;
        }
    }

    /// <summary>
    /// NormalQuest Class -> Available in all days.
    /// </summary>
    class NormalQuest : Quest, ICancelable
    {
        // Constructor
        public NormalQuest(string name, string description, QuestDifficulty difficulty, int rewardExp, int rewardGold) : base(name, description, difficulty, rewardExp, rewardGold) { IsSpecial = false; }
        public NormalQuest(NormalQuest quest) : base(quest) { IsSpecial = false; }
        [JsonConstructor]
        public NormalQuest(string name, string description, QuestDifficulty difficulty, int rewardExp, int rewardGold, bool isCompleted, bool isSpecial, bool isCompletable) : base(name, description, difficulty, rewardExp, rewardGold, isCompleted, isSpecial, isCompletable) { }

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

    /// <summary>
    /// SpecialQuest Class -> Only available in single day.
    /// </summary>
    class SpecialQuest : Quest
    {
        // Constructor
        public SpecialQuest(string name, string description, QuestDifficulty difficulty, int rewardExp, int rewardGold) : base(name, description, difficulty, rewardExp, rewardGold) { IsSpecial = true; }
        public SpecialQuest(SpecialQuest quest) : base(quest) { IsSpecial = true; }
        [JsonConstructor]
        public SpecialQuest(string name, string description, QuestDifficulty difficulty, int rewardExp, int rewardGold, bool isCompleted, bool isSpecial, bool isCompletable) : base(name, description, difficulty, rewardExp, rewardGold, isCompleted, isSpecial, isCompletable) { }
        
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
