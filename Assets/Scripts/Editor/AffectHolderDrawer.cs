using UnityEditor;
using UnityEngine;

// IngredientDrawer
[CustomPropertyDrawer(typeof(AffectHolder))]
public class AffectHolderDrawer : PropertyDrawer
{
    

    public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing*2;
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(rect, label, property);
        //rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        EditorGUI.DrawRect(rect, new Color(0.318897f, 0.322834f, 0.078740f));

        //Draw Affect Type Enum

        rect.height = (rect.height - EditorGUIUtility.standardVerticalSpacing) / 2f;
        AffectType affectType;
        SerializedProperty affectTypeProp = property.FindPropertyRelative("affectType");
        EditorGUI.PropertyField(rect, affectTypeProp, GUIContent.none);

        affectType = (AffectType)affectTypeProp.enumValueIndex;

        //Draw Affect parmetres
        rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
        DrawAffectParametres(affectType, rect, property);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    private void DrawAffectTypeProparty(Rect rect, SerializedProperty property,out AffectType affectType)
    {
        SerializedProperty affectTypeProp = property.FindPropertyRelative("affectType");
        EditorGUI.PropertyField(rect, affectTypeProp, GUIContent.none);

        affectType = (AffectType)affectTypeProp.enumValueIndex;
    }

    private void DrawProperty(Rect rect,string name,float value)
    {
        EditorGUI.FloatField(rect, new GUIContent(name), value);
    }

    private void DrawAffectParametres(AffectType affectType, Rect rect, SerializedProperty property)
    {
        rect.width = rect.width / 4f;

        SerializedProperty firstValueProperty = property.FindPropertyRelative("firstValue");
        float firstValue = firstValueProperty.floatValue;
        SerializedProperty secondValueProperty = property.FindPropertyRelative("secondValue");
        float secondValue = secondValueProperty.floatValue;

        SerializedProperty affect = property.FindPropertyRelative("affect");

        switch (affectType)
        {
            case AffectType.AddActionPoints:
                {
                    DrawProperty(rect, "Action Points",firstValue);
                    rect.x += rect.width*2f;
                    DrawProperty(rect, "Maximum Action Points",secondValue);

                    //affect.objectReferenceValue = (object)Affects.AddActionPoints(firstValue, secondValue);
                }
                break;
            case AffectType.AddBlock:
                break;
            case AffectType.Armor:
                break;
            case AffectType.Attack:
                break;
            case AffectType.AttackOnDefense:
                break;
            case AffectType.Discard:
                break;
            case AffectType.DoubleNextAffect:
                break;
            case AffectType.DropKickWithoutAttack:
                break;
            case AffectType.Exhaust:
                break;
            case AffectType.GiveEnemyWeaknessOnHit:
                break;
            case AffectType.MultiplyBlock:
                break;
            case AffectType.Power:
                break;
            case AffectType.PullCard:
                break;
            case AffectType.SaveBlock:
                break;
            case AffectType.SteelBlock:
                break;
            case AffectType.Vulnerability:
                break;
            case AffectType.Weakness:
                break;
            default:
                break;
        }   
    }
}
