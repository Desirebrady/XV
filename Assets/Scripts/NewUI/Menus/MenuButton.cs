using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void DeactivateSpecificMenu(MenuMode mode)
    {
        UIElementManager.Instance.ActivateOrDeactivateMenu(mode);
    }
    public void DeactivateSpecificMenu(int mode)
    {
        MenuMode mod = MenuMode.BuildMode;
        switch (mode)
        {   
            case 0:
                mod = MenuMode.BuildMode;
                break;
            case 1:
                mod = MenuMode.ActionsMode;
                break;
            case 2:
                mod = MenuMode.PlaybackMode;
                break;
            case 3:
                mod = MenuMode.StorageMode;
                break;
            default:
                break;
        }

        UIElementManager.Instance.ActivateOrDeactivateMenu(mod);
    }

    public void RemoveSelectedSceneObject()
    {
        SceneObjectsManager.Instance.RemoveSelectedInstance();
    }

    public void TransformSelectedSceneObject()
    {
        SceneObjectsManager.Instance.TriggerTransformState();
    }

    public void CloseEditMenu()
    {
        SceneObjectsManager.Instance.CloseEditMenu();
    }


    public void ClearAllActions()
    {
        if (SceneObjectsManager.Instance.selectedPerson != null)
            SceneObjectsManager.Instance.selectedPerson.Instructions.Clear();
    }

    public void PauseGame()
    {
        GameManager.Instance.Running = false;
        UIElementManager.Instance.DisableMenuButtons();
    }

    public void PlayGame()
    {
        GameManager.Instance.Running = true;
        UIElementManager.Instance.EnableMenuButtons();
    }
}
