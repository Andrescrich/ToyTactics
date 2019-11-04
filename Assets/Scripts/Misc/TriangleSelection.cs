using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleSelection : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(0f, 75f * Time.fixedDeltaTime, 0f);
    }
}
