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
        SerializedProperty firstValueProperty = property.FindPropertyRelative("firstValue");
        float firstValue = firstValueProperty.floatValue;
        SerializedProperty secondValueProperty = property.FindPropertyRelative("secondValue");
        float secondValue = secondValueProperty.floatValue;

        SerializedProperty affect = property.FindPropertyRelative("affect");

        switch (affectType)
        {
            case AffectType.AddActionPoints:
                {
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Action Points");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    EditorGUI.LabelField(GetSecondValueNameRect(rect), "Maximum AP");
                    EditorGUI.PropertyField(GetSecondValueFieldRect(rect),secondValueProperty, GUIContent.none);
                    //DrawProperty(rect, "Maximum Action Points",secondValue);

                    affect.objectReferenceValue = Affects.AddActionPoints(firstValue, secondValue);
                    //affect.objectReferenceValue = (object)Affects.AddActionPoints(firstValue, secondValue);
                }
                break;
            case AffectType.AddBlock:
                {
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Block Amount");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    affect.objectReferenceValue = Affects.AddBlock(firstValue);
                }
                break;
            case AffectType.Armor:
                {
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Armor Amount");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    affect.objectReferenceValue = Affects.Armor(firstValue);
                }
                break;
            case AffectType.Attack:
                {
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Attack force");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    int ac = Mathf.FloorToInt(firstValue);
                    firstValueProperty.floatValue = ac;
                    EditorGUI.LabelField(GetSecondValueNameRect(rect), "Attack count");
                    EditorGUI.PropertyField(GetSecondValueFieldRect(rect), secondValueProperty, GUIContent.none);
                    //DrawProperty(rect, "Maximum Action Points",secondValue);

                    affect.objectReferenceValue = Affects.Attack(firstValue, ac);

                }
                break;
            case AffectType.AttackOnDefense:
                {
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Attack force");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    affect.objectReferenceValue = Affects.AttackOnDefense(firstValue);
                }
                break;
            case AffectType.Discard:
                affect.objectReferenceValue = Affects.Discard();
                break;
            case AffectType.DoubleNextAffect:
                affect.objectReferenceValue = Affects.DoubleNextAffect();
                break;
            case AffectType.DropKickWithoutAttack:
                affect.objectReferenceValue = Affects.DropKickWithouAttack();
                break;
            case AffectType.Exhaust:
                affect.objectReferenceValue = Affects.Exhaust();
                break;
            case AffectType.GiveEnemyWeaknessOnHit:
                affect.objectReferenceValue = Affects.GiveEnemyWeaknessOnHit();
                break;
            case AffectType.MultiplyBlock:
                {
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Block multiplier");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    affect.objectReferenceValue = Affects.MultiplyBlock(firstValue);
                }
                break;
            case AffectType.Power:
                {
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Damage");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    affect.objectReferenceValue = Affects.MultiplyBlock(firstValue);
                }
                break;
            case AffectType.PullCard:
                {
                    int cc = Mathf.FloorToInt(firstValue);
                    firstValueProperty.floatValue = cc;
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Count");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    affect.objectReferenceValue = Affects.PullCard(cc);
                }
                break;
            case AffectType.SaveBlock:
                affect.objectReferenceValue = Affects.SaveBlock();
                break;
            case AffectType.SteelBlock:
                {
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Damage");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    affect.objectReferenceValue = Affects.SteelBlock(firstValue);
                }
                break;
            case AffectType.Vulnerability:
                {
                    int vc = Mathf.FloorToInt(firstValue);
                    firstValueProperty.floatValue = vc;
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Count");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    affect.objectReferenceValue = Affects.Vulnerablity(vc);
                }
                break;
            case AffectType.Weakness:
                {
                    int wc = Mathf.FloorToInt(firstValue);
                    firstValueProperty.floatValue = wc;
                    EditorGUI.LabelField(GetFirstValueNameRect(rect), "Count");
                    EditorGUI.PropertyField(GetFirstValueFieldRect(rect), firstValueProperty, GUIContent.none);

                    affect.objectReferenceValue = Affects.Weakness(wc);
                }
                break;
            default:
                break;
        }   
    }

    public Rect GetFirstValueNameRect(Rect position)
    {
        return new Rect(position.x,
                                    position.y,
                                    position.width * 0.3f - 5,
                                    position.height);
    }
    public Rect GetFirstValueFieldRect(Rect position)
    {
        return new Rect(position.x + position.width * 0.3f - 5,
                                     position.y,
                                     position.width * 0.1f,
                                     position.height);
    }
    public Rect GetSecondValueNameRect(Rect position)
    {
        return new Rect(position.x + position.width * 0.5f + 5,
                                position.y,
                                position.width * 0.3f - 5,
                                position.height);
    }
    public Rect GetSecondValueFieldRect(Rect position)
    {
        return new Rect(position.x + position.width * 0.8f + 5,
                                position.y,
                                position.width * 0.1f - 5,
                                position.height);
    }
}