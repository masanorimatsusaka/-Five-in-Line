using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginScene : MonoBehaviourPunCallbacks, IPointerClickHandler
{
    bool isConnecting = false;
    public InputField inputField;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        isConnecting = true;
        if (inputField.text != "")
        {
            if (PhotonNetwork.IsConnected)
            {

                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = "1.0f";
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("名前を入力しろ");
        }
    }
    /// <summary>
    /// マスターとして入室した
    /// </summary>
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
    /// <summary>
    /// ランダムにルームに入れなかった
    /// </summary>
    public override void OnJoinRandomFailed(short returnCode, string message)

    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    /// <summary>
    /// ルームに入室成功
    /// </summary>
    public override void OnJoinedRoom()

    {
        PhotonNetwork.NickName = inputField.text;
        PhotonNetwork.LoadLevel("MainBattle");
    }

}
