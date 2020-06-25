using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisplay : MonoBehaviour
{
    private void Awake()
    {
        Registry.AddRegister<DebugDisplay>("debug_display", this);
    }
}
