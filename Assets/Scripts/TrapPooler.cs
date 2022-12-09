using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPooler : FastSingleton<TrapPooler>
{
    [SerializeField] public Transform parent;
    [SerializeField] private int amount;
    [SerializeField] private GameObject waterPrefab;
    private Queue<GameObject> poolTraps;

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
    public GameObject SpawnFromPool()
    {
        GameObject trap = poolTraps.Dequeue();
        poolTraps.Enqueue(trap);
        return trap;
    }
}
