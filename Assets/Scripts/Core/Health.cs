using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth = 100f;

    public bool IsDead => currentHealth <= 0f;

    public UnityEvent<float> OnDamaged;

    private IDie dieInterface;

    private void Awake()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        dieInterface = GetComponent<IDie>();
    }

    public void ApplyDamage(float amount)
    {
        currentHealth -= amount;
        OnDamaged?.Invoke(amount);
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            dieInterface?.OnDied();
        }
    }
}
