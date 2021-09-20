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
        public const string Path = @"C:\VisualStudioProjects\Test\data.json";

        // GET api/values
        public List<Transaction> Get()
        {
            return ReadTransactions(Path);
        }

        // GET api/values/5
        public Transaction Get(string id)
        {
            return ReadTransactions(Path).Where(t => t.Id == id).FirstOrDefault(); // AJA note: consider doing something if FirstOrDefault returns null
        }

        // POST api/values
        public void Post([FromBody]Transaction value)
        {
            // AJA note: avoid reading and rewriting entire file as explained here - https://stackoverflow.com/questions/43529386/append-data-in-a-json-file-in-c-sharp/43529523
            var transactions = ReadTransactions(Path);
            transactions.Add(value);
            WriteTransactions(transactions);
        }

        // PUT api/values/5
        public void Put(string id, [FromBody]Transaction value)
        {
            var transactions = ReadTransactions(Path);

            // AJA note: opted for foreach instead of linq and lambda as it looks cleaner on this occasion
            foreach (var t in transactions.Where(t => t.Id == value.Id))
            {
                t.ApplicationId = value.ApplicationId;
                t.Type = value.Type;
                t.Summary = value.Summary;
                t.Amount = value.Amount;
                t.PostingDate = value.PostingDate;
                t.IsCleared = value.IsCleared;
                t.ClearedDate = value.ClearedDate;
            }
            WriteTransactions(transactions);
        }

        // DELETE api/values/5
        public void Delete(string id)
        {
            var transactions = ReadTransactions(Path);
            var transaction = transactions.Find(t => t.Id == id);
            transactions.Remove(transaction);
            WriteTransactions(transactions);
        }

        public List<Transaction> ReadTransactions(string path)
        {
            string json = File.ReadAllText(path);
            var transactionList = JsonConvert.DeserializeObject<List<Transaction>>(json);
            return transactionList;
        }

        private void WriteTransactions(List<Transaction> transactions)
        {
            string json = JsonConvert.SerializeObject(transactions);
            File.WriteAllText(Path, json);
        }
    }
}
