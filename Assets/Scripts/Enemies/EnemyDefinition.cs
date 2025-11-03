using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "Game/Enemy Definition")]
public class EnemyDefinition : ScriptableObject
{
    [Header("Prefab & Identity")]
    public GameObject prefab;
    public string enemyId;

    [Header("Spawn / Balance")]
    public float dangerFactor = 1f;

}
