using System.Collections;
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
        public int currenHealth;
        public Healthbar healthbar;

        protected int amount;
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

            EnemyController[] enemies = GameObject.FindObjectsOfType<EnemyController>();
            for (int i = 0; i < enemies.Length; i++)
            {
                // set player vao enemy, bo cai code o start cua enemy di
                if (photonView.IsMine)
                    enemies[i].SetActivePlayer(this);
            }
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

            if(currenHealth <= 0)
            {
                animator.SetBool("Death", true);
                photonView.RPC("playerDestroy", RpcTarget.AllBuffered);
                //FindObjectOfType<AudioManager>().Play("PlayerDeath");
            }

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
            if (Input.GetButtonDown("Fire1") && coolDownTime == 0)
            {
                animator.SetBool("Shoot", true);
                FindObjectOfType<AudioManager>().Play("PlayerShoot");
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
        public void playerTakeDamage(int damage)
        {
            amount = damage;
            photonView.RPC("fixHealthBar", RpcTarget.AllBuffered);
            FindObjectOfType<AudioManager>().Play("PlayerTakeDamage");
        }

        [PunRPC]
        public void playerDestroy()
        {
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
            Destroy(gameObject, 2f);
        }

        [PunRPC]
        public void fixHealthBar()
        {
            currenHealth -= amount;
            healthbar.SetHealth(currenHealth);
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

