using System;
using UnityEditor.Experimental;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private MovingInputReference _moveInput;

    private Rigidbody2D _rb;
    private StateManager _stateManager;
    private Vector2 _movedirection;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _stateManager = _moveInput.variable.StateManager;
        _moveInput.variable.ValueChanged += MoveInput_ValueChanged;
    }

    private void MoveInput_ValueChanged(object sender, EventArgs e)
    {
        _movedirection = _moveInput.Value;

        _rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + (_movedirection * 5) * Time.deltaTime);
    }
}
