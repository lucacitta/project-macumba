using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;



public class Player : MonoBehaviour
{
    // Component references
    private Rigidbody rb;
    public List<GameObject> EquippedWeapons;
    private GameObject enemyManager;


    // Health stats
    public float maxHealth = 100f;
    public float health = 100f;


    // Movement stats
    public float baseSpeed = 8f;
    public float actualSpeed = 8f;
    public bool canMove = true;
    public float jumpForce = 5f;
    private bool isGrounded = true;
    private float _origDrag;


    // Attack stats
    private readonly float aimMaxAngle = 90f;
    private readonly float aimSmoothSpeed = 10f;
    private readonly float orbitSpeed = 50f;



    // Basic Unity functions
    void Awake()
    {
        _origDrag = GetComponent<Rigidbody>().drag;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EquippedWeapons = new List<GameObject>();
        for (int i = 0; i < transform.Find("EquippedWeapons").childCount; i++)
        {
            EquippedWeapons.Add(transform.Find("EquippedWeapons").GetChild(i).gameObject);
        }
        enemyManager = GameObject.Find("EnemyManager");
    }

    void FixedUpdate()
    {
        Handle_movement();
        Handle_jump();

        Aim();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5)
        {
            isGrounded = true;
        }

    }


    // Movement functions

    void Handle_movement()
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

    void Handle_jump()
    {
        float jump = Input.GetAxis("Jump");
        if (jump > 0 && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }


    // Damage functions
    public void GetDamage(float damage)
    {
        health -= damage;

        Debug.Log("Player took " + damage + " damage. Current health: " + health);

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


    // Attack functions
    public void Attack()
    {
        Debug.Log("Player attacks!");
    }

    void Aim()
    {
        foreach (var EquippedWeapon in EquippedWeapons)
        {
            AimWeapon(EquippedWeapon);
        }
    }

    void AimWeapon(GameObject EquippedWeapon)
    {
        EquippedWeapon.transform.RotateAround(transform.position, Vector3.up, orbitSpeed * Time.deltaTime);

        Vector3 orbitDirection = (EquippedWeapon.transform.position - transform.position);
        orbitDirection.y = 0f; 
        if (orbitDirection.sqrMagnitude < 0.0001f) return;
        orbitDirection.Normalize(); 

        GameObject closestEnemy = null;
        float closestDist = Mathf.Infinity;

        List<GameObject> enemies = EnemyManager.ActiveEnemies;

        foreach (var enemy in enemies)
        {
            Vector3 toEnemyFromPlayer = enemy.transform.position - transform.position;
            float distPlayer = toEnemyFromPlayer.magnitude;

            if (distPlayer <= EquippedWeapon.GetComponent<Guns>().range && distPlayer < closestDist)
            {
                closestDist = distPlayer;
                closestEnemy = enemy;
            }
        }

        Vector3 targetDirection = orbitDirection;

        if (closestEnemy != null)
        {
            Vector3 directionToClosestEnemy = (closestEnemy.transform.position - transform.position);
            directionToClosestEnemy.y = 0f;
            if (directionToClosestEnemy.sqrMagnitude < 0.0001f)
            {
                targetDirection = orbitDirection; 
            }
            else
            {
                directionToClosestEnemy.Normalize();

                float angleToEnemy = Vector3.SignedAngle(orbitDirection, directionToClosestEnemy, Vector3.up);

                float clampedAngle = Mathf.Clamp(angleToEnemy, -aimMaxAngle, aimMaxAngle);

                targetDirection = Quaternion.AngleAxis(clampedAngle, Vector3.up) * orbitDirection;
            }
        }

        if (targetDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            EquippedWeapon.transform.rotation = Quaternion.Slerp(
                EquippedWeapon.transform.rotation,
                desiredRotation,
                Time.deltaTime * aimSmoothSpeed
            );
        }
    }
}
