using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackEnd
{
	[Obsolete]
	public class Candidate
	{
		/// <summary>
		/// Times are in milliseconds Epoch
		/// </summary>
		public string Name { get; protected set; }
		public int ID { get; protected set; }

		public Candidate(string name, int ID)
		{
			this.Name = name;
			this.ID = ID;
		}
	}
}
