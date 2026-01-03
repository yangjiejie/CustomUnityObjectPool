// 1. 创建按钮特性
using System;
using UnityEngine;

[System.Diagnostics.Conditional("UNITY_EDITOR")] 
[AttributeUsage(AttributeTargets.Method)]
public class ShowInInspector : PropertyAttribute
{
    public string ButtonName { get; }
    public float ButtonHeight { get; }

    public ShowInInspector(string name = "", float height = 20f)
    {
        ButtonName = name;
        ButtonHeight = height;
    }
}