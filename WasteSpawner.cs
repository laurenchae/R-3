using UnityEngine;

public class WasteSpawner : MonoBehaviour
{
    public int maxWaste = 5;
    public float chanceToSpawn = 0.5f;
    public bool forceSpawnAll = false;

    private GameObject[] wastes;

    private void Awake()
    {
        wastes = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            wastes[i] = transform.GetChild(i).gameObject;

        OnDisable();
    }

    private void OnEnable()
    {
        if (Random.Range(0.0f, 1.0f) < chanceToSpawn)
            return;

        if (forceSpawnAll)
        {
            for (int i = 0; i < maxWaste; i++)
            {
                wastes[i].SetActive(true);
            }
        }
        else
        {
            int r = Random.Range(0, maxWaste);
            for (int i = 0; i < r; i++)
                wastes[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject go in wastes)
            go.SetActive(false);
    }

}