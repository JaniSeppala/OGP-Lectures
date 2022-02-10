using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// Example script for spawning a network object
public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject spawnPrefab; // The prefab we want to spawn

    // This is script automatically spawns a spawnPrefab into the online session when the object that has the ObjectSpawner script component is spawned into the session
    public override void OnNetworkSpawn()
    {
        // Only the server is allowed to spawn objects
        if (IsServer)
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        // Only the server is allowed to spawn objects
        if (IsServer)
        {
            GameObject go = Instantiate(spawnPrefab); // Instantiate the player prefab locally on the server
            NetworkObject no = go.GetComponent<NetworkObject>(); // Get a reference to the instantiated objects NetworkObject component 
            no.Spawn(); // Spawn the object into the online session as an object owned by the server
        }
    }
}
