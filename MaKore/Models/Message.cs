﻿using System.ComponentModel.DataAnnotations;
namespace MaKore.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Time { get; set; }

        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }



        public static string getTime()
        {
            DateTime date = DateTime.Now;
            return date.ToString("o");
        }

        public string getContentFromMessage()
        {
            int start = this.Content.ToString().IndexOf(":") + 1;
            return this.Content.ToString().Substring(start);
        }

        public string getSenderFromMessage()
        {
            int end = this.Content.ToString().IndexOf(":");
            return this.Content.ToString().Substring(0, end);
        }
    }
}
