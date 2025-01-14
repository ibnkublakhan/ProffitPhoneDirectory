using Microsoft.EntityFrameworkCore;
using ProffitPhoneDirectory._3cxModels;

namespace ProffitPhoneDirectory.Context;

public partial class DatabaseSingleContext : DbContext
{
    public DatabaseSingleContext()
    {
    }

    public DatabaseSingleContext(DbContextOptions<DatabaseSingleContext> options) : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Dn> Dns { get; set; }

    public virtual DbSet<Ringgroup> Ringgroups { get; set; }

    public virtual DbSet<Ringgroup2dn> Ringgroup2dns { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    //    => optionsBuilder.UseNpgsql( "name=_3cx_connect" );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_audit_log");

            entity.ToTable("audit_log");

            entity.HasIndex(e => e.Action, "audit_log_action");

            entity.HasIndex(e => e.ObjectType, "audit_log_object_type");

            entity.HasIndex(e => e.TimeStamp, "audit_log_time");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('saudit_log'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.Ip).HasColumnName("ip");
            entity.Property(e => e.NewData)
                .HasColumnType("character varying")
                .HasColumnName("new_data");
            entity.Property(e => e.ObjectName)
                .HasColumnType("character varying")
                .HasColumnName("object_name");
            entity.Property(e => e.ObjectType).HasColumnName("object_type");
            entity.Property(e => e.PrevData)
                .HasColumnType("character varying")
                .HasColumnName("prev_data");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("now()")
                .HasColumnName("time_stamp");
            entity.Property(e => e.UserName)
                .HasColumnType("character varying")
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<Dn>(entity =>
        {
            entity.HasKey(e => e.Iddn).HasName("dn_pkey");

            entity.ToTable("dn");

            entity.HasIndex(e => e.Fkidcalendar, "dn_fkidcalendar_key").IsUnique();

            entity.HasIndex(e => new { e.Fkidtenant, e.Value }, "dn_value_key").IsUnique();

            entity.HasIndex(e => e.Value, "dn_value_key2").IsUnique();

            entity.Property(e => e.Iddn)
                .HasDefaultValueSql("nextval('sqdn'::regclass)")
                .HasColumnName("iddn");
            entity.Property(e => e.Fkidcalendar).HasColumnName("fkidcalendar");
            entity.Property(e => e.Fkidtenant).HasColumnName("fkidtenant");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("1")
                .HasColumnName("status");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .HasColumnName("value");
        });

        modelBuilder.Entity<Ringgroup>(entity =>
        {
            entity.HasKey(e => e.Fkiddn).HasName("ringgroup_pkey");

            entity.ToTable("ringgroup");

            entity.HasIndex(e => e.Fkiddn, "index_ringgroup_fkiddn").IsUnique();

            entity.Property(e => e.Fkiddn)
                .ValueGeneratedNever()
                .HasColumnName("fkiddn");
            entity.Property(e => e.Cidprefix)
                .HasMaxLength(255)
                .HasColumnName("cidprefix");
            entity.Property(e => e.Fknoanswerdn).HasColumnName("fknoanswerdn");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Noanswertoout)
                .HasMaxLength(255)
                .HasDefaultValueSql("''::character varying")
                .HasColumnName("noanswertoout");
            entity.Property(e => e.Noanswertype).HasColumnName("noanswertype");
            entity.Property(e => e.Ringstrategy)
                .HasDefaultValueSql("1")
                .HasColumnName("ringstrategy");
            entity.Property(e => e.Ringtime).HasColumnName("ringtime");

            entity.HasOne(d => d.FkiddnNavigation).WithOne(p => p.RinggroupFkiddnNavigation)
                .HasForeignKey<Ringgroup>(d => d.Fkiddn)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("ringgroup_fkiddn_fkey");

            entity.HasOne(d => d.FknoanswerdnNavigation).WithMany(p => p.RinggroupFknoanswerdnNavigations)
                .HasForeignKey(d => d.Fknoanswerdn)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ringgroup_fknoanswerdn_fkey");
        });

