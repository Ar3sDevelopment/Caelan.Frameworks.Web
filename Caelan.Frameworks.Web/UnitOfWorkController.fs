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

    let uowCaller = UnitOfWorkCaller.UnitOfWork<'TUnitOfWork>()

    interface IUnitOfWorkCaller<'TUnitOfWork> with
        member __.UnitOfWork<'T>(call: Func<IUnitOfWork, 'T>) = uowCaller.UnitOfWork(call)
        member __.UnitOfWork(call: Action<IUnitOfWork>) = uowCaller.UnitOfWork(call)

        member this.Repository<'T, 'TRepository when 'TRepository :> IRepository>(call: Func<'TRepository, 'T>) =
            uowCaller.Repository(call)

        member this.Repository<'T, 'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>(call: Func<IRepository<'TEntity, 'TDTO>, 'T>) =
            uowCaller.Repository<'T, 'TEntity, 'TDTO>(call)

        member this.RepositoryList<'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>() =
            uowCaller.RepositoryList<'TEntity, 'TDTO>() :?> seq<'TDTO>

        member this.UnitOfWorkCallSaveChanges(call: Action<IUnitOfWork>) =
            uowCaller.UnitOfWorkCallSaveChanges(call)

        member this.Transaction(body: Action<IUnitOfWork>) =
            uowCaller.Transaction(body)
        member this.TransactionSaveChanges(body: Action<IUnitOfWork>) =
            uowCaller.TransactionSaveChanges(body)

    member this.UnitOfWork<'T>(call: Func<IUnitOfWork, 'T>) = (this :> IUnitOfWorkCaller<'TUnitOfWork>).UnitOfWork(call)
    member this.UnitOfWork(call: Action<IUnitOfWork>) = (this :> IUnitOfWorkCaller<'TUnitOfWork>).UnitOfWork(call)

    member this.Repository<'T, 'TRepository when 'TRepository :> IRepository>(call: Func<'TRepository, 'T>) =
        (this :> IUnitOfWorkCaller<'TUnitOfWork>).Repository(call)

    member this.Repository<'T, 'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>(call: Func<IRepository<'TEntity, 'TDTO>, 'T>) =
        (this :> IUnitOfWorkCaller<'TUnitOfWork>).Repository<'T, 'TEntity, 'TDTO>(call)

    member this.RepositoryList<'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>() =
        (this :> IUnitOfWorkCaller<'TUnitOfWork>).RepositoryList()

    member this.UnitOfWorkCallSaveChanges(call: Action<IUnitOfWork>) =
        (this :> IUnitOfWorkCaller<'TUnitOfWork>).UnitOfWorkCallSaveChanges(call)

    member this.Transaction(body: Action<IUnitOfWork>) =
        (this :> IUnitOfWorkCaller<'TUnitOfWork>).Transaction(body)

    member this.TransactionSaveChanges(body: Action<IUnitOfWork>) =
        (this :> IUnitOfWorkCaller<'TUnitOfWork>).TransactionSaveChanges(body)

    member __.Empty() = EmptyResult()

[<AbstractClass>]
type UnitOfWorkController<'TContext when 'TContext :> DbContext>() =
    class
    inherit GenericUnitOfWorkController<UnitOfWork<'TContext>>()
    end