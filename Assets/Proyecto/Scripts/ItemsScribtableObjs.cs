using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Item" , menuName ="New Item")]
public class ItemsScribtableObjs : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string itemName;
    [SerializeField] string description;


    public int Id { get { return id; } }
    public string Name { get { return itemName; } }
    public string Description { get { return description; } }

}
