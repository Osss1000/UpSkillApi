﻿namespace UpSkillApi.DTOs
{
    public class UpdatevolunteeringPostDto
    {
        public int VolunteeringJobId { get; set; }
        public string Title { get; set; } = null!;
        public int? NoOfPeopleNeeded { get; set; }
        public DateTime Date { get; set; }              // التاريخ فقط
        public TimeSpan? Time { get; set; }             // الوقت (اختياري)
        public string? Details { get; set; }
        public string? Location { get; set; }
        
    }
}
