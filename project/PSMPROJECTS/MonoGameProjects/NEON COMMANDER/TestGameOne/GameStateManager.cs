using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AIE
{
    public class IGameState
    {
        public IGameState()
        {
            content = AIE.GameStateManager.Game.Content;            
        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch ) { }

        public ContentManager content;
    }

    public static class GameStateManager
    {
        static Dictionary<string, IGameState>  m_gameStates;
        static List<IGameState>                m_stateStack;
        static List<GameStateEvents>           m_gameStateEvents;
        static Game                            m_game;
        static bool                            m_freezeUpdate;
        static bool                            m_freezeDraw;

        public static Game Game
        {
            get { return m_game; }            
        }

        enum GameStateCommands
        {
            CHANGE,
            PUSH,
            POP
        }

        struct GameStateEvents
        {
            public string name;
            public GameStateCommands cmd;
            public GameStateEvents(string name, GameStateCommands cmd)
            {
                this.name = name;
                this.cmd = cmd;
                
            }
        }

        // static constructor
        static GameStateManager()
        {
            m_gameStates        = new Dictionary<string, IGameState>();
            m_stateStack        = new List<IGameState>();
            m_gameStateEvents   = new List<GameStateEvents>();
            m_freezeUpdate      = false;
            m_freezeDraw        = false;
        }

        public static void Initialise(Game game)
        {
            m_game = game;
        }

        /// <summary>
        /// This function will ascoaciate a Game State Instance with a name.
        /// To reload a game state, call this function with a new instance of the GameState
        /// </summary>
        /// <param name="name">string to ascoaciate with the state</param>
        /// <param name="state">an instance of a game state object</param>
        public static void SetState(string name, IGameState state)
        {
            m_gameStates[name] = state;
        }

        /// <summary>
        /// Whatever state is currently on top of the stack will change to the new state on the next update
        /// </summary>
        /// <param name="name">The name ascoaciated with a game state instance... must have been previously defined via the SetState function</param>
        public static void ChangeState(string name)
        {
            m_gameStateEvents.Add(new GameStateEvents(name, GameStateCommands.CHANGE));
        }

        /// <summary>
        /// Will add a new state onto the game state stack
        /// this state will render on top of any other states that is currently on the stack
        /// Other states will continue to be updated and drawn as normal.
        /// </summary>
        /// <param name="name">The name ascoaciated with a game state instance... must have been previously defined via the SetState function</param>
        public static void PushState(string name)
        {
            m_gameStateEvents.Add(new GameStateEvents(name, GameStateCommands.PUSH));
        }

        /// <summary>
        /// the state on top of the stack will be removed
        /// </summary>
        public static void PopState()
        {
            m_gameStateEvents.Add(new GameStateEvents("", GameStateCommands.POP));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">the game state instance to check if is on top of the game state stack</param>
        /// <returns>True if the state is on top of the stack</returns>
        public static bool IsStateOnTop(IGameState state)
        {
            if (m_stateStack.Count == 0) return false;
            return m_stateStack[ m_stateStack.Count - 1 ] == state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The name associated with a game state instance... must have been previously defined via the SetState function</param>
        /// <returns></returns>
        public static bool IsStateOnTop(string name)
        {
            return IsStateOnTop(m_gameStates[name]);
        }

        /// <summary>
        /// will update all game states on the game state stack and process push, pop or change commands that has been called
        /// </summary>
        /// <param name="gameTime">Game time should be passed around all update functions </param>
        public static void UpdateGameStates(GameTime gameTime)
        {
            ProcessGameStateEvents();

            if (!m_freezeUpdate)
            {
                foreach (IGameState state in m_stateStack)
                    state.Update(gameTime);
            }

            else
            {
                IGameState state = m_stateStack[m_stateStack.Count - 1];
                state.Update(gameTime);
            }
        }

        /// <summary>
        /// will draw all games states on the game state stack.
        /// states on top of the stack will be drawn on top of previous states.
        /// </summary>
        /// <param name="gameTime">Game time should be passed around all draw functions </param>
        /// <param name="spriteBatch">the sprite batch object to preform 2D rendering</param>
        public static void DrawGameStates(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!m_freezeDraw)
            {
                foreach (IGameState state in m_stateStack)
                    state.Draw(gameTime, spriteBatch);
            }

            else
            {
                IGameState state = m_stateStack[m_stateStack.Count - 1];
                state.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Sets a boolean value to determine whether all states on the stack should be updated
        /// Or only the one that is currently on top. Good for pause screens.
        /// </summary>
        /// <param name="trueOrFalse"> If true, only the current state will be drawn. False means all states on the stack will be updated</param>
        public static void SetFreezeUpdate(bool trueOrFalse)
        {
            m_freezeUpdate = trueOrFalse;
        }

        /// <summary>
        /// Sets a boolean value to determine whether all states on the stack should be drawn
        /// Or only the one that is currently on top
        /// </summary>
        /// <param name="trueOrFalse"> If true, only the current state will be drawn. False means all states on the stack will be drawn</param>
        public static void SetFreezeDraw(bool trueOrFalse)
        {
            m_freezeDraw = trueOrFalse;
        }

        /// <summary>
        /// processes all recorded Push, Pop and Change events
        /// clears the event log for the next frame.
        /// Internal use only, called by the UpdateGameStates Function
        /// </summary>
        static void ProcessGameStateEvents()
        {
            foreach (GameStateEvents e in m_gameStateEvents)
            {
                if ( e.cmd == GameStateCommands.PUSH)
                {
                    if( m_gameStates.ContainsKey(e.name) == false ) continue;
                    m_stateStack.Add( m_gameStates[e.name] );
                }
                if (e.cmd == GameStateCommands.POP)
                {
                    if (m_stateStack.Count == 0) continue;
                    m_stateStack.RemoveAt(m_stateStack.Count - 1);
                }
                if (e.cmd == GameStateCommands.CHANGE)
                {
                    m_stateStack.RemoveAt(m_stateStack.Count - 1);
                    m_stateStack.Add(m_gameStates[e.name]);
                }
            }

            m_gameStateEvents.Clear();
        }
    }
}
