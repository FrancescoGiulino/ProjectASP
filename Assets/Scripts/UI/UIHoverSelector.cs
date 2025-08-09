using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverSelector : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    private MenuHandler menuHandler => GetComponentInParent<MenuHandler>();

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy || !gameObject.GetComponent<Selectable>())
            return;

        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData) => menuHandler.PlayHoverSound();
}
