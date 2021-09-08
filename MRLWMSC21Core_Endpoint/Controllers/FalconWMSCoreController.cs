using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MRLWMSC21Core.Business;
using MRLWMSC21Core.DataAccess;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.Library;
using MRLWMSC21_Endpoint.Security;

namespace MRLWMSC21_Endpoint.Controllers
{
    [RoutePrefix("Core")]
    public class MRLWMSC21CoreController : ApiController//, Interfaces.iTestInterface
    {
        private string _ClassCode = string.Empty;

        public MRLWMSC21CoreController()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.MRLWMSC21CoreController);
        }


        [Route("Test/{Hello}")]
        [HttpGet]
        public IHttpActionResult HelloWorld(string Hello)
        {
            // int Hello = 1;
            return Ok("Hello World " + Hello.ToString());
        }

        [Route("Hello/{Num}")]
        [HttpGet]
        public string HelloRequest(int Num)
        {
            return "Hello World sfdasdf " + Num.ToString();
        }


        [Route("Hello/{Num1}/{Num2}")]
        [HttpGet]
        public string HelloRequest(int Num1, int Num2)
        {
            return "Hello World sfdasdf " + Num1.ToString() + " 2: " + Num2.ToString();
        }

        [Route("HelloWorld")]
        [HttpPost]
        public string PostObject(TestDTO oTestDTO)
        {
            return "Hello World sfdasdf " + oTestDTO.HelloWorld.ToString() + " :::: Helo : " + oTestDTO.IntegerValue.ToString();
        }

        [Route("HelloTest")]
        [HttpPost]
        public List<string> PostMyObject(TestDTO oTestDTO)
        {
            List<string> lString = new List<string>();

            lString.Add("Hello");
            lString.Add("Hi");
            lString.Add(oTestDTO.HelloWorld.ToString());
            lString.Add(oTestDTO.IntegerValue.ToString());

            return lString;
        }


        [Route("ReturnListObjects")]
        [HttpPost]
        public List<TestDTO> PostTestListObject(TestDTO oTestDTO)
        {
            List<TestDTO> lString = new List<TestDTO>();

            lString.Add(new TestDTO() { HelloWorld = "HELLLLLLOOOO", IntegerValue = 123 });
            lString.Add(new TestDTO() { HelloWorld = oTestDTO.HelloWorld, IntegerValue = oTestDTO.IntegerValue });

            return lString;
        }


        //[HttpGet]
        //[Route("TestParam")]
        //public string HelloWorld1 (int hi)

        //{
        //    return "Hello World ";

        //}


        [HttpGet]
        [Route("Test")]
        public string HelloWorld(int id)
        {
            return "Hello World ";

        }

        //[HttpPost]
        //[Route("UserLogin")]
        //public UserProfile UserLogin(UserLogin oUserr)
        //{
        //    LoginBL oLogin = new LoginBL();
        //    UserProfile oUserProfile = new UserProfile()
        //    {
        //        EMailID = oUserr.EmailID.ToString(),
        //        Password = oUserr.Password

        //    };

        //    oUserProfile = oLogin.UserLogin(oUserProfile);
        //    //oUserProfile = oLogin.ValidateUserSession(oUserProfile);

        //    return oUserProfile;
        //}




    }

    public class TestDTO
    {
        public string HelloWorld;
        public object IntegerValue;

    }

}
