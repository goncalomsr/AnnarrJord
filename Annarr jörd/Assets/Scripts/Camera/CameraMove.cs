using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CameraBehaviorType {Static, MoveToPosition, RotateAround};

[Serializable]
public class CameraInfo
{
    public string Description;
    public CameraBehaviorType behavior;
    public Transform startPostion;
    public Transform endPostion;
    public Transform target;
    public float speed;
    public float timer;
    public float waitTime;
    public bool startTimerNow;
}

public class CameraMove : MonoBehaviour
{
    public CameraInfo[] cameraInfo;
    private int currentCameraIndex;


    void Start()
    {
        currentCameraIndex = 0;
        transform.position = cameraInfo[currentCameraIndex].startPostion.position;

        Time.timeScale = 1;
    }

    void Update()
    {
        if(currentCameraIndex == 24)
        {
            Time.timeScale = 1;
        }

        if (currentCameraIndex < cameraInfo.Length)
        {
            if (cameraInfo[currentCameraIndex].behavior == CameraBehaviorType.Static)
            {
                transform.position = cameraInfo[currentCameraIndex].startPostion.position;
                transform.LookAt(cameraInfo[currentCameraIndex].target);
                cameraInfo[currentCameraIndex].timer += Time.deltaTime;
                if (cameraInfo[currentCameraIndex].timer > cameraInfo[currentCameraIndex].waitTime)
                {
                    cameraInfo[currentCameraIndex].timer = 0;
                    currentCameraIndex++;
                    if (currentCameraIndex < cameraInfo.Length)
                    {
                        transform.position = cameraInfo[currentCameraIndex].startPostion.position;
                    }                        
                }
            }
            else if (cameraInfo[currentCameraIndex].behavior == CameraBehaviorType.MoveToPosition)
            {
                transform.LookAt(cameraInfo[currentCameraIndex].target);
                transform.position = Vector3.MoveTowards(transform.position, cameraInfo[currentCameraIndex].endPostion.position,
                                                         cameraInfo[currentCameraIndex].speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, cameraInfo[currentCameraIndex].endPostion.position) < 0.1f || cameraInfo[currentCameraIndex].startTimerNow)
                {
                    cameraInfo[currentCameraIndex].timer += Time.deltaTime;
                    if (cameraInfo[currentCameraIndex].timer > cameraInfo[currentCameraIndex].waitTime)
                    {
                        cameraInfo[currentCameraIndex].timer = 0;
                        currentCameraIndex++;
                        if (currentCameraIndex < cameraInfo.Length)
                        {
                            transform.position = cameraInfo[currentCameraIndex].startPostion.position;
                        }
                    }
                }
            }
            else if (cameraInfo[currentCameraIndex].behavior == CameraBehaviorType.RotateAround)
            {
                transform.RotateAround(cameraInfo[currentCameraIndex].target.position, new Vector3(0,1,0), cameraInfo[currentCameraIndex].speed * Time.deltaTime);
                transform.LookAt(cameraInfo[currentCameraIndex].target);
                cameraInfo[currentCameraIndex].timer += Time.deltaTime;

                if (cameraInfo[currentCameraIndex].timer > cameraInfo[currentCameraIndex].waitTime)
                {
                    cameraInfo[currentCameraIndex].timer = 0;
                    currentCameraIndex++;
                    if (currentCameraIndex < cameraInfo.Length)
                    {
                        transform.position = cameraInfo[currentCameraIndex].startPostion.position;
                    }
                }
            }
        }     

    }
}
