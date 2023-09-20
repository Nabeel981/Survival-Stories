using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
    public delegate void WeaponHitBoxTriggerEnter(GameObject character);
    public event WeaponHitBoxTriggerEnter hitEvent;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Player Was Hit was called");
        if (collision.TryGetComponent<AICharacterBehaviour>(out AICharacterBehaviour otherInfo))
        {
         //   Debug.Log("DAmage Dealt to " + otherInfo.playerType.ToString());
            hitEvent?.Invoke(collision.gameObject);





        }
    }
}
