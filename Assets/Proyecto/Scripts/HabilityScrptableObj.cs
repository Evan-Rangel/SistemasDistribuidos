using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hability", menuName = "New Hability")]
public class HabilityScrptableObj : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string habilityName;
    [SerializeField] string description;
    [SerializeField] string effect;

    public int Id { get { return id; } }
    public string Name { get { return habilityName; } }
    public string Description { get { return description; } }
    public string Effect { get { return effect; } }
}
