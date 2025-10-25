using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public float damage = 1f;
    public float knockbackForceToOther = 5f;
    public float knockbackForceToSelf = 3f;
    public float knockbackDurationToSelf = 0.3f;
    public float knockbackDurationToOther = 0.3f;
    public bool destroyAfterContact = true;

    private void OnCollisionEnter(Collision other)
    {
        var otherDmg = other.gameObject.GetComponent<IDamageable>();
        var kbOther = other.gameObject.GetComponent<IKnockbackable>();
        var kbSelf  = GetComponent<IKnockbackable>();

        if (otherDmg == null && kbOther == null) return;

        // Deal damage
        otherDmg?.ApplyDamage(damage);

        // Knockback directions from contact normal
        Vector3 toOther = -other.GetContact(0).normal;
        Vector3 toSelf  =  other.GetContact(0).normal;

        kbOther?.ApplyKnockback(toOther, knockbackForceToOther, knockbackDurationToOther);
        kbSelf?.ApplyKnockback(toSelf, knockbackForceToSelf, knockbackDurationToSelf);
        if (destroyAfterContact) Destroy(gameObject);
    }
}
