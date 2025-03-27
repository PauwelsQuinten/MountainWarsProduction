using System.Collections;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] GameEvent _OnKnockbackRecovery;
    public AttackState AttackState;
    public AttackHeight AttackHeight;
    public Orientation Orientation;
    //public CharacterState CharacterState;

    public GameObject Target;
    public bool IsHoldingShield;

    public EquipmentManager EquipmentManager;

    public bool IsBleeding;

    private void Start()
    {
        if (EquipmentManager == null)        
            EquipmentManager = GetComponent<EquipmentManager>();
    }

    public void GetKnockback(Component sender, object obj)
    {
        AttackState = AttackState.Knock;
         StartCoroutine(RecoverKnockback());
    }
    
    public void SetTarget(Component sender, object obj)
    {
        if(sender.gameObject != gameObject) return;
        var args = obj as NewTargetEventArgs;
        if (args == null) return;

        Target = args.NewTarget;
    }

    public void ChangeOrientation(Component sender, object obj)
    {
        if (sender.gameObject != gameObject) return;
        var args = obj as OrientationEventArgs;
        if (args == null) return;

        Orientation = args.NewOrientation;
    }

    private IEnumerator RecoverKnockback()
    {
        yield return new WaitForSeconds(5.4f);
        _OnKnockbackRecovery.Raise(this);
        AttackState = AttackState.Idle;
    }
}