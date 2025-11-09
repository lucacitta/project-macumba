using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseSpeed = 15f;
    public float jumpForce = 20f;


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
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
            Vector3 vel = (right * h + forward * v).normalized * baseSpeed;
            vel.y = rb.velocity.y;
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
