using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginScript : MonoBehaviour
{
    [SerializeField] string mail, password;
    [SerializeField, TextArea()]string text;
    [SerializeField] ShowCharacters showCharacters;
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
                text = www.downloadHandler.text;
                if (text=="null")
                {
                    Debug.Log("a");
                }
                Debug.Log("Form upload complete! " + www.downloadHandler.text);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {

            StartCoroutine(GetUser(mail, password));

        }
    }
}
