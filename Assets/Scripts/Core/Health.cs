using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth = 100f;

    public UnityEvent<float> OnDamaged;
    public UnityEvent OnDied;

    private void Awake()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        OnDamaged?.Invoke(amount);
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            OnDied?.Invoke();
            // TODO: disable actor / play death animation / notify manager
        }
    }
}
