using FMOD.Studio;
using UnityEngine;
using FMODUnity;
public class FMODAudioHandler : MonoBehaviour
{
    public static FMODAudioHandler Instance { get; private set; }
    
    private EventInstance _footstepsSFXInstance;
    private PARAMETER_ID _surfaceTypeID;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    //Global Parameters

    //Events
    [SerializeField] private EventReference _footstepsSFX;
    public EventReference FootstepsSFX => _footstepsSFX;
    
    [SerializeField] private EventReference _attackChargeSFX;
    public EventReference AttackChargeSFX => _attackChargeSFX;
    
    [SerializeField] private EventReference _swordSwooshSFX;
    public EventReference SwordSwooshSFX => _swordSwooshSFX;
    
    [SerializeField] private EventReference _swordHitSFX;
    public EventReference SwordHitSFX => _swordHitSFX;
    
    public static void GetParameterID(EventInstance eventInstance, string parameterName, out PARAMETER_ID parameterID)
    {
        // Get the parameter ID
        eventInstance.getDescription(out EventDescription eventDescription);
        eventDescription.getParameterDescriptionByName(parameterName, out PARAMETER_DESCRIPTION parameterDescription);
        parameterID = parameterDescription.id;
    }

    public static void SetParameterByID(EventInstance eventInstance, PARAMETER_ID parameterID, float desiredParameterValue)
    {
        // Set the parameter value by ID
        eventInstance.setParameterByID(parameterID, desiredParameterValue);
    }
}