using System;
using System.Collections.Generic;
using EF4Unity.Scripts.Core;
using Microsoft.EntityFrameworkCore;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace EF4Unity.Scripts.Editor
{
    public class EditorDbDesign : EditorWindow
    {
        [MenuItem("Window/Editor DB Design Window")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<EditorDbDesign>();
            wnd.titleContent = new GUIContent("Editor DB Design");
        }
        
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            RefreshEditorWindow();
        }
        
        private static void RefreshEditorWindow()
        {
            if (!HasOpenInstances<EditorDbDesign>())
                return;
            var window = GetWindow<EditorDbDesign>();
            window?.Repaint();
        }

        private DbContext CreateDesignDbContextFromSelectedType()
        {
            var factory = Activator.CreateInstance(_selectedType);
            var method = _selectedType.GetMethod("CreateDesignDbContext");
            return method!.Invoke(factory, null) as DbContext;
        }

        private List<Type> LoadDbContextFactoryType()
        {
            return TypeHelper.GetDerivedTypes(typeof(DbContextFactory<>));
        }
        
        private Type _selectedType;
        private List<Type> _dbContextTypes = new();
        public void CreateGUI()
        {
            var service = new DatabaseManagementService();
            _dbContextTypes = LoadDbContextFactoryType();
            
            var options = _dbContextTypes.ConvertAll(type => type.FullName);
            var dropdown = new DropdownField("Select DbContext", options, -1);
            rootVisualElement.Add(dropdown);
            
            rootVisualElement.Add(new VisualElement() { style = { height = 10 } }); 
            
            var label = new Label("Add Migration");
            var textField = new TextField("Enter Name:");
            var button = new Button(() =>
            {
                var dbContext = CreateDesignDbContextFromSelectedType();
                service.AddMigration(dbContext, textField.value);
                Debug.Log($"Created migration for {_selectedType.Name} with name {textField.value}");
                AssetDatabase.Refresh();
            })
            {
                text = "Add Migration"
            };
            rootVisualElement.Add(label);
            rootVisualElement.Add(textField);
            rootVisualElement.Add(button);
            button.SetEnabled(false); 
            
            rootVisualElement.Add(new VisualElement() { style = { height = 10 } });  

            var label2 = new Label("Remove Migration"); 
            var button2 = new Button(() =>
            {
                var dbContext = CreateDesignDbContextFromSelectedType();
                service.RemoveMigration(dbContext);
                Debug.Log($"Removed migration for {_selectedType.Name}");
                AssetDatabase.Refresh();
            })
            {
                text = "Remove Migration"
            };
            button2.SetEnabled(false);
            rootVisualElement.Add(label2);
            rootVisualElement.Add(button2);
            
            rootVisualElement.Add(new VisualElement() { style = { height = 10 } });  
            
            var label3 = new Label("Update the design database with migration"); 
            var button3 = new Button(() =>
            {
                var dbContext = CreateDesignDbContextFromSelectedType();
                service.UpdateDatabase(dbContext);
                Debug.Log("Updated the design database");
                AssetDatabase.Refresh();
            })
            {
                text = "Update the design database"
            };
            button3.SetEnabled(false);
            rootVisualElement.Add(label3);
            rootVisualElement.Add(button3);
            
            textField.RegisterValueChangedCallback(evt =>
            {
                button.SetEnabled(_selectedType != null && !string.IsNullOrEmpty(evt.newValue));
            });
            dropdown.RegisterValueChangedCallback(evt =>
            {
                _selectedType = _dbContextTypes[dropdown.index];
                button.SetEnabled(_selectedType != null && !string.IsNullOrEmpty(textField.value));
                button2.SetEnabled(_selectedType != null);
                button3.SetEnabled(_selectedType != null);
            });
        }
    }
}