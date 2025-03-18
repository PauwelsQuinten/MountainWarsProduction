using UnityEngine;

public class StateManager : MonoBehaviour
{
    public AttackState AttackState;
}

public enum AttackState 
{ 
    Idle,
    Attack,
    Defend,
    Parrry
}

public enum CurrentWeapon 
{
    Fist,
    Sword,
    Shield
}