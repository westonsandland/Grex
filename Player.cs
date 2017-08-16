using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Grex
{
    public class Player
    {
        const int PLAYER_SIZE = 40;
        public Texture2D texture1;
        public Texture2D texture2;
        public string name;
        public Rectangle primaryRect;
        public Rectangle secondaryRect;
        public Rectangle passiveRect;
        public int playerX;
        public int playerY;
        public string passive;
        public string primary;
        public string secondary;
        public string tertiary;
        public string special;
        public int hitpoints;
        public int MAX_HITPOINTS;
        public int invulnerability;
        public Player(int screenWidth, int screenHeight)
        {
            invulnerability = 0;
            texture1 = null;
            texture2 = null;
            name = "Bob";
            playerX = (screenWidth - PLAYER_SIZE) / 2;
            playerY = (screenHeight - PLAYER_SIZE) / 2;
            primaryRect = new Rectangle(playerX, playerY, PLAYER_SIZE, PLAYER_SIZE);
            secondaryRect = new Rectangle(playerX+PLAYER_SIZE*1/8, playerY+PLAYER_SIZE*1/8, PLAYER_SIZE*3/4, PLAYER_SIZE*3/4);
            passiveRect = new Rectangle(playerX + (PLAYER_SIZE / 2) - 2, playerY + (PLAYER_SIZE / 2) - 2, 4, 4);
            passive = "None";
            hitpoints = 100;
            MAX_HITPOINTS = 100;
        }

        public void increasePlayerX()
        {
            primaryRect.X += 1 * (invulnerability/20) +1;
            secondaryRect.X += 1 * (invulnerability/20)+1;
            passiveRect.X += 1 * (invulnerability/20)+1;
        }
        public void decreasePlayerX()
        {
            primaryRect.X -= 1 * (invulnerability/20)+1;
            secondaryRect.X -= 1 * (invulnerability/20)+1;
            passiveRect.X -= 1 * (invulnerability/20)+1;
        }

        public void increasePlayerY()
        {
            primaryRect.Y += 1 * (invulnerability/20)+1;
            secondaryRect.Y += 1 * (invulnerability/20)+1;
            passiveRect.Y += 1 * (invulnerability/20)+1;
        }
        public void decreasePlayerY()
        {
            primaryRect.Y -= 1 * (invulnerability/20)+1;
            secondaryRect.Y -= 1 * (invulnerability/20)+1;
            passiveRect.Y -= 1 * (invulnerability/20)+1;
        }

        public void setPlayerX(int u)
        {
            primaryRect.X += u;
            secondaryRect.X += u;
            passiveRect.X += u;
        }
        public void setPlayerY(int u)
        {
            primaryRect.Y += u;
            secondaryRect.Y += u;
            passiveRect.Y += u;
        }

    }
}