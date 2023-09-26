using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugInfoController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerSpeedText;
    [SerializeField] PlayerMovement catMov;

    [Space(10)]
    [SerializeField] TextMeshProUGUI playerVelocityText;
    [SerializeField] Rigidbody catRb;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateVelocityText());
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeedText.text = "Player Speed: " + catMov.movementSpeed;
    }

    public IEnumerator UpdateVelocityText()
    {
        yield return new WaitForSeconds(0.5f);

        playerVelocityText.text = "Player Velocity: " + catRb.velocity;

        StartCoroutine(UpdateVelocityText());
    }
}
