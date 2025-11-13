using UnityEngine;

public class GrabOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var grabber = collision.gameObject.GetComponent<Grabber>();
        var ObjectToGrab = this.transform.GetChild(1).gameObject;

        grabber?.GrabObject(ObjectToGrab);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
