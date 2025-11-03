using UnityEngine;


class EnemyOnDieReceiver : MonoBehaviour, IDie
{


    public void OnDied()
    {
        var manager = FindObjectOfType<EnemyManager>();
        manager?.DeregisterEnemy(gameObject);

        var col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false; // prevent further collisions while destroying
        }

        Destroy(gameObject);
    }
}