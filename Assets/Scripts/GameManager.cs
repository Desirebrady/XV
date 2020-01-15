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
    [HideInInspector] public List<ItemManager> allObjects = new List<ItemManager>();
    public bool Running = false;


    public DeliveryTruck deliveryTruck = new DeliveryTruck();

    #endregion

    void Awake()
    {
        allObjects = FindObjectsOfType<ItemManager>().ToList();
        deliveryTruck.target = items[itemindex];
    }

    private void Update()
    {
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
        }
        
        //Dante _ Commented Out
        //DeselectSelectedPerson();
    }

    public void UpdateTickTimers()
    {
        allObjects.Where(x => x.GetComponent<ItemManager>() != null)
            .ToList()
            .ForEach(x => x.GetComponent<ItemManager>().UpdateTickTimer());
    }

    void Start()
    {
        Running = false;
    }

}
