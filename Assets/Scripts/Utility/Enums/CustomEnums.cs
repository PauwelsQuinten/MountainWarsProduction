namespace UnityEngine 
{
    public enum Orientation
    {
        North = 90,
        NorthEast = 45,
        East = 0,
        SouthEast = -45,
        South = -90,
        SouthWest = -135,
        West = 180,
        NorthWest = 135
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
        ShieldDefence,
        SwordDefence,
        BlockAttack
    }

    public enum CurrentWeapon
    {
        Fist,
        Sword,
        Shield
    }

    public enum AimingInputState
    {
        Idle, 
        Moving, 
        Hold, 
        Reset
    }

    public enum Direction
    {
        ToRight, 
        ToLeft, 
        ToCenter,
        Wrong
    }

    public enum AttackSignal
    {
        Idle, 
        Stab, 
        Feint, 
        Swing,
        Charge
    }

    public enum AttackType
    {
        Stab,
        HorizontalSlashToLeft,
        HorizontalSlashToRight,
    }
}