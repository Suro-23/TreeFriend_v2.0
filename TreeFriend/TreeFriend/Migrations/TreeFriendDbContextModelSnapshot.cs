﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TreeFriend.Models;

namespace TreeFriend.Migrations
{
    [DbContext(typeof(TreeFriendDbContext))]
    partial class TreeFriendDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TreeFriend.Models.Entity.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.Hashtag", b =>
                {
                    b.Property<int>("HashtagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HashtagName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("HashtagId");

                    b.HasIndex("UserId");

                    b.ToTable("hashtags");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.HashtagDetail", b =>
                {
                    b.Property<int>("HashtagId")
                        .HasColumnType("int");

                    b.Property<int>("SkillPostId")
                        .HasColumnType("int");

                    b.HasKey("HashtagId", "SkillPostId");

                    b.HasIndex("SkillPostId");

                    b.ToTable("hashtagDetails");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.Lecture", b =>
                {
                    b.Property<int>("LectureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("EventTimeEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EventTimeStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImgPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Speaker")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpeakerImgPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Venue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LectureId");

                    b.HasIndex("UserId");

                    b.ToTable("Lectures");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LectureId")
                        .HasColumnType("int");

                    b.Property<bool>("OrderStatus")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("PayTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("PaymentStatus")
                        .HasColumnType("bit");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("LectureId");

                    b.HasIndex("UserId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.PersonalPost", b =>
                {
                    b.Property<int>("PersonalPostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PersonalPostId");

                    b.HasIndex("UserId");

                    b.ToTable("personalPosts");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.PersonalPostImage", b =>
                {
                    b.Property<int>("PersonalPostImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PersonalPostId")
                        .HasColumnType("int");

                    b.Property<string>("PostPhotoPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersonalPostImageId");

                    b.HasIndex("PersonalPostId");

                    b.ToTable("PersonalPostImages");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.PersonalPostMessage", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonalPostId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("MessageId");

                    b.ToTable("PersonalPostMessages");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.SkillPost", b =>
                {
                    b.Property<int>("SkillPostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("SkillPostId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("skillPosts");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.SkillPostMessage", b =>
                {
                    b.Property<int>("SkillPostMessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SkillPostId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("UserHeadshot")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SkillPostMessageId");

                    b.ToTable("skillPostMessages");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.SystemPost", b =>
                {
                    b.Property<int>("ArticleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("PicPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ArticleID");

                    b.HasIndex("UserId");

                    b.ToTable("SystemPost");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<bool>("UserLevel")
                        .HasColumnType("bit");

                    b.Property<bool>("UserStatus")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.UserDetail", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("HeadshotPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SelfIntrodution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Sex")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("usersDetail");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.Category", b =>
                {
                    b.HasOne("TreeFriend.Models.Entity.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.Hashtag", b =>
                {
                    b.HasOne("TreeFriend.Models.Entity.User", "User")
                        .WithMany("Hashtags")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.HashtagDetail", b =>
                {
                    b.HasOne("TreeFriend.Models.Entity.Hashtag", "Hashtag")
                        .WithMany("Hashtags")
                        .HasForeignKey("HashtagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TreeFriend.Models.Entity.SkillPost", "SkillPost")
                        .WithMany("Hashtags")
                        .HasForeignKey("SkillPostId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Hashtag");

                    b.Navigation("SkillPost");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.Lecture", b =>
                {
                    b.HasOne("TreeFriend.Models.Entity.User", "User")
                        .WithMany("Lectures")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.OrderDetail", b =>
                {
                    b.HasOne("TreeFriend.Models.Entity.Lecture", "Lecture")
                        .WithMany("OrderDetails")
                        .HasForeignKey("LectureId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TreeFriend.Models.Entity.User", "User")
                        .WithMany("OrderDetails")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Lecture");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.PersonalPost", b =>
                {
                    b.HasOne("TreeFriend.Models.Entity.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.PersonalPostImage", b =>
                {
                    b.HasOne("TreeFriend.Models.Entity.PersonalPost", "PersonalPost")
                        .WithMany("PersonalPostImages")
                        .HasForeignKey("PersonalPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PersonalPost");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.SkillPost", b =>
                {
                    b.HasOne("TreeFriend.Models.Entity.Category", "Category")
                        .WithMany("SkillPosts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TreeFriend.Models.Entity.User", "User")
                        .WithMany("SkillPosts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.SystemPost", b =>
                {
                    b.HasOne("TreeFriend.Models.Entity.User", "User")
                        .WithMany("SystemPosts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.UserDetail", b =>
                {
                    b.HasOne("TreeFriend.Models.Entity.User", "User")
                        .WithOne("UserDetail")
                        .HasForeignKey("TreeFriend.Models.Entity.UserDetail", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.Category", b =>
                {
                    b.Navigation("SkillPosts");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.Hashtag", b =>
                {
                    b.Navigation("Hashtags");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.Lecture", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.PersonalPost", b =>
                {
                    b.Navigation("PersonalPostImages");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.SkillPost", b =>
                {
                    b.Navigation("Hashtags");
                });

            modelBuilder.Entity("TreeFriend.Models.Entity.User", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Hashtags");

                    b.Navigation("Lectures");

                    b.Navigation("OrderDetails");

                    b.Navigation("Posts");

                    b.Navigation("SkillPosts");

                    b.Navigation("SystemPosts");

                    b.Navigation("UserDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
