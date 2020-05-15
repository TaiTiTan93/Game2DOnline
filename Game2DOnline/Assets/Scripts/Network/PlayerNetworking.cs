using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using GameOnline.HUB;
using GameOnline.Mechanics;

namespace GameOnline.Network
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

        public float coolDown = .5f;
        public float coolDownTime;

        public Health playerHealth;

        private PhotonView photonView;
        private Collider2D colliderPlayer;

        // Start is called before the first frame update
        void Start()
        {

            spriteRenderer = GetComponent<SpriteRenderer>();
            photonView = GetComponent<PhotonView>();
            colliderPlayer = GetComponent<Collider2D>();
            playerHealth = GetComponent<Health>();
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
            if (coolDownTime > 0)
                coolDownTime -= Time.deltaTime;
            if (coolDownTime < 0)
                coolDownTime = 0;
            if (photonView.IsMine)
            {
                ChangerDirection();
                if (Input.GetButtonDown("Fire1") && coolDownTime == 0)
                {
                    Shoot();
                    coolDownTime = coolDown;
                }
                if (playerHealth.currenHealth <= 0)
                {
                    
                }
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

