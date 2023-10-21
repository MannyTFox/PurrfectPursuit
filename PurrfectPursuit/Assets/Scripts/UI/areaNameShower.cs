using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaNameShower : MonoBehaviour
{
    [Tooltip("Must be equal to the number in UI Manager list corresponding to area")]
    [SerializeField] int areaIndex;
    [SerializeField] float distanceToShow = 10;
    bool nameCanShow = true;
    Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.gameManagerInstance.GetPlayerTransform();
    }

    private void Update()
    {
        if(Vector3.Distance(playerTransform.position, transform.position) < distanceToShow && nameCanShow == true)
        {
            nameCanShow = false;
            Show();
        }
        else if(Vector3.Distance(playerTransform.position, transform.position) >= distanceToShow && nameCanShow == false)
        {
            nameCanShow = true;

            UIManager.instanceUIManager.DissapearAreaName();
        }   
    }

    public void Show()
    {
        StartCoroutine(UIManager.instanceUIManager.ShowAreaName(areaIndex));
    }
}
