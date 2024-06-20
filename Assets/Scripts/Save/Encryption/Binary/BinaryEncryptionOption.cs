using Saves.Serializiation;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Saves
{
    public class BinaryEncryptionOption : IEncryptionOption
    {
        private string savePath;

        public BinaryEncryptionOption(string savePath)
        {
            this.savePath = savePath;
        }

        public void Delete()
        {
            if (!FileExist()) return;

            File.Delete(savePath);
        }

        public bool FileExist()
        {
            return File.Exists(savePath);
        }

        public SerializableDictionary<string, SerializableDictionary<string, string>> Load()
        {
            if (!FileExist()) return default;

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(savePath, FileMode.Open);

            SerializableDictionary<string, SerializableDictionary<string, string>> data = (SerializableDictionary<string, SerializableDictionary<string, string>>)formatter.Deserialize(stream);

            stream.Close();

            SaveManager.ProcessLoadedData(data);

            return data;
        }

        public void Save(object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(savePath, FileMode.Create);

            formatter.Serialize(stream, data);

            stream.Close();
        }
    }
}