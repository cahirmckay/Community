using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Community.Core.Models;
using Community.Core.Security;
using Community.Data.Repositories;
namespace Community.Data.Services
{
    public class NewsServiceDb : INewsService
    {
        private readonly DatabaseContext ctx;
        
        //Confirgured Via DI
        public NewsServiceDb(DatabaseContext db)
        {
            ctx = db; 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        //-------------MyNews related options---------------------------

        //Only return news articles in their community
        public IList<NewsArticle> GetAllNewsArticles(User u)
        {

            return ctx.NewsArticles
                        .Where(n => n.CommunityId == u.CommunityId)
                        .OrderByDescending(n=>n.CreatedOn)
                        .ToList();            
        }

        public NewsArticle GetNewsArticle(int id)
        {
            return ctx.NewsArticles
                     .FirstOrDefault(n => n.Id == id);
        }
        
        public bool DeleteNewsArticle(int id)
        {
            var n = GetNewsArticle(id);
            if (n == null)
            {
                return false;
            }
            ctx.NewsArticles.Remove(n);
            ctx.SaveChanges();
            return true;
        }

        public NewsArticle AddNewsArticle(NewsArticle n)
        {
            ctx.NewsArticles.Add(n);
            ctx.SaveChanges();
            return n;
        }

        public bool UpdateNewsArticle(NewsArticle n)
        {
            var newsArticle = GetNewsArticle(n.Id);
            if (newsArticle== null)
            {
                return false;
            }
            newsArticle.Id = n.Id;
            newsArticle.Source = n.Source;
            newsArticle.Headline = n.Headline;
            newsArticle.CreatedOn = n.CreatedOn;
            newsArticle.CommunityId = n.CommunityId;
            newsArticle.ArticleUrl = n.ArticleUrl;
           
            ctx.SaveChanges();
            return true;
        }
    }
}
