using UnityEngine;

public class PhysicsBootstrap : MonoBehaviour
{
    private void Awake()
    {
        int proj = LayerMask.NameToLayer("Projectile");
        if (proj >= 0)
        {
            Physics.IgnoreLayerCollision(proj, proj, true);
        }
    }
}
