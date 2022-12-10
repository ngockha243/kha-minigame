using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPooler : FastSingleton<GroundPooler>
{
    /// <summary>
    /// Use queue to restore ground pooler
    /// </summary>
    [SerializeField] public Transform parent;
    [SerializeField] public int amount;
    [SerializeField] private GameObject groundPrefab;
    private Queue<GameObject> poolGrounds;

    // Init amount of ground to pool
    protected override void Awake()
    {
        base.Awake();
        poolGrounds = new Queue<GameObject>();
        for(int i = 0; i < amount; i++)
        {
            GameObject ground = Instantiate(groundPrefab, parent);
            ground.SetActive(false);
            poolGrounds.Enqueue(ground);
        }
    }
    // Spawn ground from pool (Get top of queue and add to end of queue)
    public GameObject SpawnFromPool()
    {
        GameObject ground = poolGrounds.Dequeue();
        poolGrounds.Enqueue(ground);
        return ground;
    }
}
