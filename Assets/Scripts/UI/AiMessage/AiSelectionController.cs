using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AiSelectionController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    public GameObject FirstSelectable { get; set; }

    private GameObject lastSelected;
    private GameObject lastMessageSent; // ultimo messaggio inviato

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        // Gestisci solo lo scroll se la selezione cambia
        var selected = EventSystem.current.currentSelectedGameObject;

        if (selected != null && selected.transform.IsChildOf(scrollRect.content))
        {
            if (selected != lastSelected)
            {
                lastSelected = selected;
                ScrollTo(selected.GetComponent<RectTransform>());
            }
        }
        else
        {
            // Non forzare selezione qui, lascia fare solo all'apertura o all'interazione utente
        }
    }

    public void ResetSelection()
    {
        EventSystem.current?.SetSelectedGameObject(null);
        lastSelected = null;
    }

    public void SelectNextFrame(GameObject go)
    {
        if (go != null)
            StartCoroutine(SetSelectedNextFrame(go));
    }

    private IEnumerator SetSelectedNextFrame(GameObject go)
    {
        yield return null;

        if (go != null && go.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(go);
            lastSelected = go;
            ScrollTo(go.GetComponent<RectTransform>());
        }
    }

    // Metodo da chiamare all'apertura pannello per selezionare l'ultimo messaggio
    public void SelectLastMessage()
    {
        if (lastMessageSent != null)
        {
            SelectNextFrame(lastMessageSent);
        }
        else if (FirstSelectable != null)
        {
            SelectNextFrame(FirstSelectable);
        }
    }

    // Quando arriva un nuovo messaggio, aggiorno i riferimenti ma NON cambio selezione o scroll!
    public void RegisterNewMessage(GameObject obj)
    {
        if (obj == null) return;

        lastMessageSent = obj;
        FirstSelectable = obj;

        // NOTA: qui NON chiamiamo ScrollTo o SetSelectedGameObject!
    }

    private void ScrollTo(RectTransform target)
    {
        if (target == null) return;

        Vector3 localPos = scrollRect.content.InverseTransformPoint(target.position);
        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float targetY = -localPos.y;

        float normalized = Mathf.Clamp01((targetY - viewportHeight / 2) / (contentHeight - viewportHeight));
        scrollRect.verticalNormalizedPosition = 1f - normalized;
    }
}
