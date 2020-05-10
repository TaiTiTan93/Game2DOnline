using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameOnline.Mechanics
{
    public class Enemy : MonoBehaviour
    {
        // Start is called before the first frame update
        public int health = 120;

        public Transform RangeRight;
        public Transform RangeLeft;
        public float speed = 1f;

        //private SpriteRenderer spriteRendererEnemy;

        // Update is called once per frame
        public void takeDamage(int damage)
        {
            health -= damage;
            if(health <= 0)
            {
                Die();
            }
        }
        public void Die()
        {
            // if enemy die destroy enemy
            Destroy(gameObject);
        }

        private void Update()
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
            if (transform.position.x <= RangeLeft.position.x || transform.position.x >= RangeRight.position.x)

                transform.Rotate(0f, 180f, 0f);

        }

    }
}

