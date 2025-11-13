using UnityEngine;

[CreateAssetMenu(fileName = "StructureDefinition", menuName = "Game/Structure Definition")]
public class StructureDefinition : ScriptableObject
{
    [Header("Prefab & Identity")]
    public GameObject prefab;
    public string structureId;

    [Header("Spawn / Balance")]
    public float structureSpawnFactor = 1f;

}
