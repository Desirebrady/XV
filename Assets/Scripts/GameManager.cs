using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region Singleton Access
    private static GameManager instance;//Use of a singleton here, needs to be static in order for other scripts to access it.
    [SerializeField]
    public List<Item> items;
    public LoadingRules loadingRules;
    int itemindex = 0;


    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }
    #endregion

    #region Variables
    private float tickTimer;
    public float levelTimer = 180.0f;
    [HideInInspector] public List<ItemManager> allObjects = new List<ItemManager>();
    public bool Running = false;


    public DeliveryTruck deliveryTruck = new DeliveryTruck();
    public MoneySystem moneySystem = new MoneySystem();
    public int uniqueID_Tracker;
    #endregion

    void Awake()
    {
        uniqueID_Tracker = 0;
        loadingRules.InitSceneRules(this);

        deliveryTruck.target = items[itemindex];
    }

    void Start()
    {
        Running = false;
    }

    private void Update()
    {
        if (levelTimer <= 0)
        {
            GameHasEnded();
            return;
        }

        tickTimer += Time.deltaTime;
        if (deliveryTruck.UpdateTruck())
        {
            itemindex ++;
            itemindex %= items.Count;
            deliveryTruck.target = items[itemindex];
        }

        if (tickTimer >= 1)
        {
            tickTimer %= 1;
            UpdateTickTimers();

            
            if (Running)
                levelTimer--;

            LevelTimer.Instance.RefreshTimerUI();
        }
        
        //Dante _ Commented Out
        //DeselectSelectedPerson();
    }

    private bool showingEndStates = false;

    private void GameHasEnded()
    {
        if (showingEndStates == false)
        {
            showingEndStates = true;
            UIElementManager.Instance.SetCompletionScreen(moneySystem.currentMoney.ToString());
            UIElementManager.Instance.CompletionCanvas.SetActive(true);
        }
    }

    public void UpdateTickTimers()
    {
        allObjects.Where(x => x.GetComponent<ItemManager>() != null)
            .ToList()
            .ForEach(x => x.GetComponent<ItemManager>().UpdateTickTimer());
    }

    public int GetUniqueID_ForObject()
    {
        int currentUID = GameManager.Instance.uniqueID_Tracker;
        GameManager.Instance.uniqueID_Tracker++;
        return currentUID;
    }
}
