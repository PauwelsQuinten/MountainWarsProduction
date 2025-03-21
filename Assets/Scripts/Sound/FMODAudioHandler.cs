using FMOD.Studio;
using UnityEngine;
using FMODUnity;

public class FMODAudioHandler : MonoBehaviour
{
    private AimingOutputArgs _aimingEventArgs;

    private PARAMETER_ID _surfaceTypeID;
    
    private PARAMETER_ID _attacksStrengthID;
    private float _attacksStrengthIDValue;

    private PARAMETER_ID _swordHitSurfaceID;
    private float _swordHitSurfaceIDValue;


    private float volume;
    // Events
    [SerializeField] private EventReference _footstepsSFX;
    private EventInstance _footstepsSFXInstance;
    [SerializeField] private EventReference _attackChargeSFX;
    private EventInstance _attackChargeSFXInstance;
    [SerializeField] private EventReference _swordSwooshSFX;
    private EventInstance _swordSwooshSFXInstance;
    [SerializeField] private EventReference _swordHitSFX;
    private EventInstance _swordHitSFXInstance;

    private void Start()
    {
        // Get the parameter ID for SurfaceType
        GetParameterID(_footstepsSFXInstance, "SurfaceType", out _surfaceTypeID);

        // Get the parameter ID for SwordHitSurface
        
        // Get the parameter ID for AttacksStrength
        RuntimeManager.StudioSystem.getParameterDescriptionByName("AttackStrength", out PARAMETER_DESCRIPTION attacksStrengthDescription);
        _attacksStrengthID = attacksStrengthDescription.id;
    }

    private void GetParameterID(EventInstance eventInstance, string parameterName, out PARAMETER_ID parameterID)
    {
        // Get the parameter ID
        eventInstance.getDescription(out EventDescription eventDescription);
        eventDescription.getParameterDescriptionByName(parameterName, out PARAMETER_DESCRIPTION parameterDescription);
        parameterID = parameterDescription.id;
    }

    private void SetParameterByID(EventInstance eventInstance, PARAMETER_ID parameterID, float desiredParameterValue)
    {
        // Set the parameter value by ID
        eventInstance.setParameterByID(parameterID, desiredParameterValue);
    }

    private void SetGlobalParameterByID(PARAMETER_ID parameterID, float desiredParameterValue)
    {
        // Set the global parameter value by ID
        RuntimeManager.StudioSystem.setParameterByID(parameterID, desiredParameterValue);
    }

    public void PlayFootstepsSFX(Component sender, object obj)
    {
        _footstepsSFXInstance = RuntimeManager.CreateInstance(_footstepsSFX);
        _footstepsSFXInstance.start();
        _footstepsSFXInstance.release();

    }

    public void PlaySwordHitSFX(Component sender, object obj)
    {
        _swordHitSFXInstance = RuntimeManager.CreateInstance(_swordHitSFX);
        GetParameterID(_swordHitSFXInstance, "SwordHitSurface", out _swordHitSurfaceID);
        if (_aimingEventArgs == null)
        {
            _aimingEventArgs = obj as AimingOutputArgs;
        }
        SetGlobalParameterByID(_attacksStrengthID, _aimingEventArgs.Speed);
        SetParameterByID(_swordHitSFXInstance, _swordHitSurfaceID, _swordHitSurfaceIDValue);
        _swordHitSFXInstance.start();
        _swordSwooshSFXInstance.release();
    }

    public void PlaySwordSwooshSFX(Component sender, object obj)
    {
        _swordSwooshSFXInstance = RuntimeManager.CreateInstance(_swordSwooshSFX);
        if (_aimingEventArgs == null)
        {
            _aimingEventArgs = obj as AimingOutputArgs;
        }
        SetGlobalParameterByID(_attacksStrengthID, _aimingEventArgs.Speed);

        _swordSwooshSFXInstance.start();
        _swordSwooshSFXInstance.release();
    }
}