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


namespace SpaceGame
{

    public enum State
    {
        MainMenu,
        GamePlay,
        GameOver,
        ChangeLevel,
        Winner
    }

    public class GameState
    {
        public State currentState = State.MainMenu;
        public int total = 0;
        public int TotalGameState
        {
            get
            {
                return total;
            }
            set
            {
            }

        }

        public GameState()
        {
            //Debug state defaulted.
            //currentState = State.GamePlay;
        }

        public void ChangeState(string inputState)
        {
            int previousGameState = 0;
            TotalGameState += previousGameState;

            switch (inputState)
            {
                case "MainMenu": currentState = State.MainMenu;
                    previousGameState = 1;
                    break;
                case "GamePlay": currentState = State.GamePlay;
                    break;
                case "GameOver": currentState = State.GameOver;
                    previousGameState = 3;
                    break;
                case "ChangeLevel": currentState = State.ChangeLevel;
                    break;
                case "Winner": currentState = State.Winner;
                    break;
            }
        }





    }
}
