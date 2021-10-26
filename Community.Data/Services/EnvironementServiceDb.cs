using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Community.Core.Models;
using Community.Core.Security;
using Community.Data.Repositories;
namespace Community.Data.Services
{
    public class EnvironmentServiceDb : IEnvironmentService
    {
        private readonly DatabaseContext ctx;
        

        public EnvironmentServiceDb(DatabaseContext db)
        {
            ctx = db; 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        //-------------MyEnvironment related options---------------------------
        public IList<Issue> GetAllIssues()
        {

            return ctx.Issues
                        .ToList();            
        }

        public Issue GetIssue(int id)
        {
            return ctx.Issues
                     .FirstOrDefault(i => i.Id == id);
        }
        
        public bool DeleteIssue(int id)
        {
            var i = GetIssue(id);
            if (i == null)
            {
                return false;
            }
            ctx.Issues.Remove(i);
            ctx.SaveChanges();
            return true;
        }

        public Issue AddIssue(Issue i)
        {
            ctx.Issues.Add(i);
            ctx.SaveChanges();
            return i;
        }

        public bool UpdateIssue(Issue i)
        {
            var issue = GetIssue(i.Id);
            if (issue == null)
            {
                return false;
            }

            issue.Description = i.Description;
            issue.IssueType = i.IssueType;
            issue.Longitude = i.Longitude;
            issue.Latitude = i.Latitude;
            ctx.SaveChanges();
            return true;
        }

    }
}
