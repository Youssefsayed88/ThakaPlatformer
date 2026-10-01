using System.IO;
using NUnit.Framework;
using Thaka.Platformer.Persistence;

namespace Thaka.Platformer.Tests
{
    public class JsonFileSaveServiceTests
    {
        string folder;
        string savePath;
        JsonFileSaveService service;

        [SetUp]
        public void SetUp()
        {
            folder = Path.Combine(Path.GetTempPath(), "ThakaPlatformerTests");
            savePath = Path.Combine(folder, "save.json");
            service = new JsonFileSaveService(savePath);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);
        }

        [Test]
        public void SaveThenLoad_ReturnsSameData()
        {
            var data = new SaveData { hp = 2, coins = 5 };
            data.collectedCoinIds.Add("coin");

            service.Save(data);
            service.TryLoad(out var loaded);

            Assert.AreEqual(2, loaded.hp);
            Assert.AreEqual(5, loaded.coins);
            Assert.AreEqual("coin", loaded.collectedCoinIds[0]);
        }

        [Test]
        public void Delete_RemovesSave()
        {
            service.Save(new SaveData());

            service.Delete();

            Assert.IsFalse(service.HasSave);
        }

        [Test]
        public void CorruptFile_IsNotTreatedAsSave()
        {
            Directory.CreateDirectory(folder);
            File.WriteAllText(savePath, "not json");

            Assert.IsFalse(service.HasSave);
        }
    }
}
