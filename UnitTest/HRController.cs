using React_Frontend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System;
using System.Text.Json;
using System.Threading.Tasks;



namespace UnitTest {

    [TestClass]
    public class HRControllerTest{

        private HRController hrController = new HRController(new TestDB());
        
        [TestMethod]
        public void LoginSuccesTest(){            
            
            string creds = "{\"username\":\"testUsername\",\"password\":\"testPassword\"}";
            JsonDocument jdoc = JsonDocument.Parse(creds);
            JsonElement credentials = jdoc.RootElement;
            ActionResult<string> response = hrController.Login((JsonElement) credentials);

            Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void LoginNoUsernameTest(){            
            
            string creds = "{\"username\":\"\",\"password\":\"testPassword\"}";
            JsonDocument jdoc = JsonDocument.Parse(creds);
            JsonElement credentials = jdoc.RootElement;
            ActionResult<string> response = hrController.Login((JsonElement) credentials);

            Assert.IsInstanceOfType(response.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void LoginNoPasswordTest(){            
            
            string creds = "{\"username\":\"testUsername\",\"password\":\"\"}";
            JsonDocument jdoc = JsonDocument.Parse(creds);
            JsonElement credentials = jdoc.RootElement;
            ActionResult<string> response = hrController.Login((JsonElement) credentials);

            Assert.IsInstanceOfType(response.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task AddCandidateSuccess() {
            ActionResult<string> response = await hrController.AddCandidate("testadd");
            Assert.AreEqual(((StatusCodeResult) response.Result).StatusCode, 200);
        }

         [TestMethod]
        public async Task AddCandidateEmptyStringAsName() {
            ActionResult<string> response = await hrController.AddCandidate("");
            Assert.AreEqual(((StatusCodeResult) response.Result).StatusCode, 422);
        }

         [TestMethod]
        public async Task AddCandidateNameIsNull() {
            ActionResult<string> response = await hrController.AddCandidate(null);
            Assert.AreEqual(((StatusCodeResult) response.Result).StatusCode, 422);
        }

        
    }
}