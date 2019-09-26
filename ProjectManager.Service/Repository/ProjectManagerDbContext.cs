namespace ProjectManager.Service.Repository
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ProjectManagerDbContext : DbContext
    {
        public ProjectManagerDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<TaskDetailModel> Tasks { get; set; }

        public virtual DbSet<ProjectDetailModel> Projects { get; set; }

        public virtual DbSet<UserDetailModel> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {                        
            optionsBuilder.UseSqlServer(@"Server = DOTNET; Database = TaskManagerDB; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            BuildTaskDetailModelTable(builder);
            BuildProjectTable(builder);
            BuildUserTable(builder);
        }

        private static void BuildTaskDetailModelTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskDetailModel>().HasKey("Id");
            modelBuilder.Entity<TaskDetailModel>().ToTable("Task");
            modelBuilder.Entity<TaskDetailModel>().Property(t => t.Name).HasColumnName("Task").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<TaskDetailModel>().Property(t => t.StartDate).HasColumnName("Start_Date");
            modelBuilder.Entity<TaskDetailModel>().Property(t => t.EndDate).HasColumnName("End_Date");
            modelBuilder.Entity<TaskDetailModel>().Property(t => t.ParentTaskId).HasColumnName("ParentId");
            modelBuilder.Entity<TaskDetailModel>().Property(t => t.Priority).HasColumnName("Priority").IsRequired();
            modelBuilder.Entity<TaskDetailModel>().Property(t => t.EndTask).HasColumnName("EndTask").IsRequired();
            modelBuilder.Entity<TaskDetailModel>().Property(t => t.Id).ValueGeneratedOnAdd().HasColumnName("Id").IsRequired();
            modelBuilder.Entity<TaskDetailModel>().Property(t => t.UserId).HasColumnName("User_Id");
            modelBuilder.Entity<TaskDetailModel>().Property(t => t.ProjectId).HasColumnName("Project_Id");
            modelBuilder.Entity<TaskDetailModel>().HasOne(t => t.UserDetail).WithMany(u => u.TaskDetails).HasForeignKey(t => t.UserId);
            modelBuilder.Entity<TaskDetailModel>().HasOne(t => t.ProjectDetail).WithMany(u => u.TaskDetails).HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Restrict);
        }

        private static void BuildProjectTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectDetailModel>().HasKey("ProjectId");
            modelBuilder.Entity<ProjectDetailModel>().ToTable("Project");
            modelBuilder.Entity<ProjectDetailModel>().Property(t => t.ProjectName).HasColumnName("ProjectName").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProjectDetailModel>().Property(t => t.StartDate).HasColumnName("Start_Date");
            modelBuilder.Entity<ProjectDetailModel>().Property(t => t.EndDate).HasColumnName("End_Date");
            modelBuilder.Entity<ProjectDetailModel>().Property(t => t.Priority).HasColumnName("Priority").IsRequired();
            modelBuilder.Entity<ProjectDetailModel>().Property(t => t.EndProject).HasColumnName("Status").IsRequired();
            modelBuilder.Entity<ProjectDetailModel>().Property(t => t.ProjectId).ValueGeneratedOnAdd().HasColumnName("Project_Id").IsRequired();
            modelBuilder.Entity<ProjectDetailModel>().Property(t => t.UserId).HasColumnName("User_Id");
            modelBuilder.Entity<ProjectDetailModel>().HasOne(t => t.UserDetail).WithMany(u => u.Projects).HasForeignKey(t => t.UserId);            
        }

        private static void BuildUserTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetailModel>().HasKey("UserId");
            modelBuilder.Entity<UserDetailModel>().ToTable("User");
            modelBuilder.Entity<UserDetailModel>().Property(t => t.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<UserDetailModel>().Property(t => t.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(100); ;
            modelBuilder.Entity<UserDetailModel>().Property(t => t.EmployeeId).HasColumnName("Employee_Id").IsRequired();
            modelBuilder.Entity<UserDetailModel>().Property(t => t.UserId).ValueGeneratedOnAdd().HasColumnName("User_Id").IsRequired();
        }
    }
}