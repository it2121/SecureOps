namespace SecureOps.Models.Dto
{
    public class PageDto
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }

        public List<int> SelectedRoleIds { get; set; } = new List<int>();
    }
}
