#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using TestGameOne;
#endregion
namespace TestGameOne
{
    public class PostGameState : AIE.IGameState
    {
        private HighScoresObject m_highScoreList;
        private SoundEffect m_blipSound;
        private float m_blipVolume;
        private int m_playerScore;
        private int m_buttonSelected;
        private string m_playerName;
        private List<string> m_letterString;
        private List<int> m_letterInt;
        private Vector2 m_indicatorLeftPosition;
        private Vector2 m_indicatorRightPosition;
        
        private Texture2D m_heading;
        private Texture2D m_indicator;
        private Texture2D m_backgroundSprite;
        private Texture2D m_scoresText;
        private Texture2D m_nameText;
        private Texture2D m_acceptText;

        public PostGameState( int score ) : base()
        {
            m_heading = content.Load<Texture2D>("Images/GameOverTitle");
            m_backgroundSprite = content.Load<Texture2D>("Images/MenuBackground");
            m_indicator = content.Load<Texture2D>("Images/LeftSelector");
            m_scoresText = content.Load<Texture2D>("Images/ScoreText");
            m_nameText = content.Load<Texture2D>("Images/NameText");
            m_acceptText = content.Load<Texture2D>("Images/AcceptText");

            m_indicatorLeftPosition = new Vector2(412, 378);
            m_indicatorRightPosition = new Vector2(448, 378);

            m_blipSound = content.Load<SoundEffect>("Sounds/Blip");
            m_blipVolume = 0.1f;

            m_playerScore = score;
            m_highScoreList = new HighScoresObject();
            m_highScoreList.LoadHighScores();
            m_buttonSelected = 0;

            m_letterInt = new List<int>();
            m_letterInt.Add(0);
            m_letterInt.Add(0);
            m_letterInt.Add(0);

            m_letterString = new List<string>();
            m_letterString.Add("A");
            m_letterString.Add("A");
            m_letterString.Add("A");
        }

