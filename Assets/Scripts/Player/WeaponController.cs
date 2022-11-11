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
    [SerializeField] private GameObject Tomahawk;
    [SerializeField] private GameObject Javeline;
    [SerializeField] private GameObject Shuriken;
    [SerializeField] private Transform shootingPos;
    [SerializeField] private float projectileSpeed;

    [Header("References")]
    [SerializeField] private Text textElement;
    [SerializeField] private GameObject kunaiImg, tomahawkImg, javelineImg, shurikenImg;
    [SerializeField] private string textContent;

    private AudioManager am;
    [SerializeField] private string weapon = "";
    private float currentAttackCooldown;
    private string[] KatanaClips = { "Katana_v1", "Katana_v2", "Katana_v3" };


    private void Start()
    {
        kunaiImg.SetActive(false);
        tomahawkImg.SetActive(false);
        javelineImg.SetActive(false);
        shurikenImg.SetActive(false);
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
        switch (weapon)
        {
            case "Kunai": throwKunai(); break;
            case "Tomahawk": throwTomahawk(); break;
            case "Javeline": throwJaveline(); break;
            case "Shuriken": throwShuriken(); break;

            default: return;

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

    private void throwTomahawk()
    {
        am.Play("KnifeThrow");
        Animator anim = this.GetComponent<Animator>();
        anim.SetTrigger("Shoot");
        weapon = "";

        GameObject obj = Instantiate(Tomahawk, shootingPos.position, shootingPos.rotation);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        tomahawkImg.SetActive(false);
        textElement.text = textContent;
    }

    private void throwJaveline()
    {
        am.Play("KnifeThrow");
        Animator anim = this.GetComponent<Animator>();
        anim.SetTrigger("Shoot");
        weapon = "";

        GameObject obj = Instantiate(Javeline, shootingPos.position, shootingPos.rotation);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        javelineImg.SetActive(false);
        textElement.text = textContent;
    }

    private void throwShuriken()
    {
        am.Play("KnifeThrow");
        Animator anim = this.GetComponent<Animator>();
        anim.SetTrigger("Shoot");
        weapon = "";

        GameObject obj = Instantiate(Shuriken, shootingPos.position, shootingPos.rotation);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        shurikenImg.SetActive(false);
        textElement.text = textContent;
    }

    private void getKunai()
    {
        weapon = "Kunai";
        kunaiImg.SetActive(true);
        textElement.text = "";
    }

    private void getTomahawk()
    {
        weapon = "Tomahawk";
        tomahawkImg.SetActive(true);
        textElement.text = "";
    }

    private void getJaveline()
    {
        weapon = "Javeline";
        javelineImg.SetActive(true);
        textElement.text = "";
    }

    private void getShuriken()
    {
        weapon = "Shuriken";
        shurikenImg.SetActive(true);
        textElement.text = "";
    }

    private void getWeapon(string weap)
    {
        Debug.Log("Obtuviste: "+weap);
        if (weapon == "")
        {
            switch (weap)
            {
                case "Kunai": getKunai(); break;
                case "Tomahawk": getTomahawk(); break;
                case "Javeline": getJaveline(); break;
                case "Shuriken": getShuriken(); break;

                default: return;
            }
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