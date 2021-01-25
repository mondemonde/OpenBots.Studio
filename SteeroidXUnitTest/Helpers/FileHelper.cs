using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Steeroid.Business.Services;
using Steeroid.Models.Enums;
using Xunit;

namespace SteeroidXUnitTest.Helpers
{
   public static class FileTestHelper
    {
        public static void ClearInputFiles()
        {
            //clear input files
            var eventFiles = Directory.GetFiles(FileEndPointManager.MyEventDirectory
               , "*" + EnumFiles.WFInput.ToString(), SearchOption.TopDirectoryOnly)
               .ToList().OrderBy(x => x);

            foreach (var item in eventFiles)
            {
                File.Delete(item);
            }

            eventFiles = Directory.GetFiles(FileEndPointManager.MyEventDirectory
              , "*" + EnumFiles.WFInput.ToString(), SearchOption.TopDirectoryOnly)
              .ToList().OrderBy(x => x);

            Assert.True(eventFiles.Count() == 0, "input file json folder is not cleared.");//clear input files


        }

    }
}
