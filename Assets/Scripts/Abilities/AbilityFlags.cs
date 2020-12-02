namespace My.Abilities
{
    [System.Flags]
    public enum AbilityFlags
    {
        None = 0,
        Melee = 1 << 1,
        Ranged = 1 << 2,
        Damageable = 1 << 3,
        Mobile = 1 << 4,
        Stealer = 1 << 5,
        Targeter = 1 << 6,
        Boss = 1 << 7,
        Robber = 1 << 8,
        Cop = 1 << 9,
    }
}