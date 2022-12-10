using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPooler : FastSingleton<DiamondPooler>
{
    /// <summary>
    /// Use Stack to restore diamond pooler
    /// </summary>
    [SerializeField] public Transform parent;
    [SerializeField] private int amount;
    [SerializeField] private GameObject diamondPrefab;
    private Stack<GameObject> poolDiamonds;

    // Init amount of diamond to pool
    protected override void Awake()
    {
        base.Awake();
        poolDiamonds = new Stack<GameObject>();
        for(int i = 0; i < amount; i++)
        {
            GameObject diamond = Instantiate(diamondPrefab, parent);
            diamond.SetActive(false);
            poolDiamonds.Push(diamond);
        }
    }
    // Spawn diamond from pool
    public GameObject SpawnFromPool()
    {
        return poolDiamonds.Pop();
    }
    // return diamond to pool
    public void BackToPool(GameObject ground)
    {
        poolDiamonds.Push(ground);
        ground.SetActive(false);
    }
}
