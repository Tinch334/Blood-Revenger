using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomahawkHandler : MonoBehaviour
{
    [SerializeField] private int tomahawkDamage;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colision proyectil");
        if (other.transform.CompareTag("Enemy"))
        {
            other.transform.GetComponent<EnemyHealthHandler>().dealDamage(tomahawkDamage);
        }
        Destroy(this.gameObject);
    }
}
