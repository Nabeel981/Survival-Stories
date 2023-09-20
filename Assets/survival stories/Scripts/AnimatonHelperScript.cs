using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatonHelperScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnAnimatorOff()
    {
        GetComponent<Animator>().enabled = false;
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
