using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // Using Unity.Netcode namespace gives us access to NetworkBehaviour related functionality

// This script component despawns the network object automatically after 3 seconds using a coroutine
public class DespawnPoolObject : NetworkBehaviour
{
    // OnNetworkSpawn() is called when the network object is spawned into the scene with the Spawn() method
    public override void OnNetworkSpawn()
    {
        // We only want to run the script on the server since only server is allowed to spawn/despawn objects
        if (IsServer)
        {
            // Start a coroutine for the Despawn() method
            StartCoroutine("Despawn");
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds before continuing running the script
        NetworkObject no = gameObject.GetComponent<NetworkObject>();
        no.Despawn();
    }
}
