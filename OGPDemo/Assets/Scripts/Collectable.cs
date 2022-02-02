using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // Using Unity.Netcode namespace gives us access to NetworkBehaviour

// This script component parents the GameObject to a colliding GameObject if the colliding GameObject is tagged "Player"
public class Collectable : NetworkBehaviour // Inheriting from NetworkBehaviour gives us access to network related methods and properties
{
    // OnTriggerEnter is run when a collider on another GameObject hits a collider marked as trigger on this GameObject
    // The GameObject this script component is attached to must also have a RigidBody component for the trigger event to work
    private void OnTriggerEnter(Collider other)
    {
        // Only the server is allowed reparent NetworkObjects so we use the IsServer boolean to make sure we only run the code if we are running as a server or host
        // We also don't need to parent the object to the colliding object if we are already parented to that object
        // We also want to parent the collectable only if we collided with the player character, so we verify that the colliding object has the Player-tag
        if (IsServer && transform.parent != other.transform && other.tag == "Player")
        {
            transform.parent = other.transform;
        }
    }
}
