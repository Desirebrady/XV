using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Loading Rules", order = 1)]
public class LoadingRules : ScriptableObject
{
    public int activeLevel = -1;
    public List<Item> items = new List<Item>();
    public float StartingMoney;

    public void InitSceneRules(GameManager gameManager)
    {
        gameManager.items = items;
        gameManager.moneySystem.SetStartMoney(StartingMoney);
    }
}
