using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Dropdown))]
public class DropdownScroller : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDropdownChanged(int value)
    {
        // Non serve qui, ma teniamo il listener attivo
    }

    public void OnDropdownShown()
    {
        //StartCoroutine(ScrollToSelectedCoroutine());
    }

    private void Update()
    {
        if (GameObject.Find("Dropdown List") && ShouldUseGamepad())
        {
            ScrollToSelectedCoroutine();
        }
    }


    private void ScrollToSelectedCoroutine()
    {
        Debug.Log("[ScrollToSelectedCoroutine] started.");

        Transform dropdownList = GameObject.Find("Dropdown List")?.transform;
        
        if (dropdownList == null)
        {
            Debug.LogWarning("Dropdown List non trovato.");
        }

        ScrollRect scrollRect = dropdownList.GetComponentInChildren<ScrollRect>();
        if (scrollRect == null)
        {
            Debug.LogWarning("ScrollRect non trovato nel Dropdown List.");
        }

        RectTransform content = scrollRect.content;

        if (scrollRect.GetComponent<ScrollToSelected>() == null)
        {
            ScrollToSelected sts = scrollRect.gameObject.AddComponent<ScrollToSelected>();
            sts.SetScrollRect(scrollRect);
            sts.SetContent(content);
        }

        // Forza update immediato
        scrollRect.GetComponent<ScrollToSelected>()?.ScrollNow();
    }

    private bool ShouldUseGamepad()
    {
        // Puoi espandere con altre condizioni se usi il nuovo Input System
        return Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || Input.GetButton("Submit");
    }
}
