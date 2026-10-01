using System;
using System.IO;
using UnityEngine;

namespace Thaka.Platformer.Persistence
{
    public sealed class JsonFileSaveService : ISaveService
    {
        const string DefaultFileName = "save.json";

        readonly string path;

        public JsonFileSaveService()
            : this(Path.Combine(Application.persistentDataPath, DefaultFileName))
        {
        }

        public JsonFileSaveService(string path)
        {
            this.path = path;
        }

        public string FilePath => path;

        public bool HasSave => TryLoad(out _);

        public bool Save(SaveData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var tempPath = path + ".tmp";
            try
            {
                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);

                // write to temp first so a crash can't corrupt the existing save
                File.WriteAllText(tempPath, JsonUtility.ToJson(data, true));
                if (File.Exists(path))
                    File.Replace(tempPath, path, null);
                else
                    File.Move(tempPath, path);

                return true;
            }
            catch (Exception e) when (IsFileError(e))
            {
                Debug.LogError($"Failed to save game to '{path}': {e.Message}");
                TryDeleteFile(tempPath);
                return false;
            }
        }

        public bool TryLoad(out SaveData data)
        {
            data = null;
            if (!File.Exists(path))
                return false;

            try
            {
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
            }
            catch (Exception e) when (IsFileError(e) || e is ArgumentException)
            {
                Debug.Log($"Ignoring unreadable save file at '{path}': {e.Message}");
                return false;
            }

            if (data == null || data.version != SaveData.CurrentVersion)
            {
                data = null;
                return false;
            }

            return true;
        }

        public void Delete()
        {
            TryDeleteFile(path);
            TryDeleteFile(path + ".tmp");
        }

        static void TryDeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (Exception e) when (IsFileError(e))
            {
                Debug.LogError($"Failed to delete '{filePath}': {e.Message}");
            }
        }

        static bool IsFileError(Exception e) => e is IOException || e is UnauthorizedAccessException;
    }
}
