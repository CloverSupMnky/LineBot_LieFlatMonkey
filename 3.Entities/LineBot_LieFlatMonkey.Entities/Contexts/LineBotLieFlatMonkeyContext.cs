using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LineBot_LieFlatMonkey.Entities.Models;

namespace LineBot_LieFlatMonkey.Entities.Contexts
{
    public partial class LineBotLieFlatMonkeyContext : DbContext
    {
        public virtual DbSet<EnglishSentence> EnglishSentences { get; set; }
        public virtual DbSet<QuickReply> QuickReplies { get; set; }
        public virtual DbSet<TarotCard> TarotCards { get; set; }

        public LineBotLieFlatMonkeyContext(DbContextOptions<LineBotLieFlatMonkeyContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnglishSentence>(entity =>
            {
                entity.HasKey(e => e.SeqNo)
                    .HasName("EnglishSentence_pkey");

                entity.ToTable("EnglishSentence");

                entity.HasComment("英文短句資料表");

                entity.Property(e => e.SeqNo)
                    .HasComment("流水號")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(null, null, null, 999L, null, null);

                entity.Property(e => e.Sentence)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("英文句子");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("句子出處");

                entity.Property(e => e.SourceType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("句子出處類型");

                entity.Property(e => e.Translation)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("英文翻譯");
            });

            modelBuilder.Entity<QuickReply>(entity =>
            {
                entity.HasKey(e => new { e.ItemType, e.ItemValue })
                    .HasName("QuickReply_pkey");

                entity.ToTable("QuickReply");

                entity.HasComment("QuickReply 使用項目");

                entity.Property(e => e.ItemType)
                    .HasMaxLength(10)
                    .HasComment("項目類別");

                entity.Property(e => e.ItemValue)
                    .HasMaxLength(20)
                    .HasComment("項目值");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Sort).HasComment("排序");
            });

            modelBuilder.Entity<TarotCard>(entity =>
            {
                entity.HasKey(e => e.SeqNo)
                    .HasName("TarotCard_pkey");

                entity.ToTable("TarotCard");

                entity.HasComment("塔羅牌資料表");

                entity.Property(e => e.SeqNo)
                    .HasComment("流水號")
                    .HasIdentityOptions(null, null, null, 99L, null, null);

                entity.Property(e => e.DescDaily)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Desc_Daily")
                    .HasComment("每日運勢牌意");

                entity.Property(e => e.DescRev)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Desc_Rev")
                    .HasComment("逆位牌意");

                entity.Property(e => e.DescUp)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Desc_Up")
                    .HasComment("正位牌意");

                entity.Property(e => e.ImageUrlRev)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ImageUrl_Rev")
                    .HasComment("逆位牌圖片連結");

                entity.Property(e => e.ImageUrlUp)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ImageUrl_Up")
                    .HasComment("正位牌圖片連結");

                entity.Property(e => e.Mean)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasComment("意義");

                entity.Property(e => e.MeanRev)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Mean_Rev")
                    .HasComment("逆位牌意關鍵字");

                entity.Property(e => e.MeanUp)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Mean_Up")
                    .HasComment("正位牌意關鍵字");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasComment("中文名稱");

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Name_EN")
                    .HasComment("英文名稱");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
