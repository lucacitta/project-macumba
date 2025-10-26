using UnityEngine;


class EnemyOnDieReceiver : MonoBehaviour, IDie
{


    public void OnDied()
    {
        EnemyManager.DeregisterEnemy(gameObject);
        var col = GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false; // prevent further collisions while destroying
            }

        Destroy(gameObject);
    }
}