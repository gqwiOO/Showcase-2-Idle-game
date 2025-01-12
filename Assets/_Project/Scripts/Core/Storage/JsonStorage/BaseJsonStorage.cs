using System;
using System.IO;
using Core.Scripts.Debugging;
using UnityEngine;

namespace Core.Storage.JsonStorage
{
    public abstract class BaseJsonStorage<TData> : IJsonStorage<TData>
    {
        public abstract string Path { get; }

        protected TData _data;

        public virtual void Load()
        {
            string fullPath = GetFullPath();

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                
                _data = Newtonsoft.Json.JsonConvert.DeserializeObject<TData>(json);
            }
            else
            {
                Debugging.Log(this,
                    $"File not found at path: {fullPath}. Creating a new instance of {typeof(TData).Name}.");
                _data = Activator.CreateInstance<TData>();
            }
        }

        public TData Get()
        {
            if (_data == null)
                Load();

            return _data;
        }

        public void Update(TData data) => _data = data;

        public void Save()
        {
            if (_data == null)
            {
                Debugging.Log(this,$"Cannot save because data is null for {typeof(TData).Name}.");
                return;
            }

            string fullPath = GetFullPath();
            string directory = System.IO.Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonUtility.ToJson(_data, true);
            File.WriteAllText(fullPath, json);
            Debugging.Log(this,$"Data saved successfully at {fullPath}.");
        }

        protected virtual string GetFullPath() 
            => System.IO.Path.Combine(Application.persistentDataPath, Path + ".json");
    }

    public abstract class BaseJsonResourcesStorage<TData> : BaseJsonStorage<TData>
    {
        public override void Load()
        {
            string json = Resources.Load<TextAsset>(GetFullPath())?.text;

            if (!string.IsNullOrEmpty(json))
            {
                _data = Newtonsoft.Json.JsonConvert.DeserializeObject<TData>(json);
            }
            else
            {
                Debugging.Log(this,
                    $"File not found in Resources at path: {Path}. Creating a new instance of {typeof(TData).Name}.");
                _data = Activator.CreateInstance<TData>();
            }
        }
        
        protected override string GetFullPath() 
            => Path;
    }
}