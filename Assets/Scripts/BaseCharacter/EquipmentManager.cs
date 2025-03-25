using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private Equipment _leftHand;
    [SerializeField] private Equipment _rightHand;
    [SerializeField] private Equipment _fists;
    private List<Equipment> HeldEquipment = new List<Equipment> {null, null, null };

    private const int LEFT_HAND = 0;
    private const int RIGHT_HAND = 1;
    private const int FISTS = 2;

    private Equipment _discoverdEquipment;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (_leftHand && !_leftHand.IsRightHandEquipment)
        {
            var leftEquipment = Instantiate(_leftHand);
            HeldEquipment[LEFT_HAND] = leftEquipment;
        }


        if (_rightHand && _rightHand.IsRightHandEquipment)
        {
            var rightEquipment = Instantiate(_rightHand);
            HeldEquipment[RIGHT_HAND] = rightEquipment;
        }


        if (_fists && _fists.Type == EquipmentType.Fist)
        {
            var fist = Instantiate(_fists);
            HeldEquipment[FISTS] = fist;
        }


    }

    public void CheckDurability(Component sender, object obj)
    {
        //Check for vallid signal
        if (sender.gameObject != gameObject) return;
        DefenceEventArgs args = obj as DefenceEventArgs;
        if (args == null) return;


        int index = args.BlockMedium == BlockMedium.Sword ? 1 : 0;
        HeldEquipment[index].Damage(args.AttackPower, args.BlockResult);
        if (HeldEquipment[index].Durability < 0f)
        {
            Destroy(HeldEquipment[index].gameObject);
            HeldEquipment[index] = null;
            Debug.Log($"breaks {args.BlockMedium}");
        }
    }

    public void PickupEquipment(Equipment equip)
    {

    }

    public Equipment GetEquipment(bool isRighthand)
    {
        return null;
    }
    
    public bool HasEquipmentInHand(bool isRighthand)
    {
        int index = isRighthand ? 1 : 0;
        return HeldEquipment[index] != null;
    }
    
    public float GetEquipmentPower()
    {
        return 0f;
    }

    public void DropEquipment()
    {

    }



}
