﻿// <auto-generated />
using System;
using ClinicalWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClinicalWebApplication.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230124151143_initial_6")]
    partial class initial6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClinicalWebApplication.Models.Actions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Action_Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Actual_Close_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Meeting_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Study")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StudyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Targeted_Close_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.Decisions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DecisionAttachment_path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Decisions_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Risk_Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Risk_Impact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Study")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StudyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Decisions");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.Informativs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Actual_Close_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Informative_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Informative_Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Study")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StudyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Targeted_Close_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Informativs");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.InvestigatorSites", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Clinical_Trial_Agreements")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Facility_Institution_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<int?>("Monitoring_Frequency")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Qualification_Date")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Recruitment_Target")
                        .HasColumnType("float");

                    b.Property<double?>("Site_No")
                        .HasColumnType("float");

                    b.Property<int>("StudyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("StudyId");

                    b.ToTable("InvestigatorSites");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.Organizations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.ProtocolDeviations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Action_Resolution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ethics_Notified")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InvestigatorSiteId")
                        .HasColumnType("int");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Primary_CRA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reportable_to_Ethics")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reported_by_Investigator_Site")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Significance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Sponsor_Notified")
                        .HasColumnType("datetime2");

                    b.Property<int?>("StudyId")
                        .HasColumnType("int");

                    b.Property<double?>("Subject")
                        .HasColumnType("float");

                    b.Property<string>("Subject_Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject_Visit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("protocol_type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InvestigatorSiteId");

                    b.HasIndex("StudyId");

                    b.ToTable("ProtocolDeviations");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.StaffInvestigatorSite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Full_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InvestigatorSiteId")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InvestigatorSiteId");

                    b.ToTable("StaffInvestigatorSites");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.Study", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Actual_Study_Start_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Allocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Condition_Or_Disease")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Enrollment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Estimated_Primary_Completion_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Estimated_Study_Completion_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Intervention_Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Intervention_treatment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Masking")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NCT_No")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("Phase")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Primary_Purpose")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Protocol_Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Study_Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Visit_Frequency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Study");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Organization")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StudyIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.InvestigatorSites", b =>
                {
                    b.HasOne("ClinicalWebApplication.Models.Study", "Study")
                        .WithMany("InvestigatorSites")
                        .HasForeignKey("StudyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Study");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.Organizations", b =>
                {
                    b.HasOne("ClinicalWebApplication.Models.User", "Creater")
                        .WithMany("Organizations")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creater");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.ProtocolDeviations", b =>
                {
                    b.HasOne("ClinicalWebApplication.Models.InvestigatorSites", "InvestigatorSite")
                        .WithMany("ProtocolDeviations")
                        .HasForeignKey("InvestigatorSiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicalWebApplication.Models.Study", null)
                        .WithMany("ProtocolDeviations")
                        .HasForeignKey("StudyId");

                    b.Navigation("InvestigatorSite");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.StaffInvestigatorSite", b =>
                {
                    b.HasOne("ClinicalWebApplication.Models.InvestigatorSites", "InvestigatorSite")
                        .WithMany("StaffInvestigatorSites")
                        .HasForeignKey("InvestigatorSiteId");

                    b.Navigation("InvestigatorSite");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.Study", b =>
                {
                    b.HasOne("ClinicalWebApplication.Models.Organizations", "Organization")
                        .WithMany("Studies")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.InvestigatorSites", b =>
                {
                    b.Navigation("ProtocolDeviations");

                    b.Navigation("StaffInvestigatorSites");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.Organizations", b =>
                {
                    b.Navigation("Studies");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.Study", b =>
                {
                    b.Navigation("InvestigatorSites");

                    b.Navigation("ProtocolDeviations");
                });

            modelBuilder.Entity("ClinicalWebApplication.Models.User", b =>
                {
                    b.Navigation("Organizations");
                });
#pragma warning restore 612, 618
        }
    }
}
