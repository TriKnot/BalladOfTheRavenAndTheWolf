using System.Collections.Generic;
using Enemy.Spawner;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        var so = serializedObject;

        root.TrackSerializedObjectValue(so, ClampHealthPercentage);
        InspectorElement.FillDefaultInspector(root, serializedObject, this);
        return root;
    }

    
    private void ClampHealthPercentage(SerializedObject so)
    {
        var changeMade = false;
        
        var value = 0f;
        SerializedProperty sp = so.GetIterator();
        while (sp.NextVisible(true))
        {
            if (sp.name != "MaxHealthPercentage") continue;
            if(value > sp.floatValue)
            {
                sp.floatValue = value;
                changeMade = true;
            }            
            value = sp.floatValue;
        }
        
        if(changeMade)
            so.ApplyModifiedProperties();
        


    }
}
