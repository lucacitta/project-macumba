using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseSpeed = 8f;
    public float jumpForce = 5f;


    private bool isGrounded = true;
    private IKnockbackable kb;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        kb = GetComponent<IKnockbackable>();
    }

    private void FixedUpdate()
    {
        if (kb == null || kb.CanMove)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 vel = new(h * baseSpeed, rb.velocity.y, v * baseSpeed);
            rb.velocity = vel;

            float jump = Input.GetAxis("Jump");
            if (jump > 0 && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f) isGrounded = true;
    }
}
