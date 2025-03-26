using System.Collections;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] GameEvent _OnKnockbackRecovery;
    public AttackState AttackState;
    public AttackHeight AttackHeight;
    public Orientation Orientation;
    public CharacterState CharacterState;

    public bool IsHoldingShield;

    public EquipmentManager EquipmentManager;

    private void Start()
    {
        if (EquipmentManager == null)        
            EquipmentManager = GetComponent<EquipmentManager>();
    }

    public void GetKnockback(Component sender, object obj)
    {
        
            Debug.Log("start knockback");
         StartCoroutine(RecoverKnockback());

    }

    private IEnumerator RecoverKnockback()
    {
        yield return new WaitForSeconds(5.4f);
        _OnKnockbackRecovery.Raise(this);
            Debug.Log("stop knockback");
    }

}