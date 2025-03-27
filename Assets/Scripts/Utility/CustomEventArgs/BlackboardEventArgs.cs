using System;

namespace UnityEngine
{
    public class BlackboardEventArgs : EventArgs
    {
        
        public WhatChanged ThisChanged;
        public enum WhatChanged
        {
            Stamina,//Set in staminaManager
            Health,//Set in healthManager
            RHEquipment,//set in EquipmentManager
            LHEquipment,//set in EquipmentManager

            Target,//Set in statemanager
            TargetStamina,//Set in staminaManager
            TargetHealth,//Set in healthManager
            TargetRHEquipment,//set in EquipmentManager
            TargetLHEquipment,//set in EquipmentManager
        }

    }
}
