using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] private GameEvent _succesfullParryEvent;
    [SerializeField] private GameEvent _onFailedParryEvent;

    public void ParryMovement(Component sender, object obj)
    {
        if (sender.gameObject != gameObject)
            return;
    }

    public void CheckParry(Component sender, object obj)
    {
        if (sender.gameObject == gameObject)
            return;

        AttackEventArgs args = obj as AttackEventArgs;
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
