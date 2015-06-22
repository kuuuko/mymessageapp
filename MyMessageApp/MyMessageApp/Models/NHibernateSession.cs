using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMessageApp.Models
{
    public class NHibertnateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\hibernate.cfg.xml");
            configuration.Configure(configurationPath);
            var messageConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\Message.hbm.xml");
            var articleConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\NewsArticle.hbm.xml");
            configuration.AddFile(messageConfigurationFile);
            configuration.AddFile(articleConfigurationFile);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}