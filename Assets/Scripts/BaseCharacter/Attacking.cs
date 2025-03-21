using System;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    [Header("AttackAngles")]
    [SerializeField]
    private float _minAttackAngle;
    [SerializeField]
    private float _overCommitAngle;

    [Header("Power")]
    [SerializeField]
    private float _basePower;
    [SerializeField]
    private float _chargeSpeed;

    [Header("Attack")]
    [SerializeField]
    private GameEvent _detectHit;


    private float _chargePower;
    private float _attackPower;
    private AttackType _attackType;
    public void Attack(Component sender, object obj)
    {
        if (sender.gameObject != gameObject) return;
        AimingOutputArgs args = obj as AimingOutputArgs;

        if (DidFeint(args.AttackSignal)) return;

        if(args.AttackSignal != AttackSignal.Stab)
        {
            if (!IsAngleBigEnough(args.AngleTravelled)) return;
            if (DidOverCommit(args.AngleTravelled)) return;
        }

        _attackPower = CalculatePower(args);
        _attackType = DetermineAttack(args);

        _detectHit.Raise(this, new AttackEventArgs { AttackType = _attackType, AttackHeight = args.AttackHeight, AttackPower = _attackPower });
    }

    private bool DidFeint(AttackSignal signal)
    {
        if(signal == AttackSignal.Feint) return true;
        return false;
    }

    private bool IsAngleBigEnough(float currentAngle)
    {
        if (currentAngle > _minAttackAngle) return true;
        return false;
    }

    private bool DidOverCommit(float currentAngle)
    {
        if (currentAngle > _overCommitAngle) return true;
        return false;
    }

    private float CalculateChargePower(AttackSignal signal)
    {
        _chargePower += _chargeSpeed * Time.deltaTime;
        return 0;
    }

    private float CalculatePower(AimingOutputArgs aimOutput)
    {
        float swingAngle = aimOutput.AngleTravelled / 100;
        float power = 0;
        if (aimOutput.Speed != 0) power = _basePower / aimOutput.Speed + _chargePower;
        else power = _basePower + _chargePower;
Debug.Log(swingAngle + power);
        return swingAngle + power;
    }

    private AttackType DetermineAttack(AimingOutputArgs aimOutput)
    {
        if(aimOutput.AttackSignal == AttackSignal.Stab) return AttackType.Stab;
        if (aimOutput.Direction == Direction.ToRight) return AttackType.HorizontalSlashToRight;
        return AttackType.HorizontalSlashToLeft;
    }
}
