using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPrefabInSceneWindow : EditorWindow {

    private GameObject prefabToSpawn;
    private int numberToSpawn;
    private Transform bottomLeftBound, topRightBound;

    [MenuItem ("Window/MyWindows/SpawnPrefabInScene")]
    public static void ShowWindow () {
        GetWindow (typeof (SpawnPrefabInSceneWindow));
    }

    void OnGUI () {
        EditorGUIUtility.labelWidth = 400f;
        prefabToSpawn = EditorGUILayout.ObjectField ("Link prefab to spawn" , prefabToSpawn , typeof (GameObject) , true) as GameObject;
        bottomLeftBound = EditorGUILayout.ObjectField ("Link bottom donw bound" , bottomLeftBound , typeof (Transform) , true) as Transform;
        topRightBound = EditorGUILayout.ObjectField ("Link top right bound" , topRightBound , typeof (Transform) , true) as Transform;
        numberToSpawn = EditorGUILayout.IntField (numberToSpawn);
        if (prefabToSpawn != null && numberToSpawn > 0 && topRightBound != null && bottomLeftBound != null) {
            if (GUILayout.Button ("Spawn ")) {
                Spawn ();
            }
        }
        if (prefabToSpawn != null) {
            if (GUILayout.Button ("Destroy reference")) {
                DestroyReferences ();
            }
        }
    }

    private void Spawn () {
        GameObject gameObjectSpawned;
        Vector3 tempPosition;
        for (int i = 0; i < numberToSpawn; i++) {
            gameObjectSpawned = PrefabUtility.InstantiatePrefab (prefabToSpawn , SceneManager.GetActiveScene ()) as GameObject;
            tempPosition = new Vector3 (Random.Range (bottomLeftBound.position.x , topRightBound.position.y) , Random.Range (bottomLeftBound.position.y , topRightBound.position.y) , Random.Range (bottomLeftBound.position.z , topRightBound.position.y));
            gameObjectSpawned.transform.position = tempPosition;
        }

    }

    private void DestroyReferences () {
        List<GameObject> prefabReferences = new List<GameObject> ();
        GameObject[] allSceneGameobjec = (GameObject[]) FindObjectsOfType (typeof (GameObject));
        foreach (GameObject go in allSceneGameobjec) {
            if (EditorUtility.GetPrefabType (go) == PrefabType.PrefabInstance) {
                Object go_prefab = EditorUtility.GetPrefabParent (go);
                if (go_prefab == prefabToSpawn) {
                    prefabReferences.Add (go);
                }
            }
        }
        foreach (GameObject go in prefabReferences) {
            DestroyImmediate (go);
        }
        AssetDatabase.SaveAssets ();
    }

}
