using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden
{
    public class GameState
    {
        public List<GameObject> GameObjects { get; set; }

        public GameState(JedenGame game) 
        {
            GameObjects = new List<GameObject>();
        }

        public virtual void LoadContent(JedenGame game) 
        {
            foreach (GameObject gameObject in GameObjects) 
            {
                gameObject.LoadContent(game);
            }
        }
        public virtual void Update(JedenGame game)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update(game);
            }
        }
        public virtual void Draw(JedenGame game)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Draw(game);
            }
        }
        public virtual void UnloadContent(JedenGame game)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.UnloadContent(game);
            }
        }
    }
}
