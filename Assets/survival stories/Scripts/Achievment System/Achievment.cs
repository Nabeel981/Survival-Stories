using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Achievment : ScriptableObject
{
    public string iD;
    public string achievmentName;
    public string info;
    public bool isCompleted;
    public Sprite Badge;
    public abstract bool unlockAchievment();
    public abstract void ShowBadge();
}
