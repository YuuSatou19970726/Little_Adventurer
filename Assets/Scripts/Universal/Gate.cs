using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private GameObject GateVisual;
    private Collider _gateCollider;

    [HideInInspector]
    public float OpenDuration = 2f;
    [HideInInspector]
    public float OpentargetY = -2f;

    void Awake()
    {
        _gateCollider = GetComponent<Collider>();
    }

    IEnumerator OpenGateAnimation()
    {
        float currentOpenDuration = 0;
        Vector3 startPos = GateVisual.transform.position;
        Vector3 targetPos = startPos + Vector3.down * OpentargetY;

        while (currentOpenDuration < OpenDuration)
        {
            currentOpenDuration += Time.deltaTime;
            GateVisual.transform.position = Vector3.Lerp(startPos, targetPos, currentOpenDuration / OpenDuration);

            yield return null;
        }

        _gateCollider.enabled = false;
    }

    public void Open()
    {
        StartCoroutine(OpenGateAnimation());
    }
}
