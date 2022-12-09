using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPooler : FastSingleton<GroundPooler>
{
    [SerializeField] public Transform parent;
    [SerializeField] private int amount;
    [SerializeField] private GameObject groundPrefab;
    private Queue<GameObject> poolGrounds;

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
    public GameObject SpawnFromPool()
    {
        GameObject ground = poolGrounds.Dequeue();
        poolGrounds.Enqueue(ground);
        return ground;
    }
}