        modelBuilder.Entity<Ringgroup2dn>(entity =>
        {
            entity.HasKey(e => e.Idringgroup2dn).HasName("ringgroup2dn_pkey");

            entity.ToTable("ringgroup2dn");

            entity.HasIndex(e => e.Fkiddn, "indesx_ringgroup2dn_fkiddn");

            entity.HasIndex(e => new { e.Fkidringgroup, e.Priority }, "ringgroup2dn_fkidringgroup_priority_key").IsUnique();

            entity.HasIndex(e => new { e.Fkidringgroup, e.Fkiddn }, "ringgroup2dn_key").IsUnique();

            entity.Property(e => e.Idringgroup2dn)
                .HasDefaultValueSql("nextval('sqringgroup2dn'::regclass)")
                .HasColumnName("idringgroup2dn");
            entity.Property(e => e.Fkiddn).HasColumnName("fkiddn");
            entity.Property(e => e.Fkidringgroup).HasColumnName("fkidringgroup");
            entity.Property(e => e.Priority).HasColumnName("priority");

            entity.HasOne(d => d.FkiddnNavigation).WithMany(p => p.Ringgroup2dns)
                .HasForeignKey(d => d.Fkiddn)
                .HasConstraintName("ringgroup2dn_fkiddn_fkey");

            entity.HasOne(d => d.FkidringgroupNavigation).WithMany(p => p.Ringgroup2dns)
                .HasForeignKey(d => d.Fkidringgroup)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("ringgroup2dn_fkidringgroup_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Fkidextension, "users_fkidextension_key").IsUnique();

            entity.Property(e => e.Iduser)
                .HasDefaultValueSql("nextval('squsers'::regclass)")
                .HasColumnName("iduser");
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Fkidextension).HasColumnName("fkidextension");
            entity.Property(e => e.Fwduser)
                .HasMaxLength(255)
                .HasColumnName("fwduser");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");
            entity.Property(e => e.Selfidprompt)
                .HasMaxLength(255)
                .HasColumnName("selfidprompt");
            entity.Property(e => e.Sipid)
                .HasMaxLength(255)
                .HasColumnName("sipid");
        });
        modelBuilder.HasSequence("saudit_log");
        modelBuilder.HasSequence("scallcent_ag_dropped_calls").StartsAt(599L);
        modelBuilder.HasSequence("scallcent_ag_queuestatus").StartsAt(599L);
        modelBuilder.HasSequence("scallcent_queuecalls").StartsAt(774L);
        modelBuilder.HasSequence("schat_conversation");
        modelBuilder.HasSequence("schat_conversation_member");
        modelBuilder.HasSequence("schat_dest2chat_mess").StartsAt(599L);
        modelBuilder.HasSequence("schat_history").StartsAt(599L);
        modelBuilder.HasSequence("schat_history_mess").StartsAt(599L);
        modelBuilder.HasSequence("schat_message");
        modelBuilder.HasSequence("schat_participant");
        modelBuilder.HasSequence("seq_qcallback");
        modelBuilder.HasSequence("seqmeetingsession");
        modelBuilder.HasSequence("seqs_cors_settings");
        modelBuilder.HasSequence("seqs_lastlogin");
        modelBuilder.HasSequence("seqs_push");
        modelBuilder.HasSequence("seqs_reporterconfig");
        modelBuilder.HasSequence("seqs_sbc");
        modelBuilder.HasSequence("seqs_scheduledconf");
        modelBuilder.HasSequence("seqs_voicemail");
        modelBuilder.HasSequence("seqs_websitelink");
        modelBuilder.HasSequence("seqs_wmpolling");
        modelBuilder.HasSequence("seqs_wmqueue").StartsAt(8L);
        modelBuilder.HasSequence("sq_chattemplate");
        modelBuilder.HasSequence("sq_chattemplate_category");
        modelBuilder.HasSequence("sq_chattemplate_language");
        modelBuilder.HasSequence("sq_mpch_14");
        modelBuilder.HasSequence("sq_s_reportrequest");
        modelBuilder.HasSequence("sq_s_wakeupcalls");
        modelBuilder.HasSequence("sqannouncement");
        modelBuilder.HasSequence("sqaudiofeed");
        modelBuilder.HasSequence("sqaudiofeedcontent");
        modelBuilder.HasSequence("sqblacklist");
        modelBuilder.HasSequence("sqcalendar");
        modelBuilder.HasSequence("sqcalendarhours");
        modelBuilder.HasSequence("sqcalldetail");
        modelBuilder.HasSequence("sqcl_calls");
        modelBuilder.HasSequence("sqcl_participants");
        modelBuilder.HasSequence("sqcl_party_info");
        modelBuilder.HasSequence("sqcl_segments");
        modelBuilder.HasSequence("sqcodec");
        modelBuilder.HasSequence("sqcodec2gateway");
        modelBuilder.HasSequence("sqconferenceplaceproperties");
        modelBuilder.HasSequence("sqdevinfo").StartsAt(3L);
        modelBuilder.HasSequence("sqdn");
        modelBuilder.HasSequence("sqdngrp");
        modelBuilder.HasSequence("sqdnprop");
        modelBuilder.HasSequence("sqdnrange2rule");
        modelBuilder.HasSequence("sqeventlog");
        modelBuilder.HasSequence("sqextblf");
        modelBuilder.HasSequence("sqextdevice");
        modelBuilder.HasSequence("sqextensionforward");
        modelBuilder.HasSequence("sqextensionrule2profile");
        modelBuilder.HasSequence("sqfwdprofile");
        modelBuilder.HasSequence("sqgateway");
        modelBuilder.HasSequence("sqgatewaytplvalue");
        modelBuilder.HasSequence("sqgwpar2gateway");
        modelBuilder.HasSequence("sqgwpar2parvalue");
        modelBuilder.HasSequence("sqholiday");
        modelBuilder.HasSequence("sqinboundrule");
        modelBuilder.HasSequence("sqivrmenuitem");
        modelBuilder.HasSequence("sqoutboundroute");
        modelBuilder.HasSequence("sqoutboundrule");
        modelBuilder.HasSequence("sqparameter");
        modelBuilder.HasSequence("sqphonebook");
        modelBuilder.HasSequence("sqphonedevice");
        modelBuilder.HasSequence("sqprompt");
        modelBuilder.HasSequence("sqpromptlang");
        modelBuilder.HasSequence("sqprstorage");
        modelBuilder.HasSequence("sqqueue2dn");
        modelBuilder.HasSequence("sqqueue2managerdn");
        modelBuilder.HasSequence("sqrecording_participant");
        modelBuilder.HasSequence("sqrecordings");
        modelBuilder.HasSequence("sqringgroup2dn");
        modelBuilder.HasSequence("sqrule2grp");
        modelBuilder.HasSequence("sqrule2line");
        modelBuilder.HasSequence("sqtenant");
        modelBuilder.HasSequence("sqtenantprop");
        modelBuilder.HasSequence("squsers");
        modelBuilder.HasSequence("sqvoicemail");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
