using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public SurroundingCheckAction surroundCheck;
    public State currentState;
    public State remainState;
    public NavMeshAgent agent;
    public Transform target;
    public Vector3 lastKnownMyLocation;
    public bool _isActive;
    public Animator characterAnim;
    private void Awake()
    {

        agent = this.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        lastKnownMyLocation = transform.position;
        InitializeAI();
    }
    void Update()
    {
        if (!_isActive) return;
        characterAnim.SetFloat("Blend", currentState.animationBlendTreeFloat);
        currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
        }
    }
    public void InitializeAI()
    {

        _isActive = true;
        agent.enabled = _isActive;
    }
}
