using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ProjektPK4.game
{
    class Button
    {
        Rectangle Body;
        String TextureName;

        Button(int posX, int posY, int width, int height, String textureName)
        {
            Body = new Rectangle(posX, posY, width, height);
            TextureName = textureName;
        }

        public void onClick()
        {

        }
        
        public void DrawButton(SpriteBatch batch)
        {
            
        }

    }
}
