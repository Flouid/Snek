using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment snakeSegmentPrefab;

    private SnakeSegment _head, _tail;
    private int snakeLength;

    public void Init(GridPosition startPos, int startLength)
    {
        Assert.IsTrue(startLength > 1);
        _head = CreateSegment(null, null, startPos, snakeLength);
        _tail = _head;
        snakeLength = 1;
        
        for (int segmentsLeft = startLength - 1; segmentsLeft > 0; --segmentsLeft)
        {
            Grow();
        }
    }

    void Grow()
    {
        SnakeSegment newSegment = CreateSegment(_tail, null, _tail._pos.Behind(), snakeLength);
        _tail._prev = newSegment;
        newSegment._next = _tail;
        _tail = newSegment;
        ++snakeLength;
    }

    SnakeSegment CreateSegment(SnakeSegment next, SnakeSegment prev, GridPosition startPos, int index)
    {
        var newSegmentObject = Instantiate(snakeSegmentPrefab);
        newSegmentObject.name = $"Segment {index}";
        SnakeSegment newSegment = newSegmentObject.GetComponent<SnakeSegment>();
        newSegment.Init(null, null, startPos, index);
        return newSegment;
    }

}
