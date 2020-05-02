using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSetup : MonoBehaviour
{

    [Header("Action Options")]
    public LinkedListNode<Instruction> FromHolder;
    public LinkedListNode<Instruction> ToHolder;
    
    [Header("Instruction Images")]
    public RawImage fromImage;
    public RawImage toImage;
    public Texture2D defaultImage;
    public Text targetItem;
    private bool itemHasBeenSet = false;
    private bool imageHasBeenSet = false;


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
        onCreate();
        if (!itemHasBeenSet)
        {
            if (FromHolder.Value.getter != null)
                targetItem.text = FromHolder.Value.getter.GetOutputs()[FromHolder.Value.index].item.GetItemType().ToString();
            else
                targetItem.text = "Workstation";
        }
        if (FromHolder.Value.interactionType != InteractionType.Get)
        {
            toImage.texture = defaultImage;
        }
        
    }

    public void onClose()
    {
        //Remove from Entity Actions list
        SceneObjectsManager.Instance.selectedPerson.RemoveInstruction(FromHolder);
        Destroy(gameObject);
    }

    private void onCreate()
    {
        if (imageHasBeenSet)
            return ;

        if (FromHolder != null)
            fromImage.texture = FromHolder.Value.location.gameObject.GetComponent<UIElementController>().myIcon.texture;

        if (ToHolder != null)   
            toImage.texture = ToHolder.Value.location.gameObject.GetComponent<UIElementController>().myIcon.texture;
        
        imageHasBeenSet = true;
    }

    public void selectTarget()
    {
        itemHasBeenSet = true;
        if (FromHolder.Value.interactionType != InteractionType.Get)
            return;
            
        targetItem.text = "Nothing";

        FromHolder.Value.index += 1;
        FromHolder.Value.index %= FromHolder.Value.getter.GetOutputs().Count;
        int currentIndex = FromHolder.Value.index;
        FromHolder.Value.target = FromHolder.Value.getter.GetOutputs()[currentIndex].item;
        FromHolder.Next.Value.target = FromHolder.Value.getter.GetOutputs()[currentIndex].item;

        targetItem.text = FromHolder.Value.getter.GetOutputs()[currentIndex].item.GetItemType().ToString();
        
    }

}
