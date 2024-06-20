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

    void CallThunder()
    {
        Destroy(this.gameObject, 0.1f);
        isTriggered = true;
        Instantiate(thunderbolt, transform.position, Quaternion.identity);
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
