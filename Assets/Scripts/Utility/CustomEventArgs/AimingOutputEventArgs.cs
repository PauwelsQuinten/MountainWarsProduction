using System;
namespace UnityEngine
{
    public class AimingOutputArgs : EventArgs
    {
        public Direction Direction;
        public AimingInputState AimingInputState;
        public AttackHeight AttackHeight;
        public float Speed;
        public float AngleTravelled;
    }


}