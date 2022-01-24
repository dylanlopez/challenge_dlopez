namespace Application.Dtos
{
	public class PermissionDto
	{
		public int Id { get; set; }
		public string EmployeeForename { get; set; } = null!;
		public string EmployeeSurname { get; set; } = null!;
		public int PermissionType { get; set; }
		public DateTime PermissionDate { get; set; }
	}
}
