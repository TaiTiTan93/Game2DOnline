using GameOnline.HUB;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameOnline.Mechanics
{
    public class Health : MonoBehaviour,IPunObservable
{
        public int maxHealth = 100;
        public int currenHealth;
        public Healthbar healthbar;
        protected PhotonView photonView;
        // Start is called before the first frame update
        void Start()
        {
            healthbar.SetMaxHealth(maxHealth);
            currenHealth = maxHealth;

            photonView = GetComponent<PhotonView>();
        }
        

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.D) && photonView.IsMine)
            {
                photonView.RPC("TakeDamage", RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        public void TakeDamage()
        {
            currenHealth -= 20;
            healthbar.SetHealth(currenHealth);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(currenHealth);
            } else if(stream.IsReading)
            {
                currenHealth = (int)stream.ReceiveNext();
            }
        }
    }

}
