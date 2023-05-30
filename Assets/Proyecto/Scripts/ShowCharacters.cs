using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShowCharacters : MonoBehaviour
{
    [SerializeField] string txt;

    [SerializeField] string[] charactersName;
    [SerializeField] string[] charactersDescription;

    [SerializeField] string newCharacterName;
    [SerializeField] string newCharacterDescription;


    string _URL = "http://localhost/LastFantasy/getCharacters.php";
    string _URLC = "http://localhost/LastFantasy/createCharacter.php";
    IEnumerator GetCharacters()
    {
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
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator CreateCharacter()
    {
        WWWForm form = new WWWForm();
        form.AddField("nombre", newCharacterName);
        form.AddField("descripcion", newCharacterDescription);

        using (UnityWebRequest www = UnityWebRequest.Post(_URLC, form))
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
    public void GetChartactersFunc()
    {
        StartCoroutine(GetCharacters());
    }
    public void CreateCharacterFunc()
    {
        StartCoroutine(CreateCharacter());
    }

    

}
