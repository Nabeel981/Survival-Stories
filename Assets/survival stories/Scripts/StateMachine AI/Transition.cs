using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition 
{
    public Decisions decision;
    public State trueState;
    public State falseState;
}
