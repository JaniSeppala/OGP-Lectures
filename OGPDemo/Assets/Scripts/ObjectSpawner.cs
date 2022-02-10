using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject spawnPrefab;


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnObject();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SpawnObject()
    {
        if (IsServer)
        {
            GameObject go = Instantiate(spawnPrefab);
            NetworkObject no = go.GetComponent<NetworkObject>();
            no.Spawn();
        }
    }
}
