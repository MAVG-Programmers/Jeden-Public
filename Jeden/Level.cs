using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Jeden
{
    [Serializable]
    public class Level
    {
        public List<GameObject> GameObjects { get; set; }
        public List<RectangleCollider> Colliders { get; set; }

        public Level() 
        {
            GameObjects = new List<GameObject>();
        }

        public void LoadContent(JedenGame game)
        {
            foreach (GameObject gameObject in GameObjects) 
            {
                gameObject.LoadContent(game);
            }
        }
        public void Update(JedenGame game)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update(game);
            }
        }
        public void Draw(JedenGame game)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Draw(game);
            }
        }
        public void UnloadContent(JedenGame game)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.UnloadContent(game);
            }
        }

        public static Level GetDebugLevel() 
        {
            return null;
        }
    }
}
