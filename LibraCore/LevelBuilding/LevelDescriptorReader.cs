using System;
using System.IO;

namespace LibraCore.LevelBuilding
{
    public class LevelDescriptorReader
    {
        public LevelDescriptorReader(string levelFileName)
        {
            if (string.IsNullOrEmpty(levelFileName)) throw new ArgumentNullException(nameof(levelFileName));
            this.levelFileName = levelFileName;
        }

        public LevelDescriptor Load()
        {
            var levelFileContent = File.ReadAllText(levelFileName);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<LevelDescriptor>(levelFileContent);
        }
        
        private readonly string levelFileName;
    }
}
