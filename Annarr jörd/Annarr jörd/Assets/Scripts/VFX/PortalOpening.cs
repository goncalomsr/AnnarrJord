using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalOpening : MonoBehaviour
{
    public GameObject portal1, portal2;


    public Vector3 maxLocalScale;
    private float maxlocalScaleMagnitude;
    public float timer;
    public float ScaleTime1;
    public float ScaleTime2;
    public float ScaleTime3;
    public bool Scale1;
    public bool Scale2;
    public bool Scale3;
    public float Pos2Time;
    public float Pos3Time;
    public Transform[] newPos;
    public bool secondEnding;


     // Start is called before the first frame update
    void Start()
    {
        maxlocalScaleMagnitude = maxLocalScale.magnitude;
        portal1.transform.localScale = new Vector3(0, 0, 0);
        portal2.transform.localScale = new Vector3(0, 0, 0);
        transform.position = newPos[0].position;
    }

      // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float actualLocalScaleMagnitude = portal1.transform.localScale.magnitude;

        if (timer > ScaleTime1)
        {
            Scale1 = true;

            if (Scale1 == true && (actualLocalScaleMagnitude < maxlocalScaleMagnitude))
            {
                portal1.transform.localScale += new Vector3(0.2F, 0.4F, 0) * Time.deltaTime;
                portal2.transform.localScale += new Vector3(0.2F, 0.4F, 0) * Time.deltaTime;
            }
        }

        if (timer > Pos2Time && Scale2 == false)
        {
            transform.position = newPos[1].position;
            portal1.transform.localScale = new Vector3(1f, 2f, 0);
            portal1.transform.rotation = newPos[1].rotation;
            portal2.transform.localScale = new Vector3(1f, 2f, 0);
            portal2.transform.rotation = newPos[1].rotation;  
        }

        if (secondEnding == false)
        {
            if (timer > Pos3Time && Scale2 == false)
            {
                transform.position = newPos[2].position;
                portal1.transform.localScale = new Vector3(0, 0, 0);
                portal1.transform.rotation = newPos[2].rotation;
                portal2.transform.localScale = new Vector3(0, 0, 0);
                portal2.transform.rotation = newPos[2].rotation;
            }

            if (timer > ScaleTime2)
            {
                Scale2 = true;

                if (Scale2 == true && (actualLocalScaleMagnitude < maxlocalScaleMagnitude))
                {
                    portal1.transform.localScale += new Vector3(0.2F, 0.4F, 0) * Time.deltaTime;
                    portal2.transform.localScale += new Vector3(0.2F, 0.4F, 0) * Time.deltaTime;
                }
            }
        }
        else if (secondEnding == true)
        {
            if (timer > Pos3Time && Scale2 == false)
            {
                transform.position = newPos[3].position;
                portal1.transform.localScale = new Vector3(0, 0, 0);
                portal1.transform.rotation = newPos[3].rotation;
                portal2.transform.localScale = new Vector3(0, 0, 0);
                portal2.transform.rotation = newPos[3].rotation;
            }

            if (timer > ScaleTime3 && secondEnding == true)
            {
                Scale3 = true;

                if (Scale3 == true && (actualLocalScaleMagnitude < maxlocalScaleMagnitude))
                {
                    portal1.transform.localScale += new Vector3(0.2F, 0.4F, 0) * Time.deltaTime;
                    portal2.transform.localScale += new Vector3(0.2F, 0.4F, 0) * Time.deltaTime;
                }
            }
        }   
    }
}
