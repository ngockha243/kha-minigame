using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPooler : FastSingleton<TrapPooler>
{
    /// <summary>
    /// Use queue to restore trap pooler
    /// </summary>
    [SerializeField] public Transform parent;
    [SerializeField] private int amount;
    [SerializeField] private GameObject waterPrefab;
    private Queue<GameObject> poolTraps;

    // Init amount of trap to pool
    protected override void Awake()
    {
        base.Awake();
        poolTraps = new Queue<GameObject>();
        for(int i = 0; i < amount; i++)
        {
            GameObject trap = Instantiate(waterPrefab, parent);
            trap.SetActive(false);
            poolTraps.Enqueue(trap);
        }
    }
    // Spawn trap from pool (Get top of queue and add to end of queue)
    public GameObject SpawnFromPool()
    {
        GameObject trap = poolTraps.Dequeue();
        poolTraps.Enqueue(trap);
        return trap;
    }
}
