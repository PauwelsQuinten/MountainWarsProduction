using UnityEngine;

public class StateManager : MonoBehaviour
{
    public AttackState AttackState;
    public AttackHeight AttackHeight;
    public Orientation Orientation;

    public bool IsHoldingShield;

    public EquipmentManager EquipmentManager;

    private void Start()
    {
        if (EquipmentManager == null)        
            EquipmentManager = GetComponent<EquipmentManager>();
    }
}