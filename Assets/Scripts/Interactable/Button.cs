using UnityEngine;

public class Button : Interactable
{
    private int pressCount = 0;

    public override void Interact()
    {
        // Nothing to do here, Button activate only when pressed
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Rigidbody"))
        {
            pressCount++;
        }
        UpdateButtonState();
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Rigidbody"))
        {
            pressCount--;
            if (pressCount < 0)
                pressCount = 0; // Ensure pressCount does not go negative
        }
        UpdateButtonState();
    }

    protected void UpdateButtonState()
    {
        if (pressCount > 0)
            State = true;
        else
            State = false;

        HandleAnimation();
        HandleColorChange();
        HandleLight();

        Debug.Log($"Button --> pressCount: {pressCount}");
        Debug.Log($"Button --> State: {State}");
    }
}
