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
        ToCenter
    }

    public enum AttackSignal
    {
        Idle, 
        Stab, 
        Feint, 
        Swing
    }

    public enum AttackType
    {
        Stab,
        HorizontalSlashToLeft,
        HorizontalSlashToRight,
    }
}