using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelBuilder : MonoBehaviour
{
    public GameObject _panel;
    public GameObject _grate;

    public float _grateToPanelRatio = 0.4f;

    [Space(12)]

    public int _width = 10;
    public int _height = 10;

    public void BuildLevel ()
    {
        if (_panel == null || _grate == null) return;

        Vector3 spawn = Vector3.zero;

        float startX = (_width * 10f) * -0.5f + 5f;

        spawn.x = startX;

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                GameObject prefab = Random.Range(0f, 1f) < _grateToPanelRatio ? _grate : _panel;

                GameObject g = (GameObject)Instantiate(prefab, this.transform, false);
                g.transform.localPosition = spawn;
                spawn.x += 10f;

                g.GetComponent<AssetRandomizer>().RandomizeAsset();
                g.GetComponent<AssetRandomizer>().RandomizeMaterials();
            }

            spawn.x = startX;
            spawn.y += 10f;

        }
    }

    public void ClearLevel()
    {
        int children = transform.childCount;
        for (int i = children-1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}


#if UNITY_EDITOR


[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelBuilder myTarget = (LevelBuilder)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Build Level"))
        {
            myTarget.BuildLevel();
        }

        if (GUILayout.Button("Clear Level"))
        {
            myTarget.ClearLevel();
        }
    }
}

#endif