        public override void Update(GameTime gameTime)
        {
            #region GAMEPAD/BUTTON MOVEMENT
            if ((InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadLeft) || InputManager.InputManager.IsKeyJustPressed( Keys.Left) || (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickLeft) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0 && Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X) > Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y))))
            {
                m_blipSound.Play(m_blipVolume * Globals.m_volume, 0.75f, 0.75f);

                m_buttonSelected -= 1;

                m_indicatorLeftPosition.X -= 50;
                m_indicatorRightPosition.X -= 50;

                if (m_buttonSelected < 0)
                {
                    m_buttonSelected = 0;
                    m_indicatorLeftPosition.X += 50;
                    m_indicatorRightPosition.X += 50;
                }
            }

            else if (m_buttonSelected != 2 && (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadRight) || InputManager.InputManager.IsKeyJustPressed(Keys.Right) || (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickRight) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0 && Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X) > Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y)) || InputManager.InputManager.IsKeyJustPressed(Keys.Enter) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A)))
            {
                m_buttonSelected += 1;

                m_blipSound.Play(m_blipVolume * Globals.m_volume, 0.75f, 0.75f);

                m_indicatorLeftPosition.X += 50;
                m_indicatorRightPosition.X += 50;

                if (m_buttonSelected > 2)
                {
                    m_buttonSelected = 2;
                    m_indicatorLeftPosition.X -= 50;
                    m_indicatorRightPosition.X -= 50;
                }
            }

            else if (InputManager.InputManager.IsKeyJustPressed(Keys.Down) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadDown) || (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickDown) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0 && Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y) > Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X)))
            {
                m_letterInt[m_buttonSelected]++;

                if (m_letterInt[m_buttonSelected] > 25)
                    m_letterInt[m_buttonSelected] = 0;
            }

            else if (InputManager.InputManager.IsKeyJustPressed(Keys.Up) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadUp) || (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickUp) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0 && Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y) > Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X)))
            {
                m_letterInt[m_buttonSelected]--;

                if (m_letterInt[m_buttonSelected] < 0)
                    m_letterInt[m_buttonSelected] = 25;
            }


            //Button 2 is far right
            else if (m_buttonSelected == 2 && (InputManager.InputManager.IsKeyJustPressed(Keys.Enter) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A)))
            {
                m_blipSound.Play(m_blipVolume * Globals.m_volume, 0.75f, 0.75f);

                foreach (string letter in m_letterString)
                {
                    m_playerName += letter;
                }

                m_highScoreList.m_highScoresList.Insert(10, new HighScoresStruct(m_playerName, m_playerScore));
                m_highScoreList.SaveHighScores();
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.SetState("LEADERBOARD", new Leaderboards());
                AIE.GameStateManager.PushState("LEADERBOARD");
            }
            #endregion

            #region LETTER LOOPING
            for (int i = 0; i < m_letterInt.Count; i++)
            {
                if (m_letterInt[i] == 0)
                {
                    m_letterString[i] = "A";
                }

                else if (m_letterInt[i] == 1)
                {
                    m_letterString[i] = "B";
                }

                else if (m_letterInt[i] == 2)
                {
                    m_letterString[i] = "C";
                }

                else if (m_letterInt[i] == 3)
                {
                    m_letterString[i] = "D";
                }

                else if (m_letterInt[i] == 4)
                {
                    m_letterString[i] = "E";
                }

                else if (m_letterInt[i] == 5)
                {
                    m_letterString[i] = "F";
                }

                else if (m_letterInt[i] == 6)
                {
                    m_letterString[i] = "G";
                }

                else if (m_letterInt[i] == 7)
                {
                    m_letterString[i] = "H";
                }

                else if (m_letterInt[i] == 8)
                {
                    m_letterString[i] = "I";
                }

                else if (m_letterInt[i] == 9)
                {
                    m_letterString[i] = "J";
                }

                else if (m_letterInt[i] == 10)
                {
                    m_letterString[i] = "K";
                }

                else if (m_letterInt[i] == 11)
                {
                    m_letterString[i] = "L";
                }

                else if (m_letterInt[i] == 12)
                {
                    m_letterString[i] = "M";
                }

                else if (m_letterInt[i] == 13)
                {
                    m_letterString[i] = "N";
                }

                else if (m_letterInt[i] == 14)
                {
                    m_letterString[i] = "O";
                }

                else if (m_letterInt[i] == 15)
                {
                    m_letterString[i] = "P";
                }

                else if (m_letterInt[i] == 16)
                {
                    m_letterString[i] = "Q";
                }

                else if (m_letterInt[i] == 17)
                {
                    m_letterString[i] = "R";
                }

                else if (m_letterInt[i] == 18)
                {
                    m_letterString[i] = "S";
                }

                else if (m_letterInt[i] == 19)
                {
                    m_letterString[i] = "T";
                }

                else if (m_letterInt[i] == 20)
                {
                    m_letterString[i] = "U";
                }

                else if (m_letterInt[i] == 21)
                {
                    m_letterString[i] = "V";
                }

                else if (m_letterInt[i] == 22)
                {
                    m_letterString[i] = "W";
                }

                else if (m_letterInt[i] == 23)
                {
                    m_letterString[i] = "X";
                }

                else if (m_letterInt[i] == 24)
                {
                    m_letterString[i] = "Y";
                }

                else if (m_letterInt[i] == 25)
                {
                    m_letterString[i] = "Z";
                }
            }
