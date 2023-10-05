using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mortarPestleAnimation : MonoBehaviour
{
    IngredientProcessor processor;
    [SerializeField] Animator mortarNPestleAnimator;

    // Start is called before the first frame update
    void Start()
    {
        processor = GetComponent<IngredientProcessor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (processor.IsMachineProcessing())
        {
            mortarNPestleAnimator.SetBool("inUse", true);
        }
        else if(processor.IsMachineProcessing() == false)
        {
            mortarNPestleAnimator.SetBool("inUse", false);
        }
    }
}
