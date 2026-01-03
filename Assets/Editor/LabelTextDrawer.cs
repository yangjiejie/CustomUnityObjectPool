// LabelTextDrawer.cs
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LabelTextAttribute))]
public class LabelTextDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
    {
        var att = (LabelTextAttribute)attribute;
        // 把默认 label 换成中文
        label.text = att.Chinese;
        EditorGUI.PropertyField(rect, prop, label, true);
    }
}
#endif