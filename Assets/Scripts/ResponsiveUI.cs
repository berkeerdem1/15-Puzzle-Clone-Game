using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveUI : MonoBehaviour
{
    void Update()
    {
        float scaleFactor = Screen.width / 1080f;
        this.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }

}
