using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerNameSetter : NetworkBehaviour
{
    [SerializeField] private Text nameText; // Text field for displaying the player name
    Button buttonComponent;
    InputField inputFieldComponent;
    NetworkVariable<int> health;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner) // Everything here is related to the ServerRPC and only the owner of this object is allowed to call that
        {
            GameObject button = GameObject.Find("SubmitNameButton"); // Find the submit button from the scene
            GameObject input = GameObject.Find("TextInput"); // Find the name input field from the scene
            buttonComponent = button.GetComponent<Button>(); // Get the Button component of the Submit button Game Object
            inputFieldComponent = input.GetComponent<InputField>(); // Get the InputField component of the name input Game Object
            buttonComponent.onClick.AddListener(SendNameToServer); // Start listening for click events on the submit button. Runs SendNameToServer() when the submit button is clicked
        }
    }
    private void SendNameToServer() // This sends the text inside the name input field to the server
    {
        if (IsOwner) // Only the onwer is allowed to send the RPC
        {
            SetNameServerRPC(inputFieldComponent.text); // Sends the RPC to the server
        }
    }

    private void TakeDamage(int damage)
    {
        if (IsOwner)
        {
            SetHPServerRPC(health.Value - damage);
        }
    }

    [ServerRpc]
    private void SetHPServerRPC(int hp)
    {
        health.Value = hp;
        ClientRpcParams clientparams = new ClientRpcParams();
        clientparams.Send.TargetClientIds = new ulong[] { 1, 2, 3 };
        PlayTakeDamageAnimationClientRPC(clientparams);
    }

    [ServerRpc] // This is required for each ServerRPC. Also all ServerRPC method names must end with ServerRPC
    private void SetNameServerRPC(string message) // RPC For sending the name from the client to the server
    {
        SetNameClientRPC(message); // Send the name change from the server to all the clients
    }

    [ClientRpc] // This is required for each ClientRPC. Also all ClientRPC method names must end with ClientRPC
    private void SetNameClientRPC(string message) // RPC for setting the new name sent by the server
    {
        nameText.text = message;
    }

    [ClientRpc]
    private void PlayTakeDamageAnimationClientRPC(ClientRpcParams p)
    {
        //Play the animation
    }
}
