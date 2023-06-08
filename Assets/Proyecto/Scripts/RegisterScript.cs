using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class RegisterScript : MonoBehaviour
{
    [SerializeField] Text nombre, correo, password,confirmPassword, edad, pais, telefono, nickname;

    string _URL= "http://localhost/LastFantasy/registeruser.php";
    string _compareD_URL = "http://localhost/LastFantasy/getuserdata.php";
    string _duplicado_URL = "http://localhost/LastFantasy/checkduplicateddata.php";
    [SerializeField]string info;
    [SerializeField] List<char> charNicknames;
    [SerializeField] List<string> nicknames;
    IEnumerator CrearUsuario()
    {
        WWWForm form = new WWWForm();

        form.AddField("nombre", nombre.text);
        form.AddField("correo", correo.text);
        form.AddField("edad", edad.text);
        form.AddField("telefono", telefono.text);
        form.AddField("password", password.text);
        form.AddField("nickname", nickname.text);
        form.AddField("pais", pais.text);

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
                StartCoroutine(DeleteDuplicatedData());
            }
        }
    }
    IEnumerator DeleteDuplicatedData()
    {

        using (UnityWebRequest www = UnityWebRequest.Get(_duplicado_URL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Data duplicada borrada");
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

                    CompareNicknames(info);
                    break;
            }
        }
    }

    void CompareNicknames(string _info)
    {
        nicknames = new List<string>();
        charNicknames = new List<char>();
        int indexNicknames = 0;

        for (int i = 0; i < _info.Length; i++)
        {
            if (_info[i]=='_')
            {
                nicknames.Add("");
                nicknames[indexNicknames] = new string(charNicknames.ToArray());
                charNicknames.Clear();
                indexNicknames++;
            }
            else
            {
                charNicknames.Add(_info[i]);
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
        if (password.text=="" || confirmPassword.text== "" || nombre.text== "" || correo.text== "" || telefono.text== "" || edad.text== "" || pais.text== "" || nickname.text== "")
        {
            Debug.Log("Algo es Null");
        }
        else
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


}
