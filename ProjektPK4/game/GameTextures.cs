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
    static class GameTextures
    {
        static private List<Texture2D> Textures = new List<Texture2D>();
        static private string folderTexture = "Sprites\\";


        static GameTextures() {}

        static public void AddTexture(Texture2D texture)
        {
            if(texture.Name == null)
            {
                texture.Name = "Empty";
            }

            Textures.Add(texture);
        }

        static public Texture2D GetTexture(string textureName)
        {
            Texture2D texture1 = null;
            if (textureName != "Empty")
            {
                textureName = folderTexture + textureName;
            }

            foreach (Texture2D texture in Textures)
            {
                if(texture.Name == textureName)
                {
                    texture1 = texture;
                    return texture1;
                }
            }
            return texture1;
        }
    }
}
