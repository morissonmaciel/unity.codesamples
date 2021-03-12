using UnityEngine;
using UnityEditor;

public class GameFrameworkUnityEditor : Editor
{
    [MenuItem("GameObject/Game Framework/Player Character", false, 0)]
    public static void CreatePlayer()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Game Framework/Characters/Player Character.prefab");

        if (prefab == null) return;

        GameObject instance = Instantiate(prefab);
        instance.name = prefab.name;

        instance.transform.position = GetSceneViewCameraLookAtPoint();
        instance.transform.parent = Selection.activeTransform;
        Selection.activeGameObject = instance;
    }

    public static Vector3 GetSceneViewCameraLookAtPoint()
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
}
