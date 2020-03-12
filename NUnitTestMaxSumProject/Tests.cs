using NUnit.Framework;
using MaxSumProject;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private string _path;
        private Stream _pathStream;
        private FileManager _fileManager;

        [SetUp]
        public void Setup()
        {
            _fileManager = new FileManager();
            _path = @"temp.txt";
            File.Create(_path).Dispose();

            string testData = "-2, , 789568546.6, 4,7.9, ,\n4 ,tr, 5.56, 6 \n,,,, ,,, ,,,,,\nstr ttttt\n56666, 586.7858, 0,8 \n2";

            using (var writer = new StreamWriter(_path, false, Encoding.Default))
            {
                writer.WriteLine(testData);
            }
            _pathStream = new MemoryStream(Encoding.UTF8.GetBytes(_path ?? ""));
        }

        [TearDown]
        public void DeleteFile()
        {
            File.Delete(_path);
        }

        [Test]
        public void SetFilePathTest()
        {
            var args_0 = new string[0];
            var args_2 = new string[1] { _path };

            using (var reader = new StreamReader(_pathStream))
            {
                Console.SetIn(reader);
                _fileManager.SetFileData(args_0);
                Assert.IsNotNull(_fileManager.FileData.FilePath);

                Console.SetIn(reader);
                _fileManager.SetFileData(args_2);
                Assert.AreEqual(_path, _fileManager.FileData.FilePath);
            }
            Assert.Pass();
        }

        [Test]
        public void SetFileDataTest()
        {
            using (var reader = new StreamReader(_pathStream))
            {
                var initFileMethod = GetMethod("TryInitFileData");
                _fileManager.FileData.FilePath = _path;
                initFileMethod.Invoke(_fileManager, null);

                CollectionAssert.AreEqual(new List<int> { 1 }, _fileManager.FileData.RowsWithMaxSum, "rows with max sum are not correct");
                CollectionAssert.AreEqual(new List<int> { 2, 3, 4 }, _fileManager.FileData.DefectedRows, "defected rows are not correct");
            }
            Assert.Pass();
        }

        private MethodInfo GetMethod(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
                Assert.Fail("methodName cannot be null or whitespace");

            var method = _fileManager.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
                Assert.Fail(string.Format("{0} method not found", methodName));

            return method;
        }
    }
}