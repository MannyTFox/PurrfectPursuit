using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientDroppedObject : MonoBehaviour
{
    // Has to have timer to dissapear, the rest doenst really matter, because it will be on already
    [SerializeField] float timerToDissapear = 60;
    float blinkTime;

    [SerializeField] List<Renderer> objectRenderers = new List<Renderer>();
    bool canBlink = false;
    bool startBlinkOnce = true;

    private void Start()
    {
        blinkTime = timerToDissapear / 3;
    }

    // Update is called once per frame
    void Update()
    {
        DissapearCheck();
    }

    public void DissapearCheck()
    {
        if(timerToDissapear < 0)
        {
            // Dissapear
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            timerToDissapear -= Time.deltaTime;
        }

        // Blink time
        if(timerToDissapear < blinkTime && startBlinkOnce)
        {
            startBlinkOnce = false;

            canBlink = true;

            if (objectRenderers.Count > 0)
            {
                // Start blinking to signalize to player item is dissapearing
                StartCoroutine(Blink());
            }
            else
            {
                Debug.LogWarning("Object: " + gameObject.name + " | Has no renderer on list to blink when dissapearing");
            }
        }
    }

    public void ChangeDissapearTimer(float newTime)
    {
        timerToDissapear = newTime;
        blinkTime = newTime / 3;
    }

    public IEnumerator Blink()
    {
        // Dissapear
        MakeRenderersEnabled(false);

        yield return new WaitForSeconds(0.3f);
        
        // Appear
        MakeRenderersEnabled(true);

        yield return new WaitForSeconds(0.3f);

        if (canBlink)
        {
            StartCoroutine(Blink());
            //print("recall blink");
        }
    }

    public void MakeRenderersEnabled(bool lever)
    {
        if (lever)
        {
            foreach (Renderer renderer in objectRenderers)
            {
                renderer.enabled = true;
            }
        }
        else
        {
            foreach (Renderer renderer in objectRenderers)
            {
                renderer.enabled = false;
            }
        }
    }
}
