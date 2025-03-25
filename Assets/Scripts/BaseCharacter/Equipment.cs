using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private EquipmentType _type;
    public EquipmentType Type { get { return _type; } private set { _type = value; } }
    [SerializeField] private float _durability = 10f;
    public float Durability { get { return _durability; } set {_durability = value; } }
    [SerializeField] private bool _isRightHandEquipment = false;
    public bool IsRightHandEquipment { get { return _isRightHandEquipment; } private set { _isRightHandEquipment = value; } }

    public void Damage(float damage, BlockResult blockResult)
    {
        float damageMultiplier = 1f;
        switch (blockResult)
        {
            case BlockResult.Hit:
                damageMultiplier = 0f;
                break;
            case BlockResult.SwordBlock:
                damageMultiplier = 0.7f;
                break;
            case BlockResult.SwordHalfBlock:
                damageMultiplier = 1f;
                break;
            case BlockResult.HalfBlocked:
                damageMultiplier = 0.7f;
                break;
            case BlockResult.FullyBlocked:
                damageMultiplier = 0.5f;
                break;
            case BlockResult.Parried:
                damageMultiplier = 0f;
                break;
        }

        _durability -= damage * damageMultiplier;
    }

}
