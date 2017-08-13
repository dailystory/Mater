using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mater.Library;
using System.IO;
using System.Runtime.Caching;

namespace Mater.Test
{
    [TestClass]
    public class SettingsTest
    {
        [TestMethod]
        public void Settings__GetSettings()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string pathWithParent = currentDirectory + @"\..\..\..\WebApp\articles\settings.json";
            string fullPath = Path.GetFullPath(pathWithParent);

            Settings s = Settings.GetSettings(fullPath);

            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void Settings__GetSettings__Missing__File()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            // path is purposefully incorrect
            string pathWithParent = currentDirectory + @"\..\..\..\WebApp\settings.json";
            string fullPath = Path.GetFullPath(pathWithParent);

            Assert.ThrowsException<FileNotFoundException>(() =>
                Settings.GetSettings(fullPath)
            );

        }

        [TestMethod]
        public void Settings__GetSettings__Valid__Cache()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string pathWithParent = currentDirectory + @"\..\..\..\WebApp\articles\settings.json";
            string fullPath = Path.GetFullPath(pathWithParent);

            Settings s = Settings.GetSettings(fullPath);
            Assert.IsNotNull(s);

            // validate in cache
            bool cached = MemoryCache.Default.Contains(Settings.CACHE_KEY__SETTINGS);

            Assert.IsTrue(cached);

        }

    }
}
