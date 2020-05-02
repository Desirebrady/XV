using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    public int saveID;
    public int uniqueID;
    public bool isHuman;

    public void Awake()
    {
        uniqueID = GameManager.Instance.GetUniqueID_ForObject();
    }

    public ObjectToBeSaved Save()
    {
        ObjectToBeSaved info = new ObjectToBeSaved()
        {
            index = saveID,
            Position = new float[3] { transform.position.x, transform.position.y, transform.position.z },
            Rotation = new float[3] { transform.rotation.x, transform.rotation.y, transform.rotation.z },
            Scale = new float[3] { transform.localScale.x, transform.localScale.y, transform.localScale.z },
            isHuman = isHuman
        };

        if (isHuman == true)
        {
            Person p = GetComponent<Person>();
            info.instructions = new List<SavedInstructions>();

            foreach (var instruction in p.Instructions)
            {
                SavedInstructions savedInstructions = new SavedInstructions()
                {
                    interactionType = instruction.interactionType,
                    prefabUniqueID = instruction.itemManagerObject.GetComponent<SaveableObject>().uniqueID,
                    itemID = instruction.target.ItemID
                };

                info.instructions.Add(savedInstructions);
            }
        }

        return info;
    }
}
