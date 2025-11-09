using UnityEngine;

public class Grabber : MonoBehaviour
{
    GameObject playerEquippedWeapons;


    void Start()
    {
        playerEquippedWeapons = this.transform.Find("EquippedWeapons").gameObject;
    }

    public void GrabObject(GameObject obj)
    {
        obj.transform.SetParent(playerEquippedWeapons.transform);
        this.GetComponent<OrbitingWeaponsAim>().CacheWeapons();
    }
}
