using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerReference
{
    public static PlayerController PlayerController
    {
        get => player;
        set => player ??= value;  // set { if(player==null) player = value;}  то же самое

    }
    private static PlayerController player;
}
