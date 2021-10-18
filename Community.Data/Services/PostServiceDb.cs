using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Community.Core.Models;
using Community.Core.Security;
using Community.Data.Repositories;
namespace Community.Data.Services
{
    public class PostServiceDb : IPostService
    {
        private readonly DatabaseContext ctx;
        

        public PostServiceDb(DatabaseContext db)
        {
            ctx = db; 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        //-------------MyPlaces related options---------------------------
        public IList<Post> GetAllPosts(User u)
        {

            return ctx.Posts
                        .Where(p => p.CommunityId == u.CommunityId)
                        .OrderByDescending(p=>p.CreatedOn)
                        .ToList();            
        }

        public Post GetPost(int id)
        {
            return ctx.Posts
                     .Include(p => p.Comments)
                     .FirstOrDefault(p => p.Id == id);
        }
        
        public bool DeletePost(int id)
        {
            var p = GetPost(id);
            if (p == null)
            {
                return false;
            }
            ctx.Posts.Remove(p);
            ctx.SaveChanges();
            return true;
        }

        public Post AddPost(Post p)
        {
            ctx.Posts.Add(p);
            ctx.SaveChanges();
            return p;
        }

        public bool UpdatePost(Post p)
        {
            var post = GetPost(p.Id);
            if (post == null)
            {
                return false;
            }

            post.Name = p.Name;
            post.PostType = p.PostType;
            post.CreatedOn = DateTime.Now;
            post.Id = p.Id;
            post.PostText = p.PostText;
            post.CommunityId = p.CommunityId;
            ctx.SaveChanges();
            return true;
        }

        //Comment OPERATIONS

        public Comment GetCommentById(int id)
        {
            return ctx.Comments
                     .Include(c => c.Post)
                     .FirstOrDefault(c => c.CommentId == id);
        }

        public Comment AddComment(Comment c)
        {
            //Creates a new instance of a Comment so more than can be created for each Post
            var comment = new Comment
            {
                // Id created by Database 
                CommentId = c.CommentId,              
                Description = c.Description,     
                Name = c.Name,
                CreatedOn = DateTime.Now,
                PostId = c.PostId             
            };


            ctx.Comments.Add(comment);
            ctx.SaveChanges();
            return c;
        }

        public bool DeleteComment(int id)
        {
            var comment = GetCommentById(id);
            if (comment == null)
            {
                return false;
            }
            
            //deletes one Comment
            var result = comment.Post.Comments.Remove(comment);
            ctx.SaveChanges();
            return result;
        }

    }
}
