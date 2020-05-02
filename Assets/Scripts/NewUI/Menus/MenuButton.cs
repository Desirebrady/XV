using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    private Image recordButton;
    private bool isRecording = false;

    public void DeactivateSpecificMenu(int mode)
    {
        MenuMode mod = MenuMode.None;
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
                mod = MenuMode.VideoMode;
                break;
            case 4:
                mod = MenuMode.GameMenu;
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

    void Update()
    {
        if (UIElementManager.Instance.currentMode != MenuMode.PlaybackMode)
        {
            if (recordButton != null)
            {
                recordButton.color = Color.red;
            }
            isRecording = false;
        }
    }

    public void ClearAllActions()
    {
        if (SceneObjectsManager.Instance.selectedPerson != null)
            SceneObjectsManager.Instance.selectedPerson.Instructions.Clear();
        SceneObjectsManager.Instance.actionRevalidate = true;
    }

    public void PauseGame()
    {
        if (UIElementManager.Instance.currentMode == MenuMode.PlaybackMode)
        {
            GameManager.Instance.Running = false;
            GameManager.Instance.levelTimer = 180;
            UIElementManager.Instance.DisableMenuButtons();
            UIElementManager.Instance.currentMode = MenuMode.None;
        }
    
    }

    public void StartRecordGame()
    {
        TryGetComponent<Image>(out recordButton);

        if (UIElementManager.Instance.currentMode == MenuMode.PlaybackMode)
        {
            //if Already recording. Stop Recording, otherwise Start Recording
            Debug.Log("Start/Stop Recording!!");

            if (isRecording)
            {
                //Stop Recording
                isRecording = false;
            
                if(recordButton != null)
                    recordButton.color = Color.red;

                Debug.Log("Recording Stopped!");
                return ;
            }

            //Start Recording
            isRecording = true;
            Debug.Log("Start Recording!!");

            if (recordButton != null)
                recordButton.color = Color.magenta;

        }
    }


    public void LoadScene(int index)
    {
        if (index < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(index);
        }
    }
    public void ReloadScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    public void PlayGame()
    {
        UIElementManager.Instance.currentMode = MenuMode.PlaybackMode;
        GameManager.Instance.Running = true;
        UIElementManager.Instance.EnableMenuButtons();
    }
}
