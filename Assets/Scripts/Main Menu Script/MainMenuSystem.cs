using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSystem : MonoBehaviour
{
    public GameObject MainMenuObj;
    public GameObject OptionsMenuObj;

    public void Start()
    {
        SetMainMenu(true);
    }

    public void SetMainMenu(bool state)
    {
        MainMenuObj.SetActive(state);
        OptionsMenuObj.SetActive(!state);
    }

    public void SetOptionsMenu(bool state)
    {
        MainMenuObj.SetActive(!state);
        OptionsMenuObj.SetActive(state);
    }

    public void QuitApp()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetTextureQuality(Dropdown item)
    {
        QualitySettings.masterTextureLimit = item.value;
    }

    public void SetAnisotropicFilteringQuality(Dropdown item)
    {
        QualitySettings.anisotropicFiltering = (AnisotropicFiltering)item.value;
    }

    public void SetAntiAliasingQuality(Dropdown item)
    {
        if (item.value == 0)
            QualitySettings.antiAliasing = 0;

        if (item.value == 1)
            QualitySettings.antiAliasing = 2;

        if (item.value == 2)
            QualitySettings.antiAliasing = 4;

        if (item.value == 3)
            QualitySettings.antiAliasing = 8;
    }

    public void SetSoftParticles(Toggle item)
    {
        QualitySettings.softParticles = item.isOn;
    }

    public void SetRealtimeReflectionProbes(Toggle item)
    {
        QualitySettings.realtimeReflectionProbes = item.isOn;
    }

    public void SetBillboardFaceCamera(Toggle item)
    {
        QualitySettings.billboardsFaceCameraPosition = item.isOn;
    }
}
