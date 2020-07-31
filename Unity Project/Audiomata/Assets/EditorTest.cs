using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EditorTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AssetDatabase.LoadAssetAtPath<GameObject>("Really bad asset path name");
    }
}
