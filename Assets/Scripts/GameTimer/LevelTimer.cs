using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    #region Singleton Access
    private static LevelTimer instance;//Use of a singleton here, needs to be static in order for other scripts to access it.

    public static LevelTimer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<LevelTimer>();
            }

            return instance;
        }
    }
    #endregion

    public TextMeshProUGUI m_Object;

    public void RefreshTimerUI()
    {
        m_Object.text = GameManager.Instance.levelTimer.ToString() + " S";
    }
}
