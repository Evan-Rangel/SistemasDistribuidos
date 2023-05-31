using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShowCharacters : MonoBehaviour
{
    [System.Serializable]
    public class Character
    {
        public string name;
        public string description;

        public List<char> charName;
        public List<char> charDescription;
        public Character()
        {
            charName = new List<char>();
            charDescription = new List<char>();

        }

        public void SetProps()
        {
            name = new string(charName.ToArray());
            description = new string(charDescription.ToArray());
            charName.Clear();
            charDescription.Clear();
        }
    }

    [SerializeField] string txt;

    [SerializeField] List<Character> characters;
    [SerializeField] string newCharacterName;
    [SerializeField] string newCharacterDescription;


    string _URL = "http://localhost/LastFantasy/getCharacters.php";
    string _URLC = "http://localhost/LastFantasy/createCharacter.php";

    
    IEnumerator GetCharacters(string _id_user)
    {
        WWWForm form = new WWWForm();

        form.AddField("id_usuario", _id_user);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(_URL, form))
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
                    txt = webRequest.downloadHandler.text;
                    SplitCharacters(txt);
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }
    private void Start()
    {
        characters = new List<Character>();
    }

    void SplitCharacters(string _text)
    {
        bool description=false;
        bool newCharacter = true;
        int charactersIndex = 0;
        for (int i = 0; i < _text.Length; i++)
        {
            if (newCharacter)
            {
                characters.Add(new Character());
                newCharacter = false;
            }

            if (_text[i] != '-' && _text[i] != '_')
            {
                if (!description)
                {
                    characters[charactersIndex].charName.Add(_text[i]);
                }
                else
                {
                    characters[charactersIndex].charDescription.Add(_text[i]);
                }
            }
            if (_text[i] == '-')
            {
                description = true;
            }
            if (_text[i] == '_')
            {
                characters[charactersIndex].SetProps();
                newCharacter = true;
                charactersIndex++;
                description = false;
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
    public void GetChartactersFunc(string _id_user)
    {
        StartCoroutine(GetCharacters(_id_user));
    }
    public void CreateCharacterFunc()
    {
        StartCoroutine(CreateCharacter());
    }

    

}
