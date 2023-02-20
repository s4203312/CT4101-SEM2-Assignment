using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotProductCheck : MonoBehaviour
{
    [Range(0f, 1f)]             //Like serialize feild but with limits on variable
    public float preciseness = 0.9f;

    public Transform playerTransform;
    private void OnDrawGizmos()
    {
        
        Vector2 Target = transform.position;
        Vector2 playerPos = playerTransform.position;
        Vector2 playerLookDir = playerTransform.right;                  //Direction player is looking
        Vector2 playerToTriggerDir = (Target - playerPos).normalized;   //Direction to the target from the player
        
        //Draws a line to the target object
        Gizmos.color = Color.white;
        Gizmos.DrawLine(playerPos, playerPos + playerToTriggerDir);
        
        //Draws line in direction that player is looking
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerPos, playerPos + playerLookDir);

        //How aligned the two directions are
        float aligment = Vector2.Dot(playerToTriggerDir, playerLookDir);

        //How precise the FOV is. Checks whether the player is looking at the object base on the precision.
        bool isLooking = aligment >= preciseness;

        //Changes the colour of the gizmo depending on whether the player is looking close to the target object
        Gizmos.color = isLooking ? Color.green : Color.red;
        Gizmos.DrawLine(playerPos, playerPos + playerToTriggerDir);
    }
}
