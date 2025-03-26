using UnityEngine;

public class Blocking : MonoBehaviour
{
    [SerializeField] private GameEvent _succesfullBlockevent;
    [SerializeField] private GameEvent _succesfullHitEvent;
    private Direction _blockDirection;
    private AimingInputState _aimingInputState;
    private BlockMedium _blockMedium = BlockMedium.Shield;
    private bool _holdBlock = false;

    public void BlockMovement(Component sender, object obj)
    {
        //Check for vallid signal
        if (sender.gameObject != gameObject) return;
        AimingOutputArgs args = obj as AimingOutputArgs;
        if (args == null) return;


        //only set movement when using a Blocking input
        if (args.AttackState == AttackState.BlockAttack || 
            args.AttackState == AttackState.ShieldDefence || 
            args.AttackState == AttackState.SwordDefence || 
            args.AttackState == AttackState.Knock 
            )
            _blockMedium = Blocking.GetBlockMedium(args);
        else
            return;

        Debug.Log($"package to Block State = {args.AttackState}, hold: {args.AimingInputState}, {_blockMedium}");

        //When Shield is locked and state hasnt changed, keep previous values
        if (_holdBlock && args.AttackState == AttackState.BlockAttack)
            return;
        else
            _holdBlock = false;

        //Store Blocking values
        _blockDirection = args.BlockDirection;
        _aimingInputState = args.AimingInputState;

        if (args.AttackState == AttackState.BlockAttack)
        {
            _holdBlock = true;
        }

        //Debug.Log($"BlockEvent : {_blockDirection}");
        //Debug.Log($"BlockEvent : {_aimingInputState}");
        
    }

    public void CheckBlock(Component sender, object obj)
    {
        //Check for vallid signal
        if (sender.gameObject != gameObject) return;

        AttackEventArgs args = obj as AttackEventArgs;
        if (args == null) return;


        //Compare attack with current defence
        BlockResult blockResult; 
        switch(_blockMedium)
        {
            case BlockMedium.Shield:
                blockResult = UsingShield(args);
                break;

            case BlockMedium.Sword:
                blockResult = UsingSword(args);
                break;

            case BlockMedium.Nothing:
            default:
                blockResult = BlockResult.Hit;
                break;

               
        }



        //Send the result as a gameEvent
        DefenceEventArgs defenceEventArgs = new DefenceEventArgs
        {
            BlockResult = blockResult,
            AttackHeight = args.AttackHeight,
            AttackPower = args.AttackPower,
            BlockMedium = _blockMedium
        };
        Debug.Log($"{blockResult}");


        if (blockResult == BlockResult.Hit)
            _succesfullHitEvent.Raise(this, args);
        else
            _succesfullBlockevent.Raise(this, defenceEventArgs);

    }


    static public BlockMedium GetBlockMedium(AimingOutputArgs args)
    {
        if (args.AttackState == AttackState.SwordDefence)
        {
            if (args.EquipmentManager.HasEquipmentInHand(true))
                return BlockMedium.Sword;
            else if (args.EquipmentManager.HasEquipmentInHand(false))
                return BlockMedium.Shield;
            return BlockMedium.Nothing;
        }

        else if (args.AttackState == AttackState.ShieldDefence || args.AttackState == AttackState.BlockAttack)
        {
             if (args.EquipmentManager.HasEquipmentInHand(false))
                return BlockMedium.Shield;
             else if (args.EquipmentManager.HasEquipmentInHand(true))
                return BlockMedium.Sword;
            return BlockMedium.Nothing;
        }

        else if (args.AttackState == AttackState.Knock)
            return BlockMedium.Nothing;

        Debug.LogError("No Blockstate, so what are you doing here");
        return BlockMedium.Nothing;
    }


    private BlockResult UsingSword(AttackEventArgs args)
    {
        BlockResult blockResult = BlockResult.Hit;
        if (_aimingInputState != AimingInputState.Hold)
            _blockDirection = Direction.Idle;

        switch (args.AttackType)
        {
            case AttackType.Stab:
                if (_blockDirection == Direction.ToCenter)
                    blockResult = BlockResult.SwordHalfBlock;
                else
                    blockResult = BlockResult.Hit;
                break;

            case AttackType.HorizontalSlashToLeft:
                if (_blockDirection == Direction.ToLeft)
                    blockResult = BlockResult.FullyBlocked;
                else
                    blockResult = BlockResult.HalfBlocked;
                break;

            case AttackType.HorizontalSlashToRight:
                if (_blockDirection == Direction.ToCenter)
                    blockResult = BlockResult.Hit;
                else if (_blockDirection == Direction.ToRight)
                    blockResult = BlockResult.SwordBlock;
                else
                    blockResult = BlockResult.Hit;
                break;

            default:
                break;
        }

        return blockResult;
    }

    private BlockResult UsingShield(AttackEventArgs args)
    {
        BlockResult blockResult = BlockResult.Hit;
        if (_aimingInputState != AimingInputState.Hold)
            _blockDirection = Direction.Idle;

        switch (args.AttackType)
        {
            case AttackType.Stab:
                if (_blockDirection == Direction.Idle)
                    blockResult = BlockResult.Hit;
                else if (_blockDirection == Direction.ToCenter)
                    blockResult = BlockResult.FullyBlocked;
                break;

            case AttackType.HorizontalSlashToLeft:
                if (_blockDirection == Direction.ToLeft)
                    blockResult = BlockResult.FullyBlocked;
                else if (_blockDirection == Direction.ToRight)
                    blockResult = BlockResult.Hit;
                else
                    blockResult = BlockResult.HalfBlocked;
                break;

            case AttackType.HorizontalSlashToRight:
                if (_blockDirection == Direction.Idle)
                    blockResult = BlockResult.Hit;
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

        return blockResult;
    }

}