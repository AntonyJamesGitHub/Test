using Interview.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Interview.Controllers
{
    public class ValuesController : ApiController
    {
        //Used Google Chrome extension "ReqBin HTTP Client" to test
        
        // GET api/values
        public List<Transaction> Get()
        {
            return GetTransactions();
        }

        // GET api/values/5
        public Transaction Get(string id)
        {
            return GetTransactions().Where(t => t.Id == id).FirstOrDefault(); //TODO: do something if FirstOrDefault returns null?
        }

        // POST api/values
        public void Post([FromBody]Transaction value)
        {
            //TODO: avoid reading and rewriting entire file as explained here - https://stackoverflow.com/questions/43529386/append-data-in-a-json-file-in-c-sharp/43529523

            var transactionList = GetTransactions();
            transactionList.Add(value);
            string json = JsonConvert.SerializeObject(transactionList);
            File.WriteAllText(@"C:\VisualStudioProjects\Test\data.json", json);
        }

        // PUT api/values/5
        public void Put(string id, [FromBody]Transaction value)
        {
            var transactionList = GetTransactions();

            //Opted for foreach instead of linq and lambda as it looks cleaner on this occasion
            foreach (var t in transactionList.Where(t => t.Id == value.Id))
            {
                t.ApplicationId = value.ApplicationId;
                t.Type = value.Type;
                t.Summary = value.Summary;
                t.Amount = value.Amount;
                t.PostingDate = value.PostingDate;
                t.IsCleared = value.IsCleared;
                t.ClearedDate = value.ClearedDate;
            }
            string json = JsonConvert.SerializeObject(transactionList);
            File.WriteAllText(@"C:\VisualStudioProjects\Test\data.json", json);
        }

        // DELETE api/values/5
        public void Delete(string id)
        {
            var transactionList = GetTransactions();
            var transaction = transactionList.Find(t => t.Id == id);
            transactionList.Remove(transaction);
            string json = JsonConvert.SerializeObject(transactionList);
            File.WriteAllText(@"C:\VisualStudioProjects\Test\data.json", json);
        }

        private List<Transaction> GetTransactions()
        {
            string json = File.ReadAllText(@"C:\VisualStudioProjects\Test\data.json");
            var transactionList = JsonConvert.DeserializeObject<List<Transaction>>(json);
            return transactionList;
        }
    }
}
