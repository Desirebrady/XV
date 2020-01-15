using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour, IGet, IOperate, IPut
{
    #region Variables
    public float operateTime;
    public enum ItemManagerType
    {
        nothing,
        creates,
        refines,
        destroys
    }
    public ItemManagerType ManagerType = ItemManagerType.nothing;
    public Recipe itemRecipe = new Recipe();
    List<Person> workers = new List<Person>();

    private GameObject childrenHolder;
    public float spawnTimer = 5, currentSpawnTimer = 0;
    public bool requiresWorkers = false;
    public float baseSpawnTimer = 5, personWorkDecrease = 2;
    public int maxWorkers = 2;
    public int maxStorage = 5;
    public GameObject station;
    [HideInInspector] public Outline outline;
    #endregion

    public void Awake()
    {
        childrenHolder = transform.Find("Children").gameObject;
        outline = GetComponent<Outline>();

        if (outline != null)
            outline.enabled = false;

        int offset = 0, i = 0, inventory = 0;
        for (; i < childrenHolder.transform.childCount && inventory < (itemRecipe.ingredients.Count + itemRecipe.output.Count); i++, offset++)
        {
            if (inventory < itemRecipe.ingredients.Count)
            {
                if (offset < itemRecipe.ingredients[inventory].maxStorage)
                {
                    itemRecipe.ingredients[inventory].spawnLocations.Add(childrenHolder.transform.GetChild(i));
                }
                else
                {
                    inventory++;
                    offset = -1;
                    i--;
                }
            }
            else
            {
                int inven = inventory - itemRecipe.ingredients.Count;
                if (offset < itemRecipe.output[inven].maxStorage)
                {
                    itemRecipe.output[inven].spawnLocations.Add(childrenHolder.transform.GetChild(i));
                }
                else
                {
                    inventory++;
                    offset = -1;
                    i--;
                }
            }
        }

        foreach (Ingredient ing in itemRecipe.ingredients)
        {
            foreach (Transform t in ing.spawnLocations)
            {
                if (t.childCount > 0)
                    GameObject.Destroy(t.GetChild(0).gameObject);
            }
        }
        foreach (Ingredient ing in itemRecipe.output)
        {
            foreach (Transform t in ing.spawnLocations)
            {
                if (t.childCount > 0)
                    GameObject.Destroy(t.GetChild(0).gameObject);
            }
        }
    }

    public void UpdateTickTimer()
    {
        if (requiresWorkers)
            spawnTimer = baseSpawnTimer - (workers.Count * personWorkDecrease);
        if (GameManager.Instance.Running)
        {
            if (!requiresWorkers || workers.Count > 0)
                currentSpawnTimer = Mathf.Min(currentSpawnTimer + 1, spawnTimer);

            if (currentSpawnTimer >= spawnTimer)
            {
                if (Spawn())
                {
                    currentSpawnTimer = 0;
                    for (int i = workers.Count - 1; i >= 0; i--)
                    {
                        workers[i].WorkFinished(this);
                    }
                }
            }
        }
        else
        {
            foreach (Ingredient i in itemRecipe.ingredients)
            {
                foreach (Transform t in i.spawnLocations)
                {
                    if (t.childCount > 0)
                        GameObject.Destroy(t.GetChild(0).gameObject);
                }
            }
            foreach (Ingredient i in itemRecipe.output)
            {
                foreach (Transform t in i.spawnLocations)
                {
                    if (t.childCount > 0)
                        GameObject.Destroy(t.GetChild(0).gameObject);
                }
            }
        }
    }

    private bool Spawn(GameObject item = null)
    {
        bool canSpawn = true;
        foreach (Ingredient i in itemRecipe.ingredients)
        {
            int count = 0;
            foreach (Transform t in i.spawnLocations)
            {
                if (t.childCount > 0)
                    count++;
            }
            if (count < i.itemAmt)
                canSpawn = false;
        }
        foreach (Ingredient i in itemRecipe.output)
        {
            int count = 0;
            foreach (Transform t in i.spawnLocations)
            {
                if (t.childCount > 0)
                    count++;
            }
            if ((i.maxStorage - count) < i.itemAmt)
                canSpawn = false;
        }
        if (canSpawn)
        {
            foreach (Ingredient i in itemRecipe.ingredients)
            {
                int removedTotal = 0;
                foreach (Transform t in i.spawnLocations)
                {
                    if (t.childCount > 0)
                    {
                        if (++removedTotal > i.itemAmt)
                            break;
                        GameObject.Destroy(t.GetChild(0).gameObject);
                    }
                }
            }
            foreach (Ingredient i in itemRecipe.output)
            {
                int spawnedTotal = 0;
                foreach (Transform t in i.spawnLocations)
                {
                    if (t.childCount == 0)
                    {
                        if (++spawnedTotal > i.itemAmt)
                            break;
                        GameObject SpawnedItem = (item == null) ? GameObject.Instantiate(i.item.gameObject, t) : item;
                        SpawnedItem.transform.localPosition = Vector3.zero;
                        SpawnedItem.transform.localRotation = Quaternion.identity;
                    }
                }
            }
        }
        return canSpawn;
    }

    private GameObject Despawn()
    {
        currentSpawnTimer++;

        for (int i = childrenHolder.transform.childCount; i > 0; i--)
        {
            if (i < 0)
                break;

            Transform child = childrenHolder.transform.GetChild(i - 1);

            if (child.childCount > 0)
            {
                GameObject ChildItem = child.GetChild(0).gameObject;
                ChildItem.transform.SetParent(null);
                ChildItem.SetActive(false);
                return ChildItem;
            }
        }

        return null;
    }

    #region IGet
    public Item get(Item item)
    {
        foreach (Ingredient i in itemRecipe.output)
        {
            if (i.item == item)
            {
                foreach (Transform t in i.spawnLocations)
                {
                    if (t.childCount > 0)
                    {
                        GameObject ChildItem = t.GetChild(0).gameObject;
                        ChildItem.transform.SetParent(null);
                        ChildItem.SetActive(false);
                        return ChildItem.GetComponent<Item>();
                    }
                }
            }
        }
        return null;
        //return Despawn()?.GetComponent<Item>();
    }
    #endregion
    
    #region IOperate
    public bool InitOperation(Person person)
    {
        if (workers.Count < maxWorkers)
        {
            person.station = station;
            workers.Add(person);
            return true;
        }
        return false;
    }

    public void EndOperation(Person person)
    {
        person.station = null;
        workers.Remove(person);
    }
    #endregion

    #region IPut
    public bool put(Item item)
    {
        if (item == null)
        {
            Debug.Log("WTF");
        }
        else
        foreach (Ingredient i in itemRecipe.ingredients)
        {
            if (i.item.type == item.type)
            {
                foreach (Transform t in i.spawnLocations)
                {
                    if (t.childCount == 0)
                    {
                        item.transform.SetParent(t);
                        item.transform.localPosition = Vector3.zero;
                        item.transform.localRotation = Quaternion.identity;
                        item.gameObject.SetActive(true);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public List<Ingredient> GetOutputs()
    {
        return itemRecipe.output;
    }

    public bool Has(Item item)
    {
        foreach (Ingredient i in itemRecipe.output)
        {
            if (i.item.type == item.type)
                return true;
        }
        return false;
    }

    public bool CanTake(Item item)
    {
        foreach (Ingredient i in itemRecipe.ingredients)
        {
            if (i.item.type == item.type)
                return true;
        }
        return false;
    }

    public List<Ingredient> GetInputs()
    {
        return itemRecipe.ingredients;
    }
    #endregion
}

[System.Serializable]
public class Recipe
{
    public List<Ingredient> ingredients = new List<Ingredient>(0);
    public List<Ingredient> output = new List<Ingredient>();
}

[System.Serializable]
public class Ingredient
{
    public int itemAmt;
    public Item item;
    public int maxStorage;
    public List<Transform> spawnLocations = new List<Transform>();
}