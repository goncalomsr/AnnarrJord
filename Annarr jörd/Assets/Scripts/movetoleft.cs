using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movetoleft : MonoBehaviour
{
    public Transform targetPos;
    public float speed;
    public float timer;

    void Update()
    {
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
    }
}
