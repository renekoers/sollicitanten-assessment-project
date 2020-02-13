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
        public static Dictionary<int,Dictionary<string,int>> StatisticsCandidate(int ID)
        {
            return null;
        }
        /// <summary>
        /// Creates a dictionary consisting of all statistics of a candidate.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the combination of data and number of candidates.</returns>
        public static Dictionary<int,Dictionary<string, Dictionary<int, int>>> StatisticsEveryone(int ID)
        {
            return null;
        }
        private static Dictionary<string,T> 
    }
}
