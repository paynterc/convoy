using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThruster : Thruster
{

    public override void Rotate()
    {
        RotateWithLook();
    }

}
