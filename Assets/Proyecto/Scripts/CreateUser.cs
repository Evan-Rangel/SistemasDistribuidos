using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateUser : MonoBehaviour
{
    public class  Date
    {
        [SerializeField, Range(1980, 2023)] int year;
        [SerializeField, Range(1, 12)] int month;
        [SerializeField, Range(1, 31)] int day;
    }

    //[SerializeField] Date date;
    [SerializeField] string nombre, apellidos, correo, password, nickname, date;
    [SerializeField] int tel;

    string _URL = "C:/wamp64/www/LastFantasy/conn.php";

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


        UnityWebRequest www = UnityWebRequest.Post(_URL, form);

        yield return www.SendWebRequest();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CrearUsuario(date,nombre,apellidos,correo,password,nickname,tel);
        }
    }


}
