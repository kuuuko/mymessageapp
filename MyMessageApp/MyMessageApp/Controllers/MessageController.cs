using System;
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
        // GET: /Messages/

        public ActionResult Index()
        {
            using (ISession session = NHibertnateSession.OpenSession())
            {
                var ArticleList = session.QueryOver<NewsArticle>()
                                         .List<NewsArticle>();
                return View(ArticleList);
            }
        }

        // GET: /Messages/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Messages/Create

        [HttpPost]
        public ActionResult Create(NewsArticle myArticle)
        {
            using (ISession session = NHibertnateSession.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    NewsArticle newArticle = new NewsArticle { article_text = myArticle.article_text };
                    session.Save(newArticle);
                    Message rootParent = new Message { lft = 1, rgt = 2, article_Id = newArticle.article_Id };
                    session.Save(rootParent);
                    newArticle.root_message_Id = rootParent.message_Id;
                    session.Save(newArticle);

                    transaction.Commit();
                }

            }
            return Redirect("Index");
        }
        
        //Message/MyIndex/article_id
        public ActionResult MyIndex(int article_id)
        {
            return View(functionGetList(article_id));
        }

        public List<MessageViewModel> functionGetList(int idOfArticle)
        {
            using (ISession session = NHibertnateSession.OpenSession())
            {

                Stack<Message> stack = new Stack<Message>();
                List<MessageViewModel> msgVM = new List<MessageViewModel>();

                var tempTable = session.QueryOver<Message>()
                                       .Where(x => x.article_Id == idOfArticle)
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
                    msgVM.Add(new MessageViewModel { id = item.message_Id, messageText = item.text, identLevel = stack.Count - 1, plusNumber = item.plus, minusNumber = item.minus, article_id = idOfArticle , user_name = item.user_name, viewdate = item.date });
                }
                return msgVM;
            }
        }
      
        //Message/MyCreate "Post"
        [HttpPost]
        public bool MyCreate(MessageViewModel newMessage, int parentID, int article_id)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibertnateSession.OpenSession())
                {
                    var param = session.Get<Message>(parentID).rgt - 1;
                    var myTable = session.QueryOver<Message>()
                                         .Where(x => x.article_Id == article_id )
                                         .List<Message>().ToList();

                    foreach (var item in myTable)
                    {
                        if (item.lft > param) { item.lft += 2; }
                        if (item.rgt > param) { item.rgt += 2; }
                        session.Save(item);
                    }

                    using (ITransaction transaction = session.BeginTransaction())
                    {

                        Message temp = new Message { lft = param + 1, rgt = param + 2, text = newMessage.messageText, plus = 0, minus = 0, date = DateTime.Now , article_Id = article_id, user_name = newMessage.user_name};
                        session.Save(temp);
                        transaction.Commit();
                    }
                }
                return true;
            }
            return false;
        }

        //Message/Rate
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

        //Message/Refresh
        public ActionResult Refresh(int article_id)
        {
            return PartialView("_viewPostField", functionGetList(article_id));
        }

        //Message/ReportAbuse
        [HttpPost]
        public void ReportAbuse(int commentID)
        {
            using (ISession session = NHibertnateSession.OpenSession())
            {
                Message messageToGet = session.Get<Message>(commentID);
                
                    messageToGet.abuseFlag = true;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(messageToGet);
                    transaction.Commit();
                }
            }
        }
    }
}