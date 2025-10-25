using UnityEngine;

[CreateAssetMenu(fileName = "GunDefinition", menuName = "Game/Gun Definition", order = 0)]
public class GunDefinition : ScriptableObject
{
    [Header("Identity")]
    public string gunId = "basic_gun";

    [Header("Stats")]
    public float range = 12f;
    public float damage = 5f;
    public float fireRate = 4f;         // shots per second
    public float projectileSpeed = 24f;

    [Header("Firing")]
    public GameObject projectilePrefab; // must have DamageOnContact + Projectile + SelfDestructAfterTime
    public float spreadDegrees = 0f;    // random yaw spread (0 = perfectly accurate)

    [Header("Targeting")]
    public float fireAngleThreshold = 8f; // how aligned the muzzle must be (degrees)
}
