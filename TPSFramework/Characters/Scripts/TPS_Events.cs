using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Third Person Game/Events Hub", 100)]
public class TPS_Events : MonoBehaviour
{
    public static TPS_Events Current;

    public float PlayerMovementMagnitude;

    void Awake()
    {
        Current = this;
    }

    ~TPS_Events()
    {
        Current = null;
    }
}
