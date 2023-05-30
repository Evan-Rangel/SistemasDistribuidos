using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoginScript : MonoBehaviour
{
    [SerializeField] string mail, password;
    [SerializeField] string text;
    [SerializeField] ShowCharacters showCharacters;
    [SerializeField] Text mailText;
    [SerializeField] Text passwordText;

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

    public void LogIn()
    {
        StartCoroutine(GetUser(mailText.text, passwordText.text));
    }
}
