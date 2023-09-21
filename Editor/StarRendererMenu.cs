using UnityEditor;
using UnityEngine;
public class StarRendererMenu : MonoBehaviour
{
    [MenuItem("Packages/Star Renderer/Add to scene")]
    static void AddToScene()
    {
        GameObject go = new GameObject("Star Renderer");
        var gs = go.AddComponent<StarRenderer>();
        gs.starMaterial = AssetDatabase.LoadAssetAtPath<Material>("Packages/com.baroncreations.star-renderer/Runtime/Materials/StarMaterial.mat");
        gs.starDataAsset = AssetDatabase.LoadAssetAtPath<TextAsset>("Packages/com.baroncreations.star-renderer/Runtime/Data/hygdata_v3.csv");
    }
}
