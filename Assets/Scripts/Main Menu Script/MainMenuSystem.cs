using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuSystem : MonoBehaviour
{
    public GameObject MainMenuObj;
    public GameObject OptionsMenuObj;
    public LoadingRules loadingRules;
    public List<Item> scene1 = new List<Item>();
    public List<Item> scene2 = new List<Item>();
    public List<Item> scene3 = new List<Item>();

    public void Start()
    {
        SetMainMenu(true);
    }

    public void LoadLevel(int version)
    {
        if (version == 0)
        {
            loadingRules.activeLevel = 0;
            loadingRules.items = scene1;
            loadingRules.StartingMoney = 2500;
        }
        else if (version == 1)
        {
            loadingRules.activeLevel = 1;
            loadingRules.items = scene2;
            loadingRules.StartingMoney = 1500;
        }
        else if (version == 2)
        {
            loadingRules.activeLevel = 2;
            loadingRules.items = scene3;
            loadingRules.StartingMoney = 1000;
        }

        SceneManager.LoadScene(1);
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

    public void SetShadowMask(Dropdown item)
    {
        QualitySettings.shadowmaskMode = (ShadowmaskMode)item.value;
    }

    public void SetShadow(Dropdown item)
    {
        QualitySettings.shadows = (ShadowQuality)item.value;
    }

    public void SetShadowResolution(Dropdown item)
    {
        QualitySettings.shadowResolution = (ShadowResolution)item.value;
    }

    public void SetShadowDistance(Slider item)
    {
        QualitySettings.shadowDistance = item.value;
    }

    public void SetShadowPlaneOffset(Slider item)
    {
        QualitySettings.shadowNearPlaneOffset = item.value;
    }
}
