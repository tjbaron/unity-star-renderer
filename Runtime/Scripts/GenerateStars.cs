using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StarData {
    id,hip,hd,hr,gl,bf,proper,ra,dec,
    dist,pmra,pmdec,rv,mag,absmag,spect,ci,
    x,y,z,vx,vy,vz,rarad,decrad,
    pmrarad,pmdecrad,bayer,flam,con,comp,comp_primary,
    _base,lum,var,var_min,var_max
}

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GenerateStars : MonoBehaviour
{
    // This expects the hygdata_v3.csv star data file
    // https://github.com/astronexus/HYG-Database
    public TextAsset starDataAsset;
    public Material starMaterial;
    // Viewer position in parsecs relative to earth
    public Vector3 viewerPosition;
    public bool viewerToCameraPosition = true;

    // Default to new years on the north pole
    public Vector2 longLat = new Vector2(0f, 90f);
    public int dayOfYear = 0;
    public float gmtTime = 0f;

    public Vector2 GetSkyRotation() {
        transform.rotation = Quaternion.identity;
        float longitude = longLat.x;
        float latitude = longLat.y;

        // transform.Rotate(0, (longitude+55) + ((gmtTime-15)*360/24) + (dayOfYear*360/365), 0, Space.World);
        // transform.Rotate(90-latitude, 0, 0, Space.World);
        var degRotations = new Vector2(
            (longitude+55) + ((gmtTime-15)*360/24) + (dayOfYear*360/365),
            90-latitude
        );
        return degRotations * Mathf.Deg2Rad;
    }

    // From perspective of earth surface
    // Z axis is North
    // X axis is East
    public Quaternion GetSkyQuaternion() {
        Vector2 lonLat = GetSkyRotation();
        var go = new GameObject();
        go.transform.Rotate(0, lonLat.x*Mathf.Rad2Deg, 0, Space.World);
        go.transform.Rotate(lonLat.y*Mathf.Rad2Deg, 0, 0, Space.World);
        var rot = go.transform.rotation;
        Destroy(go);
        return rot;
    }

    //public string[] highlightedStars = new string[]{"Alnitak", "Alnilam", "Mintaka"};

    void Start()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        // Get all the stars from hyg vdatabase
        var starData = starDataAsset.text;
        var rows = starData.Split('\n');
        var starCount = rows.Length-2;
        var chunkSize = 5000;

        
        Mesh mesh = new Mesh();
        MeshFilter meshFilter = new MeshFilter();
        Vector3[] vertices = new Vector3[chunkSize*4];
        int[] tris = new int[chunkSize*6];
        Vector3[] normals = new Vector3[chunkSize*4];
        Vector2[] uv = new Vector2[chunkSize*4];
        Vector2[] uv2 = new Vector2[chunkSize*4];
        GameObject gameObject;
        for (var i=0; i<starCount; i++) {
            // Create a new mesh for each group of `chunkSize` stars
            if (i % chunkSize == 0) {
                if (i != 0) {
                    mesh.vertices = vertices;
                    mesh.triangles = tris;
                    mesh.normals = normals;
                    mesh.uv = uv;
                    mesh.uv2 = uv2;
                    meshFilter.mesh = mesh;
                }
                mesh = new Mesh();
                vertices = new Vector3[chunkSize*4];
                tris = new int[chunkSize*6];
                normals = new Vector3[chunkSize*4];
                uv = new Vector2[chunkSize*4];
                uv2 = new Vector2[chunkSize*4];
                gameObject = new GameObject();
                gameObject.name = "StarBatch";
                gameObject.transform.parent = this.gameObject.transform;
                meshFilter = gameObject.AddComponent<MeshFilter>();
                var meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.material = starMaterial;
            }
            var j = i % chunkSize;

            // Create a quad for each star
            var cols = rows[i+1].Split(',');
            var x = float.Parse( cols[(int)StarData.x] );
            var y = float.Parse( cols[(int)StarData.z] );
            var z = float.Parse( cols[(int)StarData.y] );
            var absmag = float.Parse( cols[(int)StarData.absmag] );
            var colorindex = 0f;
            try
            {
                colorindex = float.Parse(cols[(int)StarData.ci]); // <0 is blue, >0 is red
            } catch { }

            vertices[j*4] = new Vector3(x, y, z);
            vertices[j*4 + 1] = new Vector3(x, y, z);
            vertices[j*4 + 2] = new Vector3(x, y, z);
            vertices[j*4 + 3] = new Vector3(x, y, z);

            tris[j*6] = j*4;
            tris[j*6 + 1] = j*4 + 2;
            tris[j*6 + 2] = j*4 + 1;
            tris[j*6 + 3] = j*4 + 2;
            tris[j*6 + 4] = j*4 + 3;
            tris[j*6 + 5] = j*4 + 1;

            normals[j*4] = -Vector3.forward;
            normals[j*4 + 1] = -Vector3.forward;
            normals[j*4 + 2] = -Vector3.forward;
            normals[j*4 + 3] = -Vector3.forward;

            //var highlighted = 0f;
            //for (var m=0; m<highlightedStars.Length; m++) {
            //    if (cols[(int)StarData.proper] == highlightedStars[m]) {
            //        highlighted = 1f;
            //        Debug.Log("Found " + highlightedStars[m]);
            //        break;
            //    }
            //}

            uv[j*4] = new Vector2(0f, 0f);
            uv[j*4 + 1] = new Vector2(1f, 0f);
            uv[j*4 + 2] = new Vector2(0f, 1f);
            uv[j*4 + 3] = new Vector2(1f, 1f);
            uv2[j*4] = uv2[j*4 + 1] = uv2[j*4 + 2] = uv2[j*4 + 3] = new Vector2(absmag, colorindex);
        }
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.uv2 = uv2;
        meshFilter.mesh = mesh;
    }

    void Update() {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        Vector2 skyRotation = GetSkyRotation();

        if (viewerToCameraPosition)
        {
            viewerPosition = Camera.main.transform.position;
        }
        starMaterial.SetVector("ViewerPosition", viewerPosition);
        starMaterial.SetVector("CameraUp", Quaternion.Inverse(transform.rotation)*Camera.main.transform.up);
        starMaterial.SetVector("CameraRight", Quaternion.Inverse(transform.rotation)*Camera.main.transform.right);
        starMaterial.SetVector("_StarRotation", skyRotation);
    }

}
