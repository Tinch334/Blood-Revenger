using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDuration;

    private AudioManager am;
    private float currentAttackCooldown;
    private string[] KatanaClips = { "Katana_v1", "Katana_v2", "Katana_v3" };


    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
        currentAttackCooldown = 0;
        attackCollider.enabled = false;
    }


    void Update()
    {
        if (currentAttackCooldown <= 0 && Input.GetMouseButtonDown(0))
        {
            KatanaAttack();
            Debug.Log("Attack");
        }
        else
        {
            currentAttackCooldown -= Time.deltaTime;
        }
    }

    private void KatanaAttack()
    {
        Animator anim = this.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        currentAttackCooldown = attackCooldown;
        attackCollider.enabled = true;
        Invoke("disableAttack", attackDuration);
    }

    private void disableAttack()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            am.PlayRandomNoise(KatanaClips);
            other.GetComponent<EnemyHealthHandler>().dealDamage(attackDamage);
        }
    }
}   