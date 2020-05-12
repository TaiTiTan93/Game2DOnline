﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameOnline.Mechanics
{
    public class Bulet : MonoBehaviourPun
    {
        public float speed = 10f;
        public int damage = 40;
        public float destroyTime = 2f;
        public bool directionBulletRight = true;
        public GameObject impactEffect;
        //public Rigidbody2D rb;

        private SpriteRenderer spriteRendererBullet;

        // Start is called before the first frame update

        // Destroy bullet 
        IEnumerator destroyBullet()
        {
            yield return new WaitForSeconds(destroyTime);
            this.GetComponent<PhotonView>().RPC("destroy", RpcTarget.AllBuffered);
        }

        // Update is called once per frame
        private void Start()
        {
            spriteRendererBullet = GetComponent<SpriteRenderer>();
            //rb.velocity = transform.right * Time.deltaTime * speed;
        }

        private void OnTriggerEnter2D(Collider2D hitInfor)
        {
            EnemyController enemy =  hitInfor.GetComponent<EnemyController>();
            if(enemy != null)
            {
                enemy.takeDamage(damage);
                Instantiate(impactEffect, transform.position, transform.rotation);
                // if bullet collisder vs enemy destroys bullet
                Destroy(gameObject);
            }
            
        }
        void Update()
        {
            if (directionBulletRight)
            {
                transform.Translate(Vector2.right * Time.deltaTime * speed);
                spriteRendererBullet.flipX = false;
            }
            else
            {
                transform.Translate(Vector2.left * Time.deltaTime * speed);
                spriteRendererBullet.flipX = true;
            }
            Destroy(gameObject, (float)7f/speed);
        }
        

        [PunRPC]
        public void destroy()
        {
            Destroy(this.gameObject);
        }

        [PunRPC]
        public void changeDirectionBullet()
        {
            directionBulletRight = false;
        }
    }

}
