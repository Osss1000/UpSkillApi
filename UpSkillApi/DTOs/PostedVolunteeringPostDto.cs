public class PostedVolunteeringPostDto
{
    
    public string OrganizationName { get; set; }
    public int volunteeringJobId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime? DateAndTime { get; set; }
    public int? NumberOfPeopleNeeded { get; set; }
}