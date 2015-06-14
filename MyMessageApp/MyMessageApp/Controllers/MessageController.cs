﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMessageApp.Models;
using NHibernate;

namespace MyMessageApp.Controllers
{
    public class MessageController : Controller
    {
        //private MessageDBContext db = new MessageDBContext();

        ////
        //// GET: /Message/

        //public ActionResult Index()
        //{
        //    return View(db.Messages.ToList());
        //}

        ////
        //// GET: /Message/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    Message message = db.Messages.Find(id);
        //    if (message == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(message);
        //}

        ////
        //// GET: /Message/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Message/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Message message)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Messages.Add(message);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(message);
        //}

        ////
        //// GET: /Message/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    Message message = db.Messages.Find(id);
        //    if (message == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(message);
        //}

        ////
        //// POST: /Message/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Message message)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(message).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(message);
        //}

        ////
        //// GET: /Message/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Message message = db.Messages.Find(id);
        //    if (message == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(message);
        //}

        ////
        //// POST: /Message/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Message message = db.Messages.Find(id);
        //    db.Messages.Remove(message);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}

        //myMethods----------------------------------------------------
        //Message/MyIndex "Get"
        public ActionResult MyIndex()
        {
            return View(functionGetList());
        }

        public List<MessageViewModel> functionGetList()
        {
            using (ISession session = NHibertnateSession.OpenSession())
            {

                Stack<Message> stack = new Stack<Message>();
                List<MessageViewModel> msgVM = new List<MessageViewModel>(); //provjeri da li mozes da radis sa listom ili enum

                var tempTable = session.QueryOver<Message>()
                                       .OrderBy(x => x.lft).Asc
                                       .List<Message>();

                foreach (Message item in tempTable)
                {
                    if (stack.Count() > 0)
                    {
                        while (stack.Peek().rgt < item.rgt)
                        {
                            stack.Pop();
                        }
                    }
                    stack.Push(item);
                    msgVM.Add(new MessageViewModel { id = item.message_Id, messageText = item.text, identLevel = stack.Count - 1, plusNumber = item.plus, minusNumber = item.minus });
                }
                return msgVM;
            }
        }

        ////Message/MyCreate "Get"
        //public ActionResult MyCreate() //int parentId
        //{
        //    return View();      //new MessageViewModel { id = parentId }
        //}

        //Message/MyCreate "Post"
        [HttpPost]
        public ActionResult MyCreate(MessageViewModel newMessage)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibertnateSession.OpenSession())
                {
                    var param = session.Get<Message>(newMessage.id).rgt - 1;
                    var myTable = session.QueryOver<Message>().List<Message>().ToList();

                    foreach (var item in myTable)
                    {
                        if (item.lft > param) { item.lft += 2; }
                        if (item.rgt > param) { item.rgt += 2; }
                        session.Save(item);
                    }

                    using (ITransaction transaction = session.BeginTransaction()) 
                    {
                        
                        Message temp = new Message { lft = param + 1, rgt = param + 2, text = newMessage.messageText, plus = 0, minus = 0 , date=null};
                        session.Save(temp);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("MyIndex");
            }
            return View(newMessage);
        }

        [HttpPost]
        public int Rate(int commentID, int commentValue) 
        {
            int returnValue;
            using (ISession session = NHibertnateSession.OpenSession())
            {
                Message messageToGet = session.Get<Message>(commentID);
                if (commentValue > 0)
                {
                    messageToGet.plus += 1;
                    returnValue = messageToGet.plus;
                }
                else
                {
                    messageToGet.minus += 1;
                    returnValue = messageToGet.minus;
                }
                using (ITransaction transaction = session.BeginTransaction()) 
                {
                    session.Save(messageToGet);
                    transaction.Commit();
                }
            }
            return returnValue;
        }
    }
}