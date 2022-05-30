using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Globally used enums. Always add new enums at the end (before 'COUNT') 
/// and never remove enums (can replace name with unused_... for example).
/// </summary>
namespace Enums
{
    /// <summary>
    /// Playable characters
    /// </summary>
    public enum CharacterType
    {
        None,
        FireSkeleton,


        COUNT
    }

    /// <summary>
    /// Possible dice sides and character element types
    /// </summary>
    public enum ElementalType
    {
        None,
        Heart,
        Soul,
        Mind,
        Weird,

        COUNT
    }
}