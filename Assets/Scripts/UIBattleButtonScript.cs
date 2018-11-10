using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleButtonScript : MonoBehaviourPunCallbacks{
    public Canvas StartCamvas;
    public Canvas BattleUICnavas;
    public Canvas WinCanvas;
    public Text WinText;
    public CameraScript cameraScript;
    public new PhotonView photonView ;
    public TextScript textScript;
    public void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            StartCamvas.enabled = false;
        }
    }
    #region ボタン押したときのメソッド
    //退出ボタンをクリックしたときシーン移行とかする。
    public void OnTitleButton()
    {
        PhotonNetwork.LeaveRoom();
    }
    //ゲームを止めるボタンをクリックしたとき
    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      Application.Quit();
#endif
    }
    //カメラを変えるときに互いのカメラを回転させる。
    public void OnChemgeButton()
    {
        photonView.RPC("SetChanged", RpcTarget.All);
    }
    //スタートさせる。黒の碁石を召喚するだけ
    public void OnStartButton()
    {
        //もし人数が二人以上なら始める。その際はスタートCanvasを削除して、どっちのターンか確認してから始める。
        if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
        {
            Debug.Log("人数が足りていないためStart出来ません。");
            return;
        }
        StartCamvas.enabled = false;
        photonView.RPC("ResetBord", RpcTarget.All);
        if (cameraScript.blackFlag)
        {
            photonView.RPC("SetStorn", RpcTarget.MasterClient);
        }
        else
        {
            photonView.RPC("SetStorn", RpcTarget.Others);
        }
        
    }
    //置いた碁石の位置を補正かけて次の石を出すようにして配列にデータを入れて別人のメソッドをRPCで実行する。
    public void OnNextButton()
    {
        //置いた石の位置を補正かけて勝敗等の情報を出す。
        GameObject stone= GameObject.Find("SelectStone");
        //配列に格納して勝敗等の情報を出す。
        if (!StoneCalculationScript.SetStonePosition(stone, cameraScript.blackFlag))
        {
            if (StoneCalculationScript.endFlag)
            {
                Debug.Log("終わり");
                object[] winObjext = new object[] {StoneCalculationScript.winnerText };
                photonView.RPC("SetWinCanvas", RpcTarget.All, winObjext);

                return;
            }
            Debug.Log("そこにはおけないよ");
            return;
        }
        object[] args;
        if (cameraScript.blackFlag)
        {
            args = new object[] { stone.transform.position, 1 };
        }
        else
        {
            args = new object[] { stone.transform.position, 0 };
        }
        BattleUICnavas.enabled = !BattleUICnavas.enabled;
        Destroy(stone.GetComponent<DragDrop>());
        //パラメータを共有する
        photonView.RPC("ParamSerialized", RpcTarget.Others,args);
        //全員がステータスを表示させる。
        photonView.RPC("ScoreText", RpcTarget.All);
        //特になければ他人が石を置く
        photonView.RPC("SetStorn", RpcTarget.Others);
    }
    //動かせる碁石が変なところに行ったときに実行する
    public void OnResetButton()
    {
        BattleUICnavas.enabled = !BattleUICnavas.enabled;
        PhotonNetwork.Destroy(GameObject.Find("SelectStone"));
        SetStorn();
    }
    #endregion

    #region PUNRPCを用いたメソッド一覧
    /// <summary>
    /// 初期化させる。
    /// </summary>
    [PunRPC]
    void ResetBord()
    {
        StoneCalculationScript.ResetBord();
    }

    [PunRPC]
    private void SetChanged()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            cameraScript.OnCameraRotate();
        }
    }

    /// <summary>
    /// 石を出してD＆DをつけてCanvasを生成する。
    /// </summary>
    [PunRPC]
    void SetStorn()
    {
        GameObject stones;
        if (cameraScript.blackFlag)
        {
            stones = PhotonNetwork.Instantiate("Black", new Vector3(1.8f, 0.5f, -0.3f), Quaternion.identity, 0);
        }
        else
        {
            stones = PhotonNetwork.Instantiate("White", new Vector3(-1.8f, 0.5f, 0.3f), Quaternion.identity, 0);
        }

        stones.AddComponent<DragDrop>();
        stones.name = "SelectStone";
        BattleUICnavas.enabled = !BattleUICnavas.enabled;
    }
    [PunRPC]
    void ParamSerialized(Vector3 stonePosition,int blackFlag,PhotonMessageInfo info)
    {
        if (blackFlag == 1)
        {
            StoneCalculationScript.SerializeBord(stonePosition,true);
        }
        else
        {
            StoneCalculationScript.SerializeBord(stonePosition, false);
        }
    }
    [PunRPC]
    void ScoreText()
    {
        textScript.SetText();
    }
    [PunRPC]
    void SetWinCanvas(string winText, PhotonMessageInfo info)
    {
        WinCanvas.enabled = true;
        this.WinText.text = winText;
    }
    #endregion

    #region Photonからオーバライドしたメソッド
    /// <summary>
    /// 部屋を退出したときシーン移行する。
    /// </summary>
    public override void OnLeftRoom()
    {
       
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }

    /// <summary>
    /// 誰かが入出してきたとき
    /// </summary>
    /// <param name="other">Other.</param>
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log("OnPlayerEnteredRoom() " + other.NickName); 
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
            LoadArena();
        }
    }

    /// <summary>
    /// ほかの誰かが退出したとき
    /// </summary>
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("OnPlayerLeftRoom() " + other.NickName); 
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); 

            LoadArena();
        }
    }
    /// <summary>
    /// どのタイミングでも一人の状態になったら再度シーンを読み込んで初期化させる
    /// </summary>
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainBattle");
        }
    }
#endregion
}
