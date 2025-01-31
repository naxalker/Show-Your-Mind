#if UNITY_EDITOR
using ParrelSync;
#endif
using UnityEngine;

public class EditorApplicationController : MonoBehaviour
{
    public ApplicationController _controller;

    public void Start()
    {
#if UNITY_EDITOR

        if (ClonesManager.IsClone())
        {
            var argument = ClonesManager.GetArgument();
            if (argument == "server")
                _controller.OnParrelSyncStarted(true, "server");
            else if (argument == "client")
            {
                _controller.OnParrelSyncStarted(false, "client");
            }
        }
        else
            _controller.OnParrelSyncStarted(false, "client");
#endif
    }
}
