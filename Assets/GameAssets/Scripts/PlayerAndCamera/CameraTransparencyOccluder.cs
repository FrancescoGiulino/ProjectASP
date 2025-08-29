using UnityEngine;
using System.Collections.Generic;

public class CameraTransparencyOccluder : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private float checkInterval = 0.05f;
    [SerializeField] private Material transparentMaterial;

    private class OriginalRenderData
    {
        public Renderer renderer;
        public Material[] originalMaterials;
    }

    private readonly List<OriginalRenderData> currentObstructions = new();
    private readonly Dictionary<Renderer, Material[]> originalMaterialsMap = new();

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        RestorePreviousObstructions();

        Vector3 direction = player.position + (Vector3.up * 1.1f) - transform.position;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance, obstructionMask);
        //Debug.DrawRay(transform.position, direction, Color.red, 1f);

        HashSet<GameObject> processed = new();

        foreach (var hit in hits)
        {
            GameObject go = hit.collider.gameObject;
            if (processed.Contains(go)) continue;
            processed.Add(go);

            // Cambia materiale a tutti i renderer figli, 
            // ma **escludi quelli con nome "ShadowCaster"**
            foreach (Renderer rend in go.GetComponentsInChildren<Renderer>())
            {
                if (rend.gameObject.name == "ShadowCaster")
                    continue; // salta il renderer "ShadowCaster"

                if (!originalMaterialsMap.ContainsKey(rend))
                {
                    originalMaterialsMap[rend] = rend.sharedMaterials;
                    Material[] transparentMats = new Material[rend.sharedMaterials.Length];
                    for (int i = 0; i < transparentMats.Length; i++)
                        transparentMats[i] = transparentMaterial;

                    rend.materials = transparentMats;
                    currentObstructions.Add(new OriginalRenderData
                    {
                        renderer = rend,
                        originalMaterials = originalMaterialsMap[rend]
                    });
                }
            }
        }
    }

    private void RestorePreviousObstructions()
    {
        foreach (var data in currentObstructions)
        {
            if (data.renderer != null)
                data.renderer.materials = data.originalMaterials;
        }

        currentObstructions.Clear();
        originalMaterialsMap.Clear();
    }
}
