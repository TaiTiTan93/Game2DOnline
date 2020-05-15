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
        public void TakaDamage(int damage)
        {
            currenHealth -= damage;
            photonView.RPC("fixHealthBar", RpcTarget.AllBuffered);
            Debug.Log(currenHealth);
        }


        [PunRPC]
        public void fixHealthBar()
        {
            healthbar.SetHealth(currenHealth);
            Debug.Log(currenHealth);
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
