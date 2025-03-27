using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private const string PLAYER = "Player";

    [SerializeField] private Equipment _leftHand;
    [SerializeField] private Equipment _rightHand;
    [SerializeField] private Equipment _fists;
    [Header("Events")]
    [SerializeField] private GameEvent _onEquipmentBreak;
    [Header("Item")]
    [SerializeField] private LayerMask _itemMask;
    [Header("Blackboard")]
    [SerializeField]
    private BlackboardReference _blackboard;

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

    public void LoseEquipment(Component sender, object obj)
    {
        var args = obj as LoseEquipmentEventArgs;
        if (args == null) return;

        if (args.ToSelf && sender.gameObject != gameObject) return;
        if (!args.ToSelf && sender.gameObject == gameObject) return;

        bool isRighthand = false;
        switch (args.EquipmentType)
        {
            case EquipmentType.Ranged:
            case EquipmentType.Melee:
                isRighthand = true;
                break;
              
            case EquipmentType.Shield:
                isRighthand = false;
                break;

            case EquipmentType.Fist:
                return;
        }

        DropEquipment(isRighthand);
    }

    public void CheckDurability(Component sender, object obj)
    {
        //Check for vallid signal
        if (sender.gameObject != gameObject) return;
        DefenceEventArgs args = obj as DefenceEventArgs;
        if (args == null) return;

        //reduce durability
        int index = args.BlockMedium == BlockMedium.Sword ? 1 : 0;
        HeldEquipment[index].Damage(args.AttackPower, args.BlockResult);

        //Check if broken
        if (HeldEquipment[index].Durability < 0f)
        {
            Debug.Log($"!!!!!!!!!!!!!!!!!!!!! breaks {args.BlockMedium} !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Destroy(HeldEquipment[index].gameObject);
            HeldEquipment[index] = null;

            var send = new LoseEquipmentEventArgs 
            {
                EquipmentType = args.BlockMedium == BlockMedium.Shield? EquipmentType.Shield : EquipmentType.Melee,
                ToSelf = true
            };
            _onEquipmentBreak.Raise(this, send );
        }

        //Update blackboard
        if (gameObject.CompareTag(PLAYER))
        {
            _blackboard.variable.TargetLHEquipmentHealth = HeldEquipment[LEFT_HAND].GetDurabilityPercentage();
            _blackboard.variable.TargetRHEquipmentHealth = HeldEquipment[RIGHT_HAND].GetDurabilityPercentage();
        }

        else
        {
            _blackboard.variable.LHEquipmentHealth = HeldEquipment[LEFT_HAND].GetDurabilityPercentage();
            _blackboard.variable.RHEquipmentHealth = HeldEquipment[RIGHT_HAND].GetDurabilityPercentage();
        }
    }

    public void PickupEquipment(Component sender, object obj)
    {
        float radius = 1f;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, _itemMask);
        foreach (var hitCollider in hitColliders)
        {
            var newEquip = hitCollider.gameObject.GetComponent<Equipment>();
            if (newEquip)
            {
                if (newEquip.IsRightHandEquipment)
                {
                    DropEquipment(true);
                    HeldEquipment[RIGHT_HAND] = newEquip;
                }

               else if (!newEquip.IsRightHandEquipment)
               {
                    DropEquipment(false);
                    HeldEquipment[LEFT_HAND] = newEquip;
               }


                hitCollider.gameObject.transform.parent = transform;
            }
        }
    }

    public Equipment GetEquipment(bool isRighthand)
    {
        int index = isRighthand ? 1 : 0;
        return HeldEquipment[index];
    }
    
    public bool HasEquipmentInHand(bool isRighthand)
    {
        int index = isRighthand ? 1 : 0;
        return HeldEquipment[index] != null;
    }
    
    public float GetEquipmentPower()
    {
        if (HeldEquipment[RIGHT_HAND])
            return HeldEquipment[RIGHT_HAND].Power;
        else if (HeldEquipment[LEFT_HAND])
            return HeldEquipment[RIGHT_HAND].Power;
        return HeldEquipment[FISTS].Power;
    }

    private void DropEquipment(bool isRightHand)
    {
        int index = isRightHand ? 1 : 0;
        if (HeldEquipment[index] == null)
            return;
        HeldEquipment[index].transform.parent = null; 
        HeldEquipment[index] = null; 
    }



}
