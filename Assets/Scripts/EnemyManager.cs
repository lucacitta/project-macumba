using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static List<GameObject> ActiveEnemies = new();

    public static void RegisterEnemy(GameObject enemy)
    {
        if (!ActiveEnemies.Contains(enemy))
        {
            ActiveEnemies.Add(enemy);
        }
    }

    public static void DeregisterEnemy(GameObject enemy)
    {
        ActiveEnemies.Remove(enemy);
    }
}
