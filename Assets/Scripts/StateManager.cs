using UnityEngine;

public class StateManager : MonoBehaviour
{
    public AttackState AttackState;
}

public enum AttackState 
{ 
    Idle,
    Attack,
    Block,
    Parrry
}

public enum CurrentWeapon 
{
    Fist,
    Sword,
    Shield
}