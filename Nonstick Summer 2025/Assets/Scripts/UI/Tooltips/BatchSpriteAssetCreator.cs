/*
 * Credit: https://gist.github.com/kadaxo/80d3e8994575dd703164e465e7a5fb4e
 */

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class BatchSpriteAssetCreator
{
    [MenuItem("Assets/Create/TextMeshPro/Batch Sprite Asset", false, 80)]
    public static void CreateBatchSpriteAssets()
    {
        var textures = Selection.GetFiltered<Texture2D>(SelectionMode.Assets);
        if (textures.Length == 0)
        {
            EditorUtility.DisplayDialog("Batch Sprite Asset", "Select one or more Sprite-mode textures first.", "OK");
            return;
        }

        // List to store created Sprite Assets
        List<TMP_SpriteAsset> createdSpriteAssets = new List<TMP_SpriteAsset>();

        foreach (var tex in textures)
        {
            // Get the path of the texture
            string texturePath = AssetDatabase.GetAssetPath(tex);
            string textureName = Path.GetFileNameWithoutExtension(texturePath);
            string directory = Path.GetDirectoryName(texturePath);

            // Make this texture the "active" selection
            Selection.activeObject = tex;

            // Capture the assets before and after creation to identify the new Sprite Asset
            var beforeAssets = AssetDatabase.FindAssets("t:TMP_SpriteAsset", new[] { directory });
            TMP_SpriteAssetMenu.CreateSpriteAsset();
            var afterAssets = AssetDatabase.FindAssets("t:TMP_SpriteAsset", new[] { directory });

            // Find the newly created Sprite Asset
            string newAssetGuid = afterAssets.Except(beforeAssets).FirstOrDefault();
            if (!string.IsNullOrEmpty(newAssetGuid))
            {
                string newAssetPath = AssetDatabase.GUIDToAssetPath(newAssetGuid);
                TMP_SpriteAsset spriteAsset = AssetDatabase.LoadAssetAtPath<TMP_SpriteAsset>(newAssetPath);
                if (spriteAsset != null)
                {
                    AssetDatabase.RenameAsset(newAssetPath, textureName);
                    createdSpriteAssets.Add(spriteAsset);
                }
            }
        }

        // If multiple sprite assets were created, assign all others as fallbacks to the first one
        if (createdSpriteAssets.Count > 1)
        {
            TMP_SpriteAsset primarySpriteAsset = createdSpriteAssets[0];
            primarySpriteAsset.fallbackSpriteAssets = createdSpriteAssets.GetRange(1, createdSpriteAssets.Count - 1);
            EditorUtility.SetDirty(primarySpriteAsset);
        }

        // Clean up
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Batch Complete",
            $"Created {createdSpriteAssets.Count} Sprite Assets (one per texture). " +
            (createdSpriteAssets.Count > 1 ? $"Primary Sprite Asset '{createdSpriteAssets[0].name}' has {createdSpriteAssets.Count - 1} fallbacks." : ""),
            "OK");
    }

    [MenuItem("Assets/Create/TextMeshPro/Batch Sprite Asset", true)]
    private static bool ValidateCreateBatchSpriteAssets()
    {
        // Enable menu item only if at least one Texture2D is selected
        return Selection.GetFiltered<Texture2D>(SelectionMode.Assets).Length > 0;
    }
}
#endif