using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    [SerializeField]
    private VisualEffect FootStep;
    [SerializeField]
    private VisualEffect AttackVFX;
    [SerializeField]
    private ParticleSystem BeingHitVFX;
    [SerializeField]
    private VisualEffect BeingHitSplashVFX;

    public void BurstFootStep()
    {
        FootStep.SendEvent("OnPlay");
    }

    public void PlayAttackVFX()
    {
        AttackVFX.SendEvent("OnPlay");
    }

    public void PlayBeingHitVFX(Vector3 attackPos)
    {
        Vector3 forceForward = transform.position - attackPos;
        forceForward.Normalize();
        forceForward.y = 0f;
        BeingHitVFX.transform.rotation = Quaternion.LookRotation(forceForward);
        BeingHitVFX.Play();

        Vector3 splashPos = transform.position;
        splashPos.y += 2f;
        VisualEffect newSplashVFX = Instantiate(BeingHitSplashVFX, splashPos, Quaternion.identity);
        newSplashVFX.SendEvent("OnPlay");
        Destroy(newSplashVFX.gameObject, 10f);
    }
}
