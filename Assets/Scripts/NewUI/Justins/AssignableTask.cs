using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AssignableTask : MonoBehaviour, IPointerClickHandler
{
    public enum AssignType
    {
        From,
        To
    }
    public AssignType assignType = AssignType.From;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("WTF");
        // if (SceneObjectsManager.Instance.assigningTask == true)
        //     return;

        // if (SceneObjectsManager.Instance.selectedPerson == null)
        //     return;

        // for (int i = 0; i < GameManager.Instance.allObjects.Count; i++)
        // {
        //     ItemManager current = GameManager.Instance.allObjects[i];

        //     if (assignType == AssignType.From &&
        //         (current.ManagerType == ItemManager.ItemManagerType.creates || current.ManagerType == ItemManager.ItemManagerType.refines))
        //             current.outline.enabled = true;
        //     else if (assignType == AssignType.To &&
        //         (current.ManagerType == ItemManager.ItemManagerType.destroys || current.ManagerType == ItemManager.ItemManagerType.refines))//Only outline the chain of production here!!!
        //             current.outline.enabled = true;
        //     else
        //         current.outline.enabled = false;
        // }

        // if (SceneObjectsManager.Instance.selectedPerson.queuedTasks.Count <= 0)
        //     SceneObjectsManager.Instance.selectedPerson.queuedTasks.Add(new QueuedTasks());
        // else if (SceneObjectsManager.Instance.selectedPerson.queuedTasks[SceneObjectsManager.Instance.selectedPerson.queuedTasks.Count - 1].From != null &&
        //     SceneObjectsManager.Instance.selectedPerson.queuedTasks[SceneObjectsManager.Instance.selectedPerson.queuedTasks.Count - 1].To != null)
        //     SceneObjectsManager.Instance.selectedPerson.queuedTasks.Add(new QueuedTasks());

        // SceneObjectsManager.Instance.assigningTask = true;
        // //GameManager.Instance.assigningFromTask = this;
    }
}