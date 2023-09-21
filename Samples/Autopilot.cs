using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autopilot : MonoBehaviour
{
    public bool groundMode = false;

    public bool autoPilot = false;

    private string closestStarName = "";
    private string searchText = "";
    private Vector3 closestStarPosition = Vector3.zero;

    // Used to smoothly rotate towards the a star after search.
    private Quaternion startRotation = Quaternion.identity;
    private Quaternion targetRotation = Quaternion.identity;
    private float rotationTime = 5f;

    private string azalt = "";

    public StarRenderer starRendererScript;

    void Update()
    {
        // Fly us to selected star if autopilot enabled
        if (autoPilot && closestStarName != "")
        {
            var travelPath = closestStarPosition - Camera.main.transform.position;
            var travelVector = travelPath.normalized;
            var travelDistance = Mathf.Min(Time.deltaTime / 3.26156f, travelPath.magnitude); // 1 lightyear per second
            Camera.main.transform.position += (travelVector * travelDistance);
        }

        // Select a star by right clicking it
        if (Input.GetMouseButtonDown(1))
        {
            var starData = starRendererScript.starDataAsset.text;
            var closestStarDistance = 99999999999999f;
            var rows = starData.Split('\n');
            for (var i = 1; i < rows.Length - 1; i++)
            {
                var row = rows[i];
                var cols = row.Split(',');
                var x = float.Parse(cols[(int)StarData.x]);
                var y = float.Parse(cols[(int)StarData.z]);
                var z = float.Parse(cols[(int)StarData.y]);
                var mag = float.Parse(cols[(int)StarData.mag]);
                var starName = cols[(int)StarData.proper];
                var hip = cols[(int)StarData.hip];

                Vector3 starPosition = starRendererScript.GetSkyQuaternion() * new Vector3(x, y, z); //Camera.main.transform.position;
                float starDistanceFromCamera = Vector3.Distance(starPosition, Camera.main.transform.position);

                if (mag < 20 && starName != "" && starDistanceFromCamera > 1f)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    
                    float distance = Vector3.Cross(ray.direction, starPosition - starRendererScript.viewerPosition).magnitude;
                    float angularDistance = distance / Mathf.Max(starDistanceFromCamera, 0.1f); // relative distance... no particular units

                    if (angularDistance < closestStarDistance)
                    {
                        closestStarName = starName + " (HIP " + hip + ")";
                        closestStarPosition = starPosition;
                        closestStarDistance = angularDistance;
                    }
                }
            }
        }

        if (rotationTime < 5) {
            if (Time.deltaTime < 0.2f) rotationTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotationTime / 5f);
        }
    }

    private void SearchStar() {
        var starData = starRendererScript.starDataAsset.text;
        var rows = starData.Split('\n');
        for (var i = 1; i < rows.Length - 1; i++)
        {
            var row = rows[i];
            var cols = row.Split(',');
            var x = float.Parse(cols[(int)StarData.x]);
            var y = float.Parse(cols[(int)StarData.z]);
            var z = float.Parse(cols[(int)StarData.y]);
            var mag = float.Parse(cols[(int)StarData.mag]);
            var starName = cols[(int)StarData.proper];
            var hip = cols[(int)StarData.hip];

            Vector3 starPosition = starRendererScript.GetSkyQuaternion() * new Vector3(x, y, z);

            if (starName == searchText || hip == searchText) {
                closestStarName = starName + " (HIP " + hip + ")";
                closestStarPosition = starPosition;

                startRotation = transform.rotation;
                targetRotation = Quaternion.LookRotation(starPosition);
                rotationTime = 0;

                var azAltVec = starRendererScript.GetAzAlt(new Vector3(x, y, z));
                azalt = azAltVec.x.ToString("0.0") + " / " + azAltVec.y.ToString("0.0");
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        if (!groundMode) {
            if (autoPilot)
            {
                GUI.color = Color.green;
            } 
            if (GUILayout.Button("Autopilot"))
            {
                autoPilot = !autoPilot;
            }
            GUI.color = Color.white;
        }

        searchText = GUILayout.TextField(searchText, GUILayout.Width(200));
        if (GUILayout.Button("Search")) {
            SearchStar();
        }

        GUILayout.EndHorizontal();

        if (groundMode) {
            GUILayout.Label("September 20 15:00GMT");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Latitude: " + starRendererScript.longLat.y.ToString("0.000"));
            starRendererScript.longLat.y = GUILayout.HorizontalSlider(starRendererScript.longLat.y, 90, -90, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Longitude: " + starRendererScript.longLat.x.ToString("0.000"));
            starRendererScript.longLat.x = GUILayout.HorizontalSlider(starRendererScript.longLat.x, -180f, 180f, GUILayout.Width(200));
            GUILayout.EndHorizontal();
        }

        GUILayout.Label(closestStarName);
        if (!autoPilot)
        {
            var saveCam = Camera.main.transform.position;
            Camera.main.transform.position = starRendererScript.viewerPosition;
            var screenPosition = Camera.main.WorldToScreenPoint(closestStarPosition);
            Camera.main.transform.position = saveCam;

            if (screenPosition.z > 0)
            {
                GUI.Label(new Rect(screenPosition.x - 7, Screen.height - screenPosition.y - 7, 20, 20), "O");
            }
        }

        if (!groundMode) {
            if (closestStarName == "")
            {
                GUILayout.Label("Right click to select a star");
                GUILayout.Label("Click Autopilot to fly to it");
                GUILayout.Label("Search stars using their HIP number or proper name");
            } 
        } else {
            if (closestStarName == "")
            {
                GUILayout.Label("Search stars using their HIP number or proper name");
            } 
        }

        GUILayout.Label("Az / Alt: " + azalt);
    }
}
