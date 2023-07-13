using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Demo : MonoBehaviour
{
    [DebugAttribute("Time")]
    private float time = 0;
    [DebugAttribute("delta time")]
    private float deltaTime = 0;

    void Start()
    {
        
    }

    void Update()
    {
        time = Time.time;
        deltaTime = Time.deltaTime;
    }
}
