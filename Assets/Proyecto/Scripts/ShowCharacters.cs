using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ShowCharacters : MonoBehaviour
{
    [System.Serializable]
    
    public class Character
    {
        public string name;
        public string description;
        public int typeChar;
        public int id_Image;
        public List<char> charName;
        public List<char> charDescription;
        public Character()
        {
            charName = new List<char>();
            //charDescription = new List<char>();

        }

        public void SetProps()
        {
            charName.RemoveAt(0);
            charName.RemoveAt(0);

            name = new string(charName.ToArray());
            //description = new string(charDescription.ToArray());
            charName.Clear();
            //charDescription.Clear();
        }

    }
    [System.Serializable]
    public class CharacterHolder
    {
        public Image charImageHolde;
        public Text charNameHolder;
        public Text charDescriptionHolder;

        public void SetHolders(string _name, string _description)
        {
            charNameHolder.text = _name;
            charDescriptionHolder.text = _description;
        }
    }
    [SerializeField] string txt;

    [SerializeField] List<Character> characters;
    [SerializeField] string newCharacterName;
    [SerializeField] string newCharacterDescription;
    [SerializeField] List<CharacterHolder> charHolder;


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
                    //Debug.Log(webRequest.downloadHandler.text);
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
        GetChartactersFunc(PlayerPrefs.GetString("id_usuario"));
    }

    void SplitCharacters(string _text)
    {
        bool newCharacter = true;
        bool typechar=false;
        int charactersIndex=0;

        foreach (char _letter in _text)
        {
            if (newCharacter)
            {
                characters.Add(new Character());
                charactersIndex = characters.Count -1;
                newCharacter = false;
            }

            switch(_letter)
            {
                case '-':
                    typechar = true;
                    break;
                case '_':
                    typechar = false;
                    newCharacter = true;
                    characters[charactersIndex].SetProps();

                    break;
                default:
                    if (typechar)
                    {
                        characters[charactersIndex].charName.Add(_letter);

                    }
                    else
                    {
                        characters[charactersIndex].typeChar=_letter;

                    }
                    break;
            }
        }
        /*
        bool description=false;
        bool id_image = false;
        int charactersIndex = 0;
        
        
        
        foreach (char _letter in _text)
        {
            if (newCharacter)
            {
                characters.Add(new Character());
                newCharacter = false;
            }
            
            switch (_letter)
            {
                case '-':
                    description = true;
                    id_image = false;
                    break;
                case '_':
                    id_image = true;
                    description = false;
                    break;
                case '=':
                    characters[charactersIndex].SetProps();
                    charHolder[0].SetHolders(characters[charactersIndex].name, characters[charactersIndex].description);
                    newCharacter = true;
                    charactersIndex++;
                    description = false;
                    id_image = false;
                    break;

                default:
                    if (!description && !id_image)
                    {
                        characters[charactersIndex].charName.Add(_letter);
                    }
                    if (description && !id_image)
                    {
                        characters[charactersIndex].charDescription.Add(_letter);
                    }
                    if (!description && id_image)
                    {
                        characters[charactersIndex].id_Image = _letter;

                    }
                    break;
                
            }
        }
        */
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
