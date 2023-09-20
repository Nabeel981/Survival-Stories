using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;

public class NavMeshSystem : MonoBehaviour
{
    public static NavMeshSystem instance;
    public NavMeshSurface navSurface;

    private void OnEnable()
    {
        if (!instance) instance = this;
    }
    [ContextMenu("build navmesh")]
    public void BakeEnvoirnment()
    {

        navSurface.BuildNavMeshAsync();

    }
}
