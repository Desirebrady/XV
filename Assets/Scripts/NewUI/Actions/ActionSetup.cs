using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSetup : MonoBehaviour
{

    [Header("Action Options")]
    public LinkedListNode<Instruction> FromHolder;
    public LinkedListNode<Instruction> ToHolder;
    public Text targetItem;

    ActionSetup()
        :base()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClose()
    {
        //Remove from Entity Actions list
        SceneObjectsManager.Instance.selectedPerson.RemoveInstruction(FromHolder);
        Destroy(gameObject);
    }

/*
    public void selectTarget()
    {
        int currentIndex = FromHolder.index + 1;

        if (currentIndex >= FromHolder.getter.GetOutputs().Count)
            FromHolder.index = -1;

        targetItem.text = FromHolder.getter.GetOutputs()[FromHolder.index].item.type.toString();
    }
*/
}
