using System;
using UnityEngine;

public class RailController : MonoBehaviour
{
    public Action<bool> OnThisActive;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Train")
        {
            SetRailActive();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Train")
        {
            SetRailUnActive();
        }
    }

    private void SetRailActive()
    {
        OnThisActive.Invoke(true);
    }

    private void SetRailUnActive()
    {
        OnThisActive.Invoke(false);
    }
}
