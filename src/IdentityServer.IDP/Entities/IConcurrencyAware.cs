namespace IdentityServer.IDP.Entities
{
    public interface IConcurrencyAware
    {
        public string ConcurrencyStamp { get; set; }
    }
}
