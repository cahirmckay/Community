using System;
using System.Collections.Generic;

using Community.Core.Models;

namespace Community.Data.Services
{
    // This interface describes the operations that a BusinessService class should implement.
    public interface IBusinessService
    {
        // Initialise the repository - only to be used during development 
        void Initialise();

        //business management-------
        IList<Business> GetAllBusiness(User u);
        Business GetBusiness(int id);
        Business AddBusiness(Business b);
        bool DeleteBusiness(int id);
        bool UpdateBusiness(Business m);
        //Review management ---------
        Review GetReviewById(int id);
        Review AddReview(Review r);
        bool DeleteReview(int id);
    }
}
