using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // We need to use the Unity.Netcode namespace to access the NetworkManager class

// This class handles connecting and disconnecting
public class NetworkButtonController : MonoBehaviour
{
    // If we are running a Dedicated Server Build, we want to start the server immediately when the scene has finished loading
#if UNITY_SERVER && !UNITY_EDITOR
    public void Start()
    {
        NetworkManager.Singleton.StartServer();
    }
#else

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
    }
#endif
}
