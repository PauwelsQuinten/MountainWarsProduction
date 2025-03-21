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

    public enum BlockResult
    {
        Hit,
        //This will happen when you are holding the shield up in center position while the attack comes either from left or right.
        //this will not cause damage but will cut down your Stamina more. 
        HalfBlocked, 
        //This will happen when you are holding the shield up in position of the attack.
        //this will take less Stamina from you and cause a small knockback to the opponent. 
        FullyBlocked,
        //This will take the least amount of Stamina and create the biggest opening to attack the opponent afterwards
        Parried
    }

}