using UnityEngine;

public class ItemPickupRotator : MonoBehaviour
{
    public float rotationSpeed = 30f;   // Speed of rotation (degrees per second)
    public float bobAmplitude = 0.1f;   // Amplitude of the bobbing motion
    public float bobFrequency = 1.5f;   // Frequency of the bobbing motion (cycles per second)

    private Vector3 initialPosition;    // Initial position of the item pickup

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Rotate the item pickup around its y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Bob the item pickup up and down along the y-axis
        float bobOffset = Mathf.Sin(Time.time * 2 * Mathf.PI * bobFrequency) * bobAmplitude;
        transform.position = initialPosition + Vector3.up * bobOffset;
    }
}
