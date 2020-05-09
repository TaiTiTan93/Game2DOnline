using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameOnline.Mechanics
{
    public class Bulet : MonoBehaviourPun
    {
        public float speed = 10f;
        public float destroyTime = 2f;
        public bool directionBulletRight = true;
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
