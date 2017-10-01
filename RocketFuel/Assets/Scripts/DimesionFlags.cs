using System;
using UnityEngine;

[Serializable]
public class DimesionFlags
{
    public bool X = true;
    public bool Y = true;
    public bool Z = true;

    public Vector3 AdjustAToB(Vector3 _a, Vector3 _b)
    {
        Vector3 result = _a;

        if(X)
        {
            result.x = _b.x;
        }

        if (Y)
        {
            result.y = _b.y;
        }

        if (Z)
        {
            result.z = _b.z;
        }

        return result;
    }
	
}
