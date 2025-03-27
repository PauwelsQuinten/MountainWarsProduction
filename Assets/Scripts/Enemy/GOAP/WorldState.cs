using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    [Header("Type")]
    public WorldStateType WorldStateType = WorldStateType.Desired;
    //Low to high priority
    [SerializeField] private List<EWorldState> PriorityList = new List<EWorldState>();

    [Header("Values")]
    [SerializeField]
    private EWorldStateValue _targetHealth;
    [SerializeField]         
    private EWorldStateValue _targetStamina;
    [SerializeField]         
    private EWorldStateValue _targetRHEquipment;
    [SerializeField]         
    private EWorldStateValue _targetLHEquipment;
    [SerializeField]
    private EWorldStateValue _health;
    [SerializeField]
    private EWorldStateValue _stamina;
    [SerializeField]
    private EWorldStateValue _rHEquipment;
    [SerializeField]
    private EWorldStateValue _lHEquipment;

    [Header("Possesion")]
    [SerializeField]
    private EWorldStatePossesion _hasTarget;
    [SerializeField]
    private EWorldStatePossesion _hasOpening;
    
    [Header("Behaviour")]
    [SerializeField]
    private EWorldStatePossesion _targetBehaviour;
    [SerializeField]
    private EWorldStatePossesion _behaviour;
    
    [Header("Range")]
    [SerializeField]
    private EWorldStatePossesion _targetDistance;
    

    public Dictionary<EWorldState, EWorldStateValue> WorldStateValues = new Dictionary<EWorldState, EWorldStateValue>();
    public Dictionary<EWorldState, EWorldStatePossesion> WorldStatePossesions = new Dictionary<EWorldState, EWorldStatePossesion>();
    public Dictionary<EWorldState, EBehaviourValue> WorldStateBehaviours = new Dictionary<EWorldState, EBehaviourValue>();
    public Dictionary<EWorldState, EWorldStateRange> WorldStateRanges = new Dictionary<EWorldState, EWorldStateRange>();


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


}
