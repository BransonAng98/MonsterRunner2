using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowScript : MonoBehaviour
{
    public List<enemyCarDriver> affectedEnemyList = new List<enemyCarDriver>();
    public float ccDuration;

    [SerializeField] SphereCollider aoeRadius;
    // Start is called before the first frame update
    void Start()
    {
        affectedEnemyList.Clear();
        DetectEnemies();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyCarDriver enemyCar = other.GetComponent<enemyCarDriver>();

            if (!affectedEnemyList.Contains(enemyCar))
            {
                affectedEnemyList.Add(enemyCar);
                enemyCar.ccDuration = ccDuration;
                enemyCar.isCCed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyCarDriver enemyCar = other.GetComponent<enemyCarDriver>();

            if (affectedEnemyList.Contains(enemyCar))
            {
                affectedEnemyList.Remove(enemyCar);
            }
        }
    }

    void DetectEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius.radius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                enemyCarDriver enemyCar = collider.GetComponent<enemyCarDriver>();

                if (!affectedEnemyList.Contains(enemyCar))
                {
                    affectedEnemyList.Add(enemyCar);
                    enemyCar.ccDuration = ccDuration;
                    enemyCar.isCCed = true;
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius.radius);
    }

    private void OnDisable()
    {
        affectedEnemyList.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
