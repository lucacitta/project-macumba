using UnityEngine;


class EnemyOnDieReceiver : MonoBehaviour, IDie
{


    public void OnDied()
    {
        EnemyManager.DeregisterEnemy(gameObject);
        Destroy(gameObject);
    }
}