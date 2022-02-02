using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // Using Unity.Netcode namespace gives us access to NetworkBehaviour

public class PlayerMove : NetworkBehaviour // Inheriting from NetworkBehaviour gives us access to network related methods and properties
{
    // Variable for determining how fast the player is moving. The [SerializeField] attribute will show the private variable inside the script component
    // in the editor. This allows us to change the value of the variable from the Unity editor
    [SerializeField] private float movementSpeed = 5; // Meters/second

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

            // Move the GameObject towards the movement direction. Distance is determined by multiplying the direction by
            // delta time(amount of time that has elapsed) since the last update loop and by movementSpeed
            transform.localPosition += movementDirection * Time.deltaTime * movementSpeed;
        }
    }
}
