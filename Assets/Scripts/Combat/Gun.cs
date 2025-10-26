using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Definition & Setup")]
    public GunDefinition definition;
    public Transform muzzle;
    public GameObject owner;

    private float _cooldown;

    private void Awake()
    {
        muzzle ??= transform;
        owner ??= transform.root.gameObject;
    }

    private void Update()
    {
        if (_cooldown > 0f)
            _cooldown -= Time.deltaTime;
    }

    public float Range => definition != null ? definition.range : 0f;

    public void TryFireAt(Vector3 targetWorldPos)
    {
        if (definition == null || definition.projectilePrefab == null) return;
        if (_cooldown > 0f) return;

        Vector3 toTarget = (targetWorldPos - muzzle.position);
        toTarget.y = 0f; //FEATURE: Maybe consider full 3D aiming later
        if (toTarget.sqrMagnitude <= 0.0001f) return;

        float angle = Vector3.Angle(muzzle.forward, toTarget.normalized);
        if (angle > definition.fireAngleThreshold) return;

        FireInternal(toTarget.normalized);
    }

    private void FireInternal(Vector3 dir)
    {
        if (definition.spreadDegrees > 0f)
        {
            float yaw = Random.Range(-definition.spreadDegrees * 0.5f, definition.spreadDegrees * 0.5f);
            dir = Quaternion.Euler(0f, yaw, 0f) * dir;
        }

        GameObject newProjectile = Instantiate(definition.projectilePrefab, muzzle.position, Quaternion.LookRotation(dir, Vector3.up));
        //FEATURE: We can have an item that made projectiles "follow the aim" after being fired. i think we can use parenting for that.

        var proj = newProjectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.speed = definition.projectileSpeed;
            proj.ShootingGun = this;
        }

        var projDamageOnContact = newProjectile.GetComponent<DamageOnContact>();
        if (projDamageOnContact != null)
        {
            projDamageOnContact.damage = definition.damage;
        }

        _cooldown = (definition.fireRate > 0f) ? (1f / definition.fireRate) : 0f;
    }
}
