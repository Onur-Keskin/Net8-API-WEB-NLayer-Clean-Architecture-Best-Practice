using App.Domain.Entities.Common;

namespace App.Domain.Entities
{
    public class User : BaseEntity<int>, IAuditEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }

}
