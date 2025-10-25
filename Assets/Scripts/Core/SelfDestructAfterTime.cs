using UnityEngine;

public class SelfDestructAfterTime : MonoBehaviour
{
    public float lifetime = 10f;
    private void Awake()
    {
        Destroy(gameObject, lifetime);
    }
}
