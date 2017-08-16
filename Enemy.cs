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

public class Enemy
{
    public int hp;
    public int maxHealth;
    public int spd;
    public int dmg;
    public int size;
    public Rectangle enemyRect;
    public Rectangle hpRect;
    public Rectangle backgroundhp;
    public Texture2D tex;
    public double realX;
    public double realY;
    public int[] alreadyHit; //bullet indexes of already hit bullets to prevent multiple hits off of a penetrable bullet
    public int reverseTime;
    public bool shadowMarked;
    public int shadowTimer;
	public Enemy(int health, int speed, int damage, int sz, int spawnWidth, int spawnHeight, Texture2D texture)
	{
        alreadyHit = new int[5];
        maxHealth = health;
        spd = speed;
        dmg = damage;
        size = sz;
        tex = texture;
        hp = health;
        reverseTime = 0;
        shadowMarked = false;
        shadowTimer = 90;
        //enemy location determined here - tempR used to determine which "wall" enemy is placed against
        Random r = new Random();
        int tempR = r.Next(4);
        switch (tempR) {
            case 0:
                enemyRect = new Rectangle(r.Next(spawnWidth - size), 0, size, size);
                break;
            case 1:
                enemyRect = new Rectangle(0, r.Next(spawnHeight - size), size, size);
                break;
            case 2:
                enemyRect = new Rectangle(spawnWidth - size, r.Next(spawnHeight - size), size, size);
                break;
            case 3:
                enemyRect = new Rectangle(r.Next(spawnWidth - size), spawnHeight - size, size, size);
                break;
            default:
                enemyRect = new Rectangle(r.Next(spawnWidth - size), spawnHeight - size, size, size);
                break;
        }
        hpRect = new Rectangle(enemyRect.X, enemyRect.Y + sz + 1, sz, 2);
        backgroundhp = new Rectangle(enemyRect.X, enemyRect.Y + sz + 1, sz, 2);
        realX = enemyRect.X;
        realY = enemyRect.Y;
    }
}
