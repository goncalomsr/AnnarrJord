using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataConnection : MonoBehaviour
{
    [Header("Ending")]
    public ElementalMovement[] EM;
    public CameraMove CM;
    public VFX VFX;
    public PortalOpening Portal;
    public int ending;

    [Header("Transformation")]
    public GameObject heroBasic;
    public GameObject heroTransformation;
    public bool transformation = false;


    void Start()
    {
        heroTransformation.SetActive(false);

        StartCoroutine(GetText());

        StartCoroutine(Transformation());
    }

    private void Update()
    {
        if (transformation == true)
        {
            heroTransformation.SetActive(true);
            heroBasic.SetActive(false);
        }
    }

    IEnumerator GetText()
    {
        yield return new WaitForSeconds(125);
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/finaleoption/Php/unityconnection.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;

            /*
            if (www.downloadHandler.text == "0")
            {
                Debug.Log("Light above all");
            }
            else if (www.downloadHandler.text == "1")
            {
                Debug.Log("Darkness is necessary");
                EM.secondEnding = true;
                CM.secondEnding = true;
            }
            */

            if (ending == 0)
            {
                Debug.Log("Light above all");
            }
            else if (ending == 1)
            {
                Debug.Log("Darkness is necessary");
                for (int i = 0; i < EM.Length; i++)
                {
                    EM[i].secondEnding = true;
                }
                CM.secondEnding = true;
                VFX.secondEnding = true;
                Portal.secondEnding = true;
            }
        }
    }

    IEnumerator Transformation()
    {
        yield return new WaitForSeconds(0);
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/finaleoption/Php/Transformation.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;


            if (www.downloadHandler.text == "1")
            {
                transformation = true;
                Debug.Log("Transformation is ready");
            }
            else if (www.downloadHandler.text == "0")
            {
                transformation = false;
                Debug.Log("Transformation is not ready");
            }

        }
    }
}
