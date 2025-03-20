using System;
using UnityEditor.Experimental;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private MovingInputReference _moveInput;

    [Header("Movement")]
    [SerializeField]
    private float _speed;

    private Rigidbody2D _rb;
    private StateManager _stateManager;
    private Vector2 _movedirection;
    private float _angleInterval = 22.5f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _stateManager = _moveInput.variable.StateManager;
        _moveInput.variable.ValueChanged += MoveInput_ValueChanged;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + (_movedirection * (_speed * _moveInput.variable.SpeedMultiplier)) * Time.deltaTime);
    }

    private void MoveInput_ValueChanged(object sender, EventArgs e)
    {
        _movedirection = _moveInput.Value;
        UpdateOrientation();
    }

    private void UpdateOrientation()
    {
        if (_movedirection == Vector2.zero) return;
        float moveInputAngle = Mathf.Atan2(_movedirection.y, _movedirection.x);
        moveInputAngle = moveInputAngle * Mathf.Rad2Deg;

        if (moveInputAngle > 0 - _angleInterval && moveInputAngle < 0 + _angleInterval)
            _moveInput.variable.StateManager.Orientation = Orientation.East;
        else if (moveInputAngle > 45 - _angleInterval && moveInputAngle < 45 + _angleInterval)
        _moveInput.variable.StateManager.Orientation = Orientation.NorthEast;
        else if (moveInputAngle > 90 - _angleInterval && moveInputAngle < 90 + _angleInterval)
        _moveInput.variable.StateManager.Orientation = Orientation.North;
        else if (moveInputAngle > 135 - _angleInterval && moveInputAngle < 135 + _angleInterval)
            _moveInput.variable.StateManager.Orientation = Orientation.NorthWest;
        else if (moveInputAngle > 180 - _angleInterval && moveInputAngle < 180 || moveInputAngle < -180 + _angleInterval && moveInputAngle > -180)
            _moveInput.variable.StateManager.Orientation = Orientation.West;
        else if (moveInputAngle > -135 - _angleInterval && moveInputAngle < -135 + _angleInterval)
            _moveInput.variable.StateManager.Orientation = Orientation.SouthWest;
        else if (moveInputAngle > -90 - _angleInterval && moveInputAngle < -90 + _angleInterval)
            _moveInput.variable.StateManager.Orientation = Orientation.South;
        else if (moveInputAngle > -45 - _angleInterval && moveInputAngle < -45 + _angleInterval)
            _moveInput.variable.StateManager.Orientation = Orientation.SouthEast;
    }
}