#endregion

            
            #region MOVING BACKGROUND
            if (Globals.m_backgroundRectangle.X + 1 >= 960)
            {
                Globals.m_backgroundRectangle = new Rectangle(-960, 0, 960, 544);
            }

            else
            {
                Globals.m_backgroundRectangle = new Rectangle(Globals.m_backgroundRectangle.X + 1, 0, 960, 544);
            }

            if (Globals.m_backgroundRectangle2.X + 1 >= 960)
            {
                Globals.m_backgroundRectangle2 = new Rectangle(-960, 0, 960, 544);
            }

            else
            {
                Globals.m_backgroundRectangle2 = new Rectangle(Globals.m_backgroundRectangle2.X + 1, 0, 960, 544);
            }

            #endregion
                        
        }


        public override void Draw(GameTime gT, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(m_backgroundSprite, Globals.m_backgroundRectangle, Color.White);
            sb.Draw(m_backgroundSprite, Globals.m_backgroundRectangle2, Color.White);

            sb.Draw(m_heading, Vector2.Zero, Color.White);
            sb.Draw(m_scoresText, new Vector2(420, 140), Color.White);

            sb.DrawString(Globals.m_defaultFont, m_playerScore.ToString(), new Vector2(478, 213), Color.White, 0.0f, Globals.m_defaultFont.MeasureString(m_playerScore.ToString()) / 2, 1.0f, SpriteEffects.None, 0.0f);

            sb.Draw(m_nameText, new Vector2(420, 300), Color.White);

            sb.DrawString(Globals.m_defaultFont, m_letterString[0], new Vector2(420, 350), Color.White);
            sb.DrawString(Globals.m_defaultFont, m_letterString[1], new Vector2(470, 350), Color.White);
            sb.DrawString(Globals.m_defaultFont, m_letterString[2], new Vector2(520, 350), Color.White);

            sb.Draw(m_indicator, m_indicatorLeftPosition, new Rectangle(0, 0, 23, 58), Color.White, 0.0f, new Vector2(12, 29), 0.80f, SpriteEffects.None, 0.0f);
            sb.Draw(m_indicator, m_indicatorRightPosition, new Rectangle(0, 0, 23, 58), Color.White, (float)Math.PI, new Vector2(12, 29), 0.80f, SpriteEffects.None, 0.0f);

            if (m_buttonSelected == 2)
            {
                sb.Draw(m_acceptText, new Vector2(330, 430), Color.White);
            }

            sb.End();
        }

    }

    public struct HighScoresStruct
    {
        public string m_name;
        public int m_score;

        public HighScoresStruct(string name, int score)
        {
            m_name = name;
            m_score = score;
        }
    }

    public class HighScoresObject
    {
        public List<HighScoresStruct> m_highScoresList;

        public HighScoresObject()
        {
            m_highScoresList = new List<HighScoresStruct>();

            for (int i = 0; i < 11; i++)
            {
                m_highScoresList.Add( new HighScoresStruct("AAA",(i) * 10000));
            }

            m_highScoresList.Sort((score1, score2) => score2.m_score.CompareTo(score1.m_score));    
        }

        public void LoadHighScores()
        {
#if PSM
            if (File.Exists("Documents/HighScores.emu"))
            {
			    FileStream filestream = File.OpenRead("Documents/HighScores.emu");

                BinaryReader bin = new BinaryReader(filestream);

                m_highScoresList.Clear();

                for (int i = 0; i < 10; i++)
                {
                    m_highScoresList.Add(new HighScoresStruct(bin.ReadString(), bin.ReadInt32()));
                }

                m_highScoresList.Sort((score1, score2) => score2.m_score.CompareTo(score1.m_score));

                bin.Close();
            }
            
            else
            {
                SaveHighScores();
                LoadHighScores();
            }
#else
            if (File.Exists("HighScores.emu"))
            {
                FileStream filestream = File.OpenRead("HighScores.emu");

                BinaryReader bin = new BinaryReader(filestream);

                m_highScoresList.Clear();

                for (int i = 0; i < 10; i++)
                {
                    m_highScoresList.Add(new HighScoresStruct(bin.ReadString(), bin.ReadInt32()));
                }

                m_highScoresList.Sort((score1, score2) => score2.m_score.CompareTo(score1.m_score));

                bin.Close();
            }

            else
            {
                SaveHighScores();
                LoadHighScores();
            }
#endif
        }

        public void SaveHighScores()
        {

#if PSM
            FileStream filestream = File.OpenWrite("Documents/HighScores.emu");
#else
            FileStream filestream = File.OpenWrite("HighScores.emu");
#endif
            BinaryWriter bin = new BinaryWriter(filestream);

            m_highScoresList.Sort((score1, score2) => score2.m_score.CompareTo(score1.m_score));

            if (m_highScoresList.Count == 12)
                m_highScoresList.RemoveAt(11);

            foreach (HighScoresStruct entry in m_highScoresList)
            {
                bin.Write(entry.m_name);
                bin.Write(entry.m_score);
            }

            bin.Close();
        
        }
    }

}
