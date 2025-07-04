using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection.Emit;

namespace Todo.WebApi.Features.Tasks.Infrastructure
{
    public class TodoTaskEFConfiguration : IEntityTypeConfiguration<TodoTask>
    {
        public void Configure(EntityTypeBuilder<TodoTask> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(x => x.HasCheckConstraint("CK_todotask_id_uuid", $"\"id\" <> '{Guid.Empty}'"));


            builder
                .HasOne(t => t.Employee)
                .WithMany()
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull); // o DeleteBehavior.Restrict

            builder
                .HasOne(t => t.Reviewer)
                .WithMany()
                .HasForeignKey(t => t.ReviewerId)
                .OnDelete(DeleteBehavior.SetNull); // o DeleteBehavior.Restrict


            builder.Property(x => x.Status)
               .HasDefaultValue(Shared.TaskStatus.Pending)
               .HasConversion(new EnumToStringConverter<Shared.TaskStatus>());

            builder.Property(x => x.Priority)
              .HasDefaultValue(Shared.TaskPriority.Low)
              .HasConversion(new EnumToStringConverter<Shared.TaskPriority>());

        }
    }
}
