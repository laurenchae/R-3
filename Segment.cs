using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public int SegId { set; get; }
    public bool transition;

    public int length;
    public int beginY1, beginY2, beginY3;
    public int endY1, endY2, endY3;

    private ItemSpawner[] items;

    private void Awake()
    {
        items = gameObject.GetComponentsInChildren<ItemSpawner>();
        for (int i = 0; i < items.Length; i++) //$$
        {
            foreach (MeshRenderer mr in items[i].GetComponentsInChildren<MeshRenderer>())
                mr.enabled = false;
            Debug.Log(LevelManager.Instance.SHOW_COLLIDER);
            //mr.enabled = LevelManager.Instance.SHOW_COLLIDER;
        }
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < items.Length; i++)
        {
            items[i].Spawn();
    
        }
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < items.Length; i++)
        {
            items[i].Despawn();

        }
    }
}

