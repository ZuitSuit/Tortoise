using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCalls : MonoBehaviour
{
    public void Restart()
    {
        Debug.Log("pressed");
        GameManager.Instance.Restart();
    }
}
