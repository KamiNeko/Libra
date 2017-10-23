using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGameTests.Levels
{
    internal class LevelDescriptor
    {
        public LevelDescriptor()
        {
            StartPosition = new Vector2();
            EntityDescriptors = new List<EntityDescriptor>();
        }

        public Vector2 StartPosition { get; set; }
        public List<EntityDescriptor> EntityDescriptors { get; set; }
    }
}
