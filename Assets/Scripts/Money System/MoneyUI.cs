using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    #region Singleton Access
    private static MoneyUI instance;//Use of a singleton here, needs to be static in order for other scripts to access it.

    public static MoneyUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MoneyUI>();
            }

            return instance;
        }
    }
    #endregion

    public TextMeshProUGUI m_Object;

    public void RefreshMoneyUI()
    {
        m_Object.text = GameManager.Instance.moneySystem.currentMoney.ToString();
    }
}
