using System;
using UnityEngine;

public class ObjectLerper : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private AnimationCurve animationCurve;
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private float currentPos;
    private float transition;
    private bool isMoving => transform.position != targetPosition;

    private void Awake()
    {
        speed = 1f;
        transition = 1f;
        currentPos = 0f;
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveObject();
        }
    }

    public void SetMovePosition(Vector3 newTarget)
    {
        initialPosition = transform.position;
        targetPosition = newTarget;
        currentPos = 0;
    }

    private void MoveObject()
    {
        currentPos = Mathf.MoveTowards(currentPos, transition, speed * Time.deltaTime);
        transform.position = Vector3.Lerp(initialPosition, targetPosition, animationCurve.Evaluate(currentPos));
        //Debug.Log($"currentPos = {currentPos}, transition = {transition}, initial position: {initialPosition}, target: {targetPosition}");
    }
}
