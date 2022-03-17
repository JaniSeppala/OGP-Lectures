using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // Using Unity.Netcode namespace gives us access to NetworkBehaviour
using UnityEngine.UI; // We need to use this namespace to access the legacy button component

// This is a simple example for using custom messages in Netcode for GameObjects. Normally you should be able to use the built-in
// components, NetworkVariables and RPCs to create the networking logic and synchronization for your project, but with custom messages
// you can build your own custom logic for sending and receiving data between clients and the server
public class CustomMessageExample : NetworkBehaviour
{
    private Button button; // Reference to the button in the scene that we use to send the message

    // This is called when the network object with this script is spawned on the local machine
    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("HelloWorld", OnHelloWorldMessage); // Register handler for custom messages named HelloWorld
        button = GameObject.Find("CustomMessageButton").GetComponent<Button>(); // Finds a game object that is named CustomMessageButton and gets its Button component
        button.onClick.AddListener(SendHelloWorldMessage); // Add a listener to the click event of the CustomMessageButton
    }

    // The handler for the HelloWorld custom message. This is run every time a new message with the name HelloWorld is received
    // The first parameter is the ID of the client that sent the message, the second contains the actual message as a byte array
    private void OnHelloWorldMessage(ulong senderClientId, FastBufferReader messagePayload)
    {
        messagePayload.ReadValueSafe(out string message); // Reads the data from the received message as string and saves it to a variable called message
        Debug.Log("Received a message: " + message); // Print the message to the unity console to verify that the message was received correctly
    }

    // This method sends a message from the client to the server. It is run when the user clicks the CustomMessageButton
    private void SendHelloWorldMessage()
    {
        // Only run this if we are the owner of this network object
        if (IsOwner)
        {
            string message = "Hello!"; // This is the message we will send to the server
            Debug.Log("Sending message to server: " + message); // Print a message to the console so we can see that we actually sent the message

            // Create a buffer for sending the message over the network
            // First parameter is the maximum size of your message in bytes
            // Second parameter is the Unity Memory Allocator type. Since we are sending the message immediately after the message is written to the buffer, we can use Temp allocator
            using FastBufferWriter writer = new FastBufferWriter(5120, Unity.Collections.Allocator.Temp); 
            
            writer.WriteValueSafe(message); // Write the value of the message variable to the buffer

            // Send the message to the server using the message name HelloWorld
            // First parameter is the name of the message
            // Second parameter is the ID of the client that you want to send the message to
            // Third parameter is the message buffer that contains the message you want to send
            // Fourth parameter is optional. It's the send type. Default type is ReliableSequenced
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("HelloWorld", NetworkManager.Singleton.ServerClientId, writer, NetworkDelivery.ReliableSequenced);
        }
    }
}
