using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private void OnServerStarted()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                GameObject go = Instantiate(playerPrefab);
                NetworkObject no = go.GetComponent<NetworkObject>();
                no.SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
            }

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        }
    }

    private void OnClientConnectedCallback(ulong clientID)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GameObject go = Instantiate(playerPrefab);
            NetworkObject no = go.GetComponent<NetworkObject>();
            no.SpawnAsPlayerObject(clientID);
        }
    }
}
