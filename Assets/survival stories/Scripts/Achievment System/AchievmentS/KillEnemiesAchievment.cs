using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BadgeAchievment/KillEnemies")]
public class KillEnemiesAchievment : Achievment
{
    public int enemiesToKill;
    public override void ShowBadge()
    {
        throw new System.NotImplementedException();
    }

    public override bool unlockAchievment()
    {
        if (AchievmentSystem.TotalEnemiesKilled >= enemiesToKill && !isCompleted )
        {
            isCompleted = true;
            return true;
        }
        return false;

    }
}
