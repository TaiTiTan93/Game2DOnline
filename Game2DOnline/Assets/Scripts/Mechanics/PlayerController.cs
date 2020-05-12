using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameOnline.HUB;

namespace GameOnline.Mechanics
{
    public class PlayerController : KinematicObject
    {
        public float maxSpeed = 6f;
        public float jumpTakeOffSpeed = 8f;

        public Animator animator;

        //protected bool shooted = false;

        public Joystick joystick;

        private SpriteRenderer spriteRenderer;

        public float coolDown = 1f;
        public float coolDownTime;
        // Update is called once per frame

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        protected override void ComputeVelocity()
        {
            if (coolDownTime > 0)
                coolDownTime -= Time.deltaTime;
            if (coolDownTime < 0)
                coolDownTime = 0;

            // move to hozizontal
            Vector2 move = Vector2.zero;


            

            move.x = Input.GetAxis("Horizontal") + joystick.Horizontal;
            // jump
            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * .5f;
                }
            }
            // shoot animation
            if (Input.GetKeyDown(KeyCode.F) && coolDownTime == 0)
            {
                animator.SetBool("Shoot", Input.GetKeyDown(KeyCode.F));
                coolDownTime = coolDown;
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                animator.SetBool("Shoot", false);
            }

            // flip animation (direction) object

            bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
            if (flipSprite)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }

            animator.SetBool("Grounded", IsGrounded);
            animator.SetFloat("Speed", Mathf.Abs(velocity.x) / maxSpeed);
            targetVelocity = move * maxSpeed;

        }
    }
}

