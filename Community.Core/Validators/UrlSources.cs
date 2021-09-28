using System;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace Community.Data.Validators {
    
     public class UrlResource : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            string _url = (string)value; // url property being validated should be a string;           

            if (UrlResourceExists(_url)) // verify the url points to a resource
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Url resource does not exist");        
        }
        
        // verify that url points to a valid resource
        private bool UrlResourceExists(string url) {                                         
            
            // check there is a url to validate
            if (url == null || url == "") {
                return true;
            }
            
            // method HEAD verifies resource existence
            try {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Method = "HEAD";
                webRequest.GetResponse();
                return true;  // got here so valid
            } catch {
                return false; // exception thrown so invalid
            }
        }
    }
}