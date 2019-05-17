using UnityEngine;
using System.Collections;

public class XChildClass : XParentClass
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SomeFunction()
    {
        base.SomeFunction(); // Calls Item.SomeFunction
        Debug.Log("XChildClass.SomeFunction");
    }
}
