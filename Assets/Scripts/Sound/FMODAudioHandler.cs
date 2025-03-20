using System;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

public class FMODAudioHandler : MonoBehaviour
{
    private PARAMETER_ID _surfaceTypeID;
    // Global Parameters

    // Events
    [SerializeField] private EventReference _footstepsSFX;
    private EventInstance _footstepsSFXInstance;
    
    [SerializeField] private EventReference _attackChargeSFX;
     private EventInstance _attackChargeSFXInstance;
     
    [SerializeField] private EventReference _swordSwooshSFX;
    private EventInstance _swordSwooshSFXInstance;
    
    [SerializeField] private EventReference _swordHitSFX;
    private EventInstance _swordHitSFXInstance;
    

    // AimingOutputArgs args = (AimingOutputArgs)obj;
    // float AttackSpeed = args.AngleTravelled;
    public void PlayFootstepsSFX(Component sender, object obj)
    {
        _footstepsSFXInstance = RuntimeManager.CreateInstance(_footstepsSFX);
        _footstepsSFXInstance.start();
    }

    public void PlayAttackChargeSFX(Component sender, object obj)
    {
        _attackChargeSFXInstance = RuntimeManager.CreateInstance(_attackChargeSFX);
        _attackChargeSFXInstance.start();
    }
    
    public void PlaySwordSwooshSFX(Component sender, object obj)
    {
        _swordSwooshSFXInstance = RuntimeManager.CreateInstance(_swordSwooshSFX);
        _swordSwooshSFXInstance.start();
    } 
    public void PlaySwordHitSFX(Component sender, object obj)
    {
        _swordHitSFXInstance = RuntimeManager.CreateInstance(_swordHitSFX);
        _swordHitSFXInstance.start();

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
}