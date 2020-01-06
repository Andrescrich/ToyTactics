using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixeoFastUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 position;
    private void Awake()
    {
        position = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = position;
    }
}
