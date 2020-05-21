using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static GameOnline.Core.Simulation;
using GameOnline.Core;
using Photon.Realtime;


namespace GameOnline.Mechanics
{
    public class EnemyController : MonoBehaviour
    {
        // Start is called before the first frame update
        public int health = 120;
        public int damage =20;

        public PatrolPath path;
        internal PatrolPath.Mover mover;
        public AnimationController control;

        internal Collider2D _collider;
        SpriteRenderer spriteRenderer;
        private Transform player;
        private float eRange;
        private float timeCoolDown;
        private object eBullet;
        private float coolDown;

        // Update is called once per frame
        void Awake()
        {
            control = GetComponent<AnimationController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        public void SetActivePlayer(PlayerController p)
        {
            player = p.transform;
        }
<<<<<<< HEAD

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.PlayerTakeDamage(40);
            }
        }

=======
>>>>>>> parent of bebe7f3... abc
        public void takeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
        public void Die()
        {
            // if enemy die destroy enemy
            Destroy(gameObject);
        }

        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }
            if(player != null && Vector2.Distance(transform.position,player.position) <= eRange)
            {
                if(timeCoolDown<=0)
                {
<<<<<<< HEAD
                    //Instantiate(eBullet, transform.position, Quaternion.identity);
                    PhotonNetwork.Instantiate("eBullet", transform.position, Quaternion.identity);
                    //Instantiate(eBullet, transform.position, Quaternion.identity);
=======
                    Instantiate(eBullet, transform.position, Quaternion.identity);
>>>>>>> parent of bebe7f3... abc
                    timeCoolDown = coolDown;
                }
                else
                {
                    timeCoolDown = -Time.deltaTime;
                }
            }

        }

    }
}