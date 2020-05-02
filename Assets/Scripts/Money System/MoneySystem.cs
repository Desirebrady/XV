using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoneySystem
{
    public float currentMoney;

    public void SetStartMoney(float amt)
    {
        currentMoney = amt;
        MoneyUI.Instance.RefreshMoneyUI();
    }

    public void SetMoney(float amt)
    {
        currentMoney = amt;
        MoneyUI.Instance.RefreshMoneyUI();
    }

    public void AddMoney(float amt)
    {
        currentMoney += amt;
        MoneyUI.Instance.RefreshMoneyUI();
    }

    public void RemoveMoney(float amt)
    {
        currentMoney -= amt;
        MoneyUI.Instance.RefreshMoneyUI();
    }
}
