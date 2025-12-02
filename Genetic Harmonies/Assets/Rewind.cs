using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.Instance.Rewind();
        
    }
}
