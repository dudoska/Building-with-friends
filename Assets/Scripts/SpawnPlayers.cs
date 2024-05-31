using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject player;
    public int IsPlayers = 0;
    public GameObject spawn_player1;
    public GameObject spawn_player2;
    public GameObject spawn_player3;
    public GameObject spawn_player4;
    public GameObject spawn_player5;



    public void Start()
    {
        ////контейнер
        //GameObject player = new GameObject("Player");

        if (IsPlayers == 0)
        {
            Vector3 Position = new Vector3(spawn_player1.transform.position.x, spawn_player1.transform.position.y, spawn_player1.transform.position.z);
            PhotonNetwork.Instantiate (player.name, Position, Quaternion.identity);
            //player.name = "Player 1";
            IsPlayers = 1;
        }
        else if(IsPlayers == 1)
        {
            Vector3 Position = new Vector3(spawn_player2.transform.position.x, spawn_player2.transform.position.y, spawn_player2.transform.position.z);
            PhotonNetwork.Instantiate(player.name, Position, Quaternion.identity);
            //player.name = "Player 2";
            IsPlayers = 2;
        }
        else if (IsPlayers == 2)
        {
            Vector3 Position = new Vector3(spawn_player3.transform.position.x, spawn_player3.transform.position.y, spawn_player3.transform.position.z);
            PhotonNetwork.Instantiate(player.name, Position, Quaternion.identity);
            //player.name = "Player 3";
            IsPlayers = 3;
        }
        else if (IsPlayers == 3)
        {
            Vector3 Position = new Vector3(spawn_player4.transform.position.x, spawn_player4.transform.position.y, spawn_player4.transform.position.z);
            PhotonNetwork.Instantiate(player.name, Position, Quaternion.identity);
            //player.name = "Player 4";
            IsPlayers = 4;
        }
        else if (IsPlayers == 4)
        {
            Vector3 Position = new Vector3(spawn_player5.transform.position.x, spawn_player5.transform.position.y, spawn_player5.transform.position.z);
            PhotonNetwork.Instantiate(player.name, Position, Quaternion.identity);
            //player.name = "Player 5";
        }

    }
}
