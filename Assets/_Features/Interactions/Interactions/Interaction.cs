using UnityEngine;

namespace Spread.Interactions
{
    public class Interaction : ScriptableObject
    {
        public string Name;
        public Sprite InputIcon;

        [UnityEditor.MenuItem("Assets/Spread/Interactions/Interactable")]
        private static void CreateDatabase()
        {
            string directory = "Assets/";

            if (UnityEditor.Selection.objects.Length > 0)
            {
                directory = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.objects[0]);
            }

            Interaction instance = CreateInstance<Interaction>();
            instance.name = "Interaction";
            UnityEditor.AssetDatabase.CreateAsset(instance, $"{directory}/{instance.name}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }

    public enum InteractionType
    {
        Use
    }
}