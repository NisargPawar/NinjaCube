﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    private Vector3 offset = new Vector3(0,3,-13);
    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = playerTransform.position + offset;
    }
}
