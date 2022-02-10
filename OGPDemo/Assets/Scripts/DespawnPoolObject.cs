using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DespawnPoolObject : NetworkBehaviour
{
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            StartCoroutine("Despawn");
        }
    }

    // Update is called once per frame
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(3f);
        NetworkObject no = gameObject.GetComponent<NetworkObject>();
        no.Despawn();
    }
}
