using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform[] waypoint;
    public bool startMove = false;
    public float speed;

    public Transform[] lookTarget;

    void Start()
    {
        startMove = true;
    }

    void Update()
    {
        float moveSpeed = Time.deltaTime * speed;


        /*if (startMove == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoint[posAtual].position, moveSpeed);
            posAtual++;
        }*/
        if (startMove == true)
        {
            transform.LookAt(lookTarget[0]);
            transform.position = Vector3.MoveTowards(transform.position, waypoint[1].position, moveSpeed);
        }

        if (transform.position == waypoint[1].position)
        {
            startMove = false;
            transform.position = waypoint[2].position;
            transform.LookAt(lookTarget[1]);
        }

        StartCoroutine("waitingTime");

        if (transform.position == waypoint[3].position)
        {
            
            transform.LookAt(lookTarget[2]);
            transform.position = Vector3.Lerp(transform.position, waypoint[4].position, moveSpeed);
            
        }
        
    }

    public IEnumerator waitingTime()
    {
        if (transform.position == waypoint[2].position)
        {
            yield return new WaitForSeconds(2);
            transform.position = waypoint[3].position;
            transform.LookAt(lookTarget[2]);
        }
    }


}
