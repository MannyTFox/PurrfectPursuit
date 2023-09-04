using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseState { Idle, Running, ReturningToHole }
public class MouseAI : MonoBehaviour
{
    [SerializeField] MouseState currentMouseState;

    [Space(10)]
    [SerializeField] Transform currentMouseHole;

    [Header("RunningState_Config")]
    public float distanceToRun = 4f; 

    // Start is called before the first frame update
    void Start()
    {
        if (currentMouseHole == null)
        {
            // Assign closest mouse hole to mouse, if it doesnt have one
            FindClosestMouseHole();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentMouseState)
        {
            case MouseState.Idle:
                // Mouse turns, scouting area
                CheckToRun();

                break;
            case MouseState.Running:
                // Mouse running away from player
                break;
            case MouseState.ReturningToHole:
                // Mouse returns to hole
                break;
            default:
                break;
        }
    }

    // RUNNING METHODS

    public void CheckToRun()
    {

    }

    public void FindClosestMouseHole()
    {
        GameObject[] holes = GameObject.FindGameObjectsWithTag("Mouse_Hole");

        foreach (GameObject hole in holes)
        {
            if(currentMouseHole == null)
            {
                currentMouseHole = hole.transform;
            }
            else
            {
                // If distance from current hole is longer than the new hole, substitute
                if(Vector3.Distance(this.transform.position, currentMouseHole.position) >
                   Vector3.Distance(this.transform.position, hole.transform.position))
                {
                    currentMouseHole = hole.transform;
                }
            }
        }
    }
}
