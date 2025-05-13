using System;
using System.Collections.Generic;

namespace UpSkillApi.Models;
public class Chat
{
    public int ChatId { get; set; }

    public int User1Id { get; set; }
    public int User2Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public List<Message> Messages { get; set; }

    public User User1 { get; set; }
    public User User2 { get; set; }
}