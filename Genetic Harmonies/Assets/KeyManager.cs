using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;
    public Key C;
    public Key D;
    public Key E;
    public Key F;
    public Key G;
    public Key A;
    public Key B;

    public List<Key> keys;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        keys.Add(C);
        keys.Add(D);
        keys.Add(E);
        keys.Add(F);
        keys.Add(G);
        keys.Add(A);
        keys.Add(B);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            C.PressedDown();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            D.PressedDown();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            E.PressedDown();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            F.PressedDown();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            G.PressedDown();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            A.PressedDown();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            B.PressedDown();
        }




        if (Input.GetKeyUp(KeyCode.A))
        {
            C.PressedUp();
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            D.PressedUp();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            E.PressedUp();
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            F.PressedUp();
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            G.PressedUp();
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            A.PressedUp();
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            B.PressedUp();
        }


    }
}
