using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RasonNetwork;

public class ChatRoomInfo : MonoBehaviour
{
    public RoomInfo info;
    public Text tex;

    public void Initia(RoomInfo i) { info = i; tex.text = info.name; }

    public void Join()
    {
        RasonManager.JoinRoom(info);
    }
}
