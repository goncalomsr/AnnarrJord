using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ElementalBehaviorType { Static, MoveToPosition };

[Serializable]
public class ElementalInfo
{
    public string Description;
    public ElementalBehaviorType behavior;
    public Transform startPostion;
    public Transform endPostion;
    public Transform target;
    public float speed;
    public float timer;
    public float waitTime;
    public Animator anim;
    public string anima;
}

public class ElementalMovement : MonoBehaviour
{
    public ElementalInfo[] elementalInfo;
    private int currentElementalIndex;
    private Rigidbody rb;
    public Vector3 offset;


    void Start()
    {
        currentElementalIndex = 0;
        transform.position = elementalInfo[currentElementalIndex].startPostion.position;
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (currentElementalIndex < elementalInfo.Length)
        {
            if (elementalInfo[currentElementalIndex].behavior == ElementalBehaviorType.Static)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                transform.position = elementalInfo[currentElementalIndex].startPostion.position;
                transform.LookAt(elementalInfo[currentElementalIndex].target);
                elementalInfo[currentElementalIndex].timer += Time.deltaTime;
                elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, true);

                if (elementalInfo[currentElementalIndex].timer > elementalInfo[currentElementalIndex].waitTime)
                {
                    elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, false);
                    elementalInfo[currentElementalIndex].timer = 0;
                    currentElementalIndex++;
                    if (currentElementalIndex < elementalInfo.Length)
                    {
                        transform.position = elementalInfo[currentElementalIndex].startPostion.position;
                    }
                }
            }
            else if (elementalInfo[currentElementalIndex].behavior == ElementalBehaviorType.MoveToPosition)
            {
                rb.constraints = ~RigidbodyConstraints.FreezeAll;
                transform.LookAt(elementalInfo[currentElementalIndex].target);
                transform.position = Vector3.MoveTowards(transform.position, elementalInfo[currentElementalIndex].endPostion.position - offset,
                                                         elementalInfo[currentElementalIndex].speed * Time.deltaTime);
                elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, true);
                if (Vector3.Distance(transform.position, elementalInfo[currentElementalIndex].endPostion.position) < 1f)
                {
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    elementalInfo[currentElementalIndex].anim.SetBool(elementalInfo[currentElementalIndex].anima, false);
                    elementalInfo[currentElementalIndex].timer += Time.deltaTime;
                    if (elementalInfo[currentElementalIndex].timer > elementalInfo[currentElementalIndex].waitTime)
                    {
                        elementalInfo[currentElementalIndex].timer = 0;
                        currentElementalIndex++;
                        if (currentElementalIndex < elementalInfo.Length)
                        {
                            transform.position = elementalInfo[currentElementalIndex].startPostion.position;
                        }
                    }
                }
            }
        }

    }

    /*
    public Transform[] waypoint;
    public Transform waypointToMove;
    public int waypointNum;

    public bool startMove = false;
    public float speed;

    public Rigidbody rb;

    Animator anim;

    void Start()
    {
        startMove = true;
        rb = GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
        transform.position = waypoint[waypointNum].position;
    }

    void Update()
    {
        float moveSpeed = Time.deltaTime * speed;

        if (startMove == true)
        {
            transform.LookAt(waypoint[waypointNum]);
            if (Vector3.Distance(waypoint[waypointNum].position, transform.position) > 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoint[waypointNum].position, moveSpeed);
                anim.SetBool("Run", true);
                if (Vector3.Distance(waypoint[1].position, transform.position) < 2)
                {
                    StartCoroutine("waitingTime");
                    anim.SetBool("Yelling", true);
                }
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                anim.SetBool("Run", false);
            }
            if (Vector3.Distance(waypoint[1].position, transform.position) < 2)
            {
                anim.SetBool("Yelling", true);
            }
        }

        if (Vector3.Distance(waypoint[waypointNum].position, transform.position) < 2 && waypointNum < waypoint.Length - 1)
        {
            waypointNum++;
            //waypointToMove = waypoint[waypointNum];
        }


        
    }

    /*
    public IEnumerator waitingTime()
    {
        if (transform.position == waypoint[1].position)
        {
            yield return new WaitForSeconds(4);
        }
    }
    /

    public IEnumerator meetingForest()
    {
        if (startMove == true)
        {
            transform.LookAt(lookTarget[0]);
            transform.position = Vector3.MoveTowards(transform.position, waypoint[1].position, moveSpeed);
        }
    }
    */
}
