using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavelineHandler : MonoBehaviour
{
    [SerializeField] private int tomahawkDamage;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.CompareTag("Enemy"))
        {
            other.transform.GetComponent<EnemyHealthHandler>().dealDamage(tomahawkDamage);
            Destroy(this.gameObject);
        }
        
    }
}
