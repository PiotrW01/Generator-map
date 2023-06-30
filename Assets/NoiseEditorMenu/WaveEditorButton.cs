using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEditorButton : MonoBehaviour
{
    public void RemoveWave()
    {
        Destroy(transform.parent.gameObject);
    }
}
