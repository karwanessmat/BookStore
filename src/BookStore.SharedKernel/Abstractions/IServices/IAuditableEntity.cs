namespace BookStore.SharedKernel.Abstractions.IServices;

public interface IAuditableEntity
{
    Guid? CreatedBy { get;   }
    DateTimeOffset? CreatedDateTimeOnUtc { get; }
    Guid? LastModifiedBy { get; }
    DateTimeOffset? UpdatedDateTimeOnUtc { get;  }
}
