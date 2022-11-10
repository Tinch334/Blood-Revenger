using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeMovimiento : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float dashingSpeed;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;

    private AudioManager am;
    private Rigidbody rb;
    private Animator animador;
    private bool canDash = true;
    private bool isDashing = false;
    private float hspeed;
    private float vspeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animador = GetComponent<Animator>();
        am = FindObjectOfType<AudioManager>();
    }


    void Update()
    {
        if (isDashing)
        {
            return;
        }

        hspeed = Input.GetAxisRaw("Horizontal");
        vspeed = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && (Mathf.Abs(hspeed) > 0 || Mathf.Abs(vspeed) > 0))
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        HandleMouseInput();

        if (isDashing)
        {
            return;
        }

        HandleMovementInput();
    }

    IEnumerator Dash()
    {   
        Animator anim = this.GetComponent<Animator>();
        am.Play("Dash");
        anim.SetTrigger("Dash");
        canDash = false;
        isDashing = true;

        rb.velocity = dashingSpeed * Time.fixedDeltaTime * new Vector3(hspeed, 0f, vspeed).normalized;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    void HandleMouseInput()
    {
        Vector3 mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
    void HandleMovementInput()
    {
        rb.velocity = movementSpeed * Time.fixedDeltaTime * new Vector3(hspeed, 0f, vspeed).normalized;
        animador.SetFloat("xSpeed", (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z)) / 2);

        if(Mathf.Abs(hspeed) != 1 && Mathf.Abs(vspeed) != 1 && !isDashing)
        {
            rb.velocity = Vector3.zero;
        }
        /*else
        {
            am.Play("Walk");
        }*/
    }
}