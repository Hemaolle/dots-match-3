﻿using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.U2D.Entities;
using Unity.Jobs;
using Unity.Tiny;

public class ColorSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.
            ForEach((ref SpriteRenderer spriteRenderer, ref ColorComponent colorComponent) =>
            {
                Debug.Log("ColorSystem");
                spriteRenderer.Color = colorComponent.Flag ? Colors.Red : Colors.White;
            }).Run();

        return default;
    }
}
