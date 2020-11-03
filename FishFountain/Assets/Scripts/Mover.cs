using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float speed = 5f;
    [SerializeField] float currentSpeed;
    [SerializeField] float jumpForce = 300f;

    [Header("Control Variables")]
    [SerializeField] bool isGrounded = true;
    [SerializeField] bool canMove = true;

    [Header("Components")]
    [SerializeField] Rigidbody2D rig;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if(rig == null)
        {
            rig = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        currentSpeed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (canMove)
        {
            ProcessMovement();
            ProcessAnimation();
        }
    }

    private void ProcessMovement()
    {
        if (isGrounded)
        {
            rig.velocity = (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))) * currentSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow) && isGrounded)
        {
            rig.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }
    }

    private void ProcessAnimation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("isMoving", true);
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("isMoving",true);
            spriteRenderer.flipX = false;
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            rig.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }
    }

 #region PUBLIC METHODS
    public void SetIsGrounded(bool value) 
    {
        isGrounded = value;
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void SetCurrentSpeed(int weight)
    {
        if (weight <= 10)
        {
            currentSpeed = speed;
        }
        else if (weight <= 20)
        {
            currentSpeed *= 0.75f;
        }
        else
        {
            currentSpeed *= 0.5f;
        }
    }

    #endregion
}
