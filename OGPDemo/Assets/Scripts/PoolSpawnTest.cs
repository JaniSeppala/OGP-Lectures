using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// Test script for spawning an object from the NetworkObjectPool example from Unity
public class PoolSpawnTest : NetworkBehaviour
{
    [SerializeField] private GameObject prefab; // The prefab we want to spawn

    // Spawns a single object from the pool
    public void SpawnPooledObject()
    {
        // Only server is allowed to spawn objects
        if (IsServer)
        {
            // Get the object from the pool and spawn it
            NetworkObject no = NetworkObjectPool.Singleton.GetNetworkObject(prefab);
            no.Spawn();
        }
    }
}
