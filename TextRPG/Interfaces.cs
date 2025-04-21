namespace TextRPG
{
    interface IDamagable
    {
        public void OnDamage(AttackType type, float damage);
    }
    
    interface IWearable
    {
        public void OnEquipped(Character character);
        public void OnUnequipped(Character character);
    }

    interface IUseable
    {
        public void OnUsed(Character character);
        public void OnDeBuffed(Character character);
    }

    interface IPurchasable
    {
        public void OnPurchased(Character character);
    }

    interface ISellable
    {
        public void OnSold(Character character);
    }

    interface IPickable
    {
        public void OnPicked(Character character);
        public void OnDropped(Character character);
    }

    interface IContractable
    {
        public void OnContracted(Character character);
        public void OnCompleted(Character character);
        public void OnCompletable();
        public void OnNotCompletable();
    }

    interface ICancelable
    {
        public void OnCanceled(Character character);
    }
}
