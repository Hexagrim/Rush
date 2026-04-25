using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class CopyRuleTileEditor : EditorWindow
{
    RuleTile source;
    SharedRuleTile target;

    [MenuItem("Tools/Copy RuleTile Rules")]
    public static void ShowWindow()
    {
        GetWindow<CopyRuleTileEditor>("Copy RuleTile Rules");
    }

    void OnGUI()
    {
        source = (RuleTile)EditorGUILayout.ObjectField("Source (Old RuleTile)", source, typeof(RuleTile), false);
        target = (SharedRuleTile)EditorGUILayout.ObjectField("Target (SharedRuleTile)", target, typeof(SharedRuleTile), false);

        if (GUILayout.Button("Copy Rules"))
        {
            if (source == null || target == null)
            {
                Debug.LogError("Assign both tiles first!");
                return;
            }

            Undo.RecordObject(target, "Copy RuleTile Rules");

            target.m_DefaultSprite = source.m_DefaultSprite;
            target.m_DefaultGameObject = source.m_DefaultGameObject;
            target.m_DefaultColliderType = source.m_DefaultColliderType;
            target.m_TilingRules = new System.Collections.Generic.List<RuleTile.TilingRule>(source.m_TilingRules);

            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();

            Debug.Log("Rules copied successfully!");
        }
    }
}