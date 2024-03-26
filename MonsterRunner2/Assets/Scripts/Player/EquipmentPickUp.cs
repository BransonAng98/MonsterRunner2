using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPickUp : MonoBehaviour
{
    public Transform BackEquipmentSlot;
    public Transform FrontEquipmentSlot;
    public Transform TopEquipmentSlot;
    public Transform SideEquipmentSlot1;
    public Transform SideEquipmentSlot2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BackEquipment"))
        {
            SetEquipment(other.gameObject, BackEquipmentSlot);
        }
        else if (other.gameObject.CompareTag("FrontEquipment"))
        {
            SetEquipment(other.gameObject, FrontEquipmentSlot);
        }
        else if (other.gameObject.CompareTag("TopEquipment"))
        {
            SetEquipment(other.gameObject, TopEquipmentSlot);
        }
        else if (other.gameObject.CompareTag("SideEquipment"))
        {
            // Instantiate the object at each side equipment slot
            InstantiateAtSlot(other.gameObject, SideEquipmentSlot1);
            InstantiateAtSlot(other.gameObject, SideEquipmentSlot2);
            Destroy(other.gameObject);
        }
    }

    private void SetEquipment(GameObject equipment, Transform slot)
    {
        // This method parents the existing equipment to the target slot and resets its transform
        equipment.transform.SetParent(slot);
        equipment.transform.localPosition = Vector3.zero;
        equipment.transform.localRotation = Quaternion.identity;
        equipment.transform.localScale = Vector3.one; // Ensure the scale is reset correctly
    }

    private void InstantiateAtSlot(GameObject original, Transform slot)
    {
        // Instantiate a new object and parent it to the target slot
        GameObject clone = Instantiate(original, slot.position, slot.rotation, slot);
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localRotation = Quaternion.identity;
        clone.transform.localScale = Vector3.one; // Ensure the scale is reset correctly

        // Optionally, if the original object should be destroyed after instantiation, add:
        // Destroy(original);
        // Note: Uncommenting the above line will remove the original object after cloning. Use it if necessary.
    }
}
