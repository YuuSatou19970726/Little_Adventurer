using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    [SerializeField]
    private VisualEffect FootStep;

    public void BurstFootStep()
    {
        FootStep.SendEvent("OnPlay");
    }
}
