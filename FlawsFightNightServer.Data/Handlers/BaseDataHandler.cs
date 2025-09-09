﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Data.Handlers
{
    public abstract class BaseDataHandler<T> where T : new()
    {
        private readonly string _filePath;
        private readonly object _lock = new();

        protected BaseDataHandler(string fileName, string folderName)
    {
        _filePath = SetFilePath(fileName, folderName);
        InitializeFile();
    }

    private string SetFilePath(string fileName, string folderName)
    {
        string appBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(appBaseDirectory, folderName, fileName);

        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Console.WriteLine($"Directory created: {directory}");
        }

        return filePath;
    }

    private void InitializeFile()
    {
        if (!File.Exists(_filePath))
        {
            Save(new T());
        }
    }

    public T Load()
    {
        lock (_lock)
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }) ?? new T();
            }
            catch
            {
                Console.WriteLine($"Failed to read or parse {_filePath}. Reinitializing.");
                var data = new T();
                Save(data);
                return data;
            }
        }
    }

    public async Task<T> LoadAsync()
    {
        try
        {
            string json;
            lock (_lock)
            {
                json = File.ReadAllText(_filePath);
            }

            var data = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            return data ?? new T();
        }
        catch
        {
            Console.WriteLine($"Failed to read or parse {_filePath}. Reinitializing.");
            var data = new T();
            await SaveAsync(data);
            return data;
        }
    }

    public void Save(T data)
    {
        lock (_lock)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            File.WriteAllText(_filePath, json);
        }
    }

    public async Task SaveAsync(T data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        lock (_lock)
        {
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}  

}
