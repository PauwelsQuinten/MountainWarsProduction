using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class WorldState : MonoBehaviour
{
    [Header("Type")]
    public WorldStateType WorldStateType = WorldStateType.Desired;
    //Low to high priority
    [SerializeField] private List<EWorldState> PriorityList = new List<EWorldState>();
    private BlackboardReference _blackboard;

    [Header("Values")]
    [SerializeField]
    private EWorldStateValue _targetHealth = EWorldStateValue.Default;
    [SerializeField]         
    private EWorldStateValue _targetStamina = EWorldStateValue.Default;
    [SerializeField]         
    private EWorldStateValue _targetRHEquipment = EWorldStateValue.Default;
    [SerializeField]         
    private EWorldStateValue _targetLHEquipment = EWorldStateValue.Default;
    [SerializeField]
    private EWorldStateValue _health = EWorldStateValue.Default;
    [SerializeField]
    private EWorldStateValue _stamina = EWorldStateValue.Default;
    [SerializeField]
    private EWorldStateValue _rHEquipment = EWorldStateValue.Default;
    [SerializeField]
    private EWorldStateValue _lHEquipment = EWorldStateValue.Default;

    [Header("Possesion")]
    [SerializeField]
    private EWorldStatePossesion _hasTarget = EWorldStatePossesion.Default;
    [SerializeField]
    private EWorldStatePossesion _hasOpening = EWorldStatePossesion.Default;
    
    [Header("Behaviour")]
    [SerializeField]
    private EBehaviourValue _targetBehaviour = EBehaviourValue.Default;
    [SerializeField]
    private EBehaviourValue _behaviour = EBehaviourValue.Default;
    
    [Header("Range")]
    [SerializeField]
    private EWorldStateRange _targetDistance = EWorldStateRange.Default;
    
    public Dictionary<EWorldState, EWorldStateValue> WorldStateValues = new Dictionary<EWorldState, EWorldStateValue>();
    public Dictionary<EWorldState, EWorldStatePossesion> WorldStatePossesions = new Dictionary<EWorldState, EWorldStatePossesion>();
    public Dictionary<EWorldState, EBehaviourValue> WorldStateBehaviours = new Dictionary<EWorldState, EBehaviourValue>();
    public Dictionary<EWorldState, EWorldStateRange> WorldStateRanges = new Dictionary<EWorldState, EWorldStateRange>();



    private void Start()
    {
        _blackboard.variable.ValueChanged += Blackboard_ValueChanged;
    }

    public void UpdateWorldState()
    {

    }

    public List<EWorldState> CompareWorldState(WorldState desiredWorldState)
    {
        List<EWorldState> listOfDifference = new List<EWorldState>();

        //Values
        foreach (KeyValuePair<EWorldState, EWorldStateValue> worldState in desiredWorldState.WorldStateValues)
        {
            if (worldState.Value - WorldStateValues[worldState.Key] != 0)
            {
                listOfDifference.Add(worldState.Key);
            }
        }

        //Check Possesion
        foreach (KeyValuePair<EWorldState, EWorldStatePossesion> worldState in desiredWorldState.WorldStatePossesions)
        {
            if (worldState.Value - WorldStatePossesions[worldState.Key] != 0 && worldState.Value != EWorldStatePossesion.Default)
            {
                listOfDifference.Add(worldState.Key);
            }
        }
        
        //Check Ranges
        foreach (KeyValuePair<EWorldState, EWorldStateRange> worldState in desiredWorldState.WorldStateRanges)
        {
            if (worldState.Value - WorldStateRanges[worldState.Key] != 0 && worldState.Value != EWorldStateRange.Default)
            {
                listOfDifference.Add(worldState.Key);
            }
        }
        
        //Check behaviour
        foreach (KeyValuePair<EWorldState, EBehaviourValue> worldState in desiredWorldState.WorldStateBehaviours)
        {
            if (worldState.Value - WorldStateBehaviours[worldState.Key] != 0 && worldState.Value != EBehaviourValue.Default)
            {
                listOfDifference.Add(worldState.Key);
            }
        }

        return listOfDifference;
    }

    //------------------------------------------------------------------------------
    //WORLDSTATE VALUES UPDATE FUNCTIONS
    //------------------------------------------------------------------------------
    public void AsignBlackboard(BlackboardReference blackboardReference)
    {
        _blackboard = blackboardReference;
    }

    private void Blackboard_ValueChanged(object sender, BlackboardEventArgs e)
    {
        switch (e.ThisChanged)
        {
            case BlackboardEventArgs.WhatChanged.Stamina:
                CalculateValue(_blackboard.variable.Stamina);
                break;
            case BlackboardEventArgs.WhatChanged.Health:
                CalculateValue(_blackboard.variable.Health);
                break;
            case BlackboardEventArgs.WhatChanged.RHEquipment:
                CalculateValue(_blackboard.variable.RHEquipmentHealth);
                break;
            case BlackboardEventArgs.WhatChanged.LHEquipment:
                CalculateValue(_blackboard.variable.LHEquipmentHealth);
                break;
            case BlackboardEventArgs.WhatChanged.Target:
                break;
            case BlackboardEventArgs.WhatChanged.TargetStamina:
                CalculateValue(_blackboard.variable.TargetStamina);
                break;
            case BlackboardEventArgs.WhatChanged.TargetHealth:
                CalculateValue(_blackboard.variable.TargetHealth);
                break;
            case BlackboardEventArgs.WhatChanged.TargetRHEquipment:
                CalculateValue(_blackboard.variable.TargetRHEquipmentHealth);
                break;
            case BlackboardEventArgs.WhatChanged.TargetLHEquipment:
                CalculateValue(_blackboard.variable.TargetLHEquipmentHealth);
                break;
        }
    }


    private EWorldStateValue CalculateValue(float fValue)
    {
        if (fValue == 1f )
            return EWorldStateValue.Full;

        if (fValue >= 0.66f )
            return EWorldStateValue.High;

        if (fValue >= 0.33f )
            return EWorldStateValue.Mid;

        if (fValue < 0.33f && fValue >= 0f)
            return EWorldStateValue.Low;

        if (fValue <= 0f )
            return EWorldStateValue.Zero;

        return EWorldStateValue.Default;
    }




    //------------------------------------------------------------------------------
    //WORLDSTATE PUBLIC SETTERS TO UPDATE LIST AT SAME TIME
    //------------------------------------------------------------------------------

    #region Setters

    public EWorldStateValue TargetHealth
    {
        get { return _targetHealth; }
        set
        {
            _targetHealth = value;
            WorldStateValues[EWorldState.TargetHealth] = _targetHealth;
        }
    }

    public EWorldStateValue TargetStamina
    {
        get { return _targetStamina; }
        set
        {
            _targetStamina = value;
            WorldStateValues[EWorldState.TargetStamina] = _targetStamina;
        }
    }

    public EWorldStateValue TargetRHEquipment
    {
        get { return _targetRHEquipment; }
        set
        {
            _targetRHEquipment = value;
            WorldStateValues[EWorldState.TargetRHEquipment] = _targetRHEquipment;
        }
    }

    public EWorldStateValue TargetLHEquipment
    {
        get { return _targetLHEquipment; }
        set
        {
            _targetLHEquipment = value;
            WorldStateValues[EWorldState.TargetLHEquipment] = _targetLHEquipment;
        }
    }

    public EWorldStateValue Health
    {
        get { return _health; }
        set
        {
            _health = value;
            WorldStateValues[EWorldState.Health] = _health;
        }
    }

    public EWorldStateValue Stamina
    {
        get { return _stamina; }
        set
        {
            _stamina = value;
            WorldStateValues[EWorldState.Stamina] = _stamina;
        }
    }

    public EWorldStateValue RHEquipment
    {
        get { return _rHEquipment; }
        set
        {
            _rHEquipment = value;
            WorldStateValues[EWorldState.RHEquipment] = _rHEquipment;
        }
    }

    public EWorldStateValue LHEquipment
    {
        get { return _lHEquipment; }
        set
        {
            _lHEquipment = value;
            WorldStateValues[EWorldState.LHEquipment] = _lHEquipment;
        }
    }


    public EWorldStatePossesion HasTarget
    {
        get { return _hasTarget; }
        set
        {
            _hasTarget = value;
            WorldStatePossesions[EWorldState.HasTarget] = _hasTarget;
        }
    }

    public EWorldStatePossesion HasOpening
    {
        get { return _hasOpening; }
        set
        {
            _hasOpening = value;
            WorldStatePossesions[EWorldState.TargetOpening] = _hasOpening;
        }
    }

    public EBehaviourValue TargetBehaviour
    {
        get { return _targetBehaviour; }
        set
        {
            _targetBehaviour = value;
            WorldStateBehaviours[EWorldState.TargetBehaviour] = _targetBehaviour;
        }
    }

    public EBehaviourValue Behaviour
    {
        get { return _behaviour; }
        set
        {
            _behaviour = value;
            WorldStateBehaviours[EWorldState.Behaviour] = _behaviour;
        }
    }


    public EWorldStateRange TargetDistance
    {
        get { return _targetDistance; }
        set
        {
            _targetDistance = value;
            WorldStateRanges[EWorldState.TargetDistance] = _targetDistance;
        }
    }


    #endregion Setters

}
