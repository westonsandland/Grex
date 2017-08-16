using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Grex
{
    public class Bullet
    {
        const int SPEED_MULTIPLIER = 100;
        const int LIFESPAN_CONSTANT = 360*SPEED_MULTIPLIER;
        public int spd;
        public Rectangle bulletRect;
        public Rectangle radiusOfEffect;
        public bool penetrable;
        public int dmg;
        public int bulletX;
        public int bulletY;
        private double a, b;
        public double realX;
        public double realY;
        public int abilityType;
        public int objectiveX;
        public int objectiveY;
        public int bulletSize;
        public int lifespan;
        public Color color;
        private bool chaosblaster;
        public string special;
        public int specialNum;
        public int element;
        public bool irregular;
        public int longerDimension;
        public Random r;
        public Bullet(int x, int y, int objX, int objY, Player thePlayer, int abilTyp, int screenWidth, int screenHeight)
        {
            r = new Random();
            irregular = false;
            abilityType = abilTyp;
            if (abilTyp == -1)
            {
                chaosblaster = true;
                spd = 20*SPEED_MULTIPLIER;
                dmg = 35;
                penetrable = true;
                bulletSize = 20;
                lifespan = 180;
                color = Color.Yellow;
            }
            else
                chaosblaster = false;
            switch (abilityType)
            {
                case 0:
                    {
                        if (thePlayer.primary == "Lightning")
                        {
                            spd = 2 * SPEED_MULTIPLIER;
                            dmg = 35;
                            penetrable = true;
                            bulletSize = 20;
                            lifespan = LIFESPAN_CONSTANT / spd;
                            element = 0;
                            special = "none";
                            specialNum = 0;
                        }
                        else if (thePlayer.primary == "Fire")
                        {
                            spd = (int)(0.5 * SPEED_MULTIPLIER);
                            dmg = 100;
                            penetrable = false;
                            bulletSize = 5;
                            lifespan = LIFESPAN_CONSTANT / spd;
                            element = 1;
                            special = "none";
                            specialNum = 0;
                        }
                        else if (thePlayer.primary == "Ice")
                        {
                            spd = (int)(1 * SPEED_MULTIPLIER);
                            dmg = 50;
                            penetrable = false;
                            bulletSize = 5;
                            lifespan = LIFESPAN_CONSTANT / spd;
                            element = 2;
                            special = "slow";
                            specialNum = 2;
                        }
                        else if (thePlayer.primary == "Wind")
                        {
                            spd = (int)(2 * SPEED_MULTIPLIER);
                            dmg = 50;
                            penetrable = false;
                            bulletSize = 7;
                            lifespan = LIFESPAN_CONSTANT / spd;
                            element = 3;
                            special = "push";
                            specialNum = 60;
                        }
                        else if (thePlayer.primary == "Shadow")
                        {
                            spd = (int)(1.5 * SPEED_MULTIPLIER);
                            dmg = 15;
                            penetrable = false;
                            bulletSize = 6;
                            lifespan = LIFESPAN_CONSTANT / spd;
                            element = 4;
                            special = "heal";
                            specialNum = 1;
                        }
                    }
                    break;
                case 1:
                    {
                        if (thePlayer.secondary == "Lightning")
                        {
                            spd = 2 * SPEED_MULTIPLIER;
                            dmg = 50;
                            penetrable = false;
                            bulletSize = 7;
                            lifespan = LIFESPAN_CONSTANT / spd;
                            element = 0;
                            special = "chain";
                            specialNum = 6;
                        }
                        else if (thePlayer.secondary == "Fire")
                        {
                            spd = (int)(1 * SPEED_MULTIPLIER);
                            dmg = 650;
                            penetrable = false;
                            bulletSize = 12;
                            lifespan = LIFESPAN_CONSTANT / spd;
                            element = 1;
                            special = "none";
                            specialNum = 0;
                        }
                        else if (thePlayer.secondary == "Ice")
                        {
                            spd = (int)(1 * SPEED_MULTIPLIER);
                            dmg = 300;
                            penetrable = true;
                            bulletSize = 5;
                            lifespan = LIFESPAN_CONSTANT / spd;
                            element = 2;
                            special = "slow";
                            specialNum = -1;
                        }
                        else if (thePlayer.secondary == "Wind")
                        {
                            spd = (int)(3 * SPEED_MULTIPLIER);
                            dmg = 20;
                            penetrable = true;
                            bulletSize = 10;
                            lifespan = LIFESPAN_CONSTANT / spd;
                            element = 3;
                            special = "push";
                            specialNum = 240;
                            irregular = true;
                            longerDimension = 140;
                        }
                    }
                    break;
                case 2:
                    {
                        if (thePlayer.tertiary == "Lightning")
                        {
                            spd = SPEED_MULTIPLIER / 5;
                            dmg = 100;
                            penetrable = true;
                            bulletSize = 25;
                            lifespan = 600;
                            element = 0;
                            special = "motionrandom";
                            specialNum = 0;
                        }
                        else if (thePlayer.tertiary == "Fire")
                        {
                            spd = 0;
                            dmg = 10;
                            penetrable = true;
                            bulletSize = 12;
                            lifespan = 480;
                            element = 1;
                            special = "none";
                            specialNum = 0;
                            irregular = true;
                            longerDimension = 220;
                        }
                        else if (thePlayer.tertiary == "Ice")
                        {
                            spd = 0;
                            dmg = 10;
                            penetrable = true;
                            bulletSize = 15;
                            lifespan = 600;
                            element = 2;
                            special = "slowbeacon";
                            specialNum = -1;
                        }
                    }
                    break;
                case 3:
                    {
                        if (thePlayer.special == "Lightning")
                        {
                            spd = 1 * SPEED_MULTIPLIER;
                            dmg = 50;
                            penetrable = true;
                            bulletSize = 30;
                            lifespan = LIFESPAN_CONSTANT / spd;
                            element = 5;
                            special = "chain";
                            specialNum = 15;
                        }
                    }
                    break;
            }
            bulletX = x;
            bulletY = y;
            realX = x;
            realY = y;
            objectiveX = objX;
            objectiveY = objY;
            if (irregular) //specifically for non-squares: makes a rectangle instead
            {
                if ((objectiveX - thePlayer.passiveRect.X) / (objectiveY - thePlayer.passiveRect.Y) < screenWidth / screenHeight && (objectiveX - /*screenWidth / 2*/thePlayer.passiveRect.X) / (objectiveY - /*screenHeight / 2*/thePlayer.passiveRect.Y) > -1 * screenWidth / screenHeight)
                {
                    bulletRect = new Rectangle(bulletX, bulletY, longerDimension, bulletSize);
                    objectiveX -= longerDimension / 2;
                    objectiveY -= bulletSize / 2;
                }
                else
                {
                    bulletRect = new Rectangle(bulletX, bulletY, bulletSize, longerDimension);
                    objectiveX -= bulletSize / 2;
                    objectiveY -= longerDimension / 2;
                }
            }
            else
            {
                bulletRect = new Rectangle(bulletX, bulletY, bulletSize, bulletSize);
                if(special == "slowbeacon")
                radiusOfEffect = new Rectangle(bulletRect.X - bulletSize * 10, bulletRect.Y - bulletSize * 10, bulletSize * 21, bulletSize * 21);
                objectiveX -= bulletSize / 2;
                objectiveY -= bulletSize / 2;
            }
            a = Math.Sqrt(spd / (1 + Math.Pow(objectiveY - bulletY, 2) / Math.Pow(objectiveX - bulletX, 2)));
            b = Math.Sqrt(spd / (1 + Math.Pow(objectiveX - bulletX, 2) / Math.Pow(objectiveY - bulletY, 2)));
            if (objectiveY < bulletY)
                b *= -1;
            if (objectiveX < bulletX)
                a *= -1;
        }

        public void Update()
        {
            if (chaosblaster)
            {
                realX += (double)((spd * (objectiveX - bulletRect.X))
                / (Math.Pow(objectiveX - bulletRect.X, 2) + Math.Pow(objectiveY - bulletRect.X, 2)));
                realY += (double)((spd * (objectiveY - bulletRect.Y))
                / (Math.Pow(objectiveX - bulletRect.X, 2) + Math.Pow(objectiveY - bulletRect.Y, 2)));
                bulletRect.X = (int)realX;
                bulletRect.Y = (int)realY;
            }
            else if (special == "motionrandom")
            {
                bulletRect.X += r.Next(-1 * spd, spd + 1);
                bulletRect.Y += r.Next(-1 * spd, spd + 1);
            }
            else if (special == "slowbeacon")
            {
                if (lifespan % 2 == 0)
                {
                    bulletRect.X--;
                    bulletRect.Y--;
                    bulletRect.Width += 2;
                    bulletRect.Height += 2;
                }
                if (lifespan % 20 == 0)
                {
                    bulletRect.X += 10;
                    bulletRect.Y += 10;
                    bulletRect.Width -= 20;
                    bulletRect.Height -= 20;
                }
            }
            else
            {
                realX += a;
                realY += b;
                bulletRect.X = (int)realX;
                bulletRect.Y = (int)realY;
                /*
                realX += (spd * (objectiveX - bulletX))
                / (Math.Pow(objectiveX - bulletX, 2) + Math.Pow(objectiveY - bulletX, 2));
                realY += (spd * (objectiveY - bulletY))
                / (Math.Pow(objectiveX - bulletX, 2) + Math.Pow(objectiveY - bulletY, 2));
                bulletRect.X = (int)realX;
                bulletRect.Y = (int)realY;*/
            }
            lifespan--;
        }
    }
}