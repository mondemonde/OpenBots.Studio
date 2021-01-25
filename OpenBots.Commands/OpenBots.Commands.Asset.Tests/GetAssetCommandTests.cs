﻿using Newtonsoft.Json.Linq;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Data;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace OpenBots.Commands.Asset.Tests
{
    public class GetAssetCommandTests
    {
        private AutomationEngineInstance _engine;
        private GetAssetCommand _getAsset;
        private readonly ITestOutputHelper output;

        public GetAssetCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GetsTextAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _getAsset = new GetAssetCommand();

            _getAsset.v_AssetName = "testTextAsset";
            _getAsset.v_AssetType = "Text";
            _getAsset.v_OutputDirectoryPath = "";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            Assert.Equal("testText", "{output}".ConvertUserVariableToString(_engine));
        }

        [Fact]
        public void GetsNumberAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _getAsset = new GetAssetCommand();

            _getAsset.v_AssetName = "testNumberAsset";
            _getAsset.v_AssetType = "Number";
            _getAsset.v_OutputDirectoryPath = "";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            var asset = "{output}".ConvertUserVariableToObject(_engine);

            Assert.Equal("42", asset);
        }

        [Fact]
        public void GetsJSONAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _getAsset = new GetAssetCommand();

            _getAsset.v_AssetName = "testJSONAsset";
            _getAsset.v_AssetType = "JSON";
            _getAsset.v_OutputDirectoryPath = "";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            string jsonString = "{output}".ConvertUserVariableToString(_engine);
            JObject jsonObject = JObject.Parse(jsonString);
            Assert.Equal("testText", jsonObject["text"]);
        }

        [Fact]
        public void GetsFileAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _getAsset = new GetAssetCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filepath = Path.Combine(projectDirectory, @"Resources\");

            _getAsset.v_AssetName = "testFileAsset";
            _getAsset.v_AssetType = "File";
            _getAsset.v_OutputDirectoryPath = filepath;
            _getAsset.v_OutputUserVariableName = "";

            _getAsset.RunCommand(_engine);

            Assert.True(File.Exists(filepath + "test.txt"));

            File.Delete(filepath + "test.txt");
        }

        [Fact]
        public void HandlesNonexistentAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _getAsset = new GetAssetCommand();

            _getAsset.v_AssetName = "noAsset";
            _getAsset.v_AssetType = "Text";
            _getAsset.v_OutputDirectoryPath = "";
            _getAsset.v_OutputUserVariableName = "{output}";

            Assert.Throws<DataException>(() => _getAsset.RunCommand(_engine));
        }
    }
}
