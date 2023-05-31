using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class RegisterScript : MonoBehaviour
{
    [SerializeField] Text nombre, coreo, password,confirmPassword, edad, pais, telefono, nickname;

    string _URL= "http://localhost/LastFantasy/registeruser.php";
    string _compareD_URL= "http://localhost/LastFantasy/getuserdata.php";
    [SerializeField]string info;

    IEnumerator CrearUsuario()
    {
        WWWForm form = new WWWForm();

        form.AddField("nombre", nombre.text);
        form.AddField("correo", coreo.text);
        form.AddField("edad", edad.text);
        form.AddField("telefono", telefono.text);
        form.AddField("password", password.text);
        form.AddField("nickname", nickname.text);
        form.AddField("pais", password.text);

        using (UnityWebRequest www = UnityWebRequest.Post(_URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete! " + www.downloadHandler);
            }
        }
    }

    IEnumerator CompareDataBase()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_compareD_URL))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = _compareD_URL.Split('/');
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
                    info = webRequest.downloadHandler.text;

                    CompareNicknames();
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    void CompareNicknames()
    {
        List<string> nicknames = new List<string>();
        List<char> charNicknames = new List<char>();
        int indexNicknames = 0;
        
        for (int i = 0; i < info.Length; i++)
        {
            if (info[i]=='_')
            {
                nicknames.Add("");
                if (nicknames.Count>1)
                {
                    nicknames[indexNicknames] = new string(charNicknames.ToArray());
                    charNicknames.Clear();
                    indexNicknames++;
                }
            }
            else
            {
                charNicknames.Add(info[i]);
            }
        }

        for (int i = 0; i < nicknames.Count; i++)
        {
            if (nicknames[i]==nickname.text)
            {
                Debug.Log("mismo nickname");
                return;
            }
        }

        StartCoroutine(CrearUsuario());

    }

    public void RegisterUser()
    {
        if (password.text== confirmPassword.text)
        {
            StartCoroutine(CompareDataBase());
        }
        else
        {
            Debug.Log("Contrasenias no coinciden");
        }
    }


}
