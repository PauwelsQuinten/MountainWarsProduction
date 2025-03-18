using UnityEngine;

public enum AimingInputState
{
    Idle, Moving, Hold, Reset
}
public class Aiming : MonoBehaviour
{
    [SerializeField] private AimingInputReference _vec2AimingInput;
    private Vector2 _previousDirection = Vector2.zero;
    private AimingInputState _enmAimingInput = AimingInputState.Idle;
    void Start()
    {
        _enmAimingInput = AimingInputState.Idle;
        _vec2AimingInput.variable.ValueChanged += Variable_ValueChanged;
    }

    private void Variable_ValueChanged(object sender, AimInputEventArgs e)
    {
        switch(e.ThisChanged)
        {
            case AimInputEventArgs.WhatChaned.Input:
                OnInputChanged();
                break;
            case AimInputEventArgs.WhatChaned.State:
                OnStateChanged();
                break;
            default:
                break;

        }
    }

    private void OnInputChanged()
    {

    }
     private void OnStateChanged()
    {

    }

    
}
