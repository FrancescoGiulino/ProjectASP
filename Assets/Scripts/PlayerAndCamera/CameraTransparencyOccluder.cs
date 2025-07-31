using UnityEngine;
using System.Collections.Generic;

public class ObstructionFader : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float targetAlpha = 0.3f;

    private Dictionary<Renderer, float> fadingObjects = new();
    private List<Renderer> currentlyHit = new();

    void Update()
    {
        Vector3 camToPlayer = player.position - transform.position;
        Ray ray = new(transform.position, camToPlayer);
        float distance = camToPlayer.magnitude;

        currentlyHit.Clear();
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, obstructionMask);

        foreach (RaycastHit hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                currentlyHit.Add(rend);
                if (!fadingObjects.ContainsKey(rend))
                    fadingObjects[rend] = 1f; // alpha originale

                FadeTo(rend, targetAlpha);
            }
        }

        // Ripristina oggetti che non sono più occludenti
        List<Renderer> toRestore = new();
        foreach (var rend in fadingObjects.Keys)
        {
            if (!currentlyHit.Contains(rend))
            {
                FadeTo(rend, 1f);
                if (Mathf.Approximately(rend.material.color.a, 1f))
                    toRestore.Add(rend); // ripristinato
            }
        }

        foreach (var r in toRestore)
            fadingObjects.Remove(r);
    }

    void FadeTo(Renderer rend, float alphaTarget)
    {
        Material mat = rend.material;
        Color c = mat.color;
        float newAlpha = Mathf.MoveTowards(c.a, alphaTarget, fadeSpeed * Time.deltaTime);
        c.a = newAlpha;
        mat.color = c;

        // Assicura che il materiale supporti trasparenza
        mat.SetFloat("_Surface", 1); // Transparent
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}
