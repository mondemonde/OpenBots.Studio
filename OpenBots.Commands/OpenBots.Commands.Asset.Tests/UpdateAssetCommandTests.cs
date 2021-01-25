﻿using Newtonsoft.Json.Linq;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Data;
using System.IO;
using Xunit;

namespace OpenBots.Commands.Asset.Tests
{
    public class UpdateAssetCommandTests
    {
        private AutomationEngineInstance _engine;
        private UpdateAssetCommand _updateAsset;
        private GetAssetCommand _getAsset;

        [Fact]
        public void UpdatesTextAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testUpdateTextAsset";
            string newAsset = "newText";
            assetName.StoreInUserVariable(_engine, "{assetName}");
            newAsset.StoreInUserVariable(_engine, "{newAsset}");

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = "Text";
            _updateAsset.v_AssetFilePath = "";
            _updateAsset.v_AssetValue = "{newAsset}";

            _updateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "Text";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            string outputAsset = "{output}".ConvertUserVariableToString(_engine);
            Assert.Equal("newText", outputAsset);

            resetAsset(assetName, "oldText", "Text");
        }

        [Fact]
        public void UpdatesNumberAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testNumberAsset";
            string newAsset = "70";
            assetName.StoreInUserVariable(_engine, "{assetName}");
            newAsset.StoreInUserVariable(_engine, "{newAsset}");

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = "Number";
            _updateAsset.v_AssetFilePath = "";
            _updateAsset.v_AssetValue = "{newAsset}";

            _updateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "Number";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            string outputAsset = "{output}".ConvertUserVariableToString(_engine);
            Assert.Equal("70", outputAsset);

            resetAsset(assetName, "42", "Number");
        }

        [Fact]
        public void UpdatesJSONAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "testJSONAsset";
            string newAsset = "{ \"text\": \"newText\" }";
            assetName.StoreInUserVariable(_engine, "{assetName}");
            newAsset.StoreInUserVariable(_engine, "{newAsset}");

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = "JSON";
            _updateAsset.v_AssetFilePath = "";
            _updateAsset.v_AssetValue = "{newAsset}";

            _updateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "JSON";
            _getAsset.v_OutputUserVariableName = "{output}";

            _getAsset.RunCommand(_engine);

            JObject outputAsset = JObject.Parse("{output}".ConvertUserVariableToString(_engine));
            Assert.Equal("newText", outputAsset["text"]);

            resetAsset(assetName, "{ \"text\": \"testText\" }", "JSON");
        }

        [Fact]
        public void UpdatesFileAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();
            _getAsset = new GetAssetCommand();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filepath = Path.Combine(projectDirectory, @"Resources\");
            string assetName = "testUpdateFileAsset";
            string newAsset = filepath + @"Upload\newtest.txt";
            assetName.StoreInUserVariable(_engine, "{assetName}");
            newAsset.StoreInUserVariable(_engine, "{newAsset}");

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = "File";
            _updateAsset.v_AssetFilePath = newAsset;

            _updateAsset.RunCommand(_engine);

            _getAsset.v_AssetName = "{assetName}";
            _getAsset.v_AssetType = "File";
            _getAsset.v_OutputDirectoryPath = filepath + @"Download\";

            _getAsset.RunCommand(_engine);

            string outputAsset = "{output}".ConvertUserVariableToString(_engine);
            Assert.True(File.Exists(filepath + @"Download\newtest.txt"));

            File.Delete(filepath+@"Download\newtest.txt");
            resetAsset(assetName, filepath + @"Upload\oldtest.txt", "File");
        }

        [Fact]
        public void HandlesNonexistentAsset()
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();
            _getAsset = new GetAssetCommand();

            string assetName = "noAsset";
            string newAsset = "newText";
            assetName.StoreInUserVariable(_engine, "{assetName}");
            newAsset.StoreInUserVariable(_engine, "{newAsset}");

            _updateAsset.v_AssetName = "{assetName}";
            _updateAsset.v_AssetType = "Text";
            _updateAsset.v_AssetFilePath = "";
            _updateAsset.v_AssetValue = "{newAsset}";

            Assert.Throws<DataException>(() => _updateAsset.RunCommand(_engine));
        }

        private void resetAsset(string assetName, string assetVal, string type)
        {
            _engine = new AutomationEngineInstance(null);
            _updateAsset = new UpdateAssetCommand();

            _updateAsset.v_AssetName = assetName;
            _updateAsset.v_AssetType = type;
            _updateAsset.v_AssetValue = assetVal;

            if(type == "File")
            {
                _updateAsset.v_AssetFilePath = assetVal;
            }

            _updateAsset.RunCommand(_engine);
        }
    }
}
