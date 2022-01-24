namespace Domain.Entities
{
	public partial class PermissionType
    {
        public PermissionType()
        {
            Permissions = new HashSet<Permission>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
