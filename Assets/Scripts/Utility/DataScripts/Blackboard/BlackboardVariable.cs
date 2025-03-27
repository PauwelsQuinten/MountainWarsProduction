using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "BlackboardVariable", menuName = "DataScripts / Blackboard Variable")]
public class BlackboardVariable : ScriptableObject
{
    public event EventHandler<BlackboardEventArgs> ValueChanged;


    private AttackState _state;
    public AttackState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.Behaviour });
            }
        }
    }

    private float _stamina;
    public float Stamina
    {
        get => _stamina;
        set
        {
            if (_stamina != value)
            {
                _stamina = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.Stamina});
            }
        }
    }
    private float _health;
    public float Health
    {
        get => _health;
        set
        {
            if (_health != value)
            {
                _health = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.Health });
            }
        }
    }
    private float _rHEquipmentHealth;
    public float RHEquipmentHealth
    {
        get => _rHEquipmentHealth;
        set
        {
            if (_rHEquipmentHealth != value)
            {
                _rHEquipmentHealth = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.RHEquipment });
            }
        }
    }
    private float _lHEquipmentHealth;
    public float LHEquipmentHealth
    {
        get => _lHEquipmentHealth;
        set
        {
            if (_lHEquipmentHealth != value)
            {
                _lHEquipmentHealth = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.LHEquipment });
            }
        }
    }

    private GameObject _target;
    public GameObject Target
    {
        get => _target;
        set
        {
            if (_target != value)
            {
                _target = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.Target });
            }
        }
    }

    private AttackState _targetState;
    public AttackState TargetState
    {
        get => _targetState;
        set
        {
            if (_targetState != value)
            {
                _targetState = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.TargetBehaviour });
            }
        }
    }

    private float _targetStamina;
    public float TargetStamina
    {
        get => _targetStamina;
        set
        {
            if (_targetStamina != value)
            {
                _targetStamina = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.TargetStamina });
            }
        }
    }

    private float _targetHealth;
    public float TargetHealth
    {
        get => _targetHealth;
        set
        {
            if (_targetHealth != value)
            {
                _targetHealth = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.TargetHealth });
            }
        }
    }

    private float _targetRHEquipmentHealth;
    public float TargetRHEquipmentHealth
    {
        get => _targetRHEquipmentHealth;
        set
        {
            if (_targetRHEquipmentHealth != value)
            {
                _targetRHEquipmentHealth = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.TargetRHEquipment });
            }
        }
    }

    private float _targetLHEquipmentHealth;
    public float TargetLHEquipmentHealth
    {
        get => _targetLHEquipmentHealth;
        set
        {
            if (_targetLHEquipmentHealth != value)
            {
                _targetLHEquipmentHealth = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.TargetLHEquipment });
            }
        }
    }
    
    private float _targetWeaponRange;
    public float TargetWeaponRange
    {
        get => _targetWeaponRange;
        set
        {
            if (_targetWeaponRange != value)
            {
                _targetWeaponRange = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.TargetWeaponRange });
            }
        }
    }
    
    private float _weaponRange;
    public float WeaponRange
    {
        get => _weaponRange;
        set
        {
            if (_weaponRange != value)
            {
                _weaponRange = value;
                ValueChanged?.Invoke(this, new BlackboardEventArgs { ThisChanged = BlackboardEventArgs.WhatChanged.WeaponRange });
            }
        }
    }



}