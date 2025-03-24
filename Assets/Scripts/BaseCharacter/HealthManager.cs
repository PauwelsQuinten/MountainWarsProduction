using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private FloatReference _health;
    [SerializeField]
    private float _maxHealth;

    private 

    public void TakeDamage(Component sender, object obj)
    {
        if (sender.gameObject != gameObject) return;

        DamageEventArgs args = obj as DamageEventArgs;
        if (args == null) return;

        _health.variable.value -= args.AttackPower;
    }
}
