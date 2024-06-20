using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThundercrackerGrenade : MonoBehaviour
{
    public float timeToDetonate;
    public GameObject thunderbolt;
    public bool isTriggered;

    public List<enemyCarDriver> affectedEnemyList = new List<enemyCarDriver>();
    [SerializeField] SphereCollider aoeRadius;

    // Start is called before the first frame update
    void Start()
    {
        thunderbolt.SetActive(false);
    }

    void CallThunder()
    {
        Destroy(this.gameObject, 0.1f);
        isTriggered = true;
        thunderbolt.SetActive(true);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius.radius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                enemyCarDriver enemyCar = collider.GetComponent<enemyCarDriver>();

                if (!affectedEnemyList.Contains(enemyCar))
                {
                    enemyCar.CarDeath(1);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToDetonate > 0)
        {
            timeToDetonate -= Time.deltaTime;
        }
        else
        {
            if (!isTriggered)
            {
                CallThunder();
            }
        }

    }
}
