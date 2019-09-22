using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RasonNetwork;

public class Chat_Manager : NetworkedBehaviour
{
    public Text Title;
    public Text ChatText;
    public InputField Message;
    public Transform playersF;
    public GameObject PlayerPrefab;

    void OnEnable()
    {
        RasonManager.onPlayerJoin += OnPlayerJoined;
        RasonManager.onPlayerLeft += OnPlayerLeft;
        RasonManager.onJoinRoom += OnJoinedRoom;
        RasonManager.onLeftRoom += OnLeftRoom;
        RasonManager.onNetwokCreate += OnCreateObject;
    }
    void OnDisable()
    {
        RasonManager.onPlayerJoin -= OnPlayerJoined;
        RasonManager.onPlayerLeft -= OnPlayerLeft;
        RasonManager.onJoinRoom -= OnJoinedRoom;
        RasonManager.onLeftRoom -= OnLeftRoom;
        RasonManager.onNetwokCreate -= OnCreateObject;
    }
    void OnCreateObject(NetworkObject netO)
    {
        if (netO.GetComponent<ChatPlayerInfo>())
        {
            netO.transform.SetParent(playersF);
            netO.GetComponent<ChatPlayerInfo>().Intia(netO.Owner);
            netO.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
    public void SendMsg()
    {
        if (!RTools.IsNullOrEmpty(Message.text))
        {
            netObject.CallRpc("GetMsg", TargetPlayer.All, true, Message.text, RasonManager.Id);
            Message.text = "";
        }
    }
    [RRPC]
    void GetMsg(string msg, int SenderID)
    {
        RPlayer sender = RasonManager.GetPlayer(SenderID);
        string fullMsg = "\n" + sender.name + " : " + msg;
        ChatText.text += fullMsg;
    }

    void OnPlayerJoined(RPlayer p)
    {
        Title.text = "Chat - " + RasonManager.players.Count + " Player";
        string fullMsg = "\n" + p.name + " Joined the room.";
        ChatText.text += fullMsg;
    }
    void OnPlayerLeft(RPlayer p)
    {
        Title.text = "Chat - " + RasonManager.players.Count + " Player";
        string fullMsg = "\n" + p.name + " Left the room.";
        ChatText.text += fullMsg;
    }
    public void LeaveRoom()
    {
        RasonManager.LeaveRoom();
    }
    void OnJoinedRoom(bool Succes, int RoomId)
    {
        if (Succes)
        {
            Title.text = "Chat - " + RasonManager.players.Count + " Player";
            RasonManager.Create(PlayerPrefab, false, Vector3.zero, new Quaternion(0, 0, 0, 0));
        }
    }
    void OnLeftRoom(int rid)
    {
        ChatText.text = "";
    }

}
