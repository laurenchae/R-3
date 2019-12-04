using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    none = -1,
    ramp = 0,
    longblock = 1,
    jump = 2,
    house = 3
}

public class Item : MonoBehaviour
{
    public ItemType type;
    public int visualIndex;
}
