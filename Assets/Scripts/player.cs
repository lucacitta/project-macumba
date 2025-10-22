using System.Collections;

using UnityEngine.UI;
using UnityEngine;



public class Player : MonoBehaviour
{
    public int maxHealth = 3;
    public int health = 3;

    public float baseSpeed = 8f;
    public float actualSpeed = 8f;
    public bool canMove = true;

    public float jumpForce = 5f;
    private bool isGrounded = true;
    private Rigidbody rb;

    private float _origDrag;

    void Awake()
    {
        _origDrag = GetComponent<Rigidbody>().drag;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void handle_movement()
    {
        if (!canMove) return;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            Vector3 movement = new Vector3(
                moveHorizontal * actualSpeed,
                rb.velocity.y,
                moveVertical * actualSpeed
            );
            rb.velocity = movement;
        }
    }

    void handle_jump()
    {
        float jump = Input.GetAxis("Jump");
        if (jump > 0 && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void Update()
    {
        handle_movement();
        handle_jump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5)
        {
            isGrounded = true;
        }

    }

    public void GetDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("Player has died.");
        }
    }

    private void EnableMovement()
    {
        canMove = true;
        rb.drag = _origDrag;
    }

    public void ApplyKnockback(float knockbackForce, Collision collision)
    {
        if (knockbackForce <= 0) return;

        Vector3 dir = -collision.GetContact(0).normal;
        dir.y = 0.2f;
        rb.AddForce(dir * knockbackForce, ForceMode.Impulse);

        rb.drag = 0.6f;
        canMove = false;
        Invoke("EnableMovement", 0.3f);
    }
}