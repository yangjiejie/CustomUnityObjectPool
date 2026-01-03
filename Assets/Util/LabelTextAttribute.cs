// LabelTextAttribute.cs
using System;
using UnityEngine;
[System.Diagnostics.Conditional("UNITY_EDITOR")] // 仅在UNITY_EDITOR开发下生效，其他情况打包该接口和没写一样 
[AttributeUsage(AttributeTargets.Field)]
public class LabelTextAttribute : PropertyAttribute
{
    public readonly string Chinese;
    public LabelTextAttribute(string chinese) => Chinese = chinese;
}