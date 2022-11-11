using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [Header("Meelee")]
    [SerializeField] private Collider attackCollider;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDuration;

    [Header("Weapons")]
    [SerializeField] private KeyCode throwKey = KeyCode.Space;
    [SerializeField] private GameObject Kunai;
    [SerializeField] private Transform shootingPos;
    [SerializeField] private float projectileSpeed;

    [Header("References")]
    [SerializeField] private Text textElement;
    [SerializeField] private GameObject kunaiImg;
    [SerializeField] private string textContent;

    private AudioManager am;
    private string weapon = "Kunai";
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
            if (Input.GetKeyDown(throwKey))
            {
                useWeapon();
            }
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
    private void useWeapon()
    {
        if (weapon == "Kunai")
        {
            throwKunai();
        }
    }

    private void throwKunai()
    {
        am.Play("KnifeThrow");
        Animator anim = this.GetComponent<Animator>();
        anim.SetTrigger("Shoot");
        weapon = "";

        GameObject obj = Instantiate(Kunai, shootingPos.position, shootingPos.rotation);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        kunaiImg.SetActive(false);
        textElement.text = textContent;
    }

    private void getKunai()
    {
        weapon = "Kunai";
        kunaiImg.SetActive(true);
        textElement.text = "";
    }

    private void getWeapon(string weap)
    {
        if(weap == "Kunai")
        {
            getKunai();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            am.PlayRandomNoise(KatanaClips);
            other.GetComponent<EnemyHealthHandler>().dealDamage(attackDamage);
            getWeapon(other.GetComponent<EnemyHealthHandler>().getWeapon());
        }
    }
}   