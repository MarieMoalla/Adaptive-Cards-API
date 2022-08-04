using AdaptiveCards;
using AdaptiveCardsFakeAPI.Data;
using AdaptiveCardsFakeAPI.Entities;
using AdaptiveCardsFakeAPI.Models;
using AdaptiveCardsFakeAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AdaptiveCardsFakeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdaptiveCardsController : ControllerBase
    {
        #region variables and constructure 
        private readonly AppDBContext _context;
        private readonly IAdaptiveCardsService _service;
        private readonly IUserServices _userService;
        public AdaptiveCardsController(AppDBContext context,IUserServices userService,IAdaptiveCardsService service) 
        {
            _context = context;
            _service = service;
            _userService = userService;
        }
        #endregion

        // generate adaptive card for recipient
        #region Get User List Card
        [HttpGet]
        public string GetUsersListcard()
        {
            IList<User> users = _userService.getUsers();
            return _service.generateUsersCheckListCard(users);
        }
        #endregion

        // post adaptive card response from recipient
        #region Post User List Card Response
        [HttpPost]
        public async Task<string> PostUsersListcardResponse(string body)
        {
            try
            {
                JObject jsonBody = JObject.Parse(body);
                Body b = new Body()
                {
                    data= jsonBody["data"].ToString()
                };
                JObject jsonData = JObject.Parse(b.data);
                User user = new User();
                foreach(var element in jsonData)
                {
                    // get user by username 
                    user = _userService.getUserByUserName(element.Value.ToString());
                    user.selected = true;
                    _context.Update<User>(user);

                    _context.SaveChanges();
                }
                return b.data;
                
            }
            catch (AdaptiveSerializationException ex)
            {
                return ex.Message;
            }
            

        }
        #endregion
    }
}
