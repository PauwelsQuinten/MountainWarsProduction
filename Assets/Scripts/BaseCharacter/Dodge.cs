using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dodge : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private MovingInputReference _moveInput;

    [Header("Stamina")]
    [SerializeField]
    private FloatReference _staminaCost;
    [SerializeField]
    private GameEvent _loseStamina;

    [Header("DodgeStats")]
    [SerializeField]
    private float _dashSpeed;
    [SerializeField]
    private float _dashDistance;
    [SerializeField]
    private float _cooldown;

    private Rigidbody _rb;
    private bool _canRun = true;

    private Coroutine _dodge;
    private Coroutine _resetCanRun;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void ActivateDodge(Component sender, object obj)
    {
        if (sender.gameObject != gameObject) return;
        if (!_canRun) return;

        _loseStamina.Raise(this, new StaminaEventArgs { StaminaCost = _staminaCost.value });
        Vector3 direction = new Vector3(_moveInput.Value.x, _moveInput.Value.y, 0);

        if (_dodge != null) StopCoroutine(_dodge);
        _dodge = StartCoroutine(DoDodge(transform.position, direction, _dashDistance));
    }

    private IEnumerator DoDodge(Vector3 startPos, Vector3 direction, float distance)
    {
        _canRun = false;
        GetComponent<CharacterMovement>().enabled = false;
        while (Vector3.Distance(startPos, transform.position) < distance)
        {
            transform.position += _dashSpeed * Time.deltaTime * direction;
            yield return null;
        }
        transform.position = startPos + (direction * (_dashDistance + 1));
        GetComponent<CharacterMovement>().enabled = true;
        if (_resetCanRun != null) StopCoroutine(_resetCanRun);
        _resetCanRun = StartCoroutine(ResetCanRun());
    }

    private IEnumerator ResetCanRun()
    {
        yield return new WaitForSeconds(_cooldown);
        _canRun = true;
    }
}
