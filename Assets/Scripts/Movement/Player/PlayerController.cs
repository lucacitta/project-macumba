using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseSpeed = 8f;
    public float jumpForce = 5f;

    [Header("Aim / Orbiting Weapons")]
    public Transform equippedWeaponsRoot;
    public float orbitSpeed = 50f;
    public float aimMaxAngle = 90f;
    public float aimSmoothSpeed = 10f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private IKnockbackable kb;
    private readonly List<Transform> equippedWeapons = new();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        kb = GetComponent<IKnockbackable>();
        CacheEquippedWeapons();
    }

    private void CacheEquippedWeapons()
    {
        if (equippedWeaponsRoot == null) return;
        equippedWeapons.Clear();
        for (int i = 0; i < equippedWeaponsRoot.childCount; i++)
            equippedWeapons.Add(equippedWeaponsRoot.GetChild(i));
    }

    private void FixedUpdate()
    {
        // Movement
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

        Aim();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f) isGrounded = true;
    }

    private void Aim()
    {
        if (equippedWeapons.Count == 0) return;

        //Orbit around the player
        foreach (var w in equippedWeapons)
        {
            w.RotateAround(transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }

        //Pick closest enemy and aim
        GameObject closestEnemy = null;
        float closestDist = Mathf.Infinity;
        var enemies = EnemyManager.ActiveEnemies;

        // Use forward of each weapon as "orbitDirection"
        foreach (var enemy in enemies)
        {
            float dist = (enemy.transform.position - transform.position).magnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }

        foreach (var w in equippedWeapons)
        {
            Vector3 orbitDir = w.position - transform.position;
            orbitDir.y = 0f;
            if (orbitDir.sqrMagnitude < 0.0001f) continue;
            orbitDir.Normalize();

            Vector3 targetDir = orbitDir;

            if (closestEnemy != null)
            {
                Vector3 toEnemy = closestEnemy.transform.position - transform.position;
                toEnemy.y = 0f;
                if (toEnemy.sqrMagnitude >= 0.0001f)
                {
                    toEnemy.Normalize();
                    float angle = Vector3.SignedAngle(orbitDir, toEnemy, Vector3.up);
                    float clamped = Mathf.Clamp(angle, -aimMaxAngle, aimMaxAngle);
                    targetDir = Quaternion.AngleAxis(clamped, Vector3.up) * orbitDir;
                }
            }

            if (targetDir.sqrMagnitude > 0.0001f)
            {
                Quaternion desired = Quaternion.LookRotation(targetDir, Vector3.up);
                w.rotation = Quaternion.Slerp(w.rotation, desired, Time.deltaTime * aimSmoothSpeed);
            }
        }
    }
}
