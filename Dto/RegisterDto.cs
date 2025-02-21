using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace TrudoseAdminPortalAPI.Dtos
{
    public class RegisterDto
    {      
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }      
       
        public int role_id { get; set; }
        public int clinic_id { get; set; }
        public string? phone_number { get; set; }

        public int? created_by { get; set; }
       
        public int? updated_by { get; set; }
       


    }
}
