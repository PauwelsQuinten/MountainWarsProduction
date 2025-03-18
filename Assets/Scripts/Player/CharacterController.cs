using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [Header("State Manager")]
    [SerializeField]
    private StateManager _stateManager;

    [Header("Variables")]
    [SerializeField]
    private AimingInputReference _AimInputRef;

    public void ProcessAimInput(InputAction.CallbackContext ctx)
    {
        _AimInputRef.variable.value = ctx.ReadValue<Vector2>();
        _AimInputRef.variable.State = _stateManager.AttackState;
    }
}
