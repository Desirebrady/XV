using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DeliveryTruck
{
    public float currentTime = 0, maxTime = 10;
    private List<SellItems> SalesPoint = new List<SellItems>();
    public RectTransform BackgroundBar, TruckSprite;
    public Item target;

    public bool UpdateTruck()
    {
        currentTime += Time.deltaTime;
        if (GameManager.Instance.Running == false)
            currentTime = 0;
        TruckSprite.anchoredPosition = new Vector2((currentTime / maxTime) * Mathf.Abs(BackgroundBar.rect.width), 0);

        if (currentTime >= maxTime)
        {
            currentTime %= maxTime;
            CollectGoods();
            return true;
        }
        return false;
    }

    private void CollectGoods()
    {
        SalesPoint = GameObject.FindObjectsOfType<SellItems>().ToList();

        for (int i = 0; i < SalesPoint.Count; i++)
        {
            GameManager.Instance.moneySystem.AddMoney(SalesPoint[i].sell(target) * target.GetPrice());
        }
    }
}
