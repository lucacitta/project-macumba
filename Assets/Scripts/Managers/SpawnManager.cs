using UnityEngine;

using UnityEngine.AI;

using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Catalog")]
    public List<StructureDefinition> structureCatalog = new(); // Spawnable structure types

    [Header("Budget")]
    public float desiredStructureSpawnLevel = 5f;
    public float actualStructureSpawnLevel = 0f;

    public static SpawnManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        float remaining = Mathf.Max(0f, desiredStructureSpawnLevel - actualStructureSpawnLevel);
        while (remaining > 0f)
        {
            if (remaining <= 0f) return;

            var def = PickStructureDefinition(remaining);
            if (def == null) return;

            if (!TryGetSpawnPoint(out Vector3 spawnPos)) return;

            SpawnStructure(def, spawnPos);
            remaining = Mathf.Max(0f, desiredStructureSpawnLevel - actualStructureSpawnLevel);
        }
    }

    private StructureDefinition PickStructureDefinition(float remainingStructureSpawn)
    {
        var candidates = new List<StructureDefinition>();
        float totalWeight = 0f;

        foreach (var def in structureCatalog)
        {
            if (def == null || def.prefab == null) continue;
            if (def.structureSpawnFactor <= remainingStructureSpawn)
            {
                candidates.Add(def);
                totalWeight += 1; 
            }
        }

        if (candidates.Count == 0) return null;

        float r = Random.value * totalWeight;
        foreach (var c in candidates)
        {
            if (r <= 1) return c;
            r -= 1;
        }
        return candidates[^1];
    }


    private bool TryGetSpawnPoint(out Vector3 spawnPos)
    {
        Vector3 mapCenter = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            Vector2 dir2 = Random.insideUnitCircle.normalized;
            float dist = Random.Range(50, 140); //TODO: This map-specific values should be configurable

            Vector3 target = mapCenter + new Vector3(dir2.x, 0f, dir2.y) * dist;
            target.y = 0f;

            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 100, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
                return true;
            }
        }

        spawnPos = default;
        return false;
    }


    private void SpawnStructure(StructureDefinition def, Vector3 spawnPos)
    {
        if (def == null || def.prefab == null) return;

        GameObject obj = Instantiate(def.prefab, spawnPos, Quaternion.identity);
        actualStructureSpawnLevel += def.structureSpawnFactor;
    }

}
