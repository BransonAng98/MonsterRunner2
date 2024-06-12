using UnityEngine;

public class AbilitySO : ScriptableObject
{
    //Public Variables 
    public string abilityName;
    public float abilityCD;
    public float abilityActive;

    public virtual void AssignVariables(Transform origin, Transform player) { }
    public virtual void Activate() { }

    public virtual void UpdateSkillLevel(VehicleData data) { }
    public virtual void Deactive() { }
}
