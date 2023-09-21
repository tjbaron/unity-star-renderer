using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(StarRenderer))]
public class StarRendererEditor : Editor
{

    private VisualElement viewerPosition;

    public override VisualElement CreateInspectorGUI()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.baroncreations.star-renderer/Editor/StarRendererEditor.uxml");
        var uiDoc = visualTree.CloneTree();

        viewerPosition = uiDoc.Q<Vector3Field>("viewerPosition");
        uiDoc.Q<Toggle>("viewerToggle").RegisterValueChangedCallback((evt) => {
            viewerPosition.style.display = !evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
        });

        var cityName = uiDoc.Q<TextField>("cityName");
        var citySearch = uiDoc.Q<Button>("citySearch");
        var longLat = uiDoc.Q<Vector2Field>("longLat");
        citySearch.clicked += () => {
            var city = GetCity(cityName.value);
            if (city != null)
            {
                cityName.value = city.name + ", " + city.country;
                longLat.value = new Vector2(city.lng, city.lat);
            }
        };

        return uiDoc;
    }

    public City GetCity(string name)
    {
        var json = AssetDatabase.LoadAssetAtPath<TextAsset>("Packages/com.baroncreations.star-renderer/Editor/cities.json");
        var jsonText = "{\"cities\": " + json.text + "}";
        var cities = JsonUtility.FromJson<Cities>(jsonText);
        foreach (var city in cities.cities)
        {
            if ((city.name + ", " + city.country).ToLower() == name.ToLower() || city.name.ToLower() == name.ToLower())
            {
                return city;
            }
        }
        return null;
    }
}

[Serializable]
public class City
{
    public string country;
    public string name;
    public float lat;
    public float lng;
}

[Serializable]
public class Cities
{
    public City[] cities;
}
