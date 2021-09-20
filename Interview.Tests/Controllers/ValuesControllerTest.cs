using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interview;
using Interview.Controllers;
using Interview.Models;

namespace Interview.Tests.Controllers
{
    // AJA note: Used Google Chrome extension "ReqBin HTTP Client" to do some manual tests

    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void Get()
        {
            ValuesController controller = new ValuesController();

            // Arrange
            Transaction t1 = new Transaction();
            t1.Id = "3f2b12b8-2a06-45b4-b057-45949279b4e5";
            t1.ApplicationId = 197104;
            t1.Type = TransactionType.Debit;
            t1.Summary = TransactionSummary.Payment;
            t1.Amount = (decimal)58.26;
            t1.PostingDate = new DateTime(2016, 07, 01, 00, 00, 00);
            t1.IsCleared = true;
            t1.ClearedDate = new DateTime(2016, 07, 02, 00, 00, 00);
            Transaction t2 = new Transaction();
            t2.Id = "d2032222-47a6-4048-9894-11ab8ebb9f69";
            t2.ApplicationId = 197104;
            t2.Type = TransactionType.Debit;
            t2.Summary = TransactionSummary.Payment;
            t2.Amount = (decimal)50.09;
            t2.PostingDate = new DateTime(2016, 08, 01, 00, 00, 00);
            t2.IsCleared = true;
            t2.ClearedDate = new DateTime(2016, 08, 02, 00, 00, 00);

            // Act
            List<Transaction> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(17, result.Count());
            Assert.AreEqual(t1.Id, result.ElementAt(0).Id);
            Assert.AreEqual(t1.ApplicationId, result.ElementAt(0).ApplicationId);
            Assert.AreEqual(t1.Type, result.ElementAt(0).Type);
            Assert.AreEqual(t1.Summary, result.ElementAt(0).Summary);
            Assert.AreEqual(t1.Amount, result.ElementAt(0).Amount);
            Assert.AreEqual(t1.PostingDate, result.ElementAt(0).PostingDate);
            Assert.AreEqual(t1.IsCleared, result.ElementAt(0).IsCleared);
            Assert.AreEqual(t1.ClearedDate, result.ElementAt(0).ClearedDate);
            Assert.AreEqual(t2.Id, result.ElementAt(1).Id);
            Assert.AreEqual(t2.ApplicationId, result.ElementAt(1).ApplicationId);
            Assert.AreEqual(t2.Type, result.ElementAt(1).Type);
            Assert.AreEqual(t2.Summary, result.ElementAt(1).Summary);
            Assert.AreEqual(t2.Amount, result.ElementAt(1).Amount);
            Assert.AreEqual(t2.PostingDate, result.ElementAt(1).PostingDate);
            Assert.AreEqual(t2.IsCleared, result.ElementAt(1).IsCleared);
            Assert.AreEqual(t2.ClearedDate, result.ElementAt(1).ClearedDate);

            //t3, t4...
        }

        [TestMethod]
        public void GetById()
        {
            ValuesController controller = new ValuesController();

            // Arrange
            Transaction t = new Transaction();
            t.Id = "3f2b12b8-2a06-45b4-b057-45949279b4e5";
            t.ApplicationId = 197104;
            t.Type = TransactionType.Debit;
            t.Summary = TransactionSummary.Payment;
            t.Amount = (decimal)58.26;
            t.PostingDate = new DateTime(2016, 07, 01, 00, 00, 00);
            t.IsCleared = true;
            t.ClearedDate = new DateTime(2016, 07, 02, 00, 00, 00);

            // Act
            Transaction result = controller.Get("3f2b12b8-2a06-45b4-b057-45949279b4e5");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(t.Id, result.Id);
            Assert.AreEqual(t.ApplicationId, result.ApplicationId);
            Assert.AreEqual(t.Type, result.Type);
            Assert.AreEqual(t.Summary, result.Summary);
            Assert.AreEqual(t.Amount, result.Amount);
            Assert.AreEqual(t.PostingDate, result.PostingDate);
            Assert.AreEqual(t.IsCleared, result.IsCleared);
            Assert.AreEqual(t.ClearedDate, result.ClearedDate);
        }

