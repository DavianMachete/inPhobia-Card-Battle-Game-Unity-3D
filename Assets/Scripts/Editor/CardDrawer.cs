using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Card))]
public class CardDrawer : Editor
{
    private Card T;

    private void OnEnable()
    {
        T = target as Card;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        foreach (AffectHolder ah in T.affects)
        {
            switch (ah.affectType)
            {
                case AffectType.AddActionPoints:
                    ah.affect = Affects.AddActionPoints(ah.firstValue, ah.secondValue);
                    //Debug.Log("dasda");
                    break;
                case AffectType.AddBlock:
                    ah.affect = Affects.AddBlock(ah.firstValue);
                    break;
                case AffectType.AddHealth:
                    ah.affect = Affects.AddHealth(ah.firstValue);
                    break;
                case AffectType.AddPoison:
                    ah.affect = Affects.AddPoison(ah.firstValue);
                    break;
                case AffectType.AddPower:
                    ah.affect = Affects.AddPower(ah.firstValue);
                    break;
                case AffectType.AddSpikes:
                    ah.affect = Affects.AddSpikes(ah.firstValue);
                    break;
                case AffectType.AddWeaknessOnDefense:
                    ah.affect = Affects.AddWeaknessOnDamage(ah.firstValue);
                    break;
                case AffectType.Armor:
                    ah.affect = Affects.Armor(ah.firstValue);
                    break;
                case AffectType.Attack:
                    ah.affect = Affects.Attack(ah.firstValue, Mathf.FloorToInt(ah.secondValue));
                    break;
                case AffectType.AttackOnDefense:
                    ah.affect = Affects.AttackOnDefense(ah.firstValue);
                    break;
                case AffectType.BlockTheDamage:
                    ah.affect = Affects.BlockTheDamage();
                    break;
                case AffectType.Discard:
                    ah.affect = Affects.Discard();
                    break;
                case AffectType.DiscardAndAddBlockForEach:
                    ah.affect = Affects.DiscardAndAddBlockForEach(Mathf.FloorToInt(ah.firstValue));
                    break;
                case AffectType.DoubleNextAffect:
                    ah.affect = Affects.DoubleNextAffect();
                    break;
                case AffectType.DoubleBlock:
                    ah.affect = Affects.DoubleTheBlock();
                    break;
                case AffectType.DropKickWithoutAttack:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                case AffectType.Exhaust:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                case AffectType.GiveEnemyWeaknessOnHit:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                case AffectType.MultiplyBlock:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                case AffectType.Power:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                case AffectType.PullCard:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                case AffectType.SaveBlock:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                case AffectType.SteelBlock:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                case AffectType.TurnWeaknessIntoPoison:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                case AffectType.Vulnerability:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                case AffectType.Weakness:
                    ah.affect = Affects.DropKickWithouAttack();
                    break;
                default:
                    break;
            }
        }
        if (EditorUtility.IsDirty(T))
        {

            EditorUtility.SetDirty(T);
            EditorSceneManager.MarkAllScenesDirty();
        }
    }
}
