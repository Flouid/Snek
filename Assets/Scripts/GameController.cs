using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int levelX, levelY;
    [SerializeField] private int startX, startY, foodX, foodY;
    [SerializeField] private int startLength;
    [SerializeField] private int fpsTarget;

    public void Awake()
    {
        Application.targetFrameRate = fpsTarget;
    }

    public int GetLevelX()
    {
        return levelX;
    }

    public int GetLevelY()
    {
        return levelY;
    }
}
