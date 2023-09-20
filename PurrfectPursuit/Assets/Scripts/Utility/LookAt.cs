using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
            this.transform.eulerAngles = new Vector3(180, this.transform.eulerAngles.y, 180);
        }
    }
}
