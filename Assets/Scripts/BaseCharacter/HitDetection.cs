using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    private List<Bodyparts> _hitParts = new List<Bodyparts>();

    public void DetectHit(Component sender, object obj)
    {
        if (sender.gameObject == gameObject) return;

        AttackEventArgs args = obj as AttackEventArgs;
        if (args == null) return;

        _hitParts = GetDamagedParts(args);
    }

    private List<Bodyparts> GetDamagedParts(AttackEventArgs args)
    {
        _hitParts.Clear();
        List<Bodyparts> parts = new List<Bodyparts>();
        switch (args.AttackType) 
        {
            case AttackType.Stab:

                switch (args.AttackHeight) 
                {
                    case AttackHeight.Head:
                        parts.Add(Bodyparts.Head);
                        break;
                    case AttackHeight.Torso:
                        parts.Add(Bodyparts.Torso);
                        break;
                }
                break;
            case AttackType.HorizontalSlashToLeft:

                switch (args.AttackHeight)
                {
                    case AttackHeight.Head:
                        parts.Add(Bodyparts.Head);
                        break;
                    case AttackHeight.Torso:
                        //TODO shield gets hit animation
                        break;
                }
                break;
            case AttackType.HorizontalSlashToRight:

                switch (args.AttackHeight)
                {
                    case AttackHeight.Head:
                        parts.Add(Bodyparts.Head);
                        break;
                    case AttackHeight.Torso:
                        parts.Add(Bodyparts.LeftArm);
                        break;
                }
                break;
        }
        return parts;
    }
}
