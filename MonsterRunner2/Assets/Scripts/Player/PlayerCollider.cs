using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public LayerMask enemyLayer;
    public PlayerSO playerData;
    public List<Transform> enemyList = new List<Transform>();

    [SerializeField] float damage;
    [SerializeField] DemoPlayer player;

    private void Start()
    {
        damage = playerData.damage;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<DemoPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            if (!enemyList.Contains(other.transform))
            {
                enemyList.Add(other.transform);
                //Damage enemy logic here, use the damage float
                player.TakeDamage(damage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == enemyLayer)
        {
            if (enemyList.Contains(other.transform))
            {
                //Removes the enemy from the player's list
                enemyList.Remove(other.transform);
            }
        }
    }
}
