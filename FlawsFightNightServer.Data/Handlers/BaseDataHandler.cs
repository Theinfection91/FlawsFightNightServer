using Newtonsoft.Json;
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
        private readonly SemaphoreSlim _semaphore = new(1, 1);

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
                var data = new T();
                var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                File.WriteAllText(_filePath, json);
            }
        }

        public async Task<T> LoadAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                string json = await File.ReadAllTextAsync(_filePath);
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
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task SaveAsync(T data)
        {
            await _semaphore.WaitAsync();
            try
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                await File.WriteAllTextAsync(_filePath, json);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

}
