using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    [HideInInspector]
    public float CurrentStamina;

    [Header("Regen")]
    [SerializeField]
    private float _regenSpeed;

    private bool _canRegen;
    private void Update()
    {
        if(_canRegen) RegenStamina();
    }

    private void RegenStamina()
    {

    }
    public void LoseStamina(Component sender, object obj)
    {
        if (sender.gameObject != gameObject) return;

        float? staminaLos = obj as float?;
        if (staminaLos == null) return;

        if(CurrentStamina > staminaLos) CurrentStamina -= (float)staminaLos;
    }
}
