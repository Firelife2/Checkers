using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileNamer : MonoBehaviour
{
    void Update()
    {
        name = $"({transform.position.x}, {transform.position.z})";
    }
}
