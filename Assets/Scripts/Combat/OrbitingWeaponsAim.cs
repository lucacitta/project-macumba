using System.Collections.Generic;
using UnityEngine;

public class OrbitingWeaponsAim : MonoBehaviour
{
    [Header("Setup")]
    public Transform weaponsRoot;
    public Transform owner;

    [Header("Orbit")]
    public float orbitRadius = 3f;
    public float orbitSpeed = 0f;
    public bool orbitClockwise = false;

    [Header("Aiming")]
    public float aimMaxAngle = 90f;
    public float aimSmoothSpeed = 10f;

    private readonly List<Transform> _weapons = new();
    private readonly List<Gun> _guns = new();

    private void Awake()
    {
        if (owner == null) owner = transform;
    }

    public void CacheWeapons()
    {
        _weapons.Clear();
        _guns.Clear();

        if (weaponsRoot == null) return;
        for (int i = 0; i < weaponsRoot.childCount; i++)
        {
            var w = weaponsRoot.GetChild(i);
            _weapons.Add(w);
            _guns.Add(w.GetComponent<Gun>());
        }
    }

    private void Update()
    {
        if (weaponsRoot == null || owner == null) return;

        //Orbit all weapons around the owner
        float dir = orbitClockwise ? -1f : 1f;
        float angleStep = 360f / _weapons.Count;
        List<float> currentAngles = new (_weapons.Count);
        for (int i = 0; i < _weapons.Count; i++)
        {
            var w = _weapons[i];
            float targetAngle = i * angleStep + (Time.time * orbitSpeed * dir);
            Vector3 offset = new(Mathf.Cos(targetAngle * Mathf.Deg2Rad), 0f, Mathf.Sin(targetAngle * Mathf.Deg2Rad));
            w.position = owner.position + offset * orbitRadius;
            currentAngles.Add(targetAngle);
        }
        foreach (var w in _weapons)
        {
            w.RotateAround(owner.position, Vector3.up, dir * orbitSpeed * Time.deltaTime);
        }

        //For each weapon, pick closest enemy in range and aim/auto-fire
        var enemies = EnemyManager.ActiveEnemies;
        if (enemies == null || enemies.Count == 0) return;

        for (int i = 0; i < _weapons.Count; i++)
        {
            var w = _weapons[i];
            var gun = _guns[i];

            // Compute "orbit direction" (from owner to weapon on the XZ plane)
            Vector3 orbitDir = w.position - owner.position; 
            orbitDir.y = 0f;
            if (orbitDir.sqrMagnitude < 0.0001f) continue;
            orbitDir.Normalize();

            // Find closest enemy within THIS GUN range
            Transform nearest = null;
            float bestDist = Mathf.Infinity;
            float range = (gun != null) ? gun.Range : Mathf.Infinity;

            foreach (var enemyGo in enemies)
            {
                if (enemyGo == null) continue;
                Vector3 toEnemy = enemyGo.transform.position - owner.position;
                float dist = toEnemy.magnitude;
                if (dist <= range && dist < bestDist)
                {
                    bestDist = dist;
                    nearest = enemyGo.transform;
                }
            }

            // Decide target direction
            Vector3 targetDir = orbitDir;

            if (nearest != null)
            {
                Vector3 toEnemy = nearest.position - owner.position;
                toEnemy.y = 0f;
                if (toEnemy.sqrMagnitude >= 0.0001f)
                {
                    toEnemy.Normalize();
                    float angle = Vector3.SignedAngle(orbitDir, toEnemy, Vector3.up);
                    float clamped = Mathf.Clamp(angle, -aimMaxAngle, aimMaxAngle);
                    targetDir = Quaternion.AngleAxis(clamped, Vector3.up) * orbitDir;
                }
            }

            // Smooth-rotate the weapon to target direction
            if (targetDir.sqrMagnitude > 0.0001f)
            {
                Quaternion desired = Quaternion.LookRotation(targetDir, Vector3.up);
                w.rotation = Quaternion.Slerp(w.rotation, desired, Time.deltaTime * aimSmoothSpeed);
            }

            // Auto-fire if aligned and target exists
            if (nearest != null && gun != null && gun.enabled)
            {
                gun.TryFireAt(nearest.position);
            }
        }
    }
}
