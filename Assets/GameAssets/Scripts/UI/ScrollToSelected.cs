using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollToSelected : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;

    private GameObject lastSelected;

    private void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current == null || current == lastSelected)
            return;

        if (current.transform.IsChildOf(content))
        {
            ScrollTo(current.GetComponent<RectTransform>());
            lastSelected = current;
        }
    }

    private void ScrollTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases(); // for good measure

        // Viewport e target devono essere nello stesso sistema di riferimento
        RectTransform viewport = scrollRect.viewport;

        // Calcola la posizione normalizzata Y dell'elemento rispetto al content
        Vector2 localPosition = (Vector2)content.InverseTransformPoint(target.position);
        Vector2 viewportLocalPosition = (Vector2)content.InverseTransformPoint(viewport.position);
        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;

        float extraOffset = 30f; // altezza in pixel da spostare verso l’alto
        float offset = localPosition.y - viewportLocalPosition.y + extraOffset;

        float normalizedPosition = scrollRect.verticalNormalizedPosition + (offset / (contentHeight - viewportHeight));

        // Clampa la posizione tra 0 e 1
        normalizedPosition = Mathf.Clamp01(normalizedPosition);

        scrollRect.verticalNormalizedPosition = normalizedPosition;
    }

    public void ScrollNow()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current == null || !current.transform.IsChildOf(content)) return;

        ScrollTo(current.GetComponent<RectTransform>());
    }

    public ScrollRect GetScrollRect() => scrollRect;
    public RectTransform GetContent() => content;

    public void SetScrollRect(ScrollRect scrollRect) => this.scrollRect=scrollRect;
    public void SetContent(RectTransform content) => this.content=content;
}
