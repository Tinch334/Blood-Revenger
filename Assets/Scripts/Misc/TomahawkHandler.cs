using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomahawkHandler : MonoBehaviour
{
    [SerializeField] private int projectileDamage;
    private bool detectedEnemy = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            other.transform.GetComponent<EnemyHealthHandler>().dealDamage(projectileDamage);
            Debug.Log("colision proyectil");
            detectedEnemy = true;
        } 
        else if (!other.transform.CompareTag("Player") && !detectedEnemy)
        {
            Destroy(gameObject);
        }
    }
}
