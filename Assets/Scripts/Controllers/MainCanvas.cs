using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class MainCanvas : SingletonBase<MainCanvas>
{
    [SerializeField]
    private Canvas canvas;
    public Canvas Canvas => canvas;
    [SerializeField]
    private Transform menusParent;
    public Transform MenusParent => menusParent;

}
