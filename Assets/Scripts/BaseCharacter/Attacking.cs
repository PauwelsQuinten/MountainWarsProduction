using System.Collections.Generic;
using System.Linq;
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
    private float _attackRange;
    [SerializeField]
    private GameEvent _doAttack;

    [Header("Enemy")]
    [SerializeField]
    private LayerMask _characterLayer;


    private float _chargePower;
    private float _attackPower;
    private AttackType _attackType;

    private bool _wasCharging;
    private float _startChargeTime;
    private float _endChargeTime;
    public void Attack(Component sender, object obj)
    {
        if (sender.gameObject != gameObject) return;

        AimingOutputArgs args = obj as AimingOutputArgs;
        if (args == null) return;

        CalculateChargePower(args);

        if (DidFeint(args.AttackSignal)) return;


        if (args.AttackSignal != AttackSignal.Stab || args.AttackSignal != AttackSignal.Swing) return;

        if(args.AttackSignal != AttackSignal.Stab)
        {
            if (!IsAngleBigEnough(args.AngleTravelled)) return;
            if (DidOverCommit(args.AngleTravelled)) return;
        }

        _attackPower = CalculatePower(args);
        _attackType = DetermineAttack(args);

        if (!IsEnemyInRange()) return;
        _doAttack.Raise(this, new AttackEventArgs { AttackType = _attackType, AttackHeight = args.AttackHeight, AttackPower = _attackPower});
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

    private void CalculateChargePower(AimingOutputArgs args)
    {
        if(args.AttackSignal == AttackSignal.Charge && !_wasCharging)
        {
            _wasCharging = true;
            _startChargeTime = Time.time;
        }
        else if (args.AttackSignal != AttackSignal.Charge && _wasCharging)
        {
            _endChargeTime = Time.time;
            _chargePower = _chargeSpeed * (_endChargeTime - _startChargeTime);
            _startChargeTime = 0;
            _endChargeTime = 0;
            _wasCharging = false;
        }
    }

    private float CalculatePower(AimingOutputArgs aimOutput)
    {
        float swingAngle = aimOutput.AngleTravelled / 100;
        float power = 0;
        if (aimOutput.Speed != 0) power = _basePower / aimOutput.Speed + _chargePower;
        else power = _basePower + _chargePower;
        return swingAngle + power;
    }

    private AttackType DetermineAttack(AimingOutputArgs aimOutput)
    {
        if(aimOutput.AttackSignal == AttackSignal.Stab) return AttackType.Stab;
        if (aimOutput.Direction == Direction.ToRight) return AttackType.HorizontalSlashToRight;
        return AttackType.HorizontalSlashToLeft;
    }

    private bool IsEnemyInRange()
    {
        List<Collider2D> enemy = Physics2D.OverlapCircleAll(transform.position, _attackRange * 2).ToList();
        if (enemy == null) return false;
        foreach (Collider2D c in enemy)
        {
            if (((1 << c.gameObject.layer) & _characterLayer) != 0)
            {
                if (c.gameObject == gameObject) continue;
                if(Vector2.Distance(transform.position, c.transform.position) < _attackRange) return true;
            }
        }
            return false;
    }
}
