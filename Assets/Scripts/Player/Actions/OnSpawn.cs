using UnityEngine;

public class OnSpawn : MonoBehaviour
{
    [Header("Setup")]
    public GameObject defaultWeaponPrefab;

    private GameObject playerEquippedWeapons;

    void Start()
    {
        playerEquippedWeapons = this.transform.Find("EquippedWeapons").gameObject;
        if (defaultWeaponPrefab != null)
        {
            Instantiate(defaultWeaponPrefab, transform.position, transform.rotation, playerEquippedWeapons.transform);
            this.GetComponent<OrbitingWeaponsAim>().CacheWeapons();
        }
    }
}
