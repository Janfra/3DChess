using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : ObjectLerper
{
    private Transform boardCenter;
    private bool isFirstRotation;

    private void Awake()
    {
        if (speed == 0f)
        {
            speed = 1f;
        }
        transition = 1f;
        isFirstRotation = true;
        GameManager.OnGameStateChanged += CheckRotation;
    }

    private void Start()
    {
        transform.position = new Vector3((float)GridManager.Instance.GetGridWidth() / 2 - 0.5f, 5, -10);
        initialPosition = transform.position;
        boardCenter = GridManager.Instance.GetGridCenter();
        targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 20);
    }

    private void CheckRotation(GameState state)
    {
        currentPos = 0;
        if(state == GameState.BlackTurn)
        {
            StartCoroutine(EvaluateSlerpPoints(initialPosition, targetPosition, 0));
        } 
        else if(state == GameState.WhiteTurn)
        {
            if (isFirstRotation)
            {
                isFirstRotation = false;
                return;
            }
            StartCoroutine(EvaluateSlerpPoints(targetPosition, initialPosition, 0));
        }
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        transform.LookAt(boardCenter);
    }

    IEnumerator<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 target, float centerOffset)
    {
        var centerPivot = (start + target) * 0.5f;

        centerPivot -= new Vector3(0, -centerOffset);

        var startRelativeCenter = start - centerPivot;
        var targetRelativeCenter = target - centerPivot;


        while (currentPos != transition)
        {
            currentPos = Mathf.MoveTowards(currentPos, transition, speed * Time.deltaTime);
            yield return transform.position = Vector3.Slerp(startRelativeCenter, targetRelativeCenter, animationCurve.Evaluate(currentPos)) + centerPivot;
        }
    }
}
