using UnityEngine;

public class Blocking : MonoBehaviour
{
    [SerializeField] private GameEvent _blockEvent;
    private Direction _blockDirection;
    private AimingInputState _aimingInputState;

    public void BlockMovement(Component sender, object obj)
    {
        //Check for vallid signal
        if (sender.gameObject != gameObject) return;

        AimingOutputArgs args = obj as AimingOutputArgs;
        if (args == null) return;

        if (args.AttackState == AttackState.Attack || args.AttackState == AttackState.BlockAttack) return;


        //Store Blocking values
        _blockDirection = args.Direction;
        _aimingInputState = args.AimingInputState;
    }

    public void CheckBlock(Component sender, object obj)
    {
        //Check for vallid signal
        if (sender.gameObject == gameObject) return;

        AttackEventArgs args = obj as AttackEventArgs;
        if (args == null) return;


        //Compare attack with current defence
        BlockResult blockResult = BlockResult.Hit;
        switch (args.AttackType)
        {
            case AttackType.Stab:
                if (_blockDirection == Direction.ToCenter)
                    blockResult = BlockResult.FullyBlocked;
                break;

            case AttackType.HorizontalSlashToLeft:
                if (_blockDirection == Direction.ToCenter)
                    blockResult = BlockResult.HalfBlocked;
                else if (_blockDirection == Direction.ToLeft)
                    blockResult = BlockResult.FullyBlocked;
                else
                    blockResult = BlockResult.Hit;
                break;

            case AttackType.HorizontalSlashToRight:
                if (_blockDirection == Direction.ToCenter)
                    blockResult = BlockResult.HalfBlocked;
                else if (_blockDirection == Direction.ToRight)
                    blockResult = BlockResult.FullyBlocked;
                else
                    blockResult = BlockResult.Hit;
                break;

            default:
                break;
        }


        //Send the result as a gameEvent
        DefenceEventArgs defenceEventArgs = new DefenceEventArgs
        {
            BlockResult = blockResult,
            AttackHeight = args.AttackHeight,
            AttackPower = args.AttackPower
        };
        _blockEvent.Raise(sender, defenceEventArgs);

    }
}