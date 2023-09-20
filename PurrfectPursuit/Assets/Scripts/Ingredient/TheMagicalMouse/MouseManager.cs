using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager mouseManagerInstance;

    [SerializeField] List<Transform> mouseHoles = new List<Transform>();

    [SerializeField] GameObject mousePrefab;
    GameObject currentMouse;

    bool canSpawnMouse = true;
    bool isMouseAlive = true;

    private void Awake()
    {
        mouseManagerInstance = this.GetComponent<MouseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseCondition();

        if(currentMouse == null)
        {
            isMouseAlive = false;
        }
    }

    public void CanMouseSpawnLever(bool lever)
    {
        if (lever)
        {
            canSpawnMouse = true;
        }
        else
        {
            canSpawnMouse = false;
        }
    }

    public void CheckMouseCondition()
    {
        if (canSpawnMouse && isMouseAlive == false)
        {
            // Choose at random one of the holes to spawn the mouse, excluding the hole the mouse is closest to!
            Transform chosenHole;
            List<Transform> possibleHoles = new List<Transform>();

            foreach (Transform hole in mouseHoles)
            {
                possibleHoles.Add(hole);
            }

            Transform holeCloseToCat = null;

            foreach  (Transform hole in possibleHoles)
            {
                float distanceFromCat = Vector3.Distance(hole.position,
                                                        GameManager.gameManagerInstance.playerMov.gameObject.transform.position);

                if(holeCloseToCat == null)
                {
                    holeCloseToCat = hole;
                }
                // If distance of new hole is bigger than the old one, substitute
                else if(Vector3.Distance(holeCloseToCat.position, GameManager.gameManagerInstance.playerMov.gameObject.transform.position)
                    > distanceFromCat)
                {
                    holeCloseToCat = hole;
                }
            }

            // Random number of hole
            int randomNumber;

            // Remove worst hole form list and get a random one
            if (possibleHoles.Count > 1)
            {
                possibleHoles.Remove(holeCloseToCat);

                // Checking if its still bigger than 1
                if (possibleHoles.Count > 1)
                {
                    randomNumber = Random.Range(0, possibleHoles.Count);
                }
                else
                {
                    // If list doesnt have enough holes, set it to the first one
                    randomNumber = 0;
                }
            }
            else
            {
                // If list doesnt have enough holes, set it to the first one
                randomNumber = 0;
            }

            // Choose random hole from the rest
            chosenHole = possibleHoles[randomNumber];

            // Spawn mouse at chosen hole!
            Vector3 desiredSpawn = chosenHole.position + (Random.insideUnitSphere.normalized * 4);
            GameObject newMouse = Instantiate(mousePrefab, desiredSpawn, Quaternion.identity);

            currentMouse = newMouse;
            isMouseAlive = true;

            // Reset spawn condition
            canSpawnMouse = false;
        }
    }
}
