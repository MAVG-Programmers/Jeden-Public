using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden
{
    public class InGameState : GameState
    {
        //public Level ActiveLevel { get; set; }

        public override void LoadContent(JedenGame game)
        {
            PlayerCharacter playerCharacter = new PlayerCharacter(game);
            GameObjects.Add(playerCharacter);
            base.LoadContent(game);
        }
        public override void Update(JedenGame game)
        {

        }
        public override void Draw(JedenGame game)
        {

        }
        public override void UnloadContent(JedenGame game)
        {

        }
    }
}
