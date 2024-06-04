using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        HEAL,
        COIN
    }

    public PickUpType Type;
    public int value = 20;
    public ParticleSystem CollectedVFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER))
        {
            other.gameObject.GetComponent<Character>().PickUpItem(this);

            if (CollectedVFX != null)
                Instantiate(CollectedVFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
