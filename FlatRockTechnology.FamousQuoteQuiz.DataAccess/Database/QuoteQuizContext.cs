#nullable disable
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Configurations.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database
{
    public partial class QuoteQuizContext : IdentityDbContext<User>
    {
        public QuoteQuizContext(DbContextOptions<QuoteQuizContext> options) : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<UserAnsweredQuestion> UserAnsweredQuestion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.QuestionID).HasColumnName("QuestionID");

                entity.Property(e => e.Text).HasColumnName("Text");

                entity.Property(e => e.IsCorrect).HasColumnName("IsCorrect");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answer)
                    .HasForeignKey(d => d.QuestionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.Text).HasColumnName("Text");
            });

            modelBuilder.Entity<UserAnsweredQuestion>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.QuestionID).HasColumnName("QuestionID");

                entity.Property(e => e.AnswerID).HasColumnName("AnswerID");

                entity.Property(e => e.UserID).HasColumnName("UserID");


                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAnsweredQuestion)
                    .HasForeignKey(d => d.UserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserAnsweredQuestion");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.UserAnsweredQuestion)
                    .HasForeignKey(d => d.AnswerID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answer_UserAnsweredQuestion");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.UserAnsweredQuestion)
                    .HasForeignKey(d => d.QuestionID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_UserAnsweredQuestion");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
