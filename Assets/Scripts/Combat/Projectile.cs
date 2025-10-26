using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(DamageOnContact))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public Gun ShootingGun;
    public float speed = 20f;
    public float radius = 0.1f;
    public LayerMask hitMask = ~0; // which layers can be hit (exclude Projectile layer)
    public float lifetime = 10f;

    private DamageOnContact DamageOnContact;
    private Vector3 lastPos;
    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        DamageOnContact = GetComponent<DamageOnContact>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // Kinematic + Trigger
        rb.isKinematic = true;
        if (col) col.isTrigger = true;
    }

    private void OnEnable()
    {
        lastPos = transform.position;
        if (lifetime > 0f) Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        Vector3 dir = transform.forward;
        float dist = speed * Time.deltaTime;

        if (Physics.SphereCast(lastPos, radius, dir, out RaycastHit hit, dist, hitMask, QueryTriggerInteraction.Ignore))
        {
            transform.position = hit.point;

            Vector3 contactNormal = hit.normal;
            DamageOnContact.HandleHit(hit.collider.gameObject, contactNormal);

            lastPos = transform.position;
            return;
        }

        transform.position = lastPos + dir * dist;
        lastPos = transform.position;
    }
}
