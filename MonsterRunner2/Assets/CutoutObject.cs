using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    public Transform targetObject;

    [SerializeField] private LayerMask wallMask;

    public Camera mainCamera;

    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);


        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitobjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for(int i = 0; i < hitobjects.Length; ++i)
        {
            Material[] materials = hitobjects[i].transform.GetComponent<Renderer>().materials;

            for(int m = 0; i < hitobjects.Length; ++m)
            {
                materials[m].SetVector("CutoutPosition",cutoutPos);
                materials[m].SetFloat("_CutoutSize", 0.1f);
                materials[m].SetFloat("_FalloffSize", 0.05f);
            }
        }
    }
}
