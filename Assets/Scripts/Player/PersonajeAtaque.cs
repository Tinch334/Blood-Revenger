using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonajeAtaque : MonoBehaviour
{
    [Header("References")]
    [SerializeField]  private GameObject projectile;
    [SerializeField] private Transform shootingPos;
    [SerializeField] private float projectileSpeed;

    [Header("Throwing")]
    [SerializeField] private KeyCode throwKey = KeyCode.Space;

    [SerializeField] private bool Kunai;
    [SerializeField] private Text textElement;
    [SerializeField] private GameObject kunaiImg;
    [SerializeField] private string textContent;
    private AudioManager am;


    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        getKunai();
    }

    void Update()
    {
        if (Input.GetKeyDown(throwKey) && !Kunai)
        {
            textElement.text = textContent;
        }

        if (Input.GetKeyDown(throwKey) && Kunai)
        {
            Throw();
            kunaiImg.SetActive(false);
        }
    }
    public void getKunai()
    {
        Kunai = true;
        kunaiImg.SetActive(true);
    }
    private void Throw()
    {
        am.Play("KnifeThrow");
        Animator anim = this.GetComponent<Animator>();
        anim.SetTrigger("Shoot");
        Kunai = false;

        GameObject obj = Instantiate(projectile, shootingPos.position, shootingPos.rotation);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);

    }
}
