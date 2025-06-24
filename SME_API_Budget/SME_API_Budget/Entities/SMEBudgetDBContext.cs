using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SME_API_Budget.Entities;

namespace SME_API_SMEBudget.Entities;

public partial class SMEBudgetDBContext : DbContext
{
    public SMEBudgetDBContext()
    {
    }

    public SMEBudgetDBContext(DbContextOptions<SMEBudgetDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MapiInformation> MapiInformations { get; set; }

    public virtual DbSet<MscheduledJob> MscheduledJobs { get; set; }

    public virtual DbSet<RecPR> RecPRs { get; set; }

    public virtual DbSet<ReturnPActivity> ReturnPActivities { get; set; }

    public virtual DbSet<ReturnPActivitySub> ReturnPActivitySubs { get; set; }

    public virtual DbSet<ReturnPArea> ReturnPAreas { get; set; }

    public virtual DbSet<ReturnPExpected> ReturnPExpecteds { get; set; }

    public virtual DbSet<ReturnPExpectedSub> ReturnPExpectedSubs { get; set; }

    public virtual DbSet<ReturnPOutcome> ReturnPOutcomes { get; set; }

    public virtual DbSet<ReturnPOutput> ReturnPOutputs { get; set; }

    public virtual DbSet<ReturnPPay> ReturnPPays { get; set; }

    public virtual DbSet<ReturnPPlanBdg> ReturnPPlanBdgs { get; set; }

    public virtual DbSet<ReturnPPlanBdgSub> ReturnPPlanBdgSubs { get; set; }

    public virtual DbSet<ReturnPRisk> ReturnPRisks { get; set; }

    public virtual DbSet<ReturnProject> ReturnProjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("SISMEBudget");

        modelBuilder.Entity<MapiInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MApiData");

            entity.ToTable("MApiInformation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApiKey).HasMaxLength(150);
            entity.Property(e => e.AuthorizationType).HasMaxLength(50);
            entity.Property(e => e.ContentType).HasMaxLength(150);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MethodType).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(150);
            entity.Property(e => e.ServiceNameCode).HasMaxLength(250);
            entity.Property(e => e.ServiceNameTh).HasMaxLength(250);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Urldevelopment).HasColumnName("URLDevelopment");
            entity.Property(e => e.Urlproduction).HasColumnName("URLProduction");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<MscheduledJob>(entity =>
        {
            entity.ToTable("MScheduledJobs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.JobName).HasMaxLength(150);
        });

        modelBuilder.Entity<RecPR>(entity =>
        {
            entity.ToTable("Rec_P_Rs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActivityName)
                .HasMaxLength(200)
                .HasColumnName("Activity_name");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DataPS1).HasColumnName("DATA_P_S1");
            entity.Property(e => e.DataPS2).HasColumnName("DATA_P_S2");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("project_code");
            entity.Property(e => e.RefCode).HasColumnName("REF_CODE");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.YearBdg)
                .HasMaxLength(50)
                .HasColumnName("year_bdg");
        });

        modelBuilder.Entity<ReturnPActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Return_P__3213E83F2A48FB31");

            entity.ToTable("Return_P_Activity");

