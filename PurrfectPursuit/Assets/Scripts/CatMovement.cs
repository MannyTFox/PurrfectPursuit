using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CatMovement : MonoBehaviour
{

    [Header("References")]
    public Transform HeadBone;
    public float angleLimit = 30f;
    public Vector3 defaultFacing;
    public Transform bell;
    public LayerMask NavmeshIgnoreLayer; //needs to be set to all bell objects with colliders.


    float distanceToBell;
    Vector3 directionToBell;
    Animator anim;
    NavMeshAgent agent;
    

    void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        GetBellDistance();
        Move();


       LookAtBell();


    }

    void LookAtBell()
    {

        Vector3 bellX = new Vector3(bell.position.x, 0, bell.position.z);
        Vector3 headX = new Vector3(transform.position.x, 0, transform.position.z);

        float lookRange = Vector3.Distance(bellX, headX);
        

        if(lookRange > 2)
        {
            Quaternion lookRotation = Quaternion.LookRotation(bell.position - HeadBone.position);
            HeadBone.rotation = lookRotation;
        }
        else
        {

            HeadBone.rotation = new Quaternion(0,0,0,0);
        }

    }

    void GetBellDistance()
    {
        //get direction to bell vector
        directionToBell = (bell.position - transform.position);
        //get distance to bell float
        distanceToBell = Vector3.Distance(bell.position,transform.position); 
    }

    void Move()
    {
        // Cast a ray from the bell downwards, if it hits a valid navmesh set that hitPoint to the destination
        RaycastHit hit;
        if (Physics.Raycast(bell.position, Vector3.down, out hit, Mathf.Infinity, NavmeshIgnoreLayer))
        {
            // Check if the hit point is on the NavMesh
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                // Move the cat towards the hit position on the NavMesh
                agent.SetDestination(navHit.position);
            }
        }

        //check anims
        if(agent.velocity.magnitude != 0) 
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }
    }


}
