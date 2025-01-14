using Microsoft.EntityFrameworkCore;
using ProffitPhoneDirectory.Models;

namespace ProffitPhoneDirectory.Context;

public interface IProffitCorporateInfoContext
{
    public DbSet<Branch> Branch { get; set; }
    public DbSet<Position> Position { get; set; }
    public DbSet<EmployeePosition> EmployeePosition { get; set; }
    public DbSet<GroupBranch> GroupBranch { get; set; }
    public DbSet<PhoneOuter> PhoneOuter { get; set; }
    public DbSet<ChangeLog> ChangeLogs { get; set; }
    public DbSet<Users> User { get; set; }
}

public class ProffitCorporateInfoContext : DbContext, IProffitCorporateInfoContext
{
    public DbSet<Branch> Branch { get; set; }
    public DbSet<Position> Position { get; set; }
    public DbSet<EmployeePosition> EmployeePosition { get; set; }
    public DbSet<GroupBranch> GroupBranch { get; set; }
    public DbSet<ChangeLog> ChangeLogs { get; set; }
    public DbSet<Users> User { get; set; }
    public DbSet<PhoneOuter> PhoneOuter { get; set; }
    //public DbSet<GroupPhone> GroupPhone { get; set; }

    public ProffitCorporateInfoContext( DbContextOptions<ProffitCorporateInfoContext> options ) : base( options ) { }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        modelBuilder
        .Entity<Users>( b =>
        {
            b.Property( e => e.CreatedAt )
                .HasColumnType( "timestamp without time zone" );
            b.Property( e => e.LastLogin )
                .HasColumnType( "timestamp without time zone" );
        } )
        .Entity<ChangeLog>( b =>
        {
            b.Property( e => e.ChangeDate )
                .HasColumnType( "timestamp without time zone" );
        } )
        .Entity<Position>( b =>
        {
            // Добавление автоинкрементации столбцу Id
            b.Property( e => e.Id )
                .ValueGeneratedOnAdd();
            //b.Property<int>( "Id" );
            b.HasIndex( e => e.Id )
                .IsUnique();
        } )
        .Entity<Branch>( b =>
        {
            b.Property( e => e.Id )
                .ValueGeneratedOnAdd();
            b.HasIndex( e => e.Id )
                .IsUnique();
        } )
        .Entity<EmployeePosition>( b =>
        {
            b.Property<Guid>( "Id" );
            b.HasIndex( "Id" )
                .IsUnique();
        } )
        .Entity<GroupBranch>( b =>
        {
            b.Property<Guid>( "Id" );
            b.HasIndex( "Id" )
                .IsUnique();
        } )
        //.Entity<GroupPhone>( b =>
        //{
        //    b.Property<Guid>( "Id" );
        //    b.HasIndex( "Id" )
        //        .IsUnique();
        //} )
        .Entity<PhoneOuter>( b =>
        {
            b.Property<Guid>( "Id" );
            b.HasIndex( "Id" )
                .IsUnique();
        } )

        //{
        //    // Добавление автоинкрементации столбцу Id
        //    b.Property( e => e.Id )
        //        .ValueGeneratedOnAdd();
        //    //b.Property<int>( "Id" );
        //    b.HasIndex( e => e.Id )
        //        .IsUnique();
        //} )
        ;

        base.OnModelCreating( modelBuilder );
    }
}
