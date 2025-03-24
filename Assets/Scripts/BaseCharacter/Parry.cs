using UnityEngine;

public class Parry : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEvent _succesfullParryEvent;
    [SerializeField] private GameEvent _onFailedParryEvent;

    private Direction _swingDirection = Direction.Idle;
    private float _swingAngle = 0f;

    [Header("ParryValues")]
    [SerializeField] private float _minParrySwingAngle = 100f;
    [SerializeField] private float _minParryStabAngle = 60f;

    public void ParryMovement(Component sender, object obj)
    {
        if (sender.gameObject != gameObject)
            return;
        AttackEventArgs args = obj as AttackEventArgs;
        if (args == null) return;

    }

    public void CheckParry(Component sender, object obj)
    {
        if (sender.gameObject == gameObject)
            return;
        AttackEventArgs args = obj as AttackEventArgs;
        if (args == null) return;

        switch (args.AttackType)
        {
            case AttackType.Stab:
                if (_swingAngle >= _minParrySwingAngle)
                {
                    OnSuccesfullParry(args);
                    return;
                }
                break;

            case AttackType.HorizontalSlashToLeft:
                if (_swingDirection == Direction.ToLeft && _swingAngle >= _minParrySwingAngle)
                {
                    OnSuccesfullParry(args);
                    return;
                }
                break;

            case AttackType.HorizontalSlashToRight:
                if (_swingDirection == Direction.ToRight && _swingAngle >= _minParrySwingAngle)
                {
                    OnSuccesfullParry(args);
                    return;
                } 
                break;
        }

        OnFaildedParry(args);
    }




    private void OnSuccesfullParry(AttackEventArgs attackValues)
    {
        _succesfullParryEvent.Raise(this, attackValues);
    }
    
    private void OnFaildedParry(AttackEventArgs attackValues)
    {
        _onFailedParryEvent.Raise(this, attackValues);
    }

}
