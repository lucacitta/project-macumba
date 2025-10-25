using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public Gun ShootingGun;
    public float speed = 20f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetVelocity()
    {
        rb.velocity = transform.forward * speed;
        Quaternion rotation = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, 90, 90); //TODO: Fix model to avoid this hack
        rb.MoveRotation(rotation);
    }
}
