using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;

namespace BackEnd
{
    public class Api
    {
        /// <summary>
        /// Gives the state of the given level and the recommended numbers of lines to use.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>JSON consisting of State (string[][]) and RecommendedLines (int)</returns>
        public static string GetLevel(int level)
        {
            string levelJSON = Levels.Get(level);
            Puzzle puzzle = new Puzzle(levelJSON);
            return @"{'State':" + JsonConvert.SerializeObject(puzzle.GetState()) + ", 'RecommendedLines': " + puzzle.GetParLines() + "}";
        }

        /// <summary>
        /// This method runs the given commands for the given level. Returns an arraylist of all the states.
        /// </summary>
        /// <param name="level">Current level.</param> 
        /// <param name="input">String of commands seperated by ';'.</param>
        /// <returns>Arraylist of all the states.</returns>
        public static string RunCommands(int level, string input)
        {
            Puzzle puzzle = new Puzzle(Levels.Get(level));
            List<string[][]> states = RunListOfCommands(puzzle, input);
            if (puzzle.IsFinished())
            {
                Console.WriteLine("User solved level " + level + " in " + input.Count(f => f == ';') + " lines. Par is " + puzzle.GetParLines() + ".");
            }
            return @"{'States':" + JsonConvert.SerializeObject(states) + ", 'Finished': " + JsonConvert.SerializeObject(puzzle.IsFinished()) + "}";

        }
        /// <summary>
        /// This method runs the given commands for the given level. Returns an arraylist of all the states.
        /// </summary>
        /// <param name="level">Current level.</param> 
        /// <param name="input">String of commands seperated by ';'.</param>
        /// <returns>Arraylist of all the states.</returns>
        private static List<string[][]> RunListOfCommands(Puzzle puzzle, string input)
        {
            ICharacter character = puzzle.GetCharacter();

            Command[] commands = ConvertStringToCommands(input.Trim());
            List<string[][]> states = new List<string[][]>();

            foreach (Command command in commands)
            {
                character.RunCommand(command);
                states.Add(puzzle.GetState());
            }
            return states;
        }
        /// <summary>
        /// Converts a string into an array of commands.
        /// </summary>
        /// <param name="input">String of commands seperated by ';'.</param>
        /// <returns>Array of commands.</returns>
        /// <exception cref="ArgumentException">Throws exception if the string is not a command.</exception>
        private static Command[] ConvertStringToCommands(string input)
        {
            string[] inputArray = input.Split(';');
            if (inputArray[inputArray.Length - 1] != "")
            {
                throw new ArgumentException("Last command did not end with ';'");
            }
            Command[] commands = new Command[inputArray.Length-1];
            for(int index=0; index<inputArray.Length-1; index++)
            {
                commands[index] = (Command)Enum.Parse(typeof(Command), inputArray[index].Trim());
            }
            return commands;
        }
    }
}
