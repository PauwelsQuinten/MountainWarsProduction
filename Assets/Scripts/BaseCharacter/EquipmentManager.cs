using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private Equipment _leftHand;
    [SerializeField] private Equipment _rightHand;
    [SerializeField] private Equipment _fists;
    private List<Equipment> HeldEquipment = new List<Equipment>();

    private const int LEFT_HAND = 0;
    private const int RIGHT_HAND = 1;
    private const int FISTS = 2;

    private Equipment _discoverdEquipment;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_leftHand && !_leftHand.IsRightHandEquipment)
            HeldEquipment.Add(_leftHand);

        if (_rightHand && _rightHand.IsRightHandEquipment)
            HeldEquipment.Add(_rightHand);

    }

    public void CheckDurability(float damage, bool isRightHand)
    {

    }

    public void PickupEquipment(Equipment equip)
    {

    }

    public Equipment GetEquipment(bool isRighthand)
    {
        return null;
    }
    
    public float GetEquipmentPower()
    {
        return 0f;
    }

    public void DropEquipment()
    {

    }



}
