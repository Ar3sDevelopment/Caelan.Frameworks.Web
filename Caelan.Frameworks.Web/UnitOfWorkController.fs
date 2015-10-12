namespace Caelan.Frameworks.Web.Classes

open System
open System.Data.Entity
open System.Linq
open System.Web.Mvc
open Caelan.Frameworks.BIZ.Interfaces
open Caelan.Frameworks.BIZ.Classes

type ICRUDController<'TModel, 'TKey when 'TKey : equality> =
    abstract member Index : unit -> ActionResult
    abstract member Detail : 'TKey -> ActionResult
    abstract member Edit : 'TKey -> ActionResult
    abstract member Edit : 'TModel -> ActionResult
    abstract member Create : unit -> ActionResult
    abstract member Create : 'TModel -> ActionResult
    abstract member Delete : 'TKey -> ActionResult
    abstract member Delete : 'TModel -> ActionResult

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
    inherit GenericUnitOfWorkController<UnitOfWork<'TContext>>()

[<AbstractClass>]
type UnitOfWorkController<'TContext, 'TModel, 'TKey when 'TContext :> DbContext and 'TKey : equality>() =
    inherit UnitOfWorkController<'TContext>()

    interface ICRUDController<'TModel, 'TKey> with
        member this.Index() = this.Index()
        member this.Detail id = this.Detail id
        member this.Edit (id: 'TKey) = this.Edit id
        member this.Edit (model: 'TModel) = this.Edit model
        member this.Create() = this.Create()
        member this.Create model = this.Create model
        member this.Delete (id: 'TKey) = this.Delete id
        member this.Delete (model: 'TModel) = this.Delete model

    abstract member Index : unit -> ActionResult

    abstract member Detail : 'TKey -> ActionResult
    abstract member Edit : 'TKey -> ActionResult
    [<HttpPost>]
    abstract member Edit : 'TModel -> ActionResult
    abstract member Create : unit -> ActionResult
    [<HttpPost>]
    abstract member Create : 'TModel -> ActionResult
    abstract member Delete : 'TKey -> ActionResult
    [<HttpPost>]
    abstract member Delete : 'TModel -> ActionResult

[<AbstractClass>]
type GenericUnitOfWorkController<'TUnitOfWork, 'TModel, 'TKey when 'TUnitOfWork :> IUnitOfWork and 'TUnitOfWork :> IDisposable and 'TUnitOfWork : (new : unit -> 'TUnitOfWork) and 'TKey : equality>() =
    inherit GenericUnitOfWorkController<'TUnitOfWork>()

        interface ICRUDController<'TModel, 'TKey> with
            member this.Index() = this.Index()
            member this.Detail id = this.Detail id
            member this.Edit (id: 'TKey) = this.Edit id
            member this.Edit (model: 'TModel) = this.Edit model
            member this.Create() = this.Create()
            member this.Create model = this.Create model
            member this.Delete (id: 'TKey) = this.Delete id
            member this.Delete (model: 'TModel) = this.Delete model

    abstract member Index : unit -> ActionResult

    abstract member Detail : 'TKey -> ActionResult
    abstract member Edit : 'TKey -> ActionResult
    [<HttpPost>]
    abstract member Edit : 'TModel -> ActionResult
    abstract member Create : unit -> ActionResult
    [<HttpPost>]
    abstract member Create : 'TModel -> ActionResult
    abstract member Delete : 'TKey -> ActionResult
    [<HttpPost>]
    abstract member Delete : 'TModel -> ActionResult