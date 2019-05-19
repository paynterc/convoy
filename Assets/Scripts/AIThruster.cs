using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIThruster : Thruster
{
    public override void Rotate()
    {
        RotateWithLook();
    }
}
