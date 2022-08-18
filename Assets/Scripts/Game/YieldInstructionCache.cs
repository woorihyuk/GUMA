using System.Collections.Generic;
using UnityEngine;

internal static class YieldInstructionCache
{
    private class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x.Equals(y);
        }

        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new();

    private static readonly Dictionary<float, WaitForSeconds> TimeInterval = new(new FloatComparer());
    private static readonly Dictionary<float, WaitForSecondsRealtime> TimeIntervalRealtime = new(new FloatComparer());


    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!TimeInterval.TryGetValue(seconds, out var wfs))
            TimeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
    
    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        if (!TimeIntervalRealtime.TryGetValue(seconds, out var wfs))
            TimeIntervalRealtime.Add(seconds, wfs = new WaitForSecondsRealtime(seconds));
        return wfs;
    }
}