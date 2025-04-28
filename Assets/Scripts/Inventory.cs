using Unity.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private Grid _grid;
    [SerializeField] private GameObject _tilePrefab;

    [SerializeField] private int rows;
    [SerializeField] private int cols;
    [SerializeField] private int tileSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Calcola la posizione iniziale dal margine sinistro
        Vector3 startPosition = _grid.GetCellCenterWorld(Vector3Int.zero);

        for (int i = 0; i < rows; i++) 
        {
            for (int j = 0; j < cols; j++)
            {
                // Calcola la posizione del tile corrente
                Vector3 worldPosition = startPosition + new Vector3(j * 100, -i * 100, 0);
                var tile = Instantiate(_tilePrefab, worldPosition, Quaternion.identity, transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
