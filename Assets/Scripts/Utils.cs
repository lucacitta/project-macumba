using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void CompensateParentScale(Transform parentTransform, GameObject child)
    {
        Vector3 parentScale = parentTransform.lossyScale;
        child.transform.localScale = new(
        1f / parentScale.x,
        1f / parentScale.y,
        1f / parentScale.z
    );
    }
}