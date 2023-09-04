using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INFOObject : MonoBehaviour
{
    [SerializeField] Transform target;
    Animator infoObjectAnimator;

    public bool canLook = true;

    private void Start()
    {
        infoObjectAnimator = GetComponent<Animator>();
        infoObjectAnimator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canLook)
        {
            LookingAtTarget();
        }
    }

    public void LookingAtTarget()
    {
        transform.LookAt(target);
        this.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void InfoStopLooking()
    {
        infoObjectAnimator.enabled = true;
        canLook = false;
    }

    public void InfoCanLookAgain()
    {
        infoObjectAnimator.enabled = false;
        canLook = true;
    }

    public void PlaySpinAnimation()
    {
        infoObjectAnimator.SetTrigger("spin");
    }
}
