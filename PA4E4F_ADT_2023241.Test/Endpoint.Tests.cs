using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using PA4E4F_ADT_2023241.Endpoint;
using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Logic;
using PA4E4F_ADT_2023241.Repository;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PA4E4F_ADT_2023241.Test
{
    internal class Endpoint
    {
        Mock<Microsoft.AspNetCore.Http.HttpContext> MockContext;
        Mock<Microsoft.AspNetCore.Http.HttpContext> MockContext_BadJson;

        Stream AcceptableStream;
        Stream UnacceptableStream;

        Mock<ILogicFactory> MockedLogicFactory;

        [SetUp]
        public void Setup()
        {
            MockContext = new Mock<HttpContext>();
            MockContext_BadJson = new Mock<HttpContext>();

            AcceptableStream = new MemoryStream();
            UnacceptableStream = new MemoryStream();

            MockedLogicFactory = new Mock<ILogicFactory>();

            using (StreamWriter sw = new StreamWriter(AcceptableStream))
            {
                sw.Write(JsonConvert.SerializeObject(new Student
                {
                    Name = "Testable",
                    Id = 1
                }));
            }

            using (StreamWriter sw = new StreamWriter(UnacceptableStream))
            {
                sw.Write("  {\r\n    \"IdErr\": \"1000\",\r\n    \"NameErr\": \"Unacceptable\"\r\n  }");
            }

            MockContext_BadJson.Setup(x => x.Request.Body).Returns(UnacceptableStream);
            MockContext_BadJson.Setup(x => x.Response.StatusCode).Returns(1);

            MockedLogicFactory.Setup(x => x.CreateStudentLogic()).Returns(new Mock<IStudentLogic>().Object);

            MockContext.Setup(x => x.Request.Body).Returns(AcceptableStream);
            MockContext.Setup(x => x.Response.StatusCode).Returns(1);
        }

        // E1
        [Test]
        public void ObjectCreatorDeserializesDataCorrectly()
        {
            ObjectCreator.CreateObject<Student>(MockContext.Object, 112, MockedLogicFactory.Object);

            MockContext.Verify();

            if (MockContext.Object.Response.StatusCode == 200)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        // E2
        public void ObjectUpdaterDeserializesDataCorrectly()
        {
            ObjectUpdater.UpdateObject<Student>(MockContext.Object, 112, MockedLogicFactory.Object);

            MockContext.Verify();

            if (MockContext.Object.Response.StatusCode == 200)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        // E3
        public void GraderDeserializesDataCorrectly()
        {
            Grader.Grade(MockContext.Object, 1, 1, 1, MockedLogicFactory.Object);

            MockContext.Verify();

            if (MockContext.Object.Response.StatusCode == 200)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        // E4
        [Test]
        public void ObjectCreatorHandleInvalidInput()
        {
            ObjectCreator.CreateObject<Student>(MockContext_BadJson.Object, 112, MockedLogicFactory.Object);

            MockContext_BadJson.Verify();

            if (MockContext_BadJson.Object.Response.StatusCode == 500)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        // E5
        public void ObjectUpdaterHandleInvalidInput()
        {
            ObjectUpdater.UpdateObject<Student>(MockContext_BadJson.Object, 112, MockedLogicFactory.Object);

            MockContext_BadJson.Verify();

            if (MockContext_BadJson.Object.Response.StatusCode == 500)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        // E6
        public void GraderHandleInvalidInput()
        {
            Grader.Grade(MockContext_BadJson.Object, 1, 1, 1, MockedLogicFactory.Object);

            MockContext_BadJson.Verify();

            if (MockContext_BadJson.Object.Response.StatusCode == 500)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }
    }
}
