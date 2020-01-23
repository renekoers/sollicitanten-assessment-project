﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using BackEnd.Statements;

namespace BackEnd
{
    public class Api
    {
        /// <summary>
        /// Gives the state of the given level and the recommended numbers of lines to use.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>JSON consisting of the state</returns>
        public static string GetLevel(int level)
        {
            return Level.Get(level);
        }

        /// <summary>
        /// This method runs the given commands for the given level. Returns an arraylist of all the states.
        /// </summary>
        /// <param name="level">Current level.</param> 
        /// <param name="input">String array of commands.</param>
        /// <returns>Arraylist of all the states.</returns>
        public static string RunCommands(int level, Statement[] input)
        {
            string currentLevel = Level.Get(level);
            Puzzle puzzle = new Puzzle(currentLevel);
            JObject parsedLevel = JObject.Parse(currentLevel);
            List<string[][]> states = RunListOfStatements(puzzle, input);
            if (puzzle.IsFinished())
            {
                Console.WriteLine("User solved level " + level + " in " + CalculateScore(input) + " lines. Par is " + parsedLevel["Par"] + ".");
            }
            return @"{'States':" + JsonConvert.SerializeObject(states) + ", 'Finished': " + JsonConvert.SerializeObject(puzzle.IsFinished()) + ", 'Score': " + CalculateScore(input) + "}";

        }

        public static string RunCommands(int level, string[] input)
        {
            Statement[] commands = ConvertStringToSingleCommands(input);
            return RunCommands(level, commands);

        }
        /// <summary>
        /// This method runs the given commands for the given level. Returns an arraylist of all the states.
        /// </summary>
        /// <param name="level">Current level.</param> 
        /// <param name="input">String of commands seperated by ';'.</param>
        /// <returns>Arraylist of all the states.</returns>
        private static List<string[][]> RunListOfStatements(Puzzle puzzle, Statement[] input)
        {
            ICharacter character = puzzle.GetCharacter();

            List<string[][]> states = new List<string[][]>();

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
