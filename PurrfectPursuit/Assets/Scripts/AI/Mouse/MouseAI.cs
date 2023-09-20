using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MouseState { Idle, Running, GoingToHole }
public class MouseAI : MonoBehaviour
{
    [SerializeField] MouseState currentMouseState;

    [Space(10)]
    [SerializeField] Transform currentMouseHole;

    [Header("References")]
    Transform player;
    NavMeshAgent agent;
    MouseManager mouseManager;

    [Header("Idle")]
    public float idleRoamRange = 4f;
    float timerToRoam = 1f;
    bool canSetDestination = true;
    Vector3 finalPos;

    [Header("RunningState_Config")]
    public float distanceToRun = 4f;
    bool running = false;

    // Turning for obstacles
    public bool isDirSafe = true;

    float vRotation = 0f;

    [Header("GoingToHoleState_Config")]
    float timerToGoToHole = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        mouseManager = GameObject.Find("The_Magical_RatManager").GetComponent<MouseManager>();

        // Set timer of finding hole
        timerToGoToHole = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        // Finds hole for rat
        FindClosestAndSafestMouseHole();

        switch (currentMouseState)
        {
            case MouseState.Idle:
                // Mouse turns, scouting area
                IdleRoam();
                break;
            case MouseState.Running:
                // Mouse running away from player
                RunningAwayFromPlayer();

                // While running, mouse will get desperate and try to go to safe hole sometime
                TimerMethodToGoToHole();
                break;
            case MouseState.GoingToHole:
                // Mouse returns to hole
                RunningToHole();
                break;
            default:
                break;
        }

        // Mouse checks distance to player to run away
        CheckToRun();

        // Check if mouse enters hole
        EnterHole();
    }

    // IDLE ROAM
    public void IdleRoam()
    {
        if(timerToRoam < 0)
        {
            // Set destination to random point
            if (canSetDestination)
            {
                Vector3 randomPointRoam = Random.insideUnitSphere.normalized * idleRoamRange;
                randomPointRoam += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomPointRoam, out hit, idleRoamRange, 1);
                finalPos = hit.position;

                agent.SetDestination(finalPos);
                canSetDestination = false;
            }

            // When rat gets to point, start timer again
            if (Vector3.Distance(transform.position, finalPos) < 1)
            {
                timerToRoam = Random.Range(1f, 6f);
                print("rat arrived at roam location");
            }
            
        }
        else
        {
            canSetDestination = true;

            timerToRoam -= Time.deltaTime;
        }
    }

    // RUNNING METHODS

    public void CheckToRun()
    {
        float distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);

        if(distanceFromPlayer <= distanceToRun)
        {
            // Can run away
            if (currentMouseState != MouseState.GoingToHole)
            {
                currentMouseState = MouseState.Running;
            }

            running = true;
        }
        else
        {
            if (running)
            {
                running = false;

                StartCoroutine(DelayToStopRunning());
            }
        }
    }

    public void RunningAwayFromPlayer()
    {
        // See if rat is not close enough to a hole
        if (currentMouseHole != null)
        {
            float distanceRatToHole = Vector3.Distance(transform.position, currentMouseHole.position);

            if(distanceRatToHole < 6f)
            {
                currentMouseState = MouseState.GoingToHole;
            }
        }

        // ----

        // Run away from player
        Vector3 awayFromPlayer = transform.position - player.position;

        Vector3 newDirection = transform.position + awayFromPlayer;

        newDirection = Quaternion.Euler(0, vRotation, 0) * newDirection;

        bool isHit = Physics.Raycast(transform.position, newDirection, out RaycastHit hit, 5f);

        // Didnt find wall in front
        if(hit.transform == null)
        {
            // Run this way!
            agent.SetDestination(newDirection);
            isDirSafe = true;
        }

        if(isHit && hit.transform.CompareTag("Wall") && isDirSafe)
        {
            // This way is not safe! TURN!
            vRotation += 20;
            isDirSafe = false;
        }
        else
        {
            // Found a way to escape!
            agent.SetDestination(newDirection);
            vRotation = 0;
            isDirSafe = true;
        }
    }

    public IEnumerator DelayToStopRunning()
    {
        yield return new WaitForSeconds(1.5f);

        // If rat does not run again, go back to idle, because he is safe
        if (running == false)
        {
            // Stop running away
            currentMouseState = MouseState.Idle;
        }
    }

    // GOING TO HOLE
    public void RunningToHole()
    {
        if (currentMouseHole != null)
        {
            agent.SetDestination(currentMouseHole.position);
        }
        else
        {
            currentMouseState = MouseState.Running;
        }
    }

    public void TimerMethodToGoToHole()
    {
        if(timerToGoToHole < 0)
        {
            // If there is a safe hole
            if (currentMouseHole != null)
            {
                // Change mouse state to go to hole
                currentMouseState = MouseState.GoingToHole;
            }

            timerToGoToHole = Random.Range(5f, 7f);
        }
        else
        {
            timerToGoToHole -= Time.deltaTime;
        }
    }

    // Finding and entering hole
    public void FindClosestAndSafestMouseHole()
    {
        GameObject[] holes = GameObject.FindGameObjectsWithTag("Mouse_Hole");

        int holeChecker = 0;

        foreach (GameObject hole in holes)
        {
            float catDistanceFromHole = Vector3.Distance(player.position, hole.transform.position);
            float ratDistanceFromHole = Vector3.Distance(transform.position, hole.transform.position);

            if(catDistanceFromHole < ratDistanceFromHole)
            {
                // DO NOT CHOOSE THIS HOLE
                holeChecker += 1;
            }
            else if (currentMouseHole == null)
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

            // If all holes are bad, in the end, no mouse hole is chosen
            if(holeChecker == holes.Length)
            {
                currentMouseHole = null;
            }
        }
    }

    public void EnterHole()
    {
        if (currentMouseHole != null)
        {
            float distanceMouseHole = Vector3.Distance(this.transform.position, currentMouseHole.transform.position);

            // If rat is close enough and its not idling, enter hole!
            if (distanceMouseHole < 2 && currentMouseState != MouseState.Idle)
            {
                // Let manager spawn another mouse
                mouseManager.CanMouseSpawnLever(true);

                // Destroy rat
                Destroy(this.gameObject);
            }
        }
    }
}
