using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum InteractionType
{
    Get,
    Put,
    WorkAt
}

[System.Serializable]
public class Instruction
{
    public InteractionType interactionType;
    public IPut putter;
    public IGet getter;
    public IOperate workStation;
    public Item target;
    public Transform location;
    public int index = 0;
    public GameObject itemManagerObject;

    public Instruction(InteractionType type, GameObject im, Item item)
    {
        target = item;
        itemManagerObject = im;
        interactionType = type;
        switch (type)
        {
            case InteractionType.Get:
                getter = im.GetComponent<IGet>();
                break;
            case InteractionType.Put:
                putter = im.GetComponent<IPut>();
                break;
            case InteractionType.WorkAt:
                workStation = im.GetComponent<IOperate>();
                break;
            default:
                break;
        }
        location = im.transform;
    }
}

[System.Serializable]
[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator)), RequireComponent(typeof(Outline))]
public class Person : MonoBehaviour, IBuyable
{
    public float price;
    public LinkedList<Instruction> Instructions = new LinkedList<Instruction>();
    NavMeshAgent navMeshAgent;
    Animator animator;

    public LinkedListNode<Instruction> begin;
    public Vector3 startPos;
    public Item item;
    public GameObject station;

    public GameObject selectedDecal;

    //BONUS MARKS (IK HANDLES HERE :P)
    [Header("Player Hand IK")]
    public bool ikActive;
    public GameObject carryBox_IK;
    public Transform carryBox_IK_Parent;
    private GameObject carryBox_IK_Spawn;
    private Transform CarryPositionRight, CarryPositionLeft;

    // Start is called before the first frame update
    void Start()
    {
        station = null;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        selectedDecal = transform.Find("SelectedDecal").gameObject;
        selectedDecal.SetActive(false);
    }

    public void SetupStartingPos()
    {
        startPos = transform.position;
    }

    private void RotateTowards(Vector3 direction)
    {
        direction = direction.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
    }
    
    public float GetPrice()
    {
        return price;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        if (Instructions.Count != 0)
        {
            if (station != null)
            {
                navMeshAgent.SetDestination(station.transform.position);
                if (navMeshAgent.remainingDistance < 0.6f)
                    RotateTowards(station.transform.forward);
                return;
            }
            if (GameManager.Instance.Running)
            {
                navMeshAgent.isStopped = false;
                if (Vector3.Distance(Instructions.First.Value.location.position, transform.position) < 2)
                {
                    DoAction();
                }
                navMeshAgent.SetDestination(Instructions.First.Value.location.position);
            }
            else
            {
                navMeshAgent.Warp(startPos);
                navMeshAgent.isStopped = true;
                if (item)
                    GameObject.Destroy(item.gameObject);
                while (begin != Instructions.First)
                {
                    NextInstruction();
                }
            }
        }
    }

    public void AddCarry(GameObject from, GameObject to, Item item)
    {
        Instructions.AddLast(new Instruction(InteractionType.Get, from, item));
        if (Instructions.Count == 1)
            begin = Instructions.Last;
        Instructions.AddLast(new Instruction(InteractionType.Put, to, item));
    }

    public void RemoveInstruction(LinkedListNode<Instruction> node)
    {
        if (node.Value.interactionType == InteractionType.Get)
        {
            Instructions.Remove(node.Next);
            Instructions.Remove(node);
        }
        else if (node.Value.interactionType == InteractionType.Put)
        {
            Instructions.Remove(node.Previous);
            Instructions.Remove(node);
        }
        else if (node.Value.interactionType == InteractionType.WorkAt)
        {
            Instructions.Remove(node);
        }
    }

    public void AddWork(GameObject _station)
    {
        Instructions.AddLast(new Instruction(InteractionType.WorkAt, _station, null));
        if (Instructions.Count == 1)
            begin = Instructions.Last;
    }

    void NextInstruction()
    {
        navMeshAgent.isStopped = false;
        if (Instructions.Count > 1)
        {
            var node = Instructions.First;
            Instructions.RemoveFirst();
            Instructions.AddAfter(Instructions.Last, node);
        }
    }

    void DoAction()
    {
        var node = Instructions.First;
        if (node.Value.interactionType == InteractionType.Get)
        {
            item = node.Value.getter.get(node.Value.target);
            if (item != null)
            {
                NextInstruction();
            }
            else
            {
                navMeshAgent.isStopped = true;
            }
        }
        else if (node.Value.interactionType == InteractionType.Put)
        {
            if (node.Value.putter.put(item))
            {
                item = null;
                NextInstruction();
            }
            else
            {
                navMeshAgent.isStopped = true;
            }
        }
        else if (node.Value.interactionType == InteractionType.WorkAt)
        {
            if (!node.Value.workStation.InitOperation(this))
            {
                NextInstruction();
            }
        }
    }

    public void WorkFinished(ItemManager manager)
    {
        manager.EndOperation(this);
        NextInstruction();
    }

    #region Hand IK
    public void OnAnimatorIK()
    {
        if (animator == null)
            return;

        if (ikActive == false)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        }
        else
        {
            if (item == null)
            {
                if (carryBox_IK_Spawn != null)
                    Destroy(carryBox_IK_Spawn);
                return;
            }
            else
            {
                if (carryBox_IK_Spawn == null)
                {
                    carryBox_IK_Spawn = GameObject.Instantiate(carryBox_IK, carryBox_IK_Parent);
                    carryBox_IK_Spawn.transform.localPosition = Vector3.zero;

                    CarryPositionRight = carryBox_IK_Spawn.transform.Find("Right Hand IK").transform;
                    CarryPositionLeft = carryBox_IK_Spawn.transform.Find("Left Hand IK").transform;
                }
            }

            if (carryBox_IK == null)
                return;

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, CarryPositionRight.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, CarryPositionLeft.position);
        }
    }
    #endregion
}