using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : FastSingleton<GameplayManager>
{
    private Vector3 currentEndRoad;
    private int firstAmountGround = 15;
    private Queue<GameObject> listGround = new Queue<GameObject>();
    private const float HEIGHT_SPAWN_DIAMOND = 3.5f;
    public bool onSetting;
    void Start()
    {
        OnInit();
    }
    private void OnInit()
    {
        onSetting = true;
        currentEndRoad = Vector3.zero;
        InstantiateFirstPosition();
    }
    private void InstantiateFirstPosition()
    {
        for(int i = 0; i < firstAmountGround; i++)
        {
            SpawnGround();
        }
    }
    private Direct RandomDirect()
    {
        int rd = Random.Range(0, 2);
        if(rd == 1)
        {
            return Direct.Right;
        }
        return Direct.Left;
    }
    private Vector3 GetNextEndRoadPositionByDirect(Direct direct)
    {
        if(direct == Direct.Right)
        {
            return currentEndRoad + Vector3.forward;
        }
        else if(direct == Direct.Left)
        {
            return currentEndRoad + Vector3.left;
        }
        return currentEndRoad + Vector3.forward;
    }
    public void SpawnGround()
    {   
        GameObject ground = GroundPooler.instance.SpawnFromPool();
        ground.SetActive(true);
        ground.transform.SetPositionAndRotation(currentEndRoad, Quaternion.identity);
        Direct direct = RandomDirect();

        if(Character.instance.score % 10 == 0 && Character.instance.score != 0)
        {
            int randomDiamond = Random.Range(0, 2);
            if(randomDiamond == 1)
            {
                GameObject diamond = DiamondPooler.instance.SpawnFromPool();
                diamond.SetActive(true);
                diamond.transform.SetPositionAndRotation(currentEndRoad + Vector3.up * HEIGHT_SPAWN_DIAMOND, Quaternion.identity);
            }
        }

        if(Character.instance.score % 5 == 0 && Character.instance.score != 0)
        {
            int randomDiamond = Random.Range(0, 2);
            if(randomDiamond == 1)
            {
                if(direct == Direct.Left)
                {
                    GameObject trap = TrapPooler.instance.SpawnFromPool();
                    trap.SetActive(true);
                    trap.transform.SetPositionAndRotation(currentEndRoad + Vector3.forward, Quaternion.identity);
                }
                else if(direct == Direct.Right){
                    GameObject trap = TrapPooler.instance.SpawnFromPool();
                    trap.SetActive(true);
                    trap.transform.SetPositionAndRotation(currentEndRoad + Vector3.left, Quaternion.identity);
                }
            }
        }
        
        currentEndRoad = GetNextEndRoadPositionByDirect(direct);
    }
    public void OnLose()
    {
        UIManager.instance.CloseUI<Play>();
        SaveData();
        StartCoroutine(Lose());
    }
    IEnumerator Lose()
    {
        yield return new WaitForSeconds(3f);
        
        UIManager.instance.OpenUI<Lose>();
    }
    private void SaveData()
    {
        int score = Character.instance.score;
        if(score > PlayerPrefs.GetInt("BEST_SCORE", 0))
        {
            PlayerPrefs.SetInt("BEST_SCORE", score);
        }
        int diamond = Character.instance.diamond + PlayerPrefs.GetInt("DIAMOND", 0);
        PlayerPrefs.SetInt("DIAMOND", diamond);
    }
}
