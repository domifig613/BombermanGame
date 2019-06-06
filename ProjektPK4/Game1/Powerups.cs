using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjektPK4.game
{
    class Powerups: GameObject
    {
        int Indestructible;//cant destroy and pick frame
        int TextureNumber = 0;
        int FrameWithOneTexture { get; }
        int CountTexture=0;
        int Type { get; }//1 more bombs, 2 large area bomb, 3 speed, 4 indestructible for some frame

        public Powerups(int _indestructibleTime,int _type, int _frameWithOneTexture, int posX, int posY):base(posX, posY)
        {
            FrameWithOneTexture = _frameWithOneTexture;
            Indestructible = _indestructibleTime;
            Type = _type;
        }
        
        public void SetNextTexture(int max)
        {
            if(CountTexture % FrameWithOneTexture == 0)
            {
                CountTexture = 1;
                if (TextureNumber == max - 1)
                {
                    TextureNumber = 0;
                }
                else
                {
                    TextureNumber++;
                }
            }
            else
            {
                CountTexture++;
            }
        }

        public int GetTypePowerups()
        {
            return Type;
        }

        public int GetNumberTexture()
        {
            return TextureNumber;
        }

        public int GetIndestructible()
        {
            return Indestructible;
        }
        public void IndestructibleDecrement()
        {
            if (Indestructible > 0)
            {
                Indestructible--;
            }
        }
        public void AddPower(Character player)
        {
            switch (Type)
            {
                case 1:
                    player.AddOneBomb();
                    break;
                case 2:
                    player.AddBombPower();
                    break;
                case 3:
                    player.AddSpeed();
                    break;
                case 4:
                    player.SetIndestuctible(300);
                    break;
            }
        }
    }
}
