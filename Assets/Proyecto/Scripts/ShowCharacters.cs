using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
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

        public void SetProps(bool _first)
        {
            if (_first)
            {
                charName.RemoveAt(0);
                charName.RemoveAt(0);
            }
            name = new string(charName.ToArray());
           // Debug.Log(name);
            //description = new string(charDescription.ToArray());
            charName.Clear();
            //charDescription.Clear();
        }

    }
    [System.Serializable]
    public class CharacterHolder
    {
        public Image charImageHolde;
        public TMP_Text charNameHolder;
        public Text charDescriptionHolder;
        public Image backgroundImage;

        public void SetHolders(string _name, Sprite _typeOfCharImage)
        {
            charNameHolder.text = _name;
            backgroundImage.sprite = _typeOfCharImage;
        }
    }
    [SerializeField] string txt;

    [SerializeField] List<Character> characters;
    [SerializeField] string newCharacterName;
    [SerializeField] string newCharacterType;
    [SerializeField] List<CharacterHolder> charHolder;
    [SerializeField] Sprite[] typeOfCharImages;
    [SerializeField] Animator book;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] GameObject buttonConfirm;
    [SerializeField] TMP_Text setNameText;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject prevButton;
    [SerializeField] GameObject useButton;



    int createCharIndex ;
    bool createChar = false;

    int charIndex =-1;
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
        //charHolder = new List<CharacterHolder>();
        GetChartactersFunc(PlayerPrefs.GetString("id_usuario"));
    }

    void SplitCharacters(string _text)
    {
        bool newCharacter = true;
        bool typechar=false;
        int charactersIndex=0;
        bool first=true;

        foreach (char _letter in _text)
        {
            if (newCharacter)
            {
                characters.Add(new Character());
                //charHolder.Add(new CharacterHolder());
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
                    characters[charactersIndex].SetProps(first);
                    first = false;
                    
                    break;
                default:
                    if (typechar)
                    {

                        characters[charactersIndex].typeChar = int.Parse(_letter.ToString()) ;
                    }
                    else
                    {
                        characters[charactersIndex].charName.Add(_letter);
                    }
                    break;
            }
        }
    }
    public void NextPage()
    {
        book.SetBool("IsRight", true);
        if (createChar)
        {
            if (createCharIndex<4)
            {
                createCharIndex++;
                Debug.Log(createCharIndex);
                book.SetInteger("Paginas", createCharIndex);
            }
        }
        else
        {
            if (charIndex<characters.Count)
            {
                useButton.SetActive(false);
                StartCoroutine(ActiveInputToSelectCharacter());

                charIndex++;
                book.SetInteger("Paginas", characters[charIndex].typeChar+1);
                nameText.text = characters[charIndex].name;
            }
        }
        StartCoroutine(DisableAnimations());

    }
    public void PreviewPage()
    {
        book.SetBool("IsRight", false);
        if (createChar)
        {
            if (createCharIndex > 0)
            {
                createCharIndex--;
                Debug.Log(createCharIndex);
                book.SetInteger("Paginas", createCharIndex);
            }
        }
        else
        {
            if (charIndex < characters.Count)
            {
                useButton.SetActive(false);
                StartCoroutine(ActiveInputToSelectCharacter());
                charIndex--;
                book.SetInteger("Paginas", characters[charIndex].typeChar + 1);
                nameText.text = characters[charIndex].name;
            }
        }
        StartCoroutine(DisableAnimations());

    }
    public void SelectCharacter()
    {
        book.SetInteger("Paginas", 0);
    }

    public void SelectCharacterButton()
    {
        createChar = false;
        charIndex=0;
        book.SetInteger("Paginas", characters[charIndex].typeChar + 1);
        book.SetBool("IsForCreate", false);
        nameText.text = characters[charIndex].name;
        nextButton.SetActive(true);
        prevButton.SetActive(true);
        GetChartactersFunc(PlayerPrefs.GetString("id_usuario"));
        StartCoroutine(DisableAnimations());
        StartCoroutine(ActiveInputToSelectCharacter());

    }
    IEnumerator SetText()
    {
        yield return new WaitForSeconds( book.GetCurrentAnimatorStateInfo(0).length);
    }

    IEnumerator CreateCharacter()
    {
        WWWForm form = new WWWForm();
        newCharacterName = setNameText.text;
        newCharacterType = (book.GetInteger("Paginas")-1).ToString();
        form.AddField("nombre", newCharacterName);
        form.AddField("type_personaje", newCharacterType);
        form.AddField("id_usuario", PlayerPrefs.GetString("id_usuario"));


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
    public void CreateCharacterButton()
    {
        createCharIndex = 1;
        book.SetBool("IsRight", true);
        book.SetInteger("Paginas", createCharIndex);
        book.SetBool("IsForCreate", true);
        nextButton.SetActive(true);
        prevButton.SetActive(true);
        createChar = true;
        StartCoroutine(DisableAnimations());
        StartCoroutine(ActivateInputToCreateCharacter());

    }
    IEnumerator ActiveInputToSelectCharacter()
    {
        yield return new WaitForSeconds(0.20f);
        useButton.SetActive(true);
    }
    IEnumerator DisableAnimations()
    {
        book.SetBool("Anim", true);

        yield return new WaitForSeconds(0.17f);
        book.SetBool("Anim", false);
    }
    IEnumerator ActivateInputToCreateCharacter()
    {
        yield return new WaitForSeconds(0.2f);
        nameInput.gameObject.SetActive(true);
        nameInput.ActivateInputField();
        buttonConfirm.SetActive(true);
    }
}
