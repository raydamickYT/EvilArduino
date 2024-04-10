using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    [SerializeField]
    private FMODUnity.EventReference ScreamAction;
    [SerializeField]
    private FMODUnity.EventReference SwearingAction;
    public static Action StartScreamSFX, StartSwearing, StopSwearingAction;
    private EventInstance swearingInstance;

    // Start is called before the first frame update
    void Start()
    {
        if (!SwearingAction.IsNull)
        {
            swearingInstance = FMODUnity.RuntimeManager.CreateInstance(SwearingAction);
        }
        else
        {
            Debug.LogWarning("SwearingSFX is niet assigned.");
        }
        StartScreamSFX += PlayScreamSFX;
        StartSwearing += PlaySwearing;
        StopSwearingAction += StopSwearing;
    }

    void PlayScreamSFX()
    {
        if (!ScreamAction.IsNull)
        {

            FMODUnity.RuntimeManager.PlayOneShot(ScreamAction);
        }
        else
        {
            Debug.LogWarning("ScreamSFX is niet assigned.");
        }
    }

    void PlaySwearing()
    {
        if (!ScreamAction.IsNull)
        {
            swearingInstance.start();
        }
        else
        {
            Debug.LogWarning("SwearingSFX is niet assigned.");
        }
    }
    void StopSwearing()
    {
        if (!SwearingAction.IsNull)
        {
            swearingInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    void OnDestroy()
    {
        StartScreamSFX = null;
    }
}
