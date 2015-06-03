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

    interface IUnitOfWorkCaller with
        member __.UnitOfWork<'T>(call: Func<IUnitOfWork, 'T>) = uowCaller.UnitOfWork(call)
        member __.UnitOfWork(call: Action<IUnitOfWork>) = uowCaller.UnitOfWork(call)

        member this.CustomRepository<'T, 'TRepository when 'TRepository :> IRepository>(call: Func<'TRepository, 'T>) =
            uowCaller.CustomRepository(call)

        member this.Repository<'T, 'TEntity when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null>(call: Func<IRepository<'TEntity>, 'T>) =
            uowCaller.Repository<'T, 'TEntity>(call)

        member this.Repository<'T, 'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>(call: Func<IRepository<'TEntity, 'TDTO>, 'T>) =
            uowCaller.Repository<'T, 'TEntity, 'TDTO>(call)

        member this.RepositoryList<'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>() =
            uowCaller.RepositoryList<'TEntity, 'TDTO>()

        member this.UnitOfWorkSaveChanges(call: Action<IUnitOfWork>) =
            uowCaller.UnitOfWorkSaveChanges(call)

        member this.Transaction(body: Action<IUnitOfWork>) =
            uowCaller.Transaction(body)
        member this.TransactionSaveChanges(body: Action<IUnitOfWork>) =
            uowCaller.TransactionSaveChanges(body)

    member this.UnitOfWork<'T>(call: Func<IUnitOfWork, 'T>) = (this :> IUnitOfWorkCaller).UnitOfWork(call)
    member this.UnitOfWork(call: Action<IUnitOfWork>) = (this :> IUnitOfWorkCaller).UnitOfWork(call)

    member this.CustomRepository<'T, 'TRepository when 'TRepository :> IRepository>(call: Func<'TRepository, 'T>) =
        (this :> IUnitOfWorkCaller).CustomRepository(call)

    member this.Repository<'T, 'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>(call: Func<IRepository<'TEntity, 'TDTO>, 'T>) =
        (this :> IUnitOfWorkCaller).Repository<'T, 'TEntity, 'TDTO>(call)

    member this.RepositoryList<'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>() =
        (this :> IUnitOfWorkCaller).RepositoryList()

    member this.UnitOfWorkCallSaveChanges(call: Action<IUnitOfWork>) =
        (this :> IUnitOfWorkCaller).UnitOfWorkSaveChanges(call)

    member this.Transaction(body: Action<IUnitOfWork>) =
        (this :> IUnitOfWorkCaller).Transaction(body)

    member this.TransactionSaveChanges(body: Action<IUnitOfWork>) =
        (this :> IUnitOfWorkCaller).TransactionSaveChanges(body)

    member __.Empty() = EmptyResult()

[<AbstractClass>]
type UnitOfWorkController<'TContext when 'TContext :> DbContext>() =
    class
    inherit GenericUnitOfWorkController<UnitOfWork<'TContext>>()
    end