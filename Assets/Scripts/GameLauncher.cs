using UnityEngine;

public class GameLauncher : SingletonBase<GameLauncher>
{
    [SerializeField]
    private Canvas canvas;
    public Canvas Canvas => canvas;
}
