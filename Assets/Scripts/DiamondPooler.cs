using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPooler : FastSingleton<DiamondPooler>
{
    [SerializeField] public Transform parent;
    [SerializeField] private int amount;
    [SerializeField] private GameObject diamondPrefab;
    private Stack<GameObject> poolDiamonds;

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
    public GameObject SpawnFromPool()
    {
        return poolDiamonds.Pop();
    }
    public void BackToPool(GameObject ground)
    {
        poolDiamonds.Push(ground);
        ground.SetActive(false);
    }
}
