using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

// Example script for connecting to a server by specifying the servers IP address
public class IPChanger : MonoBehaviour
{
    [SerializeField] private InputField ipField; // Reference to the input field we use to set the IP address
    private UNetTransport transport; // Reference to the transport layer used by the Network Manager component

    // Start is called before the first frame update
    void Start()
    {
        ipField.text = "127.0.0.1"; // Set the default value of the IP address Input Field to be the local machines address. 127.0.0.1 always points to the local machine
        transport = NetworkManager.Singleton.gameObject.GetComponent<UNetTransport>(); // Find the reference to the Network Managers Transport component
        ipField.onValueChanged.AddListener(OnValueChange); // Start listening for changes in the Input Field
    }

    // This is run each time the value of the input field is changed
    private void OnValueChange(string value)
    {
        transport.ConnectAddress = value; // Set the IP address we want to connect to based on the value the user typed on the Input Field
    }
}
