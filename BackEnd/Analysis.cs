using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class Analysis
    {
        public int LinesOfCode
        {
            get
            {
                int lines = 0;
                foreach (Statement statement in _statements)
                {
                    lines += statement.GetLines();
                }
                return lines;
            }
        }

        private readonly List<Statement> _statements;
        public Analysis(List<Statement> statements)
        {
            _statements = statements;
        }


        /// <summary>
        /// Creates a dictionary consisting of all statistics of a candidate.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the data.</returns>
        public static Dictionary<int,Dictionary<string,int>> MakeStatisticsCandidate(string ID)
        {
            GameSession gameSession = Repository.GetSession(ID);
            ISet<int> solvedLevels = gameSession.SolvedLevelNumbers;
            Dictionary<string,Func<LevelSession,int>> statisticsFunctions = GetStatisticsFunctions();
            Dictionary<int,Dictionary<string,int>> dataCandidate = new Dictionary<int,Dictionary<string,int>>();
            foreach (int levelNumber in solvedLevels)
            {
                LevelSession levelSession = gameSession.GetSession(levelNumber);
                Dictionary<string,int> dataSingleLevel = new Dictionary<string, int>();
                foreach(KeyValuePair<string,Func<LevelSession,int>> nameDataAndFunctionToIntPair in statisticsFunctions){
                    dataSingleLevel.Add(nameDataAndFunctionToIntPair.Key, nameDataAndFunctionToIntPair.Value(levelSession));
                }
                dataCandidate.Add(levelNumber,dataSingleLevel);
                
            }
            return dataCandidate;
        }
        /// <summary>
        /// Creates a dictionary consisting of all statistics of all candidates.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the combination of data and number of candidates.</returns>
        public static Dictionary<int,Dictionary<string, Dictionary<int, int>>> MakeStatisticsEveryone()
        {
            Dictionary<string,Func<LevelSession,int>> statisticsFunctions = GetStatisticsFunctions();
            Dictionary<int,Dictionary<string, Dictionary<int, int>>> dataEveryone = new Dictionary<int,Dictionary<string, Dictionary<int, int>>>();
            for (int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
            {
                Dictionary<string,Dictionary<int, int>> dataSingleLevel = new Dictionary<string, Dictionary<int, int>>();
                foreach(KeyValuePair<string,Func<LevelSession,int>> nameDataAndFunctionToIntPair in statisticsFunctions){
                    dataSingleLevel.Add(nameDataAndFunctionToIntPair.Key, Repository.TallyEveryone(levelNumber, nameDataAndFunctionToIntPair.Value));
                }
                dataEveryone.Add(levelNumber, dataSingleLevel);
            }
            return dataEveryone;
        }
        /// <summary>
        /// Creates a list of functions that maps a levelSession to an int. This function are used in constructing dictionaries that represents the statistics.
        /// Add here extra functions in order to add extra statistics.
        /// </summary>
        /// <returns>Dictionary with for name of the statistic creates data that represents the statistic of the given level.</returns>
        private static Dictionary<string,Func<LevelSession,int>> GetStatisticsFunctions()
        {
            Dictionary<string,Func<LevelSession,int>> statisticsFunctions = new Dictionary<string,Func<LevelSession, int>>();
            statisticsFunctions.Add("Regels code kortste oplossing", LevelSession.GetLines);
            statisticsFunctions.Add("Tijd tot korste oplossing", LevelSession.GetDuration);
            statisticsFunctions.Add("Pogingen tot korste oplossing", session => session.NumberOfAttemptsForFirstSolved);
            return statisticsFunctions;
        }
    }
}
