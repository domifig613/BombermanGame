using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektPK4.game
{
    class Button
    {
        readonly private int Type;//0-exit, 1-next, 2-previous
        Rectangle Body;
        Color ColorOnMove = Color.LightSkyBlue;
        Texture2D Texture;
        bool OnMove = false;
        SoundEffect OnMoveSoundEffect;

        public Button(int type, int posX, int posY, int width, int height)
        {
            Type = type;
            Body = new Rectangle(posX, posY, width, height);
        }

        public void LoadSoundButton(SoundEffect _onMove)
        {
            OnMoveSoundEffect = _onMove;
        }

        public void DrawButton(SpriteBatch Batch)
        {
            if (OnMove == false)
            {
                Batch.Draw(Texture, Body, Color.White);
            }
            else
            {
                Batch.Draw(Texture, Body, ColorOnMove);
            }
        }

        public int OnClick(int state)
        {
            if(Type==1)
            {
                if(state == 3)
                {
                    return 1;
                }
                return ++state;
            }
            else if(Type==2)
            {
                return --state;
            }
            else if(Type == 0)
            { 
                return -1;
            }
            return 0;
        }

        public void SetTexture(Texture2D _texture)
        {
            Texture = _texture;
        }

        public int GetButtonType()
        {
            return Type;
        }

        public bool CheckMoveInButtonPositionX(int posX)
        {
            if(posX >= Body.X && posX<= Body.Width + Body.X)
            {
                return true;
            }
            return false;
        }
        
        public bool CheckMoveInButtonPositionY(int posY)
        {
            if(posY >= Body.Y && posY <= Body.Height + Body.Y)
            {
                return true;
            }
            return false;
        }

        public void SetOnMoveState(bool state)
        {
            OnMove = state;
        }
        public bool GetOnMoveState()
        {
            return OnMove;
        }

        public void PlaySoundEffect()
        {
            if (ProgramParameters.Get_MusicEnable())
            {
                OnMoveSoundEffect.Play(0.5f, 0f, 0f);
            }
        }

        public int CheckMoveInButtonPosition(int posX, int posY, int gameState)
        {
            if(CheckMoveInButtonPositionX(posX) && CheckMoveInButtonPositionY(posY))
            {
                gameState=OnClick(gameState);
            }
            return gameState;
        }
    }
}
