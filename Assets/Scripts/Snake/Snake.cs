using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment snakeSegmentPrefab;

    private SnakeSegment _head, _tail;
    private PlayerInputController _inputController;
    private int snakeLength;

    public void Init(GridPosition startPos, int startLength)
    {
        // assert that the snake can have at least a head and a tail
        Assert.IsTrue(startLength > 1);
        _head = CreateSegment(null, null, startPos, snakeLength);
        _tail = _head;
        snakeLength = 1;
        
        // grow potentially several times depending on start config
        for (int segmentsLeft = startLength - 1; segmentsLeft > 0; --segmentsLeft) Grow();
    }

    public Direction SampleInput()
    {
        return _inputController.queuedInput;
    }

    void Start()
    {
        _inputController = GetComponent<PlayerInputController>();
    }

    void Grow()
    {
        // spawn the new segment in the correct location
        SnakeSegment newSegment = CreateSegment(_tail, null, _tail._pos.Behind(), snakeLength);
        // adjust the references to neighboring segments
        _tail._prev = newSegment;
        newSegment._next = _tail;
        _tail = newSegment;
        // track change to length
        ++snakeLength;
    }

    SnakeSegment CreateSegment(SnakeSegment next, SnakeSegment prev, GridPosition startPos, int index)
    {
        // instantiate a new segment
        var newSegmentObject = Instantiate(snakeSegmentPrefab);
        // name it according to the length of the snake at creation time
        newSegmentObject.name = $"Segment {index}";
        // get the controller for the new segment, initialize it and return
        SnakeSegment newSegment = newSegmentObject.GetComponent<SnakeSegment>();
        newSegment.Init(null, null, startPos, index);
        return newSegment;
    }

}