            entity.HasIndex(e => e.RefCode, "UQ__Return_P__9E4C40044CF81D6D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataP1)
                .HasMaxLength(200)
                .HasColumnName("DATA_P1");
            entity.Property(e => e.DataP2)
                .HasMaxLength(200)
                .HasColumnName("DATA_P2");
            entity.Property(e => e.DataP3)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DATA_P3");
            entity.Property(e => e.DataP4)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DATA_P4");
            entity.Property(e => e.DataP5)
                .HasMaxLength(200)
                .HasColumnName("DATA_P5");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("project_code");
            entity.Property(e => e.RefCode).HasColumnName("REF_CODE");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.YearBdg)
                .HasMaxLength(50)
                .HasColumnName("year_bdg");
        });

        modelBuilder.Entity<ReturnPActivitySub>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Return_P__3213E83F5D1C979A");

            entity.ToTable("Return_P_Activity_Sub");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataP6)
                .HasMaxLength(50)
                .HasColumnName("DATA_P6");
            entity.Property(e => e.DataP7)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DATA_P7");
            entity.Property(e => e.RefCode).HasColumnName("REF_CODE");
            entity.Property(e => e.SubCode).HasMaxLength(50);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.RefCodeNavigation).WithMany(p => p.ReturnPActivitySubs)
                .HasPrincipalKey(p => p.RefCode)
                .HasForeignKey(d => d.RefCode)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Activity_Sub_Activity");
        });

        modelBuilder.Entity<ReturnPArea>(entity =>
        {
            entity.ToTable("Return_P_Area");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DataP1)
                .HasMaxLength(200)
                .HasColumnName("DATA_P1");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("project_code");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.YearBdg)
                .HasMaxLength(50)
                .HasColumnName("year_bdg");
        });

        modelBuilder.Entity<ReturnPExpected>(entity =>
        {
            entity.HasKey(e => e.KeyId).HasName("PK__Return_P__21F5BE4724609A7C");

            entity.ToTable("Return_P_Expected");

            entity.Property(e => e.KeyId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataP1)
                .HasMaxLength(200)
                .HasColumnName("DATA_P1");
            entity.Property(e => e.DataP2)
                .HasMaxLength(200)
                .HasColumnName("DATA_P2");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("project_code");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.YearBdg)
                .HasMaxLength(50)
                .HasColumnName("year_bdg");
        });

        modelBuilder.Entity<ReturnPExpectedSub>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Return_P__3214EC0741BABACD");

            entity.ToTable("Return_P_Expected_Sub");

            entity.Property(e => e.DataPS1)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DATA_P_S1");
            entity.Property(e => e.SubCode).HasMaxLength(10);

            entity.HasOne(d => d.Key).WithMany(p => p.ReturnPExpectedSubs)
                .HasForeignKey(d => d.KeyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Return_P___KeyId__498EEC8D");
        });

        modelBuilder.Entity<ReturnPOutcome>(entity =>
        {
            entity.ToTable("Return_P_Outcome");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DataP1).HasColumnName("DATA_P1");
            entity.Property(e => e.DataP2).HasColumnName("DATA_P2");
            entity.Property(e => e.DataP3).HasColumnName("DATA_P3");
            entity.Property(e => e.DataP4)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DATA_P4");
            entity.Property(e => e.DataP5).HasColumnName("DATA_P5");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("project_code");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.YearBdg)
                .HasMaxLength(50)
                .HasColumnName("year_bdg");
        });

        modelBuilder.Entity<ReturnPOutput>(entity =>
        {
            entity.ToTable("Return_P_output");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DataP1).HasColumnName("DATA_P1");
            entity.Property(e => e.DataP2).HasColumnName("DATA_P2");
            entity.Property(e => e.DataP3).HasColumnName("DATA_P3");
            entity.Property(e => e.DataP4)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DATA_P4");
            entity.Property(e => e.DataP5).HasColumnName("DATA_P5");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("project_code");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.YearBdg)
                .HasMaxLength(50)
                .HasColumnName("year_bdg");
        });

        modelBuilder.Entity<ReturnPPay>(entity =>
        {
            entity.ToTable("Return_P_Pay");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DataP1)
                .HasMaxLength(200)
                .HasColumnName("DATA_P1");
            entity.Property(e => e.DataP2)
                .HasMaxLength(200)
                .HasColumnName("DATA_P2");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("project_code");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.YearBdg)
                .HasMaxLength(50)
                .HasColumnName("year_bdg");
        });

        modelBuilder.Entity<ReturnPPlanBdg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Return_P__3213E83FB5587A2A");

            entity.ToTable("Return_P_Plan_Bdg");

            entity.HasIndex(e => e.RefCode, "UQ__Return_P__9E4C4004A74B3ED2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataP1)
                .HasMaxLength(200)
                .HasColumnName("DATA_P1");
            entity.Property(e => e.ProjectCode).HasMaxLength(50);
            entity.Property(e => e.RefCode).HasColumnName("REF_CODE");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.YearBdg)
                .HasMaxLength(50)
                .HasColumnName("year_bdg");
        });

        modelBuilder.Entity<ReturnPPlanBdgSub>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Return_P__3213E83F311D04D1");

            entity.ToTable("Return_P_Plan_Bdg_Sub");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BdgType)
                .HasMaxLength(255)
                .HasColumnName("BDG_TYPE");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataPS1)
                .HasMaxLength(50)
                .HasColumnName("DATA_P_S1");
            entity.Property(e => e.DataPS2)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("DATA_P_S2");
            entity.Property(e => e.RefCode).HasColumnName("REF_CODE");
            entity.Property(e => e.RefCode2)
                .HasMaxLength(50)
                .HasColumnName("REF_CODE_2");
            entity.Property(e => e.SubCode).HasMaxLength(10);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.RefCodeNavigation).WithMany(p => p.ReturnPPlanBdgSubs)
                .HasPrincipalKey(p => p.RefCode)
                .HasForeignKey(d => d.RefCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Return_P___REF_C__08B54D69");
        });

        modelBuilder.Entity<ReturnPRisk>(entity =>
        {
            entity.ToTable("Return_P_Risk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DataP1)
                .HasMaxLength(200)
                .HasColumnName("DATA_P1");
            entity.Property(e => e.DataP2)
                .HasMaxLength(200)
                .HasColumnName("DATA_P2");
            entity.Property(e => e.DataP3)
                .HasMaxLength(200)
                .HasColumnName("DATA_P3");
            entity.Property(e => e.DataP4)
                .HasMaxLength(200)
                .HasColumnName("DATA_P4");
            entity.Property(e => e.DataP5)
                .HasMaxLength(200)
                .HasColumnName("DATA_P5");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(50)
                .HasColumnName("project_code");
            entity.Property(e => e.RefCode).HasColumnName("REF_CODE");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.YearBdg)
                .HasMaxLength(50)
                .HasColumnName("year_bdg");
        });

        modelBuilder.Entity<ReturnProject>(entity =>
        {
            entity.ToTable("Return_Project");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DataP1)
                .HasMaxLength(200)
                .HasColumnName("DATA_P1");
            entity.Property(e => e.DataP10).HasColumnName("DATA_P10");
            entity.Property(e => e.DataP11)
                .HasMaxLength(200)
                .HasColumnName("DATA_P11");
            entity.Property(e => e.DataP12)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DATA_P12");
            entity.Property(e => e.DataP13)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DATA_P13");
            entity.Property(e => e.DataP2)
                .HasMaxLength(200)
                .HasColumnName("DATA_P2");
            entity.Property(e => e.DataP3)
                .HasMaxLength(200)
                .HasColumnName("DATA_P3");
            entity.Property(e => e.DataP4)
                .HasColumnType("datetime")
                .HasColumnName("DATA_P4");
            entity.Property(e => e.DataP5)
                .HasColumnType("datetime")
                .HasColumnName("DATA_P5");
            entity.Property(e => e.DataP6).HasColumnName("DATA_P6");
            entity.Property(e => e.DataP7)
                .HasMaxLength(200)
                .HasColumnName("DATA_P7");
            entity.Property(e => e.DataP8).HasColumnName("DATA_P8");
            entity.Property(e => e.DataP9).HasColumnName("DATA_P9");
            entity.Property(e => e.ProjectCode).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.YearBdg)
                .HasMaxLength(50)
                .HasColumnName("year_bdg");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
