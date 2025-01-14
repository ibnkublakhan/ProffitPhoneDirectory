using Microsoft.EntityFrameworkCore;
using ProffitPhoneDirectory.Context;
using ProffitPhoneDirectory.Models;
using System.Net;

namespace ProffitPhoneDirectory.Repositories;

public class BranchRepository : BaseRepository
{
    protected readonly GroupRepository groupRepository;

    public BranchRepository( ProffitCorporateInfoContext proffitContext, IConfiguration _configuration, DatabaseSingleContext single_context, GroupRepository repository ) : base( proffitContext, _configuration, single_context )
    {
        this.groupRepository = repository;
    }

    internal List<BranchOutput> Get()
    {
        List<BranchOutput> branchesOutput = new List<BranchOutput>();

        List<Branch> branches  = proffitContext.Branch.ToList();

        branchesOutput = branches.Select( b =>
        new BranchOutput()
        {
            Id = b.Id,
            Name = b.Name,
            GroupsId = proffitContext.GroupBranch.Include( p => p.Branch )
                .Where( p => p.Branch.Id == b.Id )
                .Select( p => p.GroupsId ).ToList()
        } ).ToList();

        return branchesOutput;
    }

    internal void Create( string name )
    {
        if ( name == null ) throw new ResponseErrors( $"Поле 'name' пустое", HttpStatusCode.BadRequest );
        if ( proffitContext.Branch.Any( b => b.Name == name ) ) throw new ResponseErrors( $"уже существует такое имя подразделения : {name}", HttpStatusCode.Forbidden );

        var v = new Branch { Name = name };
        proffitContext.Branch.Add( v );

        try
        {
            proffitContext.SaveChanges();
        }
        catch ( Exception e )
        {
            Console.WriteLine( e.Message );
            throw;
        }
    }

    internal void Delete( int id )
    {
        Branch? branch = proffitContext.Branch.FirstOrDefault( p => p.Id == id );
        if ( branch == null ) throw new ResponseErrors( $"Подразделения {id} не существует", HttpStatusCode.BadRequest );

        proffitContext.Branch.Remove( branch );

        proffitContext.SaveChanges();
    }

    internal void AddGroups( int branchId, List<int> groupsId )
    {
        Branch? branch = GetBranch( branchId );

        groupsId.ForEach( action: groupId =>
        {
            GroupExistenceCheck( groupId );

            GroupBranch? groupBranch = GetGropBrach( groupId );
            // если связь с должностью уже есть - пропускаем
            if ( groupBranch != null ) return;

            // создаем новую связь
            groupBranch = new GroupBranch { GroupsId = groupId };
            proffitContext.GroupBranch.Add( groupBranch );

            // изменим отделение
            groupBranch.Branch = branch;
        } );

        proffitContext.SaveChanges();
    }

    internal void RemoveGroups( int branchId, List<int> groupsId )
    {
        Branch? branch = GetBranch( branchId );

        bool noContent = true;

        groupsId.ForEach( action: groupId =>
        {
            GroupExistenceCheck( groupId );

            GroupBranch? groupBranch = GetGropBrach( groupId );
            if ( groupBranch == null ) return;

            noContent = false;

            proffitContext.Remove( groupBranch );

        } );

        if ( noContent ) throw new ResponseErrors( $"не одна группы из списка {groupsId} не привязана к подразделению {branchId}", HttpStatusCode.BadRequest );

        proffitContext.SaveChanges();
    }

    private GroupBranch? GetGropBrach( int groupId )
    {
        return proffitContext.GroupBranch.FirstOrDefault( e => e.GroupsId == groupId );
    }

    private Branch GetBranch( int branchId )
    {
        Branch? branch = proffitContext.Branch.FirstOrDefault( b => b.Id == branchId );
        if ( branch == null ) throw new ResponseErrors( $"подразделение {branchId} не существует", HttpStatusCode.BadRequest );
        return branch;
    }

    private void GroupExistenceCheck( int groupId )
    {
        ProffitGroup? group = groupRepository.Get().FirstOrDefault( b => b.Id == groupId );
        if ( group is null ) throw new ResponseErrors( $"Группа не существует {groupId}", HttpStatusCode.BadRequest );
    }
}
