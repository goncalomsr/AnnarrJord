using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataConnection : MonoBehaviour
{
    public GameObject[] Characters;
    public GameObject Camera;
    public int ending;

    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        yield return new WaitForSeconds(114);
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
                for (int i = 0; i < Characters.Length; i++)
                {
                    Characters[i].GetComponent<ElementalMovement>().enabled = false;
                    Characters[i].GetComponent<ElementalMovement1>().enabled = true;
                }
            }
            */
            if (ending == 0)
            {
                Debug.Log("Light above all");
            }
            else if (ending == 1)
            {
                Debug.Log("Darkness is necessary");

                for (int i = 0; i < Characters.Length; i++)
                {
                    Characters[i].GetComponent<ElementalMovement>().enabled = false;
                    Characters[i].GetComponent<SecondEnding>().enabled = true;
                }

                //Camera.GetComponent<CameraMove>().enabled = false;
                //Camera.GetComponent<CameraMove1>().enabled = false;
            }
        }
    }
}
