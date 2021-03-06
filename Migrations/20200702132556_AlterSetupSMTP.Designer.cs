// <auto-generated />
using System;
using ABAC.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ABAC.Migrations
{
    [DbContext(typeof(SpuContext))]
    [Migration("20200702132556_AlterSetupSMTP")]
    partial class AlterSetupSMTP
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ABAC.Models.Admin", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdminCode")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Create_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Create_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Update_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Update_On")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("ABAC.Models.AdminRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AdminID")
                        .HasColumnType("int");

                    b.Property<int?>("RoleID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("AdminID");

                    b.HasIndex("RoleID");

                    b.ToTable("AdminRoles");
                });

            modelBuilder.Entity("ABAC.Models.AuditLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Create_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Create_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("ID");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("ABAC.Models.Aumphur", b =>
                {
                    b.Property<int>("AumphurID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AumphurCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AumphurName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AumphurNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProvinceCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProvinceID")
                        .HasColumnType("int");

                    b.HasKey("AumphurID");

                    b.HasIndex("ProvinceID");

                    b.ToTable("Aumphurs");
                });

            modelBuilder.Entity("ABAC.Models.Guest", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ADCreated")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("ApprovalBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ApprovalDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ApprovalStatus")
                        .HasColumnType("int");

                    b.Property<int?>("AumphurID")
                        .HasColumnType("int");

                    b.Property<string>("Create_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Create_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("GuestCode")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("IDCard")
                        .HasColumnType("nvarchar(14)")
                        .HasMaxLength(14);

                    b.Property<string>("Job")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("MacAddress1")
                        .HasColumnType("nvarchar(17)")
                        .HasMaxLength(17);

                    b.Property<string>("MacAddress2")
                        .HasColumnType("nvarchar(17)")
                        .HasMaxLength(17);

                    b.Property<string>("MacAddress3")
                        .HasColumnType("nvarchar(17)")
                        .HasMaxLength(17);

                    b.Property<bool>("OTPVerify")
                        .HasColumnType("bit");

                    b.Property<int?>("OUID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("OpenDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(5)")
                        .HasMaxLength(5);

                    b.Property<int?>("ProvinceID")
                        .HasColumnType("int");

                    b.Property<int>("RegisterType")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("TumbonID")
                        .HasColumnType("int");

                    b.Property<string>("Update_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Update_On")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("AumphurID");

                    b.HasIndex("OUID");

                    b.HasIndex("ProvinceID");

                    b.HasIndex("TumbonID");

                    b.HasIndex("UserID");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("ABAC.Models.GuestImport", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("ApprovalBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ApprovalDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ApprovalStatus")
                        .HasColumnType("int");

                    b.Property<int?>("AumphurID")
                        .HasColumnType("int");

                    b.Property<string>("Create_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Create_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("GuestCode")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("IDCard")
                        .HasColumnType("nvarchar(14)")
                        .HasMaxLength(14);

                    b.Property<int>("ImportLine")
                        .HasColumnType("int");

                    b.Property<string>("ImportRemark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ImportVerify")
                        .HasColumnType("bit");

                    b.Property<string>("Job")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("MacAddress1")
                        .HasColumnType("nvarchar(17)")
                        .HasMaxLength(17);

                    b.Property<string>("MacAddress2")
                        .HasColumnType("nvarchar(17)")
                        .HasMaxLength(17);

                    b.Property<string>("MacAddress3")
                        .HasColumnType("nvarchar(17)")
                        .HasMaxLength(17);

                    b.Property<int?>("OUID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("OpenDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(5)")
                        .HasMaxLength(5);

                    b.Property<int?>("ProvinceID")
                        .HasColumnType("int");

                    b.Property<int>("RegisterType")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("TumbonID")
                        .HasColumnType("int");

                    b.Property<string>("Update_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Update_On")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("AumphurID");

                    b.HasIndex("OUID");

                    b.HasIndex("ProvinceID");

                    b.HasIndex("TumbonID");

                    b.ToTable("GuestImports");
                });

            modelBuilder.Entity("ABAC.Models.OTP", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Create_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("Expire_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("MobileNo")
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("OTPNumber")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("RefNo")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<int>("SendMessageType")
                        .HasColumnType("int");

                    b.Property<bool>("Used")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("ID");

                    b.ToTable("OTPs");
                });

            modelBuilder.Entity("ABAC.Models.OU", b =>
                {
                    b.Property<int>("OUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Create_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Create_On")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Editable")
                        .HasColumnType("bit");

                    b.Property<string>("OUDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OUName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Update_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Update_On")
                        .HasColumnType("datetime2");

                    b.HasKey("OUID");

                    b.ToTable("OUs");
                });

            modelBuilder.Entity("ABAC.Models.Province", b =>
                {
                    b.Property<int>("ProvinceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProvinceCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProvinceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProvinceNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProvinceID");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("ABAC.Models.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Create_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Create_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int?>("OUID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Update_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Update_On")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("OUID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ABAC.Models.Setup", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Base")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("Create_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Create_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("Host")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<bool>("OpenSMS")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("Port")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("SMSGatewayUrl")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("SMSPassword")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("SMSSender")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("SMSUsername")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("SMTP_From")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("SMTP_Password")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("SMTP_Port")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("SMTP_SSL")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("SMTP_Server")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("SMTP_Username")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("Update_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Update_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("ID");

                    b.ToTable("Setups");
                });

            modelBuilder.Entity("ABAC.Models.Tumbon", b =>
                {
                    b.Property<int>("TumbonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AumphurCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AumphurID")
                        .HasColumnType("int");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProvinceCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProvinceID")
                        .HasColumnType("int");

                    b.Property<string>("TumbonCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TumbonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TumbonNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TumbonID");

                    b.HasIndex("AumphurID");

                    b.HasIndex("ProvinceID");

                    b.ToTable("Tumbons");
                });

            modelBuilder.Entity("ABAC.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Create_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Create_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Update_By")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("Update_On")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ABAC.Models.Admin", b =>
                {
                    b.HasOne("ABAC.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ABAC.Models.AdminRole", b =>
                {
                    b.HasOne("ABAC.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ABAC.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ABAC.Models.Aumphur", b =>
                {
                    b.HasOne("ABAC.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ABAC.Models.Guest", b =>
                {
                    b.HasOne("ABAC.Models.Aumphur", "Aumphur")
                        .WithMany()
                        .HasForeignKey("AumphurID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ABAC.Models.OU", "OU")
                        .WithMany()
                        .HasForeignKey("OUID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ABAC.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ABAC.Models.Tumbon", "Tumbon")
                        .WithMany()
                        .HasForeignKey("TumbonID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ABAC.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ABAC.Models.GuestImport", b =>
                {
                    b.HasOne("ABAC.Models.Aumphur", "Aumphur")
                        .WithMany()
                        .HasForeignKey("AumphurID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ABAC.Models.OU", "OU")
                        .WithMany()
                        .HasForeignKey("OUID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ABAC.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ABAC.Models.Tumbon", "Tumbon")
                        .WithMany()
                        .HasForeignKey("TumbonID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ABAC.Models.Role", b =>
                {
                    b.HasOne("ABAC.Models.OU", "OU")
                        .WithMany()
                        .HasForeignKey("OUID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ABAC.Models.Tumbon", b =>
                {
                    b.HasOne("ABAC.Models.Aumphur", "Aumphur")
                        .WithMany()
                        .HasForeignKey("AumphurID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ABAC.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceID")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
