using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateUser : MonoBehaviour
{
    [SerializeField] string nombre, apellidos, correo, password, nickname, date;
    [SerializeField] int tel;

    string _URL = "http://localhost/LastFantasy/conn.php";

    IEnumerator CrearUsuario(string _date, string _nombre, string _apellidos, string _correo, string _password, string _nickname, int _tel)
    {
        WWWForm form = new WWWForm();

        form.AddField("nombre",_nombre);
        form.AddField("apellidos",_apellidos);
        form.AddField("correo",_correo);
        form.AddField("fecha_nacimiento",_date);
        form.AddField("telefono",_tel);
        form.AddField("password",_password);
        form.AddField("nickname",_nickname);

        using (UnityWebRequest www = UnityWebRequest.Post(_URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete! "+ www.downloadHandler);
            }
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();


            string[] pages = uri.Split('/');
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
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Entro");

            StartCoroutine(CrearUsuario(date, nombre, apellidos, correo, password, nickname, tel));
        
        }
    }


}
