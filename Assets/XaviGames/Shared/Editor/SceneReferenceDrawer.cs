using UnityEditor;
using UnityEngine;

namespace XaviGames.Shared
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty sceneAssetProperty = property.FindPropertyRelative("_sceneAsset");
            SerializedProperty sceneNameProperty = property.FindPropertyRelative("<SceneName>k__BackingField");

            Rect sceneAssetRect = new Rect(
                position.x, position.y, position.width, position.height);

            EditorGUI.PropertyField(sceneAssetRect, sceneAssetProperty, label);

            if (sceneAssetProperty.objectReferenceValue != null)
            {
                SceneAsset sceneAsset = sceneAssetProperty.objectReferenceValue as SceneAsset;
                sceneNameProperty.stringValue = sceneAsset != null ? sceneAsset.name : string.Empty;
            }
            else
            {
                sceneNameProperty.stringValue = string.Empty;
            }

            EditorGUI.EndProperty();
        }
    }
}