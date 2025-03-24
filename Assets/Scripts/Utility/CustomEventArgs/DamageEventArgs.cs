using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnityEngine 
{
    public class DamageEventArgs : EventArgs
    {
        public List<Bodyparts> HitParts = new List<Bodyparts>();
        public float AttackPower;
    }
}