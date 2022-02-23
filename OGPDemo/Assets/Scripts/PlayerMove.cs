using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // Using Unity.Netcode namespace gives us access to NetworkBehaviour

// This is used to synchronize the state of the player character over the network
public enum PlayerState
{
    Running,
    Walking
}

public class PlayerMove : NetworkBehaviour // Inheriting from NetworkBehaviour gives us access to network related methods and properties
{
    // The [SerializeField] attribute will show the private variable inside the script component
    // in the editor. This allows us to change the value of the variable from the Unity editor
    [SerializeField] private NetworkVariable<float> movementSpeed = new NetworkVariable<float>(NetworkVariableReadPermission.Everyone, 5f); // Variable for determining how fast the player is moving. Meters/second
    [SerializeField] private CharacterController characterController; // Reference to the character controller component that is moved with this script
    [SerializeField] private float speedBoostLength = 3f; // Determines how long the speed boost will be active for (in seconds)
    [SerializeField] private MeshRenderer meshRenderer; // Reference to the Mesh Renderer component of the players body (Capsule)

    private float previousMovementSpeed; // This is used to store the original movement speed of the character when we change the value of the movementSpeed NetworkVariable
    private NetworkVariable<PlayerState> playerState = new NetworkVariable<PlayerState>(PlayerState.Walking); // Is the player walking or running
    private Material material; // Reference to the material of the meshRenderer
    private Color originalColor; // Original color of the material

    // This is run when the GameObject with this script component is first spawned on the local machine
    public override void OnNetworkSpawn()
    {
        material = meshRenderer.material; // Get a reference to the material used by the meshRenderer. NOTE: This creates a new instance of that material.
        originalColor = material.color; // Save a reference to the original color of the material
        playerState.OnValueChanged += OnPlayerStateChange; // Start listening for the changes in playerState NetworkVariable
    }

    // This is run each time the value of the playerState variable changes
    private void OnPlayerStateChange(PlayerState oldState, PlayerState newState)
    {
        // Change the color of the meshRenderer material based on the state
        switch (newState)
        {
            case PlayerState.Running:
                // If the player is running, set the material color to blue
                material.color = new Color(0f, 0f, 1f);
                break;
            case PlayerState.Walking:
                // If the player is walking, return the materials color back to the original color
                material.color = originalColor;
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {

        // Only the owner of this object is allowed to move it. We can check if we are the owner with the IsOwner boolean
        if (IsOwner)
        {
            // Vector3 for determining the direction we want to move towards. We initialize all values to zero.
            Vector3 movementDirection = Vector3.zero;

            // We change the x and z values of the movementDirection based on the users input on the WASD keys
            if (Input.GetKey(KeyCode.W))
            {
                movementDirection.z++;
            }
            if (Input.GetKey(KeyCode.A))
            {
                movementDirection.x--;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movementDirection.z--;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movementDirection.x++;
            }

            // Rotate the GameObject towards the position the GameObject is going to be moved
            transform.LookAt(transform.position + movementDirection);

            /*
             * This is the original movement script used during Demo 3. This controls the transform directly.
             * 
                // Move the GameObject towards the movement direction. Distance is determined by multiplying the direction by
                // delta time(amount of time that has elapsed) since the last update loop and by movementSpeed
                transform.localPosition += movementDirection * Time.deltaTime * movementSpeed
            */

            // Move the GameObject towards the movement direction by using the Character Controller component. Distance is determined by multiplying the direction by
            characterController.Move(movementDirection * Time.deltaTime * movementSpeed.Value);

        }

    }

    // This is run when the object collides with a collider that is marked as a trigger
    private void OnTriggerEnter(Collider other)
    {
        // Only run this code if we are the server, if the playerState of this object is Walking and if the trigger object has the SpeedBoostTrigger tag
        if (IsServer && playerState.Value == PlayerState.Walking && other.CompareTag("SpeedBoostTrigger"))
        {
            previousMovementSpeed = movementSpeed.Value; // Store the original movementPpeed value
            movementSpeed.Value *= 2; // Double the movementSpeed
            playerState.Value = PlayerState.Running; // Set the players state to Running
            StartCoroutine("SpeedBoostTimer"); // Start the SpeedBoostTimer coroutine
        }
    }

    // Coroutine for stopping the speedboost of the player after a specific amount of time determined by the speedBoostLength variable
    IEnumerator SpeedBoostTimer()
    {
        // Only run this code if we are the server
        if (IsServer)
        {
            yield return new WaitForSeconds(speedBoostLength); // Stop the coroutine and wait for the amount of seconds determined by the speedBoostLength variable
            movementSpeed.Value = previousMovementSpeed; // Set the movementSpeed back to its original value
            playerState.Value = PlayerState.Walking; // Set the players state back to Walking
        }
    }
}
