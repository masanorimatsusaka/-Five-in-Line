using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public bool blackFlag=true;
	// Use this for initialization
	void Start () {
        //Photonに接続されているか確認する。
        if (PhotonNetwork.IsConnected == false)
        {
            return;
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            OnCameraRotate();
            blackFlag = false;
        }
	}

    public void OnCameraRotate()
    {
        this.transform.Rotate(new Vector3(0,0,180));
        blackFlag = !blackFlag;
    }
}
