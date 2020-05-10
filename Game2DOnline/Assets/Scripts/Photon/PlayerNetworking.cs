using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameOnline.UI;
using Photon.Pun;
using GameOnline.Mechanics;

namespace GameOnline.Photon
{
    public class PlayerNetworking : MonoBehaviour
    {
        public GameObject playerCamera;

        public MonoBehaviour[] scriptsToIgnore;
        public SpriteRenderer spriteRenderer;

        public GameObject bulletPrefabR;
        public GameObject bulletPrefabL;
        public Transform firePointRight;
        public Transform firePointLeft;

        public int maxHealth = 100;
        public int currenHealth;
        public Healthbar healthbar;

        private PhotonView photonView;
        private Collider2D colliderPlayer;

        // Start is called before the first frame update
        void Start()
        {
            currenHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);

            spriteRenderer = GetComponent<SpriteRenderer>();
            photonView = GetComponent<PhotonView>();
            colliderPlayer = GetComponent<Collider2D>();
            if (!photonView.IsMine)
            {
                foreach (var scripts in scriptsToIgnore)
                {
                    scripts.enabled = false;
                }
                playerCamera.SetActive(false);
                colliderPlayer.isTrigger = true;
                spriteRenderer.color = Color.yellow;
            }
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                ChangerDirection();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Shoot();
                }
            }
            // take damage
            if (Input.GetKeyDown(KeyCode.D))
            {
                TakeDamage(20);
            }
        }

        private void ChangerDirection()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                photonView.RPC("OnDrectionchange_LEFT", RpcTarget.Others, null);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                photonView.RPC("OnDrectionchange_RIGHT", RpcTarget.Others, null);
            }
        }

        private void Shoot()
        {
            GameObject bullet;
            if (spriteRenderer.flipX == true)
            {
                bullet = PhotonNetwork.Instantiate(bulletPrefabL.name, firePointLeft.position, Quaternion.identity);
            } else {
                bullet = PhotonNetwork.Instantiate(bulletPrefabR.name, firePointRight.position, Quaternion.identity);

            }
            if (spriteRenderer.flipX == true)
            {
                bullet.GetComponent<PhotonView>().RPC("changeDirectionBullet", RpcTarget.AllBuffered);
            }
        }

        void TakeDamage(int damage)
        {
            currenHealth -= damage;

            healthbar.SetHealth(currenHealth);
        }

        [PunRPC]
        void OnDrectionchange_LEFT()
        {
            spriteRenderer.flipX = true;
        }
        [PunRPC]
        void OnDrectionchange_RIGHT()
        {
            spriteRenderer.flipX = false;
        }
    }
}

