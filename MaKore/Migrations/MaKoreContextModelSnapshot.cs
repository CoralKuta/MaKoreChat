// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MaKore.Migrations
{
    [DbContext(typeof(MaKoreContext))]
    partial class MaKoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MaKore.Models.Conversation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("RemoteUserId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RemoteUserId")
                        .IsUnique();

                    b.HasIndex("UserName");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("MaKore.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConversationId")
                        .HasColumnType("int");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MaKore.Models.Rating", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Feedback")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("MaKore.Models.RemoteUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ConversationId")
                        .HasColumnType("int");

                    b.Property<string>("NickName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Server")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RemoteUsers");
                });

            modelBuilder.Entity("MaKore.Models.User", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NickName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MaKore.Models.Conversation", b =>
                {
                    b.HasOne("MaKore.Models.RemoteUser", "RemoteUser")
                        .WithOne("Conversation")
                        .HasForeignKey("MaKore.Models.Conversation", "RemoteUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MaKore.Models.User", "User")
                        .WithMany("ConversationList")
                        .HasForeignKey("UserName");

                    b.Navigation("RemoteUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MaKore.Models.Message", b =>
                {
                    b.HasOne("MaKore.Models.Conversation", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("MaKore.Models.Conversation", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("MaKore.Models.RemoteUser", b =>
                {
                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("MaKore.Models.User", b =>
                {
                    b.Navigation("ConversationList");
                });
#pragma warning restore 612, 618
        }
    }
}
