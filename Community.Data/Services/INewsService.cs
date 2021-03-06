using System;
using System.Collections.Generic;

using Community.Core.Models;

namespace Community.Data.Services
{
    // This interface describes the operations that a NewsService class should implement
    public interface INewsService
    {
        // Initialise the repository - only to be used during development 
        void Initialise();

        // ---------------- News Management --------------
        IList<NewsArticle> GetAllNewsArticles(User u);
        NewsArticle GetNewsArticle(int id);
        NewsArticle AddNewsArticle(NewsArticle n);
        bool DeleteNewsArticle(int id);
        bool UpdateNewsArticle(NewsArticle n);
       

       
    }
    
}