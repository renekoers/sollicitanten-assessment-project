﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BackEnd
{
    public class Api
    {
        /// <summary>
        /// Gives the state of the given level and the recommended numbers of lines to use.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>JSON consisting of the state</returns>
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
        private static List<IState> RunListOfStatements(Puzzle puzzle, Statement[] input)
        {
            ICharacter character = puzzle.Character;

            List<IState> states = new List<IState>();

            foreach (Statement statement in input)
            {
                states.AddRange(statement.ExecuteCommand(puzzle, character));
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
            for(int index=0; index< input.Length; index++)
            {
                commands[index] = new SingleCommand((Command)Enum.Parse(typeof(Command), input[index].Trim()));
            }
            return commands;
        }
        private static int CalculateScore(Statement[] input)
        {
            int lines = 0;
            foreach(Statement statement in input)
            {
                lines += statement.GetLines();
            }
            return lines;
        }
    }
}
