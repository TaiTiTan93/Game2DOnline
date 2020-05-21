﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameOnline.HUB;
using Photon.Pun;

namespace GameOnline.Mechanics
{
    public class PlayerController : KinematicObject, IPunObservable
    {
        public float maxSpeed = 6f;
        public float jumpTakeOffSpeed = 8f;

        public int maxHealth = 100;
        public int Amount;
        public int currenHealth;
        public Healthbar healthbar;

        protected PhotonView photonView;

        public Animator animator;

        //protected bool shooted = false;

        public Joystick joystick;

        private SpriteRenderer spriteRenderer;

        public float coolDown = 1f;
        private float coolDownTime;
        // Update is called once per frame

        protected override void Start()
        {
            healthbar.SetMaxHealth(maxHealth);
            currenHealth = maxHealth;
        }    
        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            photonView = GetComponent<PhotonView>();
        }

        protected override void ComputeVelocity()
        {
            if (coolDownTime > 0)
                coolDownTime -= Time.deltaTime;
            if (coolDownTime < 0)
                coolDownTime = 0;

            // move to hozizontal
            Vector2 move = Vector2.zero;

            if(currenHealth <= 0)
            {
                photonView.RPC("playerDestroy", RpcTarget.AllBuffered);
            }
<<<<<<< HEAD

            // move to hozizontal
            Vector2 move = Vector2.zero;
=======
            

>>>>>>> parent of bebe7f3... abc
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
            if (Input.GetButtonDown("Fire1") && coolDownTime == 0)
            {
                animator.SetBool("Shoot", true);
                coolDownTime = coolDown;
            }
            if (Input.GetButtonUp("Fire1"))
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

<<<<<<< HEAD
        public void PlayerTakeDamage(int damage)
        {
            if(photonView.IsMine)
            {
                Amount = damage;
                //FindObjectOfType<AudioManager>().Play("PlayerTakeDamage");
                {
                    photonView.RPC("fixHealthBar", RpcTarget.AllBuffered);
                    Debug.Log("player take damage");
=======
        void OnCollisionEnter2D(Collision2D collision)
        {
            if(photonView.IsMine)
            {
                var enemy = collision.gameObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    photonView.RPC("fixHealthBar", RpcTarget.AllBuffered);
>>>>>>> parent of bebe7f3... abc
                }
            }
        }

        [PunRPC]
        public void playerDestroy()
        {
            Destroy(gameObject);
        }

        [PunRPC]
        public void fixHealthBar()
        {
            currenHealth -= 20;
            healthbar.SetHealth(currenHealth);
            Debug.Log(currenHealth);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(currenHealth);
            }
            else if (stream.IsReading)
            {
                currenHealth = (int)stream.ReceiveNext();
            }
        }
    }
}

