using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using UnityEngine.Serialization;

public class FMODAudioHandler : MonoBehaviour
{
    private AimingOutputArgs _aimingEventArgs;
    private AttackEventArgs _attackEventArgs;
    //Parameters
    //Local
    private PARAMETER_ID _surfaceTypeID;
    private float _surfaceTypeIDValue;
    private PARAMETER_ID _weaponHitSurfaceID;
    private float _weaponHitSurfaceIDValue;
    
    //Global
    private PARAMETER_ID _attacksStrengthID;
    private float _attacksStrengthIDValue;
    private PARAMETER_ID _currentWeaponID;
    private float _currentWeaponIDValue;
    
    // Events
    [SerializeField] private EventReference _footstepsSFX;
    private EventInstance _footstepsSFXInstance;
    [SerializeField] private EventReference _attackChargeSFX;
    private EventInstance _attackChargeSFXInstance;
   [SerializeField] private EventReference _weaponSwooshSFX;
    private EventInstance _weaponSwooshSFXInstance;
    [SerializeField] private EventReference _weaponHitSFX;
    private EventInstance _weaponHitSFXInstance;

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
    private void GetGlobalParameterID( string parameterName, out PARAMETER_ID parameterID)
    {
        // Get the global parameter value by ID
        RuntimeManager.StudioSystem.getParameterDescriptionByName(parameterName, out PARAMETER_DESCRIPTION parameterDescription);
        parameterID = parameterDescription.id;
    }
    
    private void SetGlobalParameterID(PARAMETER_ID parameterID, float desiredParameterValue)
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

    public void PlayWeaponHitSFX(Component sender, object obj)
    {
        _weaponHitSFXInstance = RuntimeManager.CreateInstance(_weaponHitSFX);
        GetParameterID(_weaponHitSFXInstance, "WeaponSurfaceHit", out _weaponHitSurfaceID);
        GetGlobalParameterID("AttacksStrength", out _attacksStrengthID);
        GetGlobalParameterID("CurrentWeapon", out _currentWeaponID);
        if (_attackEventArgs == null)
        {
            _attackEventArgs = obj as AttackEventArgs;
        }
        if (_aimingEventArgs == null)
        {
            _aimingEventArgs = obj as AimingOutputArgs;
        }
        SetGlobalParameterID(_attacksStrengthID, _attackEventArgs.AttackPower);
        SetGlobalParameterID(_currentWeaponID, 2.0f);
        SetParameterByID(_weaponHitSFXInstance, _weaponHitSurfaceID, _weaponHitSurfaceIDValue);
        _weaponHitSFXInstance.start();
        _weaponSwooshSFXInstance.release();
    }

    public void PlayWooshSFX(Component sender, object obj)
    {
        _weaponSwooshSFXInstance = RuntimeManager.CreateInstance(_weaponSwooshSFX);
        GetGlobalParameterID("AttackStrength", out _attacksStrengthID);
        GetGlobalParameterID("CurrentWeapon", out _currentWeaponID);
        if (_attackEventArgs == null)
        {
            _attackEventArgs = obj as AttackEventArgs;
        }
        if (_aimingEventArgs == null)
        {
            _aimingEventArgs = obj as AimingOutputArgs;
        }

        if (_aimingEventArgs.AttackSignal != AttackSignal.Idle)
        {
            SetGlobalParameterID(_attacksStrengthID, _aimingEventArgs.Speed);
            SetGlobalParameterID(_currentWeaponID, 1.0f);
            _weaponSwooshSFXInstance.start();
            _weaponSwooshSFXInstance.release();
        }
    }
}