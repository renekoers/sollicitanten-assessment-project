﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BackEnd
{
    public class Api
    {
        /// <summary>
        /// Start a new session for a new candidate
        /// </summary>
        /// <returns>The ID of the newly created candidate used to access their session</returns>
        public static int StartSession()
        {
            return DatabaseCommunicator.CreateSession();
        }

        /// <summary>
        /// Obtain the session from a candidate. This session should at some point be stopped, and can be used to check the duration etc.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static GameSession GetSession(int ID)
        {
            return DatabaseCommunicator.GetSession(ID);
        }

        /// <summary>
        /// Begin a new level session.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="levelNumber"></param>
        /// <returns></returns>
        public static IState StartLevelSession(int ID, int levelNumber)
        {
            GameSession gameSession = GetSession(ID);
            LevelSession levelSession = new LevelSession(levelNumber);
            gameSession.AddLevel(levelSession);
            DatabaseCommunicator.UpdateSession(ID, gameSession);

            return new State(new Puzzle(Level.Get(levelNumber)));
        }

        /// <summary>
        /// Submit a new solution attempt
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="levelNumber"></param>
        /// <param name="statements"></param>
        /// <returns>A LevelSolution object which contains amongst other things a list of IState objects and whether the level was solved or not</returns>
        public static LevelSolution SubmitSolution(int ID, int levelNumber, Statement[] statements)
        {
            GameSession gameSession = GetSession(ID);
            LevelSession levelSession = gameSession.GetSession(levelNumber);
            LevelSolution solution = new LevelSolution(levelNumber, statements);
            levelSession.Attempt(solution);
            DatabaseCommunicator.UpdateSession(ID, gameSession);
            return solution;
        }

        public static void EndLevelSession(int ID, int levelNumber)
        {
            GameSession gameSession = GetSession(ID);
            LevelSession levelSession = gameSession.GetSession(levelNumber);
            levelSession.End();
            DatabaseCommunicator.UpdateSession(ID, gameSession);
        }

        public static void EndSession(int ID)
        {
            GameSession gameSession = GetSession(ID);
            gameSession.End();
            DatabaseCommunicator.UpdateSession(ID, gameSession);
        }

        /// <summary>
        /// Gives the state of the given level and the recommended numbers of lines to use.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>JSON consisting of the state</returns>
        [Obsolete("Use StartLevelSession instead")]
        public static IState GetLevel(int level)
        {
            return new State(new Puzzle(Level.Get(level)));
        }

        /// <summary>
        /// This method runs the given commands for the given level. Returns an arraylist of all the states.
        /// </summary>
        /// <param name="level">Current level.</param> 
        /// <param name="input">String array of commands.</param>
        /// <returns>Arraylist of all the states.</returns>
        [Obsolete("Use SubmitSolution instead")]
        public static List<IState> RunCommands(int level, Statement[] input)
        {
            Level currentLevel = Level.Get(level);
            Puzzle puzzle = new Puzzle(currentLevel);
            List<IState> states = RunListOfStatements(puzzle, input);
            if (puzzle.Finished)
            {
                Console.WriteLine("User solved level " + level + " in " + CalculateScore(input) + " lines. Par is " + currentLevel.Par + ".");
            }
            return states;
        }

        [Obsolete]
        public static List<IState> RunCommands(int level, string[] input)
        {
            Statement[] commands = ConvertStringToSingleCommands(input);
            return RunCommands(level, commands);

        }

        public static long GetEpochTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// This method runs the given commands for the given level. Returns an arraylist of all the states.
        /// </summary>
        /// <param name="level">Current level.</param> 
        /// <param name="input">String of commands seperated by ';'.</param>
        /// <returns>Arraylist of all the states.</returns>
        [Obsolete]
        private static List<IState> RunListOfStatements(Puzzle puzzle, Statement[] input)
        {
            List<IState> states = new List<IState>();

            foreach (Statement statement in input)
            {
                states.AddRange(statement.ExecuteCommand(puzzle));
            }
            return states;
        }
        /// <summary>
        /// Converts a string into an array of commands.
        /// </summary>
        /// <param name="input">String of commands seperated by ';'.</param>
        /// <returns>Array of commands.</returns>
        /// <exception cref="ArgumentException">Throws exception if the string is not a command.</exception>
        private static SingleCommand[] ConvertStringToSingleCommands(string[] input)
        {
            SingleCommand[] commands = new SingleCommand[input.Length];
            for (int index = 0; index < input.Length; index++)
            {
                commands[index] = new SingleCommand((Command)Enum.Parse(typeof(Command), input[index].Trim()));
            }
            return commands;
        }
        
        private static int CalculateScore(Statement[] input)
        {
            int lines = 0;
            foreach (Statement statement in input)
            {
                lines += statement.GetLines();
            }
            return lines;
        }
    }
}
