using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autopilot : MonoBehaviour
{
    public TextAsset starDataAsset;
    public string[] starsOfInterest = new string[] { "Alnitak", "Alnilam", "Mintaka", "Sol" };
    public bool autoPilot = false;

    private string closestStarName = "";
    private Vector3 closestStarPosition = Vector3.zero;

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
            var starData = starDataAsset.text;
            var closestStarDistance = 99999999999999f;
            var rows = starData.Split('\n');
            for (var i = 1; i < rows.Length - 1; i++)
            {
                var row = rows[i];
                var cols = row.Split(',');
                var x = -float.Parse(cols[(int)StarData.x]);
                var y = float.Parse(cols[(int)StarData.y]);
                var z = float.Parse(cols[(int)StarData.z]);
                var mag = float.Parse(cols[(int)StarData.mag]);
                var starName = cols[(int)StarData.proper];

                Vector3 starPosition = new Vector3(x, y, z); //Camera.main.transform.position;
                float starDistanceFromCamera = Vector3.Distance(starPosition, Camera.main.transform.position);

                if (mag < 40 && starName != "" && starDistanceFromCamera > 0.1f)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    
                    float distance = Vector3.Cross(ray.direction, starPosition - ray.origin).magnitude;
                    float angularDistance = distance / Mathf.Max(starDistanceFromCamera, 0.1f); // relative distance... no particular units

                    if (angularDistance < closestStarDistance)
                    {
                        closestStarName = starName;
                        closestStarPosition = starPosition;
                        closestStarDistance = angularDistance;
                    }
                }
            }


        }
    }

    private void OnGUI()
    {
        if (autoPilot)
        {
            GUI.color = Color.green;
        } 
        if (GUILayout.Button("Autopilot"))
        {
            autoPilot = !autoPilot;
        }
        GUI.color = Color.white;

        GUILayout.Label(closestStarName);
        if (!autoPilot)
        {
            var screenPosition = Camera.main.WorldToScreenPoint(closestStarPosition);
            GUI.Label(new Rect(screenPosition.x - 7, Screen.height - screenPosition.y - 7, 20, 20), "O");
        }
    }
}
