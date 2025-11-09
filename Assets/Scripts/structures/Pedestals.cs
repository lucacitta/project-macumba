using UnityEngine;

public class Pedestals : MonoBehaviour
{
    [Header("Setup")]
    public GameObject objectToGrabPrefab;
    public GameObject ObjectToGrab { get; private set; }

    void Start()
    {
        var ObjectPosition = transform.position;
        ObjectPosition.y += 1.5f;
        ObjectToGrab = Instantiate(objectToGrabPrefab, ObjectPosition, transform.rotation, transform);
        Utils.CompensateParentScale(transform, ObjectToGrab);
        ObjectToGrab.transform.localScale *= 3f; //TODO: Change 3D model and remove this line
    }

    void Update()
    {
    }
}
