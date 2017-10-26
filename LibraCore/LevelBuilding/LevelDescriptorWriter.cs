using System;
using System.IO;

namespace LibraCore.LevelBuilding
{
    public class LevelDescriptorWriter
    {
        public LevelDescriptorWriter(LevelDescriptor levelDescriptor)
        {
            this.levelDescriptor = levelDescriptor ?? throw new ArgumentNullException(nameof(levelDescriptor));
        }

        public void Save(string levelFileName)
        {
            var serializedLevel = Newtonsoft.Json.JsonConvert.SerializeObject(levelDescriptor, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(levelFileName, serializedLevel);            
        }

        private readonly LevelDescriptor levelDescriptor;
    }
}
