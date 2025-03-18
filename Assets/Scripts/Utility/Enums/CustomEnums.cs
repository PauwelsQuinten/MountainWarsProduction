namespace UnityEngine 
{
    public enum Orientation
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }


    public enum AttackHeight
    {
        Head,
        Torso
    }


    public enum AttackState
    {
        Idle,
        Attack,
        Block,
        Parrry,
        blockAttack
    }

    public enum CurrentWeapon
    {
        Fist,
        Sword,
        Shield
    }
}