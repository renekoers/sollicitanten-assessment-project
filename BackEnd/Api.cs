using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
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
            return Repository.CreateSession();
        }

        /// <summary>
        /// Obtain the session from a candidate. This session should at some point be stopped, and can be used to check the duration etc.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static GameSession GetSession(int ID)
        {
            return Repository.GetSession(ID);
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
            Repository.UpdateSession(ID, gameSession);

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
            Repository.UpdateSession(ID, gameSession);
            return solution;
        }

        public static void PauseLevelSession(int ID, int levelNumber)
        {
            GameSession gameSession = GetSession(ID);
            LevelSession levelSession = gameSession.GetSession(levelNumber);
            levelSession.Pause();
            Repository.UpdateSession(ID, gameSession);
        }
        public static IState ContinueLevelSession(int ID, int levelNumber)
        {
            GameSession gameSession = GetSession(ID);
            LevelSession levelSession = gameSession.GetSession(levelNumber);
            levelSession.Restart();
            Repository.UpdateSession(ID, gameSession);
            return new State(new Puzzle(Level.Get(levelNumber)));
        }

        public static void EndLevelSession(int ID, int levelNumber)
        {
            GameSession gameSession = GetSession(ID);
            LevelSession levelSession = gameSession.GetSession(levelNumber);
            levelSession.End();
            Repository.UpdateSession(ID, gameSession);
        }

        public static void EndSession(int ID)
        {
            GameSession gameSession = GetSession(ID);
            gameSession.End();
            Repository.UpdateSession(ID, gameSession);
        }
        public static bool LevelHasBeenStarted(int ID, int levelNumber)
        {
            GameSession gameSession = GetSession(ID);
            return gameSession.GetSession(levelNumber) != null;
        }

        public static int GetTotalLevelAmount()
        {
            int totalLevels = Level.TotalLevels;
            return totalLevels;
        }

        /// <summary>
        /// Gets the number of lines that a candidate needed to solve a specific level
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="levelNumber"></param>
        /// <returns></returns>
        public static int NumberOfLinesSolved(int ID, int levelNumber)
        {
            return GetSession(ID).GetSession(levelNumber).GetLeastLinesOfCodeSolution().Lines;
        }

        public static Dictionary<int, int> NumberOfLinesSolvedLevelsOf(int ID)
        {
            Dictionary<int, int> amountOfLinesByLevel = new Dictionary<int, int>();
            GameSession gameSession = GetSession(ID);
            foreach (int levelNumber in gameSession.SolvedLevelNumbers)
            {
                amountOfLinesByLevel.Add(levelNumber, NumberOfLinesSolved(ID, levelNumber));
            }
            return amountOfLinesByLevel;
        }

        /// <summary>
        /// Creates a dictionary containing only the solved levels of ID with tallies for the shortest code solution
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Dictionary<int, Dictionary<int, int>> TallyEveryoneNumberOfLinesSolvedLevelsOf(int ID)
        {
            GameSession gameSession = GetSession(ID);
            ISet<int> solvedLevels = gameSession.SolvedLevelNumbers;
            return TallyEveryoneNumberOfLines(solvedLevels);
        }

        /// <summary>
        /// Creates a dictionary labeled by level number with as entries the tallies for the shortest code solutions
        /// </summary>
        /// <param name="levelNumbers"></param>
        /// <returns></returns>
        public static Dictionary<int, Dictionary<int, int>> TallyEveryoneNumberOfLines(ISet<int> levelNumbers)
        {
            Dictionary<int, Dictionary<int, int>> talliesByLevel = new Dictionary<int, Dictionary<int, int>>();
            foreach (int levelNumber in levelNumbers)
            {
                talliesByLevel.Add(levelNumber, Repository.TallyEveryoneNumberOfLines(levelNumber));
            }
            return talliesByLevel;
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

        /// <summary>
        /// Used to insert some solutions of the levels to see the graphs
        /// </summary>
        private static void InsertMockData()
        {
            int id = StartSession();
            StartLevelSession(id, 1);
            SubmitSolution(id, 1, new Statement[]
            {
                new While(ConditionParameter.TileCurrent, ConditionValue.Finish, false, new Statement[]
                {
                    new SingleCommand(Command.RotateRight),
                    new SingleCommand(Command.PickUp),
                    new SingleCommand(Command.RotateLeft),
                    new SingleCommand(Command.RotateLeft),
                    new SingleCommand(Command.Drop),
                    new SingleCommand(Command.RotateRight),
                    new SingleCommand(Command.MoveForward)
                })
            });
            EndLevelSession(id, 1);
            StartLevelSession(id, 2);
            SubmitSolution(id, 2, new Statement[]
            {
                new While(ConditionParameter.TileLeft, ConditionValue.Impassable, true, new Statement[]
                {
                    new SingleCommand(Command.RotateLeft),
                    new SingleCommand(Command.PickUp),
                    new SingleCommand(Command.RotateRight),
                    new SingleCommand(Command.RotateRight),
                    new SingleCommand(Command.Drop),
                    new SingleCommand(Command.RotateLeft),
                    new SingleCommand(Command.MoveForward)
                }),
                new While(ConditionParameter.TileCurrent, ConditionValue.Finish, false, new Statement[]
                {
                    new While(ConditionParameter.TileFront, ConditionValue.Passable, true, new Statement[]
                    {
                        new SingleCommand(Command.MoveForward)
                    }),
                    new SingleCommand(Command.RotateRight)
                })
            });
            EndLevelSession(id, 2);
            EndSession(id);
            int idOther = StartSession();
            StartLevelSession(idOther, 1);
            SubmitSolution(idOther, 1, new Statement[]
            {
                new While(ConditionParameter.TileCurrent, ConditionValue.Finish, false, new Statement[]
                {
                    new SingleCommand(Command.RotateRight),
                    new SingleCommand(Command.PickUp),
                    new SingleCommand(Command.RotateLeft),
                    new SingleCommand(Command.RotateLeft),
                    new SingleCommand(Command.Drop),
                    new SingleCommand(Command.RotateRight),
                    new SingleCommand(Command.MoveForward),
                    new SingleCommand(Command.MoveBackward),
                    new SingleCommand(Command.MoveForward)
                })
            });
            EndLevelSession(idOther, 1);
            EndSession(idOther);
        }
        /// <summary>
        /// Receives the commands from the frontend and returns a LevelSolution
        /// </summary>
        /// <param name="input">String array of commands.</param>
        /// <returns>LevelSolution.</returns>
        public static LevelSolution ConvertAndSubmit(int ID, int levelnr, string[] input)
        {
            List<List<Statement>> totalStatements = new List<List<Statement>>();
            for (int i = 0; i < input.Length; i++)
            {
                totalStatements.Add(new List<Statement>());
            }
            Stack<Statement> st = new Stack<Statement>();
            string[] singleCommands = { "MoveForward", "RotateLeft", "RotateRight", "PickUp", "Drop" };
            //["MoveForward", "RotateLeft", "RotateRight", "PickUp","Drop",
            // "--While--","--If--","--Else--","--End--","TileFront","Passable","Button","Box"];
            int counter = 0;
            for (int i = 0; i < input.Length; i++)
            {
                var item = input[i];
                if (singleCommands.Any(item.Contains))
                {
                    totalStatements.ElementAt(counter).Add(new SingleCommand((Command)Enum.Parse(typeof(Command), item.Trim())));
                }
                else
                {
                    if (item.Equals("While"))
                    {
                        counter++;
                        Statement temp = new While(
                            (ConditionParameter)Enum.Parse(typeof(ConditionParameter), input[i + 1].Trim()),
                            (ConditionValue)Enum.Parse(typeof(ConditionValue), input[i + 2].Trim()),
                            true,
                            (Statement[])null
                            );
                        i = i + 2;
                        st.Push(temp);
                        totalStatements.ElementAt(counter).Add(temp);
                    }
                    else if (item.Equals("If"))
                    {
                        counter++;
                        Statement temp = new IfElse(
                            (ConditionParameter)Enum.Parse(typeof(ConditionParameter), input[i + 1].Trim()),
                            (ConditionValue)Enum.Parse(typeof(ConditionValue), input[i + 2].Trim()),
                            true,
                            (Statement[])null
                            );
                        i = i + 2;
                        st.Push(temp);
                        totalStatements.ElementAt(counter).Add(temp);
                    }
                    else if (item.Equals("End"))
                    {
                        int count = st.Count();
                        Statement endLoop = st.Pop();
                        if (endLoop is IfElse)
                        {
                            IfElse ifl = (IfElse) endLoop;
                            foreach (var sub in totalStatements.ElementAt(count))
                            {
                                if(sub is IfElse) continue;
                                else ifl.AddTrueStatement(sub);
                            }
                            totalStatements[count-1].Add(ifl);
                            totalStatements[count] = new List<Statement>();
                        }
                        else if (endLoop is While)
                        {
                            While wh = (While) endLoop;
                            foreach (var sub in totalStatements.ElementAt(count))
                            {
                                if(sub is While) continue;
                                else wh.AddStatement(sub);
                            }
                            totalStatements[count-1].Add(wh);
                            totalStatements[count] = new List<Statement>();
                        }
                        counter--;
                    }
                }
            }
            Statement[] statements = (Statement[])totalStatements.ElementAt(0).ToArray();
            return SubmitSolution(ID, levelnr, statements);
        }
    }
}
