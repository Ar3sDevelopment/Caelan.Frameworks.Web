namespace Caelan.Frameworks.Web.Classes

open System
open System.Data.Entity
open System.Linq
open System.Web.Mvc
open Caelan.Frameworks.BIZ.Interfaces
open Caelan.Frameworks.BIZ.Classes

[<AbstractClass>]
type GenericUnitOfWorkController<'TUnitOfWork when 'TUnitOfWork :> IUnitOfWork and 'TUnitOfWork :> IDisposable and 'TUnitOfWork : (new : unit -> 'TUnitOfWork)>() = 
    inherit Controller()

    let uowCaller = GenericUnitOfWorkCaller<'TUnitOfWork>()
    member __.UnitOfWork<'T>(call: Func<IUnitOfWork, 'T>) = uowCaller.UnitOfWork<'T>(call)
    member __.UnitOfWork(call: Action<IUnitOfWork>) = uowCaller.UnitOfWork(call)

    member this.Repository<'T, 'TRepository when 'TRepository :> IRepository>(call: Func<'TRepository, 'T>) =
        uowCaller.Repository<'T, 'TRepository>(call)

    member this.Repository<'T, 'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>(call: Func<IRepository<'TEntity, 'TDTO>, 'T>) =
        uowCaller.Repository<'T, 'TEntity, 'TDTO>(call)

    member this.RepositoryList<'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>() =
        uowCaller.RepositoryList<'TEntity, 'TDTO>()

    member this.UnitOfWorkCallSaveChanges(call: Action<IUnitOfWork>) =
        uowCaller.UnitOfWorkCallSaveChanges(call)

    member __.Empty() = EmptyResult()

[<AbstractClass>]
type UnitOfWorkController<'TContext when 'TContext :> DbContext>() =
    class
    inherit GenericUnitOfWorkController<UnitOfWork<'TContext>>()
    end