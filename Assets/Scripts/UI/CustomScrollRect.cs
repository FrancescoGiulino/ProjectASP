using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomScrollRect : ScrollRect
{
    public override void OnScroll(PointerEventData data)
    {
        if (!IsActive() || !enabled)
            return;

        // Scroll manuale indipendentemente dal fatto che ci sia un selectedGameObject
        float scrollDelta = data.scrollDelta.y * scrollSensitivity;

        if (vertical && Mathf.Abs(scrollDelta) > 0)
        {
            verticalNormalizedPosition += scrollDelta / content.rect.height;
            verticalNormalizedPosition = Mathf.Clamp01(verticalNormalizedPosition);
            data.Use();
        }
        else
        {
            base.OnScroll(data);
        }
    }
}
