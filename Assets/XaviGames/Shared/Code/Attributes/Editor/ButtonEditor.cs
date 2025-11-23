using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace XaviGames.Attributes
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MonoBehaviour targetMono = (MonoBehaviour)target;
            MethodInfo[] methods = targetMono.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
                if (buttonAttribute != null)
                {
                    bool shouldShow = buttonAttribute.ShowInPlayModeOnly && !Application.isPlaying;

                    if (shouldShow)
                    {
                        return;
                    }

                    string buttonName = string.IsNullOrEmpty(buttonAttribute.ButtonName) 
                        ? method.Name : buttonAttribute.ButtonName;

                    if (GUILayout.Button(buttonName))
                    {
                        method.Invoke(targetMono, null);
                    }
                }
            }
        }
    }
}
