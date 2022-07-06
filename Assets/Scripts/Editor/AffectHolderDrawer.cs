using UnityEditor;
using UnityEngine;

// IngredientDrawer
[CustomPropertyDrawer(typeof(AffectHolder))]
public class AffectHolderDrawer : PropertyDrawer
{
    private const float space = 5;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + space;
    }

    public override void OnGUI(Rect rect,
                               SerializedProperty property,
                               GUIContent label)
    {
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        DrawProperties(rect, property);

        EditorGUI.indentLevel = indent;
    }

    private void DrawProperties(Rect rect,
                                    SerializedProperty affectholder)
    {
        SerializedProperty affectTypeProp = affectholder.FindPropertyRelative("affectType");

        DrawProperty(rect, affectTypeProp);

        rect.y += rect.height + EditorGUIUtility.singleLineHeight;

        rect.width = (rect.width - space) / 2f;
        DrawProperty(rect, affectholder.FindPropertyRelative("firstValue"));
        rect.x += rect.width + space;
        DrawProperty(rect, affectholder.FindPropertyRelative("secondValue"));
    }

    private void DrawProperty(Rect rect,
                              SerializedProperty property)
    {
        EditorGUI.PropertyField(rect, property, GUIContent.none);
    }
}
