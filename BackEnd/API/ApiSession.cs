using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
        /// Submit a solution methods.
		public static IEnumerable<Statement> ParseStatementTreeJson(System.Text.Json.JsonElement statementTreeJson)
	   => StatementParser.ParseStatementTreeJson(statementTreeJson);
    }
}