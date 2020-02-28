using React_Frontend.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using JSonWebToken;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using BackEnd;

namespace UnitTest {

    [TestClass]
    public class HRControllerTest{

        private HRController hrController = new HRController(new TestDB());
        
        [TestMethod]
        public async Task LoginTest(){
            JsonElement credentials = await (JsonElement) JSON.Serialize("\"username\":\"testUsername\",\"password\":\"testPassword\"");
            ActionResult<string> response = hrController.Login(credentials);
            // string token = JWT.CreateToken("testUsername", "testPassword");
            // ActionResult<string> msg = JSON.Serialize("Bearer " + token);

            Assert.AreEqual(response, response);
        }

        [TestMethod]
        public async Task AddCandidateSuccess() {
            var response = await hrController.AddCandidate("testadd");
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

         [TestMethod]
        public async Task AddCandidateEmptyStringAsName() {
            var response = await hrController.AddCandidate("");
            Assert.IsInstanceOfType(response, typeof(UnprocessableEntityResult));
        }

         [TestMethod]
        public async Task AddCandidateNameIsNull() {
            var response = await hrController.AddCandidate(null);
            Assert.IsInstanceOfType(response, typeof(UnprocessableEntityResult));
        }

        
    }
}