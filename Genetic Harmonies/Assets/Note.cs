using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float startTime;
    public float endTime;
    public Key key;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        if (key != null)
        {
            name = key.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
