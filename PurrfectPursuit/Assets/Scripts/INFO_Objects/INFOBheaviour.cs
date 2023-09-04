using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class INFOBheaviour : MonoBehaviour
{
    [SerializeField] GameObject proximityImage;

    [SerializeField] GameObject canvasINFOReference;

    INFOObject lookINFOScript;

    private void Start()
    {
        lookINFOScript = transform.Find("INFO_Object").GetComponent<INFOObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            canvasINFOReference.SetActive(true);
            proximityImage.SetActive(true);

            // Info stops looking and plays animation
            lookINFOScript.InfoStopLooking();
            lookINFOScript.PlaySpinAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvasINFOReference.SetActive(false);
            proximityImage.SetActive(false);

            // Info can look again
            lookINFOScript.InfoCanLookAgain();
        }
    }
}
