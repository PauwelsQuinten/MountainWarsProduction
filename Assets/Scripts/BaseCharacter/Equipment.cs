using UnityEngine;

public class Equipment : MonoBehaviour
{
    private EquipmentType _type;
    public EquipmentType Type { get { return _type; } private set { _type = value; } }
    private float _durability = 10f;
    public float Durability { get { return _durability; } set {_durability = value; } }
    private bool _isRightHandEquipment = false;
    public bool IsRightHandEquipment { get { return _isRightHandEquipment; } private set { _isRightHandEquipment = value; } }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
