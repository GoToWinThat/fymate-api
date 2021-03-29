namespace Domain.Base.Models
{
    public interface IHasOwner
    {
        string OwnerID { get; }
    }
}