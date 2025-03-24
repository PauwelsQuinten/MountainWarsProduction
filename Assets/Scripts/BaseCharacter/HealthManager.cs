using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.GPUSort;

public class HealthManager : MonoBehaviour
{
    [Header("health")]
    [SerializeField]
    private float _maxBaseLimbHealth;

    [Header("Blood")]
    [SerializeField]
    private FloatReference _currentBlood;
    [SerializeField]
    private float _maxBlood;
    [SerializeField]
    private float _bleedOutSpeed;

    [Header("Damage")]
    [SerializeField]
    private float _damageDropOff;

    private float _currentHealth;
    private float _maxHealth;
    private Dictionary<BodyParts, float> _bodyPartHealth;
    private float _bleedOutRate;

    private void Start()
    {
        SetHealth();
    }

    private void Update()
    {
        _currentBlood.variable.value -= _bleedOutRate;
    }
    public void TakeDamage(Component sender, object obj)
    {
        if (sender.gameObject != gameObject) return;

        DamageEventArgs args = obj as DamageEventArgs;
        if (args == null) return;

        LoseHealth(args.AttackPower, args);
    }

    private void SetHealth()
    {
        _bodyPartHealth.Add(BodyParts.Head, _maxBaseLimbHealth * 0.75f);
        _bodyPartHealth.Add(BodyParts.Torso, _maxBaseLimbHealth * 1.5f);
        _bodyPartHealth.Add(BodyParts.LeftArm, _maxBaseLimbHealth);
        _bodyPartHealth.Add(BodyParts.RightArm, _maxBaseLimbHealth);
        _bodyPartHealth.Add(BodyParts.LeftLeg, _maxBaseLimbHealth);
        _bodyPartHealth.Add(BodyParts.RightLeg, _maxBaseLimbHealth);

        foreach(var part in _bodyPartHealth)
        {
            _maxHealth += part.Value;
        }

        _currentHealth = _maxHealth;
    }

    private void LoseHealth(float damage, DamageEventArgs args)
    {
        List<BodyParts> parts = args.HitParts;
        int index = 0;
        int damageTaken = (int)damage;
        foreach (BodyParts part in parts)
        {
            if (_bodyPartHealth[part] > 0)
            {
                damageTaken -= (int)(index * _damageDropOff);
                _bodyPartHealth[part] -= damageTaken;
                _currentHealth -= damage;
                index++;

                if (_bodyPartHealth[part] <= 0)
                {
                    if (part == BodyParts.Head)
                        _currentHealth = 0;
                    else if (part == BodyParts.Torso)
                        _bleedOutRate += _bleedOutSpeed * 1.5f;
                    else
                        _bleedOutRate += _bleedOutSpeed;
                }
            }
            else
            {
                Debug.Log($"{part} has taken too much damage");
            }
        }
    }
}
