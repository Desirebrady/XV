using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ItemManager))]
public class ItemManagerEditor : Editor
{
    private ItemManager itemManager;


    private ReorderableList reorderedInputList;
    private ReorderableList reorderedOutputList;

    private void OnEnable()
    {
        itemManager = (ItemManager)target;

        #region Refined
        reorderedInputList = new ReorderableList(serializedObject, serializedObject.FindProperty("itemRecipe.ingredients"), true, true, true, true);
        reorderedOutputList = new ReorderableList(serializedObject, serializedObject.FindProperty("itemRecipe.output"), true, true, true, true);
        
        reorderedInputList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Input");
        };

        reorderedInputList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            Ingredient output = itemManager.itemRecipe.ingredients[index];

            EditorGUILayout.BeginHorizontal();
            output.item = EditorGUI.ObjectField(new Rect(rect.x, rect.y + 2f, 100, EditorGUIUtility.singleLineHeight), output.item, typeof(Item), false) as Item;
            output.itemAmt = EditorGUI.IntField(new Rect(rect.x + 110, rect.y + 2f, 30, EditorGUIUtility.singleLineHeight), output.itemAmt);
            output.maxStorage = EditorGUI.IntField(new Rect(rect.x + 220, rect.y + 2f, 30, EditorGUIUtility.singleLineHeight), output.maxStorage);
            EditorGUILayout.EndHorizontal();
        };



        reorderedOutputList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Output");
        };
        
        reorderedOutputList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            Ingredient ingredient = itemManager.itemRecipe.output[index];

            EditorGUILayout.BeginHorizontal();
            ingredient.item = EditorGUI.ObjectField(new Rect(rect.x, rect.y + 2f, 100, EditorGUIUtility.singleLineHeight), ingredient.item, typeof(Item), false) as Item;
            ingredient.itemAmt = EditorGUI.IntField(new Rect(rect.x + 110, rect.y + 2f, 30, EditorGUIUtility.singleLineHeight), ingredient.itemAmt);
            ingredient.maxStorage = EditorGUI.IntField(new Rect(rect.x + 220, rect.y + 2f, 30, EditorGUIUtility.singleLineHeight), ingredient.maxStorage);
            EditorGUILayout.EndHorizontal();
        };
        #endregion
    }

    public override void OnInspectorGUI()
    {
        Draw();
    }

    private void Draw()
    {
        itemManager.price = EditorGUILayout.FloatField("Price:", itemManager.price);
        itemManager.ManagerType = (ItemManager.ItemManagerType)EditorGUILayout.EnumPopup("Manager Type Rules:", itemManager.ManagerType);
        itemManager.maxStorage = EditorGUILayout.IntField("Max Storage:", itemManager.maxStorage);
        itemManager.requiresWorkers = EditorGUILayout.Toggle("RequiresWorkers:", itemManager.requiresWorkers);
        if (itemManager.requiresWorkers)
        {
            itemManager.baseSpawnTimer = EditorGUILayout.FloatField("Base Spawn Time:", itemManager.baseSpawnTimer);
            itemManager.personWorkDecrease = EditorGUILayout.FloatField("Worker Spawn Decrease:", itemManager.personWorkDecrease);
            itemManager.maxWorkers = EditorGUILayout.IntField("Max Workers:", itemManager.maxWorkers);
        }
        EditorGUILayout.Space();
        itemManager.spawnTimer = EditorGUILayout.FloatField("Spawn Timer:", itemManager.spawnTimer);
        itemManager.currentSpawnTimer = EditorGUILayout.FloatField("Current Spawn Time:", itemManager.currentSpawnTimer);

        EditorGUILayout.Space();
        serializedObject.Update();

        if (itemManager.ManagerType == ItemManager.ItemManagerType.nothing)
            return;
        else if (itemManager.ManagerType == ItemManager.ItemManagerType.creates)
        {
            reorderedOutputList.DoLayoutList();
        }
        else if (itemManager.ManagerType == ItemManager.ItemManagerType.refines)
        {
            reorderedInputList.DoLayoutList();
            reorderedOutputList.DoLayoutList();
        }
        else if (itemManager.ManagerType == ItemManager.ItemManagerType.destroys)
        {
            reorderedInputList.DoLayoutList();
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    
}


