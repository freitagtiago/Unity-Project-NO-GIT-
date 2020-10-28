using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    Rigidbody2D playerRB;
    Animator animator;
    
    [Header("Physics")]
    float movementSpeed = 2f;

    [Header("Controllers")]
    public static PlayerController playerInstance;
    public string areaTransition;

    Vector3 bottomLeftLimit;
    Vector3 topRightLimit;

    bool canMove = true;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if(playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameManager.instance.SetStats(GetComponent<Stats>(), 0);
    }

    void Update()
    {
        if (canMove)
        {
            //ProcessMovement();
            //ProcessAnimaton();
        }
    }

    private void ProcessAnimaton()
    {
        animator.SetFloat("movex", playerRB.velocity.x);
        animator.SetFloat("movey", playerRB.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            animator.SetFloat("lastmovex", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("lastmovey", Input.GetAxisRaw("Vertical"));
        }
    }

    private void ProcessMovement()
    {
        playerRB.velocity = (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))) * movementSpeed;

        //Limit Player 
        
        transform.position =  new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x)
                                        , Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y)
                                        , transform.position.z);
    }

    public void TransportTo(Vector2 position)
    {
        transform.position = position;
    }

    public void SetBounds(Vector3 bottomLeft, Vector3 topRight)
    {
        bottomLeftLimit = bottomLeft + new Vector3(1f, 1f, 0);
        topRightLimit = topRight + new Vector3(-1f, -1f, 0);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void CancelMovement()
    {
        playerRB.velocity = Vector3.zero;
        canMove = false;
        animator.SetFloat("movex", playerRB.velocity.x);
        animator.SetFloat("movey", playerRB.velocity.y);
    }

    public bool GetCanMove()
    {
        return canMove;
    }
}
