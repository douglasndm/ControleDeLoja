namespace ControleDeLoja.Models.Users
{
    public class Administrator : User
    {
        public short Id { get; set; }
        public short AcessLevel { get; set; }

        public override object Clone()
        {
            return new Administrator()
            {
                Id = this.Id,
                AcessLevel = this.AcessLevel,
                UserId = this.UserId,
                Name = this.Name,
                LastName = this.LastName,
                Email = this.Email,
                UserName = this.UserName,
                Password = this.Password
            };
        }
    }
}
