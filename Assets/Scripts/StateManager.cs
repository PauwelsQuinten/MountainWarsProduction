using UnityEngine;

public class StateManager : MonoBehaviour
{
    public AttackState AttackState;
    public AttackHeight AttackHeight;
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