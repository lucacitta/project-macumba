using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    // Component references
    public GameObject bulletPrefab;


    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 30f;
    public float bulletSpeed = 20f;
    private float nextTimeToFire = 0f;

    void FixedUpdate()
    {
        //automatic shooting using fire rate
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 60f / fireRate;
            Shoot();
        }

        void Shoot()
        {
            Quaternion rotation = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, 90, 90);
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 1f, rotation);
            var bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.ShootingGun = this; 
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb?.AddForce((transform.forward + Vector3.left * 0.1f) * bulletSpeed, ForceMode.Impulse);
        }
    }
}