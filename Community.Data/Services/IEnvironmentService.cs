using System;
using System.Collections.Generic;

using Community.Core.Models;

namespace Community.Data.Services
{
    // This interface describes the operations that a UserService class should implement
    public interface IEnvironmentService
    {
        // Initialise the repository - only to be used during development 
        void Initialise();


        IList<Issue> GetAllIssues();
        Issue GetIssue(int id);
        Issue AddIssue(Issue n);
        bool DeleteIssue(int id);
        bool UpdateIssue(Issue s);

    }
    
}