        [TestMethod]
        public void PostPutDelete()
        {
            ValuesController controller = new ValuesController();

            // Arrange Post
            Transaction tPost = new Transaction();
            tPost.Id = "f52d2378-ee2b-4061-a24e-ff3ef6c4c327";
            tPost.ApplicationId = 123456;
            tPost.Type = TransactionType.Debit;
            tPost.Summary = TransactionSummary.Payment;
            tPost.Amount = (decimal)43.21;
            tPost.PostingDate = new DateTime(2021, 09, 19, 00, 00, 00);
            tPost.IsCleared = true;
            tPost.ClearedDate = new DateTime(2021, 09, 20, 00, 00, 00);
            // Arrange Put
            Transaction tPut = new Transaction();
            tPut.Id = "f52d2378-ee2b-4061-a24e-ff3ef6c4c327";
            tPut.ApplicationId = 654321;
            tPut.Type = TransactionType.Credit;
            tPut.Summary = TransactionSummary.Refund;
            tPut.Amount = (decimal)12.34;
            tPut.PostingDate = new DateTime(2021, 09, 20, 00, 00, 00);
            tPut.IsCleared = false;
            tPut.ClearedDate = new DateTime(2021, 09, 21, 00, 00, 00);

            // Act Post
            controller.Post(tPost);
            List<Transaction> tsPostPost = controller.ReadTransactions(ValuesController.Path);
            Transaction resultPost = tsPostPost.ElementAt(tsPostPost.Count - 1);
            // Act Put
            controller.Put("f52d2378-ee2b-4061-a24e-ff3ef6c4c327", tPut);
            List<Transaction> tsPostPut = controller.ReadTransactions(ValuesController.Path);
            Transaction resultPut = tsPostPut.ElementAt(tsPostPut.Count - 1);
            // Act Delete
            bool resultPreDelete = tsPostPut.Exists(t => t.Id == tPut.Id);
            controller.Delete("f52d2378-ee2b-4061-a24e-ff3ef6c4c327");
            List<Transaction> tsPostDelete = controller.ReadTransactions(ValuesController.Path);
            bool resultPostDelete = tsPostDelete.Contains(tPut);

            // Assert Post
            Assert.AreEqual(tPost.Id, resultPost.Id);
            Assert.AreEqual(tPost.ApplicationId, resultPost.ApplicationId);
            Assert.AreEqual(tPost.Type, resultPost.Type);
            Assert.AreEqual(tPost.Summary, resultPost.Summary);
            Assert.AreEqual(tPost.Amount, resultPost.Amount);
            Assert.AreEqual(tPost.PostingDate, resultPost.PostingDate);
            Assert.AreEqual(tPost.IsCleared, resultPost.IsCleared);
            Assert.AreEqual(tPost.ClearedDate, resultPost.ClearedDate);
            // Assert Put
            Assert.AreEqual(tPut.Id, resultPut.Id);
            Assert.AreEqual(tPut.ApplicationId, resultPut.ApplicationId);
            Assert.AreEqual(tPut.Type, resultPut.Type);
            Assert.AreEqual(tPut.Summary, resultPut.Summary);
            Assert.AreEqual(tPut.Amount, resultPut.Amount);
            Assert.AreEqual(tPut.PostingDate, resultPut.PostingDate);
            Assert.AreEqual(tPut.IsCleared, resultPut.IsCleared);
            Assert.AreEqual(tPut.ClearedDate, resultPut.ClearedDate);
            // Assert Delete
            Assert.IsTrue(resultPreDelete);
            Assert.IsFalse(resultPostDelete);
        }
    }
}
