using UnityEngine;

// ~~This is a utility class for extending the functionality of being able to directly set the individual values in C#.
// I'm personally using it as it will assist in helping to assign values a lot easier than other round about and very extensive solutions.
// Below I will reference the source of the code and the author.
//~~ Author: adammyhre
//~~ Last Commit: May 5th 2024 
//~~ Accessed: May 4th 2025
//~~ Source: https://github.com/adammyhre/Unity-Behaviour-Trees/blob/master/Assets/_Project/Scripts/Utilities/Vector3Extensions.cs

// See https://github.com/adammyhre/Unity-Utils for more extension methods
public static class Vector3Extensions
{
    /// <summary>
    /// Sets any x y z values of a Vector3
    /// </summary>
    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }
}