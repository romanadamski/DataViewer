using UnityEngine;

public class GameLauncher : BaseManager<GameLauncher>
{
    [SerializeField]
    private Canvas canvas;
    public Canvas Canvas => canvas;
}
