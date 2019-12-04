using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ItemType type;
    private Item currentItem;

    public void Spawn()
    {
        int amtObj = 0;
        switch (type)

        {
            case ItemType.jump:
                amtObj = LevelManager.Instance.jumps.Count;
                break;
            case ItemType.longblock:
                amtObj = LevelManager.Instance.longblocks.Count;
                break;
            case ItemType.ramp:
                amtObj = LevelManager.Instance.ramps.Count;
                break;
            case ItemType.house:
                amtObj = LevelManager.Instance.houses.Count;
                break;
        }

        currentItem = LevelManager.Instance.GetItem(type, Random.Range(0,amtObj));
        currentItem.gameObject.SetActive(true);
        currentItem.transform.SetParent(transform, false);
    }

    public void Despawn()
    {
        currentItem.gameObject.SetActive(false);
    }
}
