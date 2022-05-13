﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaKore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaKore.JsonClasses;

namespace MaKore.Controllers
{

    [ApiController]
    [Route("api")]
    public class ConversationsController : Controller
    {
        private readonly MaKoreContext _context;

        public ConversationsController(MaKoreContext context)
        {
            _context = context;

            User u = new User()
           {
                UserName = "Ido",
                NickName = "Tani",
                ConversationList = new List<Conversation>(),
                Password = "aaa"
            };
            Message msg = new Message() { Content = "Ido:Hello", Conversation = null, ConversationId = 1, Time = Message.getTime() };
     
            Conversation conv = new Conversation() { RemoteUser = null, Messages = new List<Message>() { msg}, RemoteUserId = 1, User = u };
            msg.Conversation = conv;
            RemoteUser ru = new RemoteUser() { UserName = "Coral", NickName = "Corali", Conversation = conv, Server = "remote", ConversationId = 1 };
            u.ConversationList.Add(conv);
            conv.RemoteUser = ru;
            _context.Add(msg);
            _context.Add(u);
            _context.Add(ru);
            _context.Add(conv);
            
            
            //_context.SaveChanges();
        }

        // GET: /contacts + /contacts/:id
        [HttpGet("contacts/{id?}")]
        public async Task<IActionResult> GettAllContacts(string? id)
        {
            string name = "Ido";
            //string name = HttpContext.Session.GetString("username");
            
            if (id != null)
            {
                var q = from conversations in _context.Conversations
                        where conversations.RemoteUser.UserName == id && conversations.User.UserName == name
                        select conversations.RemoteUser;

                RemoteUser remoteUser = q.First();

                Message lastMessage = getLastMessage(remoteUser, name);
                string content = lastMessage.getContentFromMessage();

                return Json(new JsonUser()
                {
                    Id = remoteUser.UserName,
                    Name = remoteUser.NickName,
                    Server = remoteUser.Server,
                    LastDate = lastMessage.Time,
                    Last = content
                });
            }

            var q3 = from conversations in _context.Conversations
                    where conversations.User.UserName == name
                    select conversations.RemoteUser;
            
            List<JsonUser> users = new List<JsonUser>();


            foreach (var r in q3)
            {

                RemoteUser ru = r;

                Message lm = getLastMessage(ru, name);
                string c = lm.getContentFromMessage();

                users.Add(
                    new JsonUser()
                    {
                        Id = r.UserName,
                        Name = r.NickName,
                        Server = r.Server,
                        LastDate = lm.Time,
                        Last = c
                    });
            }
            



            return Json(users);
            //return View(await _context.Conversations.ToListAsync());
        }



        [HttpPost]
        public async Task<IActionResult> Index([Bind("UserName, NickName, Server")] RemoteUser remoteUser)
        {
            if (ModelState.IsValid)
            {
                remoteUser.Id = _context.RemoteUsers.Max(x => x.Id) + 1;
                remoteUser.Conversation = new Conversation();
                _context.RemoteUsers.Add(remoteUser);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                //return View("good");
                return Json(new EmptyResult());
            }
            return View("not");
        }


        // PUT: /contacts/id
        [HttpPut, ActionName("contacts")]
        public async Task<IActionResult> Put(string contact, [Bind("UserName, NickName, Server")] RemoteUser ru)
        {
            if (ru == null)
            {
                return Json(new EmptyResult());
            }

            RemoteUser remoteUser = (RemoteUser) from remote in _context.RemoteUsers
                                                 where remote.UserName == contact && remote.Server == ru.Server
                                                 select remote;
            remoteUser.UserName = ru.UserName;
            remoteUser.NickName = ru.NickName;
            return NoContent();    //204
        }


        // DELETE: /contacts/id
        [HttpDelete, ActionName("contacts")]
        public async Task<IActionResult> Delete(string contact)
        {
            if (contact == null)
            {
                return Json(new EmptyResult());
            }

            string name = HttpContext.Session.GetString("username");

            var remoteUser = from conversations in _context.Conversations
                             where conversations.RemoteUser.UserName == contact && conversations.User.UserName == name
                             select conversations.RemoteUser;

            _context.RemoteUsers.Remove(remoteUser.First());
            _context.SaveChanges();
            return NoContent();    //204
        }


        private Message getLastMessage(RemoteUser ru, string name)
        {
            var q = from conv in _context.Conversations.Include(m => m.Messages)
                             where conv.User.UserName == name && conv.RemoteUser == ru
                             select conv;

            if (q.Any())
            {
                Conversation c = q.First();
                if (c.Messages.Count != 0)
                    return c.Messages.OrderByDescending(m => m.Id).FirstOrDefault();                 
            }
            return null;

        }
    }
}
