using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverSelector : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy || !gameObject.GetComponent<Selectable>())
            return;

        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
