using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : FastSingleton<GameplayManager>
{
    private Vector3 currentEndRoad; // current end of road --> to detect next spawn position
    private int firstAmountGround;
    private const float HEIGHT_SPAWN_DIAMOND = 3.5f;    // height of spawn diamond --> for diamond spawn above ground
    public bool onSetting;  // for check if game is on setting or not
    void Start()
    {
        OnInit();
    }
    /// <summary>
    /// Init all value, spawn amount of ground (< ground pooler) for show ground behind player
    /// </summary>
    private void OnInit()
    {
        onSetting = true;
        currentEndRoad = Vector3.zero;
        firstAmountGround = GroundPooler.instance.amount - 8;
        InstantiateFirstGround();
    }
    private void InstantiateFirstGround()
    {
        for(int i = 0; i < firstAmountGround; i++)
        {
            SpawnGround();
        }
    }
    /// <summary>
    /// Random direct for spawn ground (random road)
    /// </summary>
    private Direct RandomDirect()
    {
        int rd = Random.Range(0, 2);
        if(rd == 1)
        {
            return Direct.Right;
        }
        return Direct.Left;
    }
    /// <summary>
    /// Get next position to spawn ground by Random direct
    /// </summary>
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
    /// <summary>
    /// Get ground from pool and spawn to end road
    /// </summary>
    public void SpawnGround()
    {   
        GameObject ground = GroundPooler.instance.SpawnFromPool();
        ground.SetActive(true);
        ground.transform.SetPositionAndRotation(currentEndRoad, Quaternion.identity);
        Direct direct = RandomDirect();

        // spawn diamond and trap
        SpawnDiamond();

        SpawnTrap(direct);
        
        // set next end road
        currentEndRoad = GetNextEndRoadPositionByDirect(direct);
    }
    /// <summary>
    /// Every 10 points --> Have 50% to spawn diamond
    /// Get diamond from pool and spawn above ground
    /// </summary>
    private void SpawnDiamond()
    {
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
    }
    /// <summary>
    /// Every 5 points --> Have 50% to spawn trap oposite to ground
    /// Get trap from pool and spawn oposite to ground
    /// </summary>
    private void SpawnTrap(Direct direct)
    {
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
    }
    /// <summary>
    /// Lose game --> close Play UI --> Save Data --> open Lose UI
    /// </summary>
    public void OnLose()
    {
        UIManager.instance.CloseUI<Play>();
        SaveData();
        StartCoroutine(Lose());
    }
    IEnumerator Lose()
    {
        yield return new WaitForSeconds(2f);
        
        UIManager.instance.OpenUI<Lose>();
    }
    /// <summary>
    /// Save best score if current score > best score
    /// </summary>
    private void SaveData()
    {
        int score = Character.instance.score;
        if(score > PlayerPrefs.GetInt("BEST_SCORE", 0))
        {
            PlayerPrefs.SetInt("BEST_SCORE", score);
        }
    }
}
