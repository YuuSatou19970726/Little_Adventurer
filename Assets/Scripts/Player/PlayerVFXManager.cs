using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    [SerializeField]
    private VisualEffect footStep;

    [SerializeField]
    private ParticleSystem blade01;

    [SerializeField]
    private VisualEffect slash;

    public void Update_FootStep(bool state)
    {
        if (state)
            footStep.Play();
        else
            footStep.Stop();
    }

    public void PlayBlade01()
    {
        blade01.Play();
    }

    public void PlaySlash(Vector3 pos)
    {
        slash.transform.position = pos;
        slash.Play();
    }
}
