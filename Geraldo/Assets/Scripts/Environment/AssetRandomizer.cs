using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetRandomizer : MonoBehaviour
{
    public GameObject[] _assets;
    public Material[] _materials;

    public void RandomizeAsset()
    {
        if (_assets.Length == 0) return;

        int index = Random.Range(0, _assets.Length);

        foreach(GameObject g in _assets)
        {
            g.SetActive(false);
        }

        _assets[index].SetActive(true);
    }

    public void RandomizeMaterials ()
    {
        if (_assets.Length == 0 || _materials.Length == 0) return;

        int index = Random.Range(0, _materials.Length);

        foreach (GameObject g in _assets)
        {
            MeshRenderer[] renderers = g.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer mr in renderers)
            {
                mr.material = _materials[index];
            }
        }
    }

    public void RandomizeRotation ()
    {
        foreach (GameObject g in _assets)
        {
            if (g.activeInHierarchy)
            {
                Vector3 localRotation = g.transform.localEulerAngles;

                localRotation.y = Random.Range(0f, 360f);

                g.transform.localEulerAngles = localRotation;
            }
        }
    }
}

#if UNITY_EDITOR


[CustomEditor(typeof(AssetRandomizer))]
public class EnvironmentAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AssetRandomizer myTarget = (AssetRandomizer)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Randomize ALL"))
        {
            myTarget.RandomizeAsset();
            myTarget.RandomizeMaterials();
            myTarget.RandomizeRotation();
        }

        if (GUILayout.Button("Randomize"))
        {
            myTarget.RandomizeAsset();
        }

        if (GUILayout.Button("Randomize Material"))
        {
            myTarget.RandomizeMaterials();
        }

        if (GUILayout.Button("Randomize Y Rotation"))
        {
            myTarget.RandomizeRotation();
        }
    }
}

#endif
