using System.Collections;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] GameEvent _OnStunRecovery;
    public AttackState AttackState;
    public AttackHeight AttackHeight;
    public Orientation Orientation;

    public GameObject Target;
    public bool IsHoldingShield;

    public EquipmentManager EquipmentManager;

    public bool IsBleeding;

    private void Start()
    {
        if (EquipmentManager == null)        
            EquipmentManager = GetComponent<EquipmentManager>();
    }

    public void GetStunned(Component sender, object obj)
    {
        StunEventArgs args = obj as StunEventArgs;
        if (args == null) return;

        if (args.ComesFromEnemy)
        {
            if (sender.gameObject == gameObject) return;
        }
        else if (sender.gameObject != gameObject) return;

        AttackState = AttackState.Stun;
        StartCoroutine(RecoverStun(args.StunDuration));
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

    private IEnumerator RecoverStun(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        _OnStunRecovery.Raise(this);
        AttackState = AttackState.Idle;
    }
}