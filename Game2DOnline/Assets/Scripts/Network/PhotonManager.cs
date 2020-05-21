using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace GameOnline.Network
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerPrefab;
        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        // Update is called once per frame
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position,playerPrefab.transform.rotation);
        }
    }
}

