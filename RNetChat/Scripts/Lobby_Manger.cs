using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RasonNetwork;

public class Lobby_Manger : REventReceiver
{
    [Header("Screen Managment")]
    public GameObject ConnectScreen;
    public GameObject LobbyScreen;
    public GameObject ChatScreen;

    [Header("Connect Screen")]
    public InputField IPAdr;
    public GameObject StopServerButton;
    public int Port = 5390;

    [Header("Lobby Screen")]
    public Transform roomsF;
    public GameObject roomPrefab;
    RList<GameObject> rooms = new RList<GameObject>();
    public InputField Name;

    public void ClickConnect()
    {
        RasonManager.Connect(IPAdr.text, Port);
    }
    public void ClickChangeName()
    {
        if (!RTools.IsNullOrEmpty(Name.text))
        {
            RasonManager.ChangePlayerName(Name.text);
        }
        else { Name.text = RasonManager.Name; }
    }

    public void ClickStartServer()
    {
        if (ServerInstance.StartRelay(Port)) // We need relay for a lot of rooms.
        {
            StopServerButton.SetActive(true);
        }
    }
    public void ClickStopServer()
    {
        if (ServerInstance.IsActive)
        {
            ServerInstance.StopServer();
            StopServerButton.SetActive(false);
        }
    }

    public void ClickCreateRoom()
    {
        RasonManager.CreateRoom(RasonManager.Name + "'s Chat Room", 8, "");
    }

    public void ClickRefreshRooms()
    {
        RasonManager.GetRooms();
    }

    public void ClickDisconnect()
    {
        RasonManager.Disconnect();
    }
    void CloseAllScreens()
    {
        ChatScreen.SetActive(false);
        LobbyScreen.SetActive(false);
        ConnectScreen.SetActive(false);
    }
    void OpenScreen(int i)
    {
        if (i == 0)
        {
            ConnectScreen.SetActive(true);
        }
        else if (i == 1)
        {
            LobbyScreen.SetActive(true);
        }
        else
        {
            ChatScreen.SetActive(true);
        }
    }
    public void OnClickFindOnLAN()
    {
        // Send a descovery request.
        RasonManager.SendServerDiscovery(Port);

        // Check the available LAN servers
        ServerInfo[] lanServers = RasonManager.GetAvailableLanServers();

        if (lanServers.Length != 0)
        {
            // Peck up the first one.
            IPAdr.text = lanServers[0].ip;
        }
    }

    protected override void OnConnect(bool success)
    {
        if (success)
        {
            Name.text = RasonManager.Name;
            Debug.Log("Connected to server");
            CloseAllScreens();
            OpenScreen(1);
        }
    }

    protected override void OnDisconnect(bool ByUser, LiteNetLib.DisconnectInfo disconnectInfo)
    {
        Debug.Log("Disconnected");
        CloseAllScreens();
        OpenScreen(0);
    }

    protected override void OnJoinedRoom(bool Succes, int RoomId)
    {
        if (Succes)
        {
            CloseAllScreens();
            OpenScreen(2);
        }
    }

    protected override void OnLeftRoom(int RoomId)
    {
        CloseAllScreens();
        OpenScreen(1);
    }

    protected override void OnRoomsListRecived(RoomInfo[] info)
    {
        foreach (GameObject r in rooms)
        {
            Destroy(r);
        }
        rooms.Clear();

        for (int i = 0; i < info.Length; ++i)
        {
            GameObject go = Instantiate(roomPrefab, roomsF);
            go.GetComponent<ChatRoomInfo>().Initia(info[i]);
            rooms.Add(go);
        }
    }
}
