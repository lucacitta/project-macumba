using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    // Enemy stats
    public bool canMove = true;
    public int collisionDamage = 1;
    public float detectionRange = 10f; //to implement later
    public float knockbackForceToPlayer = 5f;
    public float knockbackForceToSelf = 3f;

    // AI
    public float chaseSpeed;
    public float angularSpeed;
    public float acceleration;


    private Rigidbody rb;
    private NavMeshAgent agent;
    private Transform playerTransform;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player?.GetDamage(collisionDamage);
            player?.ApplyKnockback(knockbackForceToPlayer, other);
            ApplyKnockbackToSelf(knockbackForceToSelf, other);
        }
    }

    public void ApplyKnockbackToSelf(float knockbackForceToSelf, Collision collision) {
        Vector3 knockbackDirection = collision.GetContact(0).normal * -1;
        knockbackDirection = Quaternion.Euler(0, 180, 0) * knockbackDirection;
        knockbackDirection.y = 0.2f;
        rb.AddForce(knockbackDirection * knockbackForceToSelf, ForceMode.Impulse);

        canMove = false;
        Invoke("EnableMovement", 0.3f);
}

    private void EnableMovement()
    {
        canMove = true;
        rb.velocity = Vector3.zero;
    }

    void Update()
    {
        if (playerTransform != null && canMove)
        {
            agent.SetDestination(playerTransform.position);
        }
    }
}
