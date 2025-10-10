namespace SecureOps.Models.Dto
{
    public class PageDtoWithRoles
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public List<RoleDto> Roles { get; set; } = new();
    }
}
