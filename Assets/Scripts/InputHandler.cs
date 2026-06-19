using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Action OnLeft;
    public Action OnRight;

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
    }
}