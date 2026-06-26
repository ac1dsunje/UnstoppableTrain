using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public event Action OnLeft;
    public event Action OnRight;
    public event Action OnRestart;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnLeft?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnRight?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnRestart?.Invoke();
        }
    }
}