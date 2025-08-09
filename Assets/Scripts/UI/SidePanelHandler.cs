using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SidePanelHandler : MonoBehaviour
{
    [SerializeField] private GameObject sidePanel;
    [SerializeField] private GameObject firstSelectable;
    [SerializeField] private ScrollRect scrollRect;

    private GameInput gameInput;
    private GameObject lastSelected;

    private void Awake()
    {
        gameInput = FindFirstObjectByType<GameInput>();
        if (gameInput == null)
            Debug.LogError("SidePanelHandler: nessun GameInput trovato.");
    }

    private void OnEnable()
    {
        if (gameInput != null)
            gameInput.OnToggleSidePanel += ToggleSidePanel;

        // Resetta la selezione all’attivazione
        EventSystem.current?.SetSelectedGameObject(null);
    }

    private void OnDisable()
    {
        if (gameInput != null)
            gameInput.OnToggleSidePanel -= ToggleSidePanel;
    }

    private void ToggleSidePanel(object sender, EventArgs e)
    {
        if (sidePanel == null)
        {
            Debug.LogWarning("SidePanelHandler: sidePanel non assegnato.");
            return;
        }

        bool isActive = !sidePanel.activeSelf;
        sidePanel.SetActive(isActive);

        if (isActive && firstSelectable != null)
            StartCoroutine(SetSelectedNextFrame(firstSelectable));
        else
            EventSystem.current?.SetSelectedGameObject(null);
    }

    private System.Collections.IEnumerator SetSelectedNextFrame(GameObject go)
    {
        yield return null; // aspetta un frame

        if (go != null && go.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(go);
            lastSelected = go;
        }
    }

    private void Update()
    {
        if (!sidePanel.activeSelf)
            return;

        var selected = EventSystem.current.currentSelectedGameObject;

        // Se nessuno è selezionato o selezione fuori dal contenuto, forza il primo selezionabile
        if (selected == null || !selected.transform.IsChildOf(scrollRect.content))
        {
            if (firstSelectable != null && lastSelected != firstSelectable)
            {
                StartCoroutine(SetSelectedNextFrame(firstSelectable));
            }
            return;
        }

        // Scroll solo se la selezione è cambiata
        if (selected != lastSelected)
        {
            lastSelected = selected;
            ScrollTo(selected.GetComponent<RectTransform>());
        }
    }

    private void ScrollTo(RectTransform target)
    {
        if (target == null)
            return;

        Vector3 localPos = scrollRect.content.InverseTransformPoint(target.position);

        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float targetY = -localPos.y;

        float normalized = Mathf.Clamp01((targetY - viewportHeight/2) / (contentHeight - viewportHeight));

        Debug.Log($"contentHeight: {contentHeight}");
        Debug.Log($"viewportHeight: {viewportHeight}");
        Debug.Log($"TargetY: {targetY}");
        Debug.Log($"normalized: {normalized}");

        // ScrollRect usa 1 = top, 0 = bottom
        scrollRect.verticalNormalizedPosition = 1f - normalized;
    }
}
