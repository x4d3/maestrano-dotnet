using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Tests.Connec
{
    [TestClass]
    public class ClientTest
    {
        private String groupId { get; set; }
        public Maestrano.Connec.Client client { get; set; }

        public ClientTest()
        {
            MnoHelper.Environment = "test";
            MnoHelper.Api.Id = "app-1";
            MnoHelper.Api.Key = "gfcmbu8269wyi0hjazk4t7o1sndpvrqxl53e1";
            groupId = "cld-3";
            client = new Maestrano.Connec.Client(this.groupId);
        }

        [TestMethod]
        public void Get_onCollection_itReturnsTheList()
        {
            RestResponse  resp = this.client.Get("/organizations");
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, object>>(resp.Content);

            Assert.IsNotNull(parsed["organizations"]);
        }

        [TestMethod]
        public void Get_onCollection_withType_itReturnsTheDeserializedResponse()
        {
            RestResponse<Dictionary<string, string>> resp = this.client.Get<Dictionary<string, string>>("/organizations");

            Assert.IsNotNull(resp.Data["organizations"]);
        }

        [TestMethod]
        public void Post_itCreatesANewEntity()
        {
            var body = new Dictionary<string, Dictionary<string, string>>();
            var entity = new Dictionary<string, string>();
            entity.Add("name", "Doe Corp Inc.");
            body.Add("organizations", entity);
            var rawBody = JsonConvert.SerializeObject(body);

            var resp = this.client.Post("/organizations", rawBody);
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(resp.Content);
            
            Assert.IsNotNull(parsed["organizations"]);
            Assert.IsNotNull(parsed["organizations"]["id"]);
        }

        [TestMethod]
        public void Put_itUpdatesAnEntity()
        {
            // Create entity
            var body = new Dictionary<string, Dictionary<string, string>>();
            var entity = new Dictionary<string, string>();
            entity.Add("name", "Jazz Corp Inc.");
            body.Add("organizations", entity);
            var rawBody = JsonConvert.SerializeObject(body);

            var resp = this.client.Post("/organizations", rawBody);
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(resp.Content);
            var id = parsed["organizations"]["id"];

            // Update entity
            var update = new Dictionary<string, bool>();
            update.Add("is_customer", true);
            var newBody = new Dictionary<string, Dictionary<string, bool>>();
            newBody.Add("organizations", update);
            rawBody = JsonConvert.SerializeObject(newBody);

            resp = this.client.Put("/organizations/" + id, rawBody);
            parsed = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(resp.Content);

            Assert.IsNotNull(parsed["organizations"]);
            Assert.AreEqual(id, parsed["organizations"]["id"]);
            Assert.AreEqual("True",parsed["organizations"]["is_customer"]);
        }
    }
}
