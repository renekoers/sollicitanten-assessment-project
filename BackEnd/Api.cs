using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace BackEnd
{
    public class Api
    {
        public static bool ValidateUser(string username, string hashedPass)
        {
            return Repository.ValidateUser(username, hashedPass);
        }
        public static void AddCandidate(string name)
        {
            Repository.AddCandidate(name);
        }
        public static Candidate GetCandidate()
        {
            return Repository.GetCandidate();
        }
        public static Candidate GetCandidate(int ID)
        {
            return Repository.GetCandidate(ID);
        }
        public static bool IsUnstarted(int ID)
        {
            return Repository.IsUnstarted(ID);
        }
        /// <summary>
        /// Start a new session for a new candidate.!-- This method is used in mockdata and tests!!!!
        /// </summary>
        /// <returns>The ID of the newly created candidate used to access their session</returns>
        [Obsolete("Use StartSession(int ID) instead")]
        public static int StartSession()
        {
            return Repository.CreateSession();
        }
        public static bool StartSession(int ID)
        {
            return Repository.StartSession(ID);
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
        public static bool IsStarted(int ID)
        {
            GameSession gameSession = Api.GetSession(ID);
            if(gameSession==null){
                return false;
            }
            return gameSession.InProgress;
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
        public static bool LevelIsSolved(int ID, int levelNumber)
        {
            GameSession gameSession = GetSession(ID);
            LevelSession levelSession = gameSession.GetSession(levelNumber);
            if(levelSession == null)
            {
                return false;
            }
            return levelSession.Solved;
        }

        public static Overview GetOverview(int ID)
        {
            return new Overview(GetSession(ID));
        }

        public static int GetTotalLevelAmount()
        {
            int totalLevels = Level.TotalLevels;
            return totalLevels;
        }
        /// <summary>
        /// This method creates a list of all IDs of candidates that finished a session after a given time
        /// </summary>
        /// <returns>List of IDs</returns>
        public static List<int> GetFinishedIDsAfterEpochTime(long epochTime)
        {
            return Repository.GetFinishedIDsAfterEpochTime(epochTime);
        }
        /// <summary>
        /// This method finds the first ID of the candidate that ended the session after the given ID.
        /// </summary>
        /// <returns>ID if there exists one</returns>
        public static int? GetNextIDWhichIsFinished(int ID)
        {
            return Repository.GetNextIDWhichIsFinished(ID);
        }
        /// <summary>
        /// This method finds the last ID of the candidate that ended the session before the given ID.
        /// </summary>
        /// <returns>ID if there exists one</returns>
        public static int? GetPreviousIDWhichIsFinished(int ID)
        {
            return Repository.GetPreviousIDWhichIsFinished(ID);
        }
        public static int? GetLastIDWhichIsFinished()
        {
            return Repository.GetLastIDWhichIsFinished();
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

        public static long GetEpochTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
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

        public static IEnumerable<Statement> ParseStatementTreeJson(System.Text.Json.JsonElement statementTreeJson)
            => StatementParser.ParseStatementTreeJson(statementTreeJson);

        /// <summary>
        /// Receives the commands from the frontend and returns a LevelSolution.!-- This method needs to be moved to a different class after draggables are implemented!!!!!!
        /// </summary>
        /// <param name="input">String array of commands.</param>
        /// <returns>LevelSolution.</returns>
        [Obsolete("Use Api.ParseStatementTreeJson(System.Text.Json.JsonElement) instead.")]
        public static LevelSolution ConvertAndSubmit(int ID, int levelnr, string[] input)
        {
            Console.WriteLine("This method needs to be moved to a different class after draggables are implemented!!!!!!");
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
