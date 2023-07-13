using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class DebugAttribute : PropertyAttribute
{
    public string DebugName { get; private set; }

    public DebugAttribute(string name = "Debug")
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                            
        DebugName = textInfo.ToTitleCase(name); ;
    }
}