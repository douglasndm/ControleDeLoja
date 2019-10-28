namespace ControleDeLoja.Models.Users
{
    public class Operator : User
    {
        public short Id { get; set; }

        public override object Clone()
        {
            return new Operator()
            {
                Id = this.Id,
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
