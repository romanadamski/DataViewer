using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class MainCanvas : SingletonBase<MainCanvas>
{
    public Canvas Canvas { get; private set; }

    private void Awake()
    {
        Canvas = GetComponent<Canvas>();
    }
}
