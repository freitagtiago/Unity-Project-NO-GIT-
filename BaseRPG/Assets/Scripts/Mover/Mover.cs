using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [Header("Components")]
        private Rigidbody2D playerRB;
        private Animator animator;

        [Header("Physics")]
        [SerializeField]float movementSpeed = 4f;

        [Header("General")]
        private static bool canMove = true;
        public static Action OnCancelMovement;

        [Header("Map Limits")]
        private Vector3 bottomLeftLimit;
        private Vector3 topRightLimit;

        private void Awake()
        {
            playerRB = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }
        private void OnEnable()
        {
            OnCancelMovement += CancelMovement;
        }

        private void OnDisable()
        {
            OnCancelMovement -= CancelMovement;
        }

        // Update is called once per frame
        void Update()
        {
            if (canMove)
            {
                ProcessMovement();
                ProcessAnimaton();
            }
        }

        private void CancelMovement()
        {
            playerRB.velocity = Vector2.zero;
        }
        
        private void ProcessMovement()
        {
            playerRB.velocity = (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))) * movementSpeed;

            //Limit Player 
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x)
                                            , Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y)
                                            , transform.position.z);
        }

        #region ANIMATION EVENTS
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
        #endregion

        #region PUBLIC METHODS
        public void TransportTo(Vector2 position)
        {
            transform.position = position;
        }

        public void SetBounds(Vector3 bottomLeft, Vector3 topRight)
        {
            bottomLeftLimit = bottomLeft + new Vector3(1f, 1f, 0);
            topRightLimit = topRight + new Vector3(-1f, -1f, 0);
        }

        public static void SetCanMove(bool value)
        {
            canMove = value;
            OnCancelMovement();
        }

        public void Cancel()
        {
            playerRB.velocity = Vector3.zero;
            canMove = false;
            animator.SetFloat("movex", playerRB.velocity.x);
            animator.SetFloat("movey", playerRB.velocity.y);
        }

        public static bool GetCanMove()
        {
            return canMove;
        }
        #endregion
    }
}

