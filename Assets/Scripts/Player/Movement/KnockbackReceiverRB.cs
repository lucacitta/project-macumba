using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KnockbackReceiverRB : MonoBehaviour, IKnockbackable
{
    [SerializeField] private readonly float dragWhileStunned = 0.6f;

    private Rigidbody rb;
    private float originalDrag;
    private float stunTimer;
    public bool CanMove => stunTimer <= 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.drag;
    }

    private void Update()
    {
        if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f) rb.drag = originalDrag;
        }
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        if (force <= 0f) return;

        direction.y = Mathf.Max(direction.y, 0.2f);
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);

        rb.drag = dragWhileStunned;
        stunTimer = duration;
    }
}
