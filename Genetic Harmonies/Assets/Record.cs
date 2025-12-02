using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.Instance.Record();
    }
}
