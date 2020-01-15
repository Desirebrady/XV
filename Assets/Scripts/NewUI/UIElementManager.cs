using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-2)]
class UIElementManager : MonoBehaviour
{
    #region Singleton Access
    private static UIElementManager instance;//Use of a singleton here, needs to be static in order for other scripts to access it.

    public static UIElementManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UIElementManager>();
            }

            return instance;
        }
    }
    #endregion

    [HideInInspector] public MenuMode currentMode;
    [HideInInspector] public bool isNewInstance;

    [Header("UI Element Settings")]  
    public UIElement[] UIElements;
    public LayerMask UIElementMask;
    public LayerMask SceneElementMask;
    
    [Header("UI Element Snapshot Camera")]  
    public Camera UICamera;
    
    [Header("Viewport Object Holder")]  
    public Transform newInstanceParent;

    [Header("UI Item Prefabs")]  
    public GameObject BuildContentItem;
    public GameObject ActionsContentItem;

    [Header("Build Menu Setup")]  
    public GameObject BuildCanvas;
    public GameObject BuildItemsHolder;
    
    [Header("Actions Menu Setup")]  
    public GameObject ActionsCanvas;
    public GameObject ActionItemsHolder;

    [Header("Storage Menu Setup")]  
    public GameObject StorageCanvas;
    public GameObject StorageItemsHolder;

    [Header("Tab Options")]
    public GameObject TabHolder;
    public GameObject Tab;

    [Header("Secondary Menus Options")]
    public GameObject EditMenuCanvas;
    public GameObject ActionsOptionsMenuCanvas;

    [Header("Instruction Textures for DEBUG")]
    public Texture2D FromImage;
    public Texture2D ToImage;

    [Header("Main Menu Holder")]
    public Transform MainMenuHolder;

    // Start is called before the first frame update
    void Start()
    {
        UICamera = GetComponentInChildren<Camera>();
        for (int i = 0; i < UIElements.Length; i++)
        {
            var element = UIElements[i];
            var inst = Instantiate(element.prefab, transform);
            element.GenerateIcon(UICamera, inst.transform);
            inst.SetActive(false);
            Destroy(inst);
        }
        UICamera.targetTexture = null;
        //Destroy(UICamera.gameObject);
        SetupBuildMenuTabs();
        SetupBuildMenu();
    }

    void SetupBuildMenuTabs()
    {
        foreach (Category categ in System.Enum.GetValues(typeof(Category)))
        {
            var inst = Instantiate(Tab, TabHolder.transform);
            Tab tabOpt = inst.GetComponent<Tab>();

            tabOpt.myCategory = categ;
            tabOpt.myName = categ.ToString().ToUpper();
            tabOpt.setName();
        }
    }

    void SetupBuildMenu()
    {
        foreach (Transform child in BuildItemsHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach(UIElement element in UIElements)
        {
            if (element.isActive)
            {
                var inst = Instantiate(BuildContentItem, BuildItemsHolder.transform);
                ItemSetup setup = inst.GetComponent<ItemSetup>();
                setup.rendTexture = element.prefabIcon;
                setup.radius = 2;
                setup.textureHeight = element.prefabIcon.height;
                setup.textureWidth = element.prefabIcon.width;
                setup.prefab = element.prefab;
                setup.SetIcon();
            }
        }
    }

    void Update()
    {
        if (!GameManager.Instance.Running)
        {
            EnableMenuButtons();
            if (currentMode != MenuMode.BuildMode)
            {
                EditMenuCanvas.SetActive(false);
            }
            if (currentMode == MenuMode.ActionsMode)
            {
                if (SceneObjectsManager.Instance.selectedPerson != null)
                {
                    ClearActionItemsBuffer();
                    LinkedListNode<Instruction> node = SceneObjectsManager.Instance.selectedPerson.Instructions.First;
                    while (node != null)
                    {
                        if (node.Value.interactionType == InteractionType.Get)
                        {
                            LinkedListNode<Instruction> from = node;
                            node = node.Next;
                            AddActionItem(from, node);
                        }
                        else if (node.Value.interactionType == InteractionType.WorkAt)
                        {
                            AddActionItem(node);
                        }
                        node = node.Next;
                    }
                }
                else
                {
                    ClearActionItemsBuffer();
                }
            }
        }
        else
        {
            DisableMenuButtons();
            DeactivateAllMenus();
        }
    }

    public void ActivateOrDeactivateBuildEditMenu()
    {
        if (currentMode == MenuMode.BuildMode)
        {
            bool isactive = EditMenuCanvas.activeSelf;

            EditMenuCanvas.SetActive(!isactive);
        }
        SceneObjectsManager.Instance.editMenuIsActive = EditMenuCanvas.activeSelf;
    }
    public void ActivateOrDeactivateActionOptionsMenu()
    {
        if (currentMode == MenuMode.ActionsMode)
        {
            bool isactive = ActionsOptionsMenuCanvas.activeSelf;

            ActionsOptionsMenuCanvas.SetActive(!isactive);
        }
    }

    public void ShowOfCategory(Category cat)
    {
        bool showall = false;

        if (cat == Category.all)
            showall = true;

        foreach(UIElement elem in UIElements)
        {
            if (elem.myCategory == cat)
                elem.isActive = true;
            else
                elem.isActive = false;

            if (showall)
                elem.isActive = true;
        }
        SetupBuildMenu();
    }
    
    public void DisableMenuButtons()
    {
        if (MainMenuHolder != null)
        {
            foreach (Transform child in MainMenuHolder)
            {
                child.GetComponent<Button>().interactable = false;
            }
        }
    }
    public void EnableMenuButtons()
    {
        if (MainMenuHolder != null)
        {
            foreach (Transform child in MainMenuHolder)
            {
                child.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void ActivateOrDeactivateMenu(MenuMode mode)
    {
        bool isactive = false;

        switch (mode)
        {
            case MenuMode.BuildMode:
                isactive = BuildCanvas.activeSelf;
                BuildCanvas.SetActive(!isactive);

                if (!BuildCanvas.activeSelf)
                    EditMenuCanvas.SetActive(false);
                
                if (ActionsCanvas != null) ActionsCanvas.SetActive(false);
                if (ActionsOptionsMenuCanvas != null) ActionsOptionsMenuCanvas.SetActive(false);
                if (StorageCanvas != null) StorageCanvas.SetActive(false);

                break;
            case MenuMode.ActionsMode:
                isactive = ActionsCanvas.activeSelf;
                ActionsCanvas.SetActive(!isactive);

                if (!ActionsCanvas.activeSelf)
                    ActionsOptionsMenuCanvas.SetActive(false);

                if (BuildCanvas != null) BuildCanvas.SetActive(false);
                if (EditMenuCanvas != null) EditMenuCanvas.SetActive(false);
                if (StorageCanvas != null) StorageCanvas.SetActive(false);
                break;
            case MenuMode.StorageMode:
                isactive = StorageCanvas.activeSelf;
                if (StorageCanvas != null) StorageCanvas.SetActive(!isactive);
                if (BuildCanvas != null) BuildCanvas.SetActive(false);
                if (EditMenuCanvas != null) EditMenuCanvas.SetActive(false);
                if (ActionsCanvas != null) ActionsCanvas.SetActive(false);
                if (ActionsOptionsMenuCanvas != null) ActionsOptionsMenuCanvas.SetActive(false);
                break;
            default:
                break;
        }

        if (!isactive)
            currentMode = mode;
        else
            currentMode = MenuMode.None;
    }

    public void DeactivateAllMenus()
    {
        if (BuildCanvas != null) BuildCanvas.SetActive(false);
        if (EditMenuCanvas != null) EditMenuCanvas.SetActive(false);
        if (ActionsCanvas != null) ActionsCanvas.SetActive(false);
        if (StorageCanvas != null) StorageCanvas.SetActive(false);
    }

    public void AddActionItem(LinkedListNode<Instruction> from, LinkedListNode<Instruction> to = null)
    {
        var inst = Instantiate(ActionsContentItem, ActionItemsHolder.transform);
        ActionSetup set = inst.GetComponent<ActionSetup>();
        set.FromHolder = from;
        set.ToHolder = to;
    }

    public void ClearActionItemsBuffer()
    {
        foreach (Transform action in ActionItemsHolder.transform)
        {
            Destroy(action.gameObject);
        }
    }
}

[System.Serializable]
public enum MenuMode
{
    BuildMode,
    ActionsMode,
    PlaybackMode,
    StorageMode,
    None
}