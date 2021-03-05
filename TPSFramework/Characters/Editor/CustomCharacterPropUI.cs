using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TPS_Player))]
public class CustomCharacterPropUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //Texture2D iconInput = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ThirdPersonGameFramework/Icons/CharacterAnimModel.png");
        //var playerContent = new GUIContent("Player", iconInput);

        //EditorGUILayout.Space();

        //if (GUILayout.Button(playerContent, GUIStyle.none))
        //{

        //}
    }

    private static Vector3 getSceneViewCameraLookAtPoint()
    {
        Transform camTrans = SceneView.lastActiveSceneView.camera.transform;
        Ray ray = new Ray(camTrans.position, camTrans.TransformDirection(Vector3.forward));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    [MenuItem("GameObject/Third Person Game/Characters/Player", false, 0)]
    public static void CreatePlayer()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TPSFramework/Characters/Player.prefab");

        if (prefab == null) return;

        GameObject instance = Instantiate(prefab);
        instance.name = prefab.name;

        instance.transform.position = getSceneViewCameraLookAtPoint();
        instance.transform.parent = Selection.activeTransform;
        Selection.activeGameObject = instance;

        //if (SceneView.currentDrawingSceneView != null)
        //{
        //    instance.transform.parent = Selection.activeTransform;

        //    Transform camTrans = SceneView.currentDrawingSceneView.camera.transform;
        //    Ray ray = new Ray(camTrans.position, camTrans.TransformDirection(Vector3.forward));
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        instance.transform.position = hit.point;
        //    }
        //    else
        //    {
        //        instance.transform.position = Vector3.zero;
        //    }

        //    Selection.activeGameObject = instance;
        //}
    }
}
