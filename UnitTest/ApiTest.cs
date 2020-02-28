// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using BackEnd;

// namespace UnitTest
// {
// 	[TestClass]
// 	public class ApiTest
// 	{
// 		[TestMethod]
// 		public void GetLevelNumberTest()
// 		{
// 			string ID = Api.StartSession();
// 			IState level = Api.StartLevelSession(ID, 1);
// 			Assert.AreEqual(1, level.PuzzleLevel);
// 		}

// 		[TestMethod]
// 		public void AddCandidateTest()
// 		{
// 			Api.AddCandidate("Test kandidaat");
// 			CandidateEntity candidate = Api.GetCandidate();
// 			Api.StartSession(candidate.ID);
// 			Assert.AreEqual("Test kandidaat", candidate.Name);
// 		}
// 		[TestMethod]
// 		public void GetCandidateWithoutAnyCandidateIsNullTest()
// 		{
// 			Assert.IsNull(Api.GetCandidate());
// 		}
// 		[TestMethod]
// 		public void GetCandidateWithoutFreeSessionIsNullTest()
// 		{
// 			Api.AddCandidate("Test kandidaat 3");
// 			Api.StartSession(Api.GetCandidate().ID);
// 			Assert.IsNull(Api.GetCandidate());
// 		}
// 	}
// }
