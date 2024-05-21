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

    public void BurstFootStep()
    {
        FootStep.SendEvent("OnPlay");
    }

    public void PlayAttackVFX()
    {
        AttackVFX.SendEvent("OnPlay");
    }
}
