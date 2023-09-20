using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/state")]
public class State : ScriptableObject
{
    public float animationBlendTreeFloat;
    public Actions[] action;
    public Transition[] transitions;

    public void UpdateState(StateController controller)
    {
        ExecuteAction(controller);
        CheckForTransition(controller);
    }
    private void ExecuteAction(StateController controller)
    {

        foreach (var item in action)
        {
            item.Act(controller);
        }
    }

    public void CheckForTransition(StateController controller)
    {

        foreach (var item in transitions)
        {
            bool decision = item.decision.Decide(controller);
            if (decision)
            {
                controller.TransitionToState(item.trueState);
             
            }
            else
            {
                controller.TransitionToState(item.falseState);
               
            }
        }
    }
}
