using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SavingSystem : MonoBehaviour
{
    #region Singleton Access
    private static SavingSystem instance;//Use of a singleton here, needs to be static in order for other scripts to access it.

    public static SavingSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SavingSystem>();
            }

            return instance;
        }
    }
    #endregion

    public List<GameObject> saveablePrefabs = new List<GameObject>();
    public List<GameObject> saveableItems = new List<GameObject>();
    public bool hasLoaded = false;
    public SaveInfo saveInfo = new SaveInfo();

    public void Save()
    {
        if (GameManager.Instance.Running == true)
        {
            Debug.Log("Please go to the setup phase before saving!");
            return;
        }

        List<SaveableObject> so = GameObject.FindObjectsOfType<SaveableObject>().ToList();
        var orderedSO = so.OrderBy(f => f.saveID).ToList();
        saveInfo.objectToBeSaved.Clear();
        hasLoaded = false;

        //Ask all object for their Transform, ID and if they are a human
        foreach (var item in orderedSO)
        {
            saveInfo.objectToBeSaved.Add(item.Save());
        }

        saveInfo.money = GameManager.Instance.moneySystem.currentMoney;

        //Save to file here
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file = (File.Exists(destination)) ? File.OpenWrite(destination) : File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, saveInfo);
        file.Close();

        Debug.Log("Saved Successfully!");
    }

    public void Load()
    {
        if (GameManager.Instance.Running == true)
        {
            Debug.Log("Please go to the setup phase before loading!");
            return;
        }

        if (hasLoaded == true)
            return;

        hasLoaded = true;

        //Load from file
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination))
            file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        saveInfo = (SaveInfo)bf.Deserialize(file);
        file.Close();

        GameManager.Instance.uniqueID_Tracker = 0;

        SaveableObject[] deleteTheseBastards = GameObject.FindObjectsOfType<SaveableObject>();

        foreach (var item in deleteTheseBastards)
            GameObject.Destroy(item.gameObject);

        //Create files here and load in the contents
        foreach (var item in saveInfo.objectToBeSaved)
        {
            GameObject loadedObject = GameObject.Instantiate(saveablePrefabs[item.index], SceneObjectsManager.Instance.transform);
            loadedObject.transform.position = new Vector3(item.Position[0], item.Position[1], item.Position[2]);
            loadedObject.transform.rotation = Quaternion.Euler(item.Rotation[0], item.Rotation[1], item.Rotation[2]);
            loadedObject.transform.localScale = new Vector3(item.Scale[0], item.Scale[1], item.Scale[2]);
            loadedObject.GetComponent<SaveableObject>().uniqueID = GameManager.Instance.GetUniqueID_ForObject();
            loadedObject.GetComponent<SaveableObject>().isHuman = item.isHuman;

            if (item.isHuman == true)
            {
                //List<ItemManager> go = GameObject.FindObjectsOfType<ItemManager>().ToList();
                GameObject selectedGameObject = null;
                int tempID = -1;

                for (int j = 0; j < item.instructions.Count; j++)
                {
                    for (int i = 0; i < saveablePrefabs.Count - 1; i++) //I minus one here to skip the last one which whould be a human
                    {
                        if (saveablePrefabs[i].GetComponent<SaveableObject>().uniqueID == item.instructions[j].prefabUniqueID)
                        {
                            selectedGameObject = saveablePrefabs[i];
                            tempID = saveablePrefabs[i].GetComponent<SaveableObject>().uniqueID;
                            break;
                        }
                    }

                    loadedObject.GetComponent<Person>().Instructions.AddLast(new Instruction(item.instructions[j].interactionType, selectedGameObject, saveableItems[item.instructions[j].itemID].GetComponent<Item>()));
                }
            }
        }

        GameManager.Instance.moneySystem.SetMoney(saveInfo.money);
        Debug.Log("Loaded Successfully!");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            Save();

        if (Input.GetKeyDown(KeyCode.F6))
            Load();
    }
}

[System.Serializable]
public class SaveInfo
{
    public float money;
    [SerializeField]
    public List<ObjectToBeSaved> objectToBeSaved = new List<ObjectToBeSaved>();
}

[System.Serializable]
public class ObjectToBeSaved
{
    public int index;
    public float[] Position = new float[3] { 0, 0, 0 };
    public float[] Rotation = new float[3] { 0, 0, 0 };
    public float[] Scale = new float[3] { 0, 0, 0 };
    public bool isHuman;
    public List<SavedInstructions> instructions = new List<SavedInstructions>();
}

[System.Serializable]
public class SavedInstructions
{
    public InteractionType interactionType;
    public int prefabUniqueID;
    public int itemID;
}