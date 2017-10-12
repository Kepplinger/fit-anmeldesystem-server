using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models.Validation;
using Newtonsoft.Json;

namespace Backend.Core.Entities
{
    public class Token
    {
        [JsonProperty("access_token")]  
       public string AccessToken { get; set; }  
  
       [JsonProperty("token_type")]  
       public string TokenType { get; set; }  
  
       [JsonProperty("expires_in")]  
       public int ExpiresIn { get; set; }  
  
       [JsonProperty("refresh_token")]  
       public string RefreshToken { get; set; }  
    }
}