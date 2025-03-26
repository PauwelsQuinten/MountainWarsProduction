using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class HealthEventArgs : EventArgs
    {
        public float CurrentHealth;
        public float CurrentBlood;
        public float MaxHealth;
        public float MaxBlood;

        public Dictionary<BodyParts, float> BodyPartsHealth;
        public Dictionary<BodyParts, float> MaxBodyPartsHealth;

        public List<BodyParts> DamagedBodyParts = new List<BodyParts>();
    }
}
