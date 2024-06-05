using UnityEngine;

public class AbilitySO : ScriptableObject
{
    //Public Variables 
    public string abilityName;
    public float abilityCD;
    public float abilityActive;

    public virtual void Activate() { }
}
