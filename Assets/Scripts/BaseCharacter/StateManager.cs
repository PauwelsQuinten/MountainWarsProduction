using System.Collections;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private const string PLAYER = "Player";

    [SerializeField] GameEvent _OnKnockbackRecovery;
    [SerializeField] BlackboardReference _blackboardRef;
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

        if (!gameObject.CompareTag(PLAYER))
            _blackboardRef.variable.State = AttackState ;
    }

    public void SetTarget(Component sender, object obj)
    {
        if(sender.gameObject != gameObject) return;
        var args = obj as NewTargetEventArgs;
        if (args == null) return;

        Target = args.NewTarget;

        //Update Blackboard
        if (!gameObject.CompareTag(PLAYER))
        {
            _blackboardRef.variable.Target = Target;
            
            if (Target)
                _blackboardRef.variable.TargetState = Target.GetComponent<StateManager>().AttackState;
        }
    }

    public void ChangeOrientation(Component sender, object obj)
    {
        if (sender.gameObject != gameObject) return;
        var args = obj as OrientationEventArgs;
        if (args == null) return;

        Orientation = args.NewOrientation;
    }

    public void GetKnockback(Component sender, object obj)
    {
            Debug.Log("start knockback");
        AttackState = AttackState.Knock;
         StartCoroutine(RecoverKnockback());
    }
    
    private IEnumerator RecoverKnockback()
    {
        yield return new WaitForSeconds(5.4f);
        _OnKnockbackRecovery.Raise(this);
        AttackState = AttackState.Idle;
            Debug.Log("stop knockback");
    }

}