using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObject : MonoBehaviour
{
    private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
    }

    public void Select(float outlineWidth)
    {
        outline.OutlineWidth = outlineWidth;
    }

    public void Deselect() 
    {
        outline.OutlineWidth = 0;
    }
}
