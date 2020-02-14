using UnityEngine;
using System.Collections;

public class PlayerMovement2 : PlayerMovement
{
    new void Update()
    {
        MovePlayer();
        
        // If press jump button, activate jumping animation and deactivate carry animation if it is playing.
        if (Input.GetButtonDown("Jump2") && canMove)
        {
            isJump = true;

            if(isCarry)
            {
                CarryPosition();
            }
        }

        // If press carry button, call CarryPosition method.
        if (Input.GetButtonDown("Carry2") && canMove && horizontalMove == 0 && animator.GetFloat(fallParamID) == 0)
        {
            CarryPosition();
        }

        // Deactivate carry animation if it is falling.
        if (isCarry && animator.GetFloat(fallParamID) < -0.01)
        {
            CarryPosition();
        }
    }

    /// <summary>
    /// Calculate and perform player movement.
    /// </summary>
    new void MovePlayer()
    {
        // Calculate the player speed
        if(canMove)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal2") * runSpeed;

            if (carriedElement != null)
            {
                horizontalMove *= 0.8f;
            }
        }
        else
        {
             horizontalMove = 0;
        }

        // Say the animator to activate the running animation.
        animator.SetFloat(speedParamID, Mathf.Abs(horizontalMove));
    }

    /// <summary>
    /// Perform the actions to pick up or drop an object and fit the animation to carry.
    /// </summary>
    new void CarryPosition()
    {
        if (Input.GetButtonDown("Carry2") && elementToCarry != null && !isCarry)
        {
            RpcTakeObject();
        }

        // Switch the carry, collider and animation state.
        isCarry = !isCarry;
        controller.EnableCarryCollider(isCarry);
        animator.SetBool(carryParamID, isCarry);

        if (Input.GetButtonDown("Carry2") && carriedElement != null && !isCarry)
        {
            RpcDropObject(5f);
        }
        else if (carriedElement != null && !isCarry)
        {
            RpcDropObject(0f);
        }
    }
}