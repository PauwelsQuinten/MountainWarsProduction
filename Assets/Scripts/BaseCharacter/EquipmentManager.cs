using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private Equipment _leftHand;
    [SerializeField] private Equipment _rightHand;
    private List<Equipment> HeldEquipment = new List<Equipment>();

    private const int LEFT_HAND = 0;
    private const int RIGHT_HAND = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_leftHand && _leftHand.Type != EquipmentType.Shield)
            _leftHand = null;

        if (_rightHand && _rightHand.Type == EquipmentType.Shield)
            _rightHand = null;

        HeldEquipment.Add(_leftHand);
        HeldEquipment.Add(_rightHand);
    }

    public void CheckDurability(float damage, bool isRightHand)
    {

    }
}
