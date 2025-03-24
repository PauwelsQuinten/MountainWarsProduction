using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private EquipmentType _type;
    public EquipmentType Type { get { return _type; } private set { _type = value; } }
    [SerializeField] private float _durability = 10f;
    public float Durability { get { return _durability; } set {_durability = value; } }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
