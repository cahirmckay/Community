using System;
using System.Collections.Generic;

using Community.Core.Models;

namespace Community.Data.Services
{
    // This interface describes the operations that a PostService class should implement
    public interface IPostService
    {
        // Initialise the repository - only to be used during development 
        void Initialise();

        // ---------------- Post Management --------------
        IList<Post> GetAllPosts(User u);
        Post GetPost(int id);
        Post AddPost(Post b);
        bool DeletePost(int id);
        bool UpdatePost(Post m);
        Comment GetCommentById(int id);
        Comment AddComment(Comment c);
        bool DeleteComment(int id);

       
    }
    
}