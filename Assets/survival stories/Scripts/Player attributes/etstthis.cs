using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class etstthis : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Enabled Called");
    }

    private void Awake()
    {
        Debug.Log("Awake Called");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Called");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
