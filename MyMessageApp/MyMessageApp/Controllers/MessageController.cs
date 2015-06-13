using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMessageApp.Models;

namespace MyMessageApp.Controllers
{
    public class MessageController : Controller
    {
        private MessageDBContext db = new MessageDBContext();

        //
        // GET: /Message/

        public ActionResult Index()
        {
            return View(db.Messages.ToList());
        }

        //
        // GET: /Message/Details/5

        public ActionResult Details(int id = 0)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        //
        // GET: /Message/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Message/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }

        //
        // GET: /Message/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        //
        // POST: /Message/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        //
        // GET: /Message/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        //
        // POST: /Message/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //myMethods----------------------------------------------------
        //Message/MyIndex "Get"
        public ActionResult MyIndex() 
        { 
            List<MessageViewModel> msgVM = new List<MessageViewModel>();
            var tempTable = from m in db.Messages
                            orderby m.order
                            select m;
            foreach (Message item in tempTable)
                    {
                        msgVM.Add(new MessageViewModel{ id = item.ID, identLevel = item.level, messageText = item.text});
                    }
            return View( msgVM );
        }

        //Message/MyCreate "Get"
        public ActionResult MyCreate(int parentId ) 
        {
            return View(new MessageViewModel { id = parentId });
        }

        //Message/MyCreate "Post"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCreate(MessageViewModel newMessage) 
        {
            if( ModelState.IsValid ) 
            {
                Message parentNode = db.Messages.Find( newMessage.id );
                var myTable = from m in db.Messages
                              where m.order > parentNode.order
                              select m;
                foreach (var item in myTable) 
                {
                    item.order++;
                }
                db.Messages.Add(new Message { level = parentNode.level + 1, order = parentNode.order + 1, parentID = parentNode.ID, text = newMessage.messageText });
                db.SaveChanges();
                return RedirectToAction("MyIndex");
            }
            return View( newMessage );
        }
    }
}