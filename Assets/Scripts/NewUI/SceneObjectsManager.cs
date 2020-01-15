using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsManager : MonoBehaviour
{
    #region Singleton Access
    private static SceneObjectsManager instance;//Use of a singleton here, needs to be static in order for other scripts to access it.

    public static SceneObjectsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SceneObjectsManager>();
            }

            return instance;
        }
    }
    #endregion
    [HideInInspector] public bool editMenuIsActive;
    [HideInInspector] public bool actionsMenuIsActive;
    [HideInInspector] public UIElementController selectedInstance;

    [HideInInspector] public bool assigningTask = false;
    [HideInInspector] public Person selectedPerson = null, previousPerson = null;
    public GameObject firstSelection;
    private Vector3 InitialHit, EndHit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (selectedPerson != null)
            {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(r.origin, r.direction, out RaycastHit rh);
                if (rh.transform.gameObject.GetComponent<IGet>() != null  || rh.transform.gameObject.GetComponent<IOperate>() != null)
                {
                    firstSelection = rh.transform.gameObject;
                    InitialHit = firstSelection.transform.position;
                    LineRenderSystem.Instance.lineRenderer.enabled = true;
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            LineRenderSystem.Instance.lineRenderer.material.SetColor("_Color", Color.grey);
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(r.origin, r.direction, out RaycastHit rh);
            EndHit = rh.point;

            LineRenderSystem.Instance.lineRenderer.SetPosition(0, new Vector3(InitialHit.x, 0.1f, InitialHit.z));

            if (rh.transform.gameObject.GetComponent<IPut>() != null && firstSelection.GetComponent<IGet>() != null)
            {
                EndHit = rh.transform.position;
                
                IGet temp = firstSelection.GetComponent<IGet>();
                if (temp != null)
                {
                    foreach(Ingredient i in temp.GetOutputs())
                    {
                        if (rh.transform.GetComponent<IPut>().CanTake(i.item))
                        {
                            LineRenderSystem.Instance.lineRenderer.material.SetColor("_Color", Color.green);
                        }
                        else
                        {
                            LineRenderSystem.Instance.lineRenderer.material.SetColor("_Color", Color.red);
                        }
                    }
                }
            }
            else
            {
                LineRenderSystem.Instance.lineRenderer.SetPosition(1, new Vector3(EndHit.x, 0.1f, EndHit.z));
            }

            LineRenderSystem.Instance.lineRenderer.SetPosition(1, new Vector3(EndHit.x, 0.1f, EndHit.z));
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (firstSelection != null)
            {
                RaycastHit rh;
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(r.origin, r.direction, out rh);
                if (firstSelection == rh.transform.gameObject && rh.transform.gameObject.GetComponent<IOperate>() != null)
                {
                    selectedPerson.AddWork(firstSelection);
                    Debug.Log("Added Work At: " + firstSelection.name);
                }
                else if (rh.transform.gameObject.GetComponent<IPut>() != null && firstSelection.GetComponent<IGet>() != null)
                {
                    IGet temp = firstSelection.GetComponent<IGet>();

                    foreach(Ingredient i in temp.GetOutputs())
                    {
                        if (rh.transform.GetComponent<IPut>().CanTake(i.item))
                        {
                            selectedPerson.AddCarry(firstSelection, rh.transform.gameObject, firstSelection.GetComponent<IGet>().GetOutputs()[0].item);
                            break;
                        }
                        else
                        {
                            LineRenderSystem.Instance.lineRenderer.material.SetColor("_Color", Color.red);
                        }
                    }
                }
            }


            firstSelection = null;
            LineRenderSystem.Instance.lineRenderer.enabled = false;
        }
        if (editMenuIsActive)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RemoveSelectedInstance();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                TriggerTransformState();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                CloseEditMenu();
            }
        }

        if (UIElementManager.Instance.currentMode == MenuMode.ActionsMode)
        {
            
        }

        DeselectSelectedPerson();
    }

    public void RemoveSelectedInstance()
    {
        if (selectedInstance != null && editMenuIsActive)
        {
            Destroy(selectedInstance.gameObject);
        }
    }

    public void TriggerTransformState()
    {
        if (selectedInstance != null && editMenuIsActive)
        {
            selectedInstance.isNewInstance = true;
        }
    }

    public void CloseEditMenu()
    {
        UIElementManager.Instance.ActivateOrDeactivateBuildEditMenu();
    }

        private void DeselectSelectedPerson()
    {
        if (previousPerson != selectedPerson)
        {
            previousPerson = selectedPerson;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Person personComponent;
                    if (hit.transform.gameObject.TryGetComponent<Person>(out personComponent))
                    {
                        ResetPersonSelection();
                    }
                }
            }
            return;
        }

        /*
        if (selectedPerson == null && previousPerson )
            return;
        */

    }


    //private void DeselectPerson()
    //{
        /*
        if (assigningTask == false)
            return;
        */

        /*
        if (selectedPerson.queuedTasks[selectedPerson.queuedTasks.Count - 1].From == null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    ItemManager item = hit.transform.GetComponent<ItemManager>();

                    if (item != null)
                        selectedPerson.queuedTasks[selectedPerson.queuedTasks.Count - 1].From = item;
                }

                ResetTaskSelection();
            }

            return;
        }

        if (selectedPerson.queuedTasks[selectedPerson.queuedTasks.Count - 1].To == null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    ItemManager item = hit.transform.GetComponent<ItemManager>();

                    if (item != null)
                        selectedPerson.queuedTasks[selectedPerson.queuedTasks.Count - 1].To = item;
                }

                ResetTaskSelection();
            }

            return;
        }
        */

        /*
        if (Input.GetKeyDown(KeyCode.Mouse0) && selectedPerson != null)
        {
            selectedPerson.selectedDecal.SetActive(false);
            selectedPerson = null;
        }

        QueuedTasks newQueuedTask = new QueuedTasks()
        {

        };

        selectedPerson.queuedTasks.Add(newQueuedTask);
        */

        //assignableTask.
    //}

    private void ResetPersonSelection()
    {
        for (int i = 0; i < GameManager.Instance.allObjects.Count; i++)
        {
            GameManager.Instance.allObjects[i].outline.enabled = false;
        }

        if (selectedPerson != null)
        {
            selectedPerson.selectedDecal.SetActive(false);
            selectedPerson = null;
        }
    }
}

