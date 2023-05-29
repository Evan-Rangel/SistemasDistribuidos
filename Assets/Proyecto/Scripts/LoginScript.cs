using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginScript : MonoBehaviour
{
    [SerializeField] string mail, password;
    [SerializeField, TextArea()]string text;

    string _URL = "http://localhost/LastFantasy/loginC.php";

    IEnumerator GetUser(string _mail, string _password)
    {
        WWWForm form = new WWWForm();

        form.AddField("correo", _mail);
        form.AddField("password", _password);



        using (UnityWebRequest www = UnityWebRequest.Post(_URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                /*
                using (UnityWebRequest webRequest = UnityWebRequest.Get(_URL))
                {
                    yield return webRequest.SendWebRequest();
                    string[] pages = _URL.Split('/');
                    int page = pages.Length - 1;

                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                            break;
                        case UnityWebRequest.Result.ProtocolError:
                            Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                            break;
                        case UnityWebRequest.Result.Success:
                            text = webRequest.downloadHandler.text;
                            Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                            break;
                    }
                }
                */
                text = www.downloadHandler.text;
                Debug.Log("Form upload complete! " + www.downloadHandler.text);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Debug.Log("Entro");

            StartCoroutine(GetUser(mail, password));

        }
    }
}
