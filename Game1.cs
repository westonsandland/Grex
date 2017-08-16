using System;
using System.IO;
using System.Text.RegularExpressions;
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
    //AUTHOR: Weston Sandland
    //DATE: 1/16/2017
    //VERSION: 1.2.0
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int MIN_ABILITY_COOLDOWN = 10;
        const int SMALL_FREQ = 190;
        const int MED_FREQ = 320;
        const int LARGE_FREQ = 1200;
        const int SHADOWMARK_RADIUS = 120;
        const int SHADOWMARK_DMG = 60;
        const int SHADOW_TELE_DMG = 200;
        const int ULT_GRACE_PERIOD = 120;
        const string GAME_VERSION = "v1.2.0";
        const string INSTRUCTIONS = "Controls: WASD = movement            Space/Esc = pause\n          E = secondary attack        Q = special attack\n          R = tertiary attack";
        string scoreDir = Directory.GetCurrentDirectory();
        const string scoreFileName = "Scores.txt";
        string scorePath;
        Regex alphaNum;
        string[] topFive;
        Vector2 topFiveVec;
        enum ScreenState { Start, Selection, Play, Pause };
        const int CURSOR_SIZE = 11;
        int currentSmallFreq;
        int currentMedFreq;
        int currentLargeFreq;
        Rectangle randomButton;
        Vector2 randomButtonText;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font, font2, font3;
        ScreenState theScreen;
        Texture2D[] outerPlayTexs;
        Texture2D[] innerPlayTexs;
        Texture2D basicTex;
        Texture2D upArrow;
        Texture2D downArrow;
        string[,] elementSelectionStrings;
        Vector2 secondaryGUIVec;
        Rectangle secondaryBackground;
        Rectangle secondaryForeground;
        Vector2 tertiaryGUIVec;
        Rectangle tertiaryBackground;
        Rectangle tertiaryForeground;
        string specialAttackGUI;
        Vector2 specialAttackGUIVec;
        Rectangle specialAttackBackground;
        Rectangle specialAttackForeground;
        Rectangle cursor1;
        Rectangle cursor2;
        Rectangle cover;
        Rectangle selectionStart;
        Vector2 selStartVec;
        Vector2 scoreGUIVec;
        Vector2 deathScoreVec;
        Vector2 controls;
        string name;
        string scoreGUI;
        int score;
        string hitpointsGUI;
        Vector2 hitpointsGUIVec;
        Rectangle hitpointsBackground;
        Rectangle hitpointsForeground;
        string pauseScreenText;
        Player thePlayer;
        KeyboardState k, oldState;
        MouseState m, oldM;
        List<Enemy> enemies;
        Rectangle[] upArrows;
        Rectangle[] downArrows;
        Vector2[] elementSelects;
        Vector2[] attackTypeVecs;
        Vector2 pauseNotification;
        Color[] colors;
        Color[] arrowColors;
        Color specColor;
        List<Bullet> playerBullets;
        Keys primaryButton, secondaryButton, tertiaryButton, ultimateButton;
        Keys[] oldPressed;
        bool written;
        int PCooldown;
        string[] attackTypeStrs;
        int[] playerCoolDowns;
        int rave;
        int specialAttackMeter;
        int frequencySmallEnemies;
        int frequencyLargeEnemies;
        int frequencyMediumEnemies;
        int PDDelay;
        int ultChargeDelay;
        bool ravemode;
        bool alternator, alternator2;
        bool PAutoFire;
        bool S1Fire;
        int S1Cooldown;
        int S1ChainNum;
        int S1ChainDmg;
        bool S2Fire;
        int S2Cooldown;
        bool UFire;
        int rotary;
        int screenWidth, screenHeight;
        int[] selectionInts;
        int distanceFlipper;
        int originalEnemyX;
        int originalEnemyY;
        bool deathMark;
        int enemyChainIndex;
        Random rand;
        //System.IO.StreamWriter file;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            rand = new Random();
            //file = new System.IO.StreamWriter("C:\\Users\\westo\\Documents\\Visual Studio 2015\\Projects\\Grex\\Grex\\Grex\\output.txt");
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            currentSmallFreq = frequencySmallEnemies = SMALL_FREQ;
            currentMedFreq = frequencyMediumEnemies = MED_FREQ;
            currentLargeFreq = frequencyLargeEnemies = LARGE_FREQ;
            alphaNum = new Regex("^[a-zA-Z0-9]*$");
            outerPlayTexs = new Texture2D[4];
            innerPlayTexs = new Texture2D[4];
            upArrows = new Rectangle[4];
            downArrows = new Rectangle[4];
            elementSelects = new Vector2[4];
            attackTypeVecs = new Vector2[4];
            pauseNotification = new Vector2(screenWidth / 2 - 100, 100);
            deathScoreVec = new Vector2(screenWidth / 2  - 100, 225);
            specColor = Color.Orange;
            colors = new Color[] { Color.Yellow, Color.Red, Color.Blue, Color.White, Color.Purple, specColor };
            arrowColors = new Color[] { Color.Gray, Color.White };
            name = "N/A";
            oldPressed = new Keys[0];
            topFiveVec = new Vector2(50, 150);
            //file access stuff here
            Directory.CreateDirectory(scoreDir);
            scorePath = Path.Combine(scoreDir, scoreFileName);
            if (!File.Exists(scorePath))
                File.Create(scorePath);
            File.SetAttributes(scorePath, FileAttributes.Normal);
            StreamReader reader = new StreamReader(scorePath);
            string nextLine = " ";
            string holder;
            nextLine = reader.ReadLine();
            topFive = new string[5];
            for(int n = 0; n < 5; n++)
                topFive[n] = "0000000";
            while (nextLine != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Int32.Parse(nextLine.Substring(5)) > Int32.Parse(topFive[i].Substring(5)))
                    {
                        holder = topFive[i];
                        topFive[i] = nextLine;
                        nextLine = holder;
                    }
                }
                nextLine = reader.ReadLine();
            }
            reader.Close(); 
            secondaryGUIVec = new Vector2(25, screenHeight - 85);
            secondaryBackground = new Rectangle(25, screenHeight - 85, 100, 30);
            secondaryForeground = new Rectangle(25, screenHeight - 85, 100, 30);
            tertiaryGUIVec = new Vector2(25, screenHeight - 45);
            tertiaryBackground = new Rectangle(25, screenHeight - 45, 100, 30);
            tertiaryForeground = new Rectangle(25, screenHeight - 45, 100, 30);
            specialAttackMeter = 200;
            PDDelay = 0;
            specialAttackBackground = new Rectangle(screenWidth - 225, screenHeight - 55, 200, 30);
            specialAttackForeground = new Rectangle(screenWidth - 225, screenHeight - 55, 200, 30);
            specialAttackGUI = "Special";
            pauseScreenText = "PAUSED";
            specialAttackGUIVec = new Vector2(screenWidth - 210, screenHeight - 55);
            hitpointsBackground = new Rectangle(screenWidth - 115, 15, 100, 30);
            hitpointsForeground = new Rectangle(screenWidth - 115, 15, 100, 30);
            hitpointsGUI = "HEALTH: ";
            hitpointsGUIVec = new Vector2(screenWidth - 105, 17);
            scoreGUIVec = new Vector2(screenWidth - 85, 50);
            scoreGUI = "Score: ";
            score = 0;
            deathMark = false;
            written = false;
            attackTypeStrs = new String[] { "Primary", "Secondary", "Tertiary", "Special" };
            selectionStart = new Rectangle(screenWidth / 2 - 50, 375, 100, 50);
            selStartVec = new Vector2(screenWidth / 2 - 28, 387);
            elementSelectionStrings = new string[4,5];
            ultChargeDelay = 0;
            for (int ele1 = 0; ele1 < 4; ele1++)
            {
                for (int ele2 = 0; ele2 < 5; ele2++)
                {
                    if (ele2 == 0)
                        elementSelectionStrings[ele1, ele2] = "Lightning";
                    else if (ele2 == 1)
                        elementSelectionStrings[ele1, ele2] = "Fire";
                    else if (ele2 == 2)
                        elementSelectionStrings[ele1, ele2] = "Ice";
                    else if (ele2 == 3)
                        elementSelectionStrings[ele1, ele2] = "Wind";
                    else if (ele2 == 4)
                        elementSelectionStrings[ele1, ele2] = "Shadow";
                }
            }
            //TEMPORARY "COMING SOON" NOTIFICATION
            elementSelectionStrings[3, 1] = "COMING SOON!";
            elementSelectionStrings[3, 2] = "COMING SOON!";
            elementSelectionStrings[3, 3] = "COMING SOON!";
            elementSelectionStrings[3, 4] = "COMING SOON!";
            //TODO: CHANGE RE TO 4 IF ADDING ULTIMATE ABILITIES
            for (int re = 0; re < 3; re++)
            {
                upArrows[re] = new Rectangle(50 + re * 200, 50, 100, 100);
                downArrows[re] = new Rectangle(50 + re * 200, 250, 100, 100);
                elementSelects[re] = new Vector2(50 + re * 200, 190);
                if (re == 1)
                    attackTypeVecs[re] = new Vector2(50 + re * 200, 10);
                else if (re == 2)
                    attackTypeVecs[re] = new Vector2(55 + re * 200, 10);
                else
                    attackTypeVecs[re] = new Vector2(60 + re * 200, 10);
            }
            randomButton = new Rectangle(610, 190, 120, 32);
            randomButtonText = new Vector2(614, 193);
            controls = new Vector2(75, 375);
            cursor1 = new Rectangle(-20, -20, CURSOR_SIZE, 1);
            cursor2 = new Rectangle(-20, -20, 1, CURSOR_SIZE);
            cover = new Rectangle(0, 0, screenWidth, screenHeight);
            rotary = 0;
            rave = 0;
            distanceFlipper = 1;
            selectionInts = new int[] { 0, 0, 0, 0 };
            ravemode = rand.Next(200) == 134;
            thePlayer = new Player(screenWidth, screenHeight);
            alternator = true;
            alternator2 = true;
            enemies = new List<Enemy>();
            theScreen = ScreenState.Start;
            //All ability variables initialized here
            primaryButton = Keys.P;
            secondaryButton = Keys.E;
            tertiaryButton = Keys.R;
            ultimateButton = Keys.Q;
            playerBullets = new List<Bullet>();
            playerCoolDowns = new int[5];
            PAutoFire = true;
            PCooldown = 0;
            S1Fire = false;
            S1Cooldown = 0;
            enemyChainIndex = -1;
            S2Fire = false;
            S2Cooldown = 0;
            //End of ability varaiables
            base.Initialize();
            k = new KeyboardState();
            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            innerPlayTexs[2] = outerPlayTexs[0] = Content.Load<Texture2D>("playerTex1");
            innerPlayTexs[3] = outerPlayTexs[1] = Content.Load<Texture2D>("playerTex2");
            innerPlayTexs[0] = outerPlayTexs[2] = Content.Load<Texture2D>("playerTex3");
            innerPlayTexs[1] = outerPlayTexs[3] = Content.Load<Texture2D>("playerTex4");
            basicTex = Content.Load<Texture2D>("empty");
            font = Content.Load<SpriteFont>("SpriteFont1");
            font2 = Content.Load<SpriteFont>("SpriteFont2");
            font3 = Content.Load<SpriteFont>("SpriteFont3");
            upArrow = Content.Load<Texture2D>("UpArrow");
            downArrow = Content.Load<Texture2D>("DownArrow");
            thePlayer.texture1 = outerPlayTexs[0];
            thePlayer.texture2 = innerPlayTexs[0];
        }

        protected override void UnloadContent()
        {
            //file.WriteLine("End of file.");
            //file.Close();
        }

        protected override void Update(GameTime gameTime)
        {
            //acquires player input
            k = Keyboard.GetState();
            m = Mouse.GetState();
            //moves cursor accordingly here
            cursor1.X = m.X - (CURSOR_SIZE - 1) / 2;
            cursor1.Y = m.Y;
            cursor2.X = m.X;
            cursor2.Y = m.Y - (CURSOR_SIZE - 1) / 2;
            //animation for player starts here
            if (rotary == 4)
                    rotary = 0;
                thePlayer.texture1 = outerPlayTexs[rotary];
                thePlayer.texture2 = innerPlayTexs[rotary];
                if (alternator)
                    alternator = false;
                else
                {
                    rotary++;
                    alternator = true;
                }
            //ANY PLAY LOGIC STARTS HERE
            if (theScreen == ScreenState.Play)
            {
                //player keyboard logic starts here
                if (k.IsKeyDown(Keys.A) && thePlayer.primaryRect.X > 0)
                    thePlayer.decreasePlayerX();
                if (k.IsKeyDown(Keys.D) && thePlayer.primaryRect.X < screenWidth - thePlayer.primaryRect.Width)
                    thePlayer.increasePlayerX();
                if (k.IsKeyDown(Keys.W) && thePlayer.primaryRect.Y > 0)
                    thePlayer.decreasePlayerY();
                if (k.IsKeyDown(Keys.S) && thePlayer.primaryRect.Y < screenHeight - thePlayer.primaryRect.Height)
                    thePlayer.increasePlayerY();
                if ((k.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape)) || 
                   (k.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space)))
                    theScreen = ScreenState.Pause;
                if (k.IsKeyDown(primaryButton) && !oldState.IsKeyDown(primaryButton))
                    PAutoFire = !PAutoFire;
                if (k.IsKeyDown(secondaryButton) && !oldState.IsKeyDown(secondaryButton) && S1Cooldown == 0)
                    S1Fire = true;
                if (k.IsKeyDown(tertiaryButton) && !oldState.IsKeyDown(tertiaryButton) && S2Cooldown == 0)
                    S2Fire = true;
                if (k.IsKeyDown(ultimateButton) && !oldState.IsKeyDown(ultimateButton) && specialAttackMeter == playerCoolDowns[3])
                    UFire = true;
                //player bullet update logic starts here
                //player bullet firing logic here
                //primary
                if (PDDelay > 0)
                {
                    PDDelay--;
                    if (PDDelay % 10 == 1)
                    {
                        playerBullets.Add(new Bullet(thePlayer.passiveRect.X, thePlayer.passiveRect.Y, m.X, m.Y, thePlayer, 0, screenWidth, screenHeight));
                    }
                }
                if (PAutoFire && PCooldown == 0)
                {
                    playerBullets.Add(new Bullet(thePlayer.passiveRect.X, thePlayer.passiveRect.Y, m.X, m.Y, thePlayer, 0, screenWidth, screenHeight));
                    if (thePlayer.primary == "Shadow")
                        PDDelay = 20;
                    PCooldown = playerCoolDowns[0];
                }
                else
                {
                    if (PCooldown > 0)
                        PCooldown--;
                }
                //secondary
                if (S1Fire && S1Cooldown == 0)
                {
                    if (thePlayer.secondary != "Shadow")
                    {
                        playerBullets.Add(new Bullet(thePlayer.passiveRect.X, thePlayer.passiveRect.Y, m.X, m.Y, thePlayer, 1, screenWidth, screenHeight));
                        S1Cooldown = playerCoolDowns[1];
                    }
                    else
                    {
                        for (int s = 0; s < enemies.Count; s++)
                        {
                            if (cursor2.Intersects(enemies[s].enemyRect))
                            {
                                enemies[s].shadowMarked = true;
                                S1Cooldown = playerCoolDowns[1];
                                break;
                            }
                        }
                    }
                    S1Fire = false;
                }
                else
                {
                    if (S1Cooldown > 0)
                        S1Cooldown--;
                }
                //tertiary
                if (S2Fire && S2Cooldown == 0)
                {
                    if (thePlayer.tertiary == "Ice" || thePlayer.tertiary == "Fire")
                        playerBullets.Add(new Bullet(m.X, m.Y, m.X, m.Y, thePlayer, 2, screenWidth, screenHeight));
                    else if (thePlayer.tertiary == "Lightning")
                    {
                        playerBullets.Add(new Bullet(thePlayer.passiveRect.X, thePlayer.passiveRect.Y, thePlayer.passiveRect.X, thePlayer.passiveRect.Y, thePlayer, 2, screenWidth, screenHeight));
                    }
                    else if (thePlayer.tertiary == "Wind")
                    {
                        foreach (Enemy e in enemies)
                            e.reverseTime = 150;
                    }
                    else
                    {
                        thePlayer.invulnerability = 60;
                        thePlayer.setPlayerX(m.X - thePlayer.passiveRect.X);
                        thePlayer.setPlayerY(m.Y - thePlayer.passiveRect.Y);
                        for (int o = 0; o < enemies.Count; o++)
                        {
                            if (enemies[o].enemyRect.Intersects(thePlayer.primaryRect))
                            {
                                enemies[o].hp -= SHADOW_TELE_DMG;
                                enemies[o].hpRect.Width -= (SHADOW_TELE_DMG * enemies[o].size) / enemies[o].maxHealth;
                                specialAttackMeter += SHADOW_TELE_DMG;
                                if (enemies[o].hp < 1)
                                {
                                    score += 10 * (enemies[o].maxHealth / 100);
                                    enemies.RemoveAt(o);
                                    o--;
                                }
                            }
                        }
                    }
                    S2Cooldown = playerCoolDowns[2];
                    S2Fire = false;
                }
                else
                {
                    if (S2Cooldown > 0)
                        S2Cooldown--;
                }
                //special
                if (UFire)
                {
                    if (thePlayer.special == "Lightning")
                    {
                        playerBullets.Add(new Bullet(thePlayer.passiveRect.X, thePlayer.passiveRect.Y, m.X, m.Y, thePlayer, 3, screenWidth, screenHeight));
                    }
                    specialAttackGUI = "Special";
                    specialAttackMeter = 0;
                    ultChargeDelay = ULT_GRACE_PERIOD;
                    UFire = false;
                }
                //all bullets updated
                for (int i = 0; i < playerBullets.Count; i++)
                {
                    playerBullets[i].Update();
                    if (playerBullets[i].lifespan == 0)
                        playerBullets.RemoveAt(i);
                }
                //enemy creation logic starts here
                if (frequencySmallEnemies < 1)
                {
                    enemies.Add(new Enemy(100, 64, 15, 20, screenWidth, screenHeight, basicTex));
                    frequencySmallEnemies = currentSmallFreq - rand.Next(currentSmallFreq / 2);
                }
                else
                    frequencySmallEnemies--;
                if (frequencyMediumEnemies < 1)
                {
                    enemies.Add(new Enemy(200, 32, 30, 35, screenWidth, screenHeight, basicTex));
                    frequencyMediumEnemies = currentMedFreq - rand.Next(currentMedFreq / 2);
                    if (currentSmallFreq > 30)
                        currentSmallFreq-=2;
                }
                else
                    frequencyMediumEnemies--;
                if (frequencyLargeEnemies < 1)
                {
                    enemies.Add(new Enemy(600, 8, 50, 80, screenWidth, screenHeight, basicTex));
                    frequencyLargeEnemies = currentLargeFreq - rand.Next(currentLargeFreq / 2);
                    if(currentSmallFreq > 30)
                    currentSmallFreq-=3;
                }
                else
                    frequencyLargeEnemies--;
                //enemy update logic starts here
                for (int s = 0; s < enemies.Count; s++)
                {
                    //enemy bullet collision
                    for (int q = 0; q < playerBullets.Count; q++)
                    {
                        if (playerBullets[q].special == "slowbeacon" && playerBullets[q].lifespan % 60 == 0)
                        {
                            if (enemies[s].enemyRect.Intersects(playerBullets[q].radiusOfEffect))
                            {
                                if(enemies[s].spd > 4)
                                    enemies[s].spd -= 4;
                                else
                                    enemies[s].spd = 1;
                                enemies[s].hp -= playerBullets[q].dmg;
                                enemies[s].hpRect.Width -= (playerBullets[q].dmg * enemies[s].size) / enemies[s].maxHealth;
                                specialAttackMeter += playerBullets[q].dmg;
                                if (enemies[s].hp < 1)
                                {
                                    deathMark = true;
                                }
                            }
                        }
                        else if (enemies[s].enemyRect.Intersects(playerBullets[q].bulletRect) && enemies[s].alreadyHit[playerBullets[q].abilityType] == 0)
                        {
                            enemies[s].hp -= playerBullets[q].dmg;
                            enemies[s].hpRect.Width -= (playerBullets[q].dmg * enemies[s].size) / enemies[s].maxHealth;
                            specialAttackMeter += playerBullets[q].dmg;
                            //checks for special effects upon collision
                            if (playerBullets[q].special != "none")
                            {
                                switch (playerBullets[q].special)
                                {
                                    case "slow":
                                        if (playerBullets[q].specialNum == 0)
                                            enemies[s].spd = playerBullets[q].specialNum;
                                        else if (playerBullets[q].specialNum == -1)
                                            enemies[s].spd = 1;
                                        else
                                        {
                                            if (enemies[s].spd > 1)
                                                enemies[s].spd /= playerBullets[q].specialNum;
                                        }
                                        break;
                                    case "push":
                                        enemies[s].reverseTime = playerBullets[q].specialNum;
                                        break;
                                    case "heal":
                                        if (thePlayer.hitpoints < thePlayer.MAX_HITPOINTS)
                                        {
                                            thePlayer.hitpoints += playerBullets[q].specialNum;
                                            hitpointsForeground.Width += playerBullets[q].specialNum;
                                        }
                                        break;
                                    case "chain":
                                        enemyChainIndex = s;
                                        S1ChainNum = playerBullets[q].specialNum;
                                        S1ChainDmg = playerBullets[q].dmg;
                                        break;        
                                }
                            }
                            //enemy death
                            if (enemies[s].hp < 1)
                            {
                                deathMark = true;
                            }
                            //checks for double hit by one bullet, prevents
                            if (playerBullets[q].penetrable) //enemy is granted immunity for 10 frames from that ability type
                                enemies[s].alreadyHit[playerBullets[q].abilityType] = MIN_ABILITY_COOLDOWN;
                            else
                            {
                                playerBullets.RemoveAt(q);
                            }
                        }
                    }

                    for (int ru = 0; ru < 5; ru++)
                    {
                        //Console.WriteLine("ru is " + ru + ", enemies[s].alreadyhit.length is " + enemies[s].alreadyHit.Length + ", s is " + s);
                        if (enemies[s].alreadyHit[ru] > 0)
                            enemies[s].alreadyHit[ru]--;
                    }

                    //enemy player collision
                    if (enemies[s].enemyRect.Intersects(thePlayer.primaryRect) && thePlayer.invulnerability == 0)
                    {
                        thePlayer.hitpoints -= enemies[s].dmg;
                        hitpointsForeground.Width = thePlayer.hitpoints;
                        deathMark = true;
                        if (thePlayer.hitpoints < 1)
                        {
                            //death of player
                            theScreen = ScreenState.Pause;
                            pauseScreenText = "GAME OVER";
                        }
                    }
                    //enemy pathing
                        if ((Math.Pow(thePlayer.passiveRect.X - enemies[s].enemyRect.X, 2) + Math.Pow(thePlayer.passiveRect.Y - enemies[s].enemyRect.Y, 2)) != 0)
                        {
                            if (enemies[s].reverseTime > 0)
                            {
                                distanceFlipper = -1;
                                enemies[s].reverseTime--;
                            }
                            else if (distanceFlipper < 0)
                                distanceFlipper = 1;
                            enemies[s].realX += distanceFlipper * ((enemies[s].spd * (thePlayer.passiveRect.X - enemies[s].enemyRect.X))
                                / (Math.Pow(thePlayer.passiveRect.X - enemies[s].enemyRect.X, 2) + Math.Pow(thePlayer.passiveRect.Y - enemies[s].enemyRect.Y, 2)));
                            enemies[s].realY += distanceFlipper * ((enemies[s].spd * (thePlayer.passiveRect.Y - enemies[s].enemyRect.Y))
                                / (Math.Pow(thePlayer.passiveRect.X - enemies[s].enemyRect.X, 2) + Math.Pow(thePlayer.passiveRect.Y - enemies[s].enemyRect.Y, 2)));
                            enemies[s].enemyRect.X = (int)enemies[s].realX;
                            enemies[s].enemyRect.Y = (int)enemies[s].realY;
                            enemies[s].hpRect.X = (int)enemies[s].realX;
                            enemies[s].hpRect.Y = (int)enemies[s].realY + enemies[s].size + 1;
                            enemies[s].backgroundhp.X = enemies[s].hpRect.X;
                            enemies[s].backgroundhp.Y = enemies[s].hpRect.Y;
                        }
                    if (deathMark)
                        enemies[s].shadowTimer = 0;
                    if (enemies[s].shadowMarked)
                    {
                        if (enemies[s].shadowTimer > 1)
                            enemies[s].shadowTimer--;
                        else
                        {
                            enemies[s].shadowTimer = 0;
                            for (int sj = 0; sj < enemies.Count; sj++)
                            {
                                if (enemies[sj].enemyRect.X > enemies[s].enemyRect.X - SHADOWMARK_RADIUS && enemies[sj].enemyRect.X < enemies[s].enemyRect.X + SHADOWMARK_RADIUS
                                    && enemies[sj].enemyRect.Y > enemies[s].enemyRect.Y - SHADOWMARK_RADIUS && enemies[sj].enemyRect.Y < enemies[s].enemyRect.Y + SHADOWMARK_RADIUS && !enemies[sj].shadowMarked)
                                {
                                    enemies[sj].hp -= SHADOWMARK_DMG;
                                    enemies[sj].hpRect.Width -= (SHADOWMARK_DMG * enemies[sj].size) / enemies[sj].maxHealth;
                                }
                            }
                            for (int o = 0; o < enemies.Count; o++)
                                if (enemies[o].hp < 1)
                                {
                                    enemies.RemoveAt(o);
                                    o--;
                                    score += 10;
                                }
                        }
                    }
                    else if (deathMark)
                    {
                        score += 10 * (enemies[s].maxHealth / 100);
                        enemies.RemoveAt(s);
                        enemyChainIndex--;
                        deathMark = false;
                    }
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].shadowMarked && enemies[i].shadowTimer == 0 || deathMark)
                    {
                        enemies.RemoveAt(i);
                        score += 10;
                        deathMark = false;
                    }
                }
                if (enemyChainIndex > -1)
                {
                    originalEnemyX = enemies[enemyChainIndex].enemyRect.X;
                    originalEnemyY = enemies[enemyChainIndex].enemyRect.Y;
                    //Console.WriteLine("Original enemy x is " + originalEnemyX + ", original enemy y is " + originalEnemyY);
                    //Console.WriteLine("Before: ");
                    //foreach (Enemy u in enemies)
                    //    Console.WriteLine(u.enemyRect.X + " " + u.enemyRect.Y);
                    //enemies.Sort(comparePythagorean);
                    //Console.WriteLine("After: ");
                    //foreach (Enemy u in enemies)
                    //    Console.WriteLine(u.enemyRect.X + " " + u.enemyRect.Y);
                    //Console.WriteLine("ENDOFCHAIN");
                    for (int p = 1; p <= S1ChainNum && p < enemies.Count; p++)
                    {
                        //Console.WriteLine(enemies[p].enemyRect.X);
                        enemies[p].hp -= S1ChainDmg;
                        enemies[p].hpRect.Width -= (S1ChainDmg * enemies[p].size) / enemies[p].maxHealth;
                        specialAttackMeter += S1ChainDmg;
                        if (enemies[p].hp < 1)
                        {
                            enemies.RemoveAt(p);
                            p--;
                            score += 10;
                        }
                    }
                    enemyChainIndex = -1;
                }
                if (ultChargeDelay > 0)
                {
                    ultChargeDelay--;
                    specialAttackMeter = 0;
                }
                //counts down invuln
                if (thePlayer.invulnerability > 0)
                    thePlayer.invulnerability--;
                //special attack readjustments
                if (specialAttackMeter / playerCoolDowns[3] >= 1)
                {
                    specialAttackMeter = playerCoolDowns[3];
                    specialAttackGUI = "Ready! Press 'Q'";
                }
                specialAttackForeground.Width = (specialAttackMeter * 200) / playerCoolDowns[3];
                //secondary and tertiary attack gui adjustment
                secondaryForeground.Width = (S1Cooldown * 100) / playerCoolDowns[1];
                tertiaryForeground.Width = (S2Cooldown * 100) / playerCoolDowns[2];
            }
            else if (theScreen == ScreenState.Start)
            {
                if (m.LeftButton == ButtonState.Pressed && oldM.LeftButton != ButtonState.Pressed)
                {
                    theScreen++;
                }
            }
            else if (theScreen == ScreenState.Selection)
            {
                for (int ju = 0; ju < 4; ju++)
                {
                    if (m.LeftButton == ButtonState.Pressed && oldM.LeftButton != ButtonState.Pressed && cursor1.Intersects(upArrows[ju]) && selectionInts[ju] > 0)
                    {
                        selectionInts[ju]--;
                    }
                    if (m.LeftButton == ButtonState.Pressed && oldM.LeftButton != ButtonState.Pressed && cursor1.Intersects(downArrows[ju]) && selectionInts[ju] < 4)
                    {
                        selectionInts[ju]++;
                    }
                }
                    if (m.LeftButton == ButtonState.Pressed && oldM.LeftButton != ButtonState.Pressed && cursor1.Intersects(randomButton))
                    {
                    for (int p = 0; p < 4; p++)
                        selectionInts[p] = rand.Next(5);
                    }
                    if (m.LeftButton == ButtonState.Pressed && oldM.LeftButton != ButtonState.Pressed && cursor1.Intersects(selectionStart))
                    {
                        //NOTE: Ability cooldowns cannot be MIN_ABILITY_COOLDOWN or lower, or the enemy will not detect both hits
                        //start of game - logic set in place
                        specialAttackMeter = 0;
                        specialAttackForeground.Width = specialAttackMeter;
                        //primary
                        thePlayer.primary = elementSelectionStrings[0, selectionInts[0]];
                        if (thePlayer.primary == "Ice" || thePlayer.primary == "Shadow")
                            playerCoolDowns[0] = 30;
                        else
                            playerCoolDowns[0] = 45;
                        //secondary
                        thePlayer.secondary = elementSelectionStrings[1, selectionInts[1]];
                        if (thePlayer.secondary == "Shadow")
                            playerCoolDowns[1] = 600;
                        else if (thePlayer.secondary == "Lightning")
                            playerCoolDowns[1] = 360;
                        else if (thePlayer.secondary == "Wind")
                            playerCoolDowns[1] = 300;
                        else
                            playerCoolDowns[1] = 420;
                        //tertiary
                        thePlayer.tertiary = elementSelectionStrings[2, selectionInts[2]];
                        if (thePlayer.tertiary == "Shadow")
                            playerCoolDowns[2] = 720;
                        else if (thePlayer.tertiary == "Lightning")
                            playerCoolDowns[2] = 1440;
                        else
                            playerCoolDowns[2] = 1200;
                        //special
                        //TODO: uncomment if wanting to add other specs
                        //thePlayer.special = elementSelectionStrings[3, selectionInts[3]];
                        thePlayer.special = "Lightning";
                        if (thePlayer.special == "Wind")
                            playerCoolDowns[3] = 1200;
                        else
                            playerCoolDowns[3] = 2000;
                        //passive
                        if (thePlayer.primary == thePlayer.secondary && thePlayer.secondary == thePlayer.tertiary && thePlayer.tertiary == thePlayer.special)
                        {
                            thePlayer.passive = thePlayer.primary;
                        }
                        if (thePlayer.passive == "Lightning" || thePlayer.passive == "Shadow")
                            playerCoolDowns[4] = 60;
                        else if (thePlayer.passive == "Ice")
                            playerCoolDowns[4] = 120;
                        else
                            playerCoolDowns[4] = -1;
                        //Screen state is set to next one
                        theScreen = ScreenState.Play;   
                    }
                
            }
            else
            {
                if (k.GetPressedKeys().Length == 1 && oldPressed.Length == 0 && !written
                    && alphaNum.IsMatch(k.GetPressedKeys().ElementAt(0).ToString()) && thePlayer.hitpoints < 1)
                {
                    if (!name.Equals("N/A") && name.Length < 3)
                        name += k.GetPressedKeys().ElementAt(0).ToString();
                    else if (name.Equals("N/A"))
                        name = k.GetPressedKeys().ElementAt(0).ToString();
                    else
                        using (StreamWriter file = new StreamWriter(@scorePath, true))
                        {
                            file.WriteLine(name.Substring(0,3) + ": " + score);
                            written = true;
                        }
                    pauseScreenText = "Your name is :" + name;
                }
                oldPressed = k.GetPressedKeys();
                if (((k.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape)) || 
                    (k.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space))) && thePlayer.hitpoints > 0)
                    theScreen = ScreenState.Play;
                if (k.IsKeyDown(Keys.Z))
                {
                    theScreen = ScreenState.Start;
                    Initialize();
                    LoadContent();
                }
            }
            //END OF UPDATE METHOD
            oldState = k;
            oldM = m;
            base.Update(gameTime);
        }

        private int comparePythagorean(Enemy a, Enemy b)
        {
            Console.WriteLine("a.enemyrect.x is " + a.enemyRect.X);
            Console.WriteLine("a.enemyrect.y is " + a.enemyRect.Y);
            Console.WriteLine("b.enemyrect.x is " + b.enemyRect.X);
            Console.WriteLine("b.enemyrect.y is " + b.enemyRect.Y);
            Console.WriteLine(originalEnemyX);
            int aXDiff = a.enemyRect.X - originalEnemyX;
            int aYDiff = a.enemyRect.Y - originalEnemyY;
            int bXDiff = b.enemyRect.X - originalEnemyX;
            int bYDiff = b.enemyRect.Y - originalEnemyY;
            return (int)(Math.Sqrt(aXDiff*aXDiff + aYDiff*aYDiff) - Math.Sqrt(bXDiff*bXDiff - bYDiff*bYDiff));

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (theScreen != ScreenState.Pause)
            {
                if (theScreen == ScreenState.Start)
                {
                    Vector2 title = new Vector2(screenWidth / 2 - 100, 100);
                    Vector2 clicktostart = new Vector2(screenWidth / 2 - 50, 300);
                    spriteBatch.DrawString(font2, "Grex "+GAME_VERSION, title, Color.White);
                    spriteBatch.DrawString(font, "Click anywhere to start", clicktostart, Color.White);
                    spriteBatch.DrawString(font, INSTRUCTIONS, controls, Color.White);
                }
                else if (theScreen == ScreenState.Selection)
                {
                    //TODO: IF READDING SPECIAL ATTACK, CHANGE "u" VARIABLE TO 4
                    for (int u = 0; u < 3; u++)
                    {
                        spriteBatch.Draw(upArrow, upArrows[u], arrowColors[(selectionInts[u] + 3) / 4]);
                        spriteBatch.Draw(downArrow, downArrows[u], arrowColors[(selectionInts[u] / 4) * -1 + 1]);
                        spriteBatch.DrawString(font, elementSelectionStrings[u, selectionInts[u]], elementSelects[u], Color.White);
                        spriteBatch.DrawString(font, attackTypeStrs[u], attackTypeVecs[u], Color.White);
                    }
                    spriteBatch.Draw(basicTex, randomButton, Color.DodgerBlue);
                    spriteBatch.DrawString(font, "Randomize", randomButtonText, Color.Black);
                    spriteBatch.Draw(basicTex, selectionStart, Color.Green);
                    spriteBatch.DrawString(font, "Start", selStartVec, Color.White);
                }
                //bottom layer
                foreach (Enemy e in enemies)
                {
                    if (ravemode)
                    {
                        switch (rave)
                        {
                            case 0:
                                spriteBatch.Draw(e.tex, e.enemyRect, Color.Yellow);
                                break;
                            case 1:
                                spriteBatch.Draw(e.tex, e.enemyRect, Color.Orange);
                                break;
                            case 2:
                                spriteBatch.Draw(e.tex, e.enemyRect, Color.Red);
                                break;
                            case 3:
                                spriteBatch.Draw(e.tex, e.enemyRect, Color.Purple);
                                break;
                            case 4:
                                spriteBatch.Draw(e.tex, e.enemyRect, Color.Blue);
                                break;
                            case 5:
                                spriteBatch.Draw(e.tex, e.enemyRect, Color.Green);
                                break;
                        }
                        if (rave != 5)
                            rave++;
                        else
                            rave = 0;
                    }
                    else
                    {
                        if (e.shadowMarked && alternator2)
                            spriteBatch.Draw(e.tex, e.enemyRect, colors[4]);
                        else
                            spriteBatch.Draw(e.tex, e.enemyRect, Color.White);
                            alternator2 = !alternator2;
                    }
                }
                foreach (Enemy e in enemies)
                    if (e.hp != e.maxHealth)
                    {
                        spriteBatch.Draw(basicTex, e.backgroundhp, Color.Red);
                        spriteBatch.Draw(basicTex, e.hpRect, Color.Lime);
                    }
                //mid layer
                spriteBatch.Draw(thePlayer.texture1, thePlayer.primaryRect, colors[selectionInts[1]]);
                spriteBatch.Draw(thePlayer.texture2, thePlayer.secondaryRect, colors[selectionInts[2]]);
                spriteBatch.Draw(basicTex, thePlayer.passiveRect, colors[selectionInts[0]]);
                foreach (Bullet b in playerBullets)
                    spriteBatch.Draw(basicTex, b.bulletRect, colors[b.element]);
                //top layer
                if (theScreen != ScreenState.Start)
                {
                    //TODO: uncomment if adding more spec attacks
                    if (theScreen == ScreenState.Play) //TODO: Comment this out if adding more spec attacks
                    {
                        spriteBatch.Draw(basicTex, specialAttackBackground, Color.Gray);
                        spriteBatch.Draw(basicTex, specialAttackForeground, specColor);
                        //spriteBatch.Draw(basicTex, specialAttackForeground, colors[selectionInts[3]]);
                        spriteBatch.DrawString(font, specialAttackGUI, specialAttackGUIVec, Color.Black);
                    }
                }
                if (theScreen == ScreenState.Play)
                {
                    spriteBatch.Draw(basicTex, hitpointsBackground, Color.Gray);
                    spriteBatch.Draw(basicTex, hitpointsForeground, Color.Lime);
                    spriteBatch.DrawString(font3, hitpointsGUI+""+thePlayer.hitpoints, hitpointsGUIVec, Color.Black);
                    spriteBatch.DrawString(font3, scoreGUI + "" + score, scoreGUIVec, Color.White);
                    spriteBatch.Draw(basicTex, secondaryBackground, colors[selectionInts[1]]);
                    spriteBatch.Draw(basicTex, secondaryForeground, Color.Gray);
                    spriteBatch.DrawString(font, "Secondary", secondaryGUIVec, Color.Black);
                    spriteBatch.Draw(basicTex, tertiaryBackground, colors[selectionInts[2]]);
                    spriteBatch.Draw(basicTex, tertiaryForeground, Color.Gray);
                    spriteBatch.DrawString(font, "Tertiary", tertiaryGUIVec, Color.Black);
                }
            }
            else
            {
                spriteBatch.Draw(basicTex, cover, Color.Black);
                spriteBatch.DrawString(font2, pauseScreenText, pauseNotification, Color.White);
                spriteBatch.DrawString(font2, scoreGUI + "" + score, deathScoreVec, Color.White);
                if (thePlayer.hitpoints > 0)
                    spriteBatch.DrawString(font, INSTRUCTIONS, controls, Color.White);
                else
                {
                    string tempString = "";
                    foreach (string e in topFive)
                        tempString += e + "\n";
                    spriteBatch.DrawString(font, "Press 'Z' to play again.", controls, Color.White);
                    spriteBatch.DrawString(font, tempString, topFiveVec, Color.White);
                }

            }
            spriteBatch.Draw(basicTex, cursor1, Color.Orange);
            spriteBatch.Draw(basicTex, cursor2, Color.Orange);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}