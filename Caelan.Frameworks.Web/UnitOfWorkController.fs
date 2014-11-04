﻿namespace Caelan.Frameworks.Web.Classes

open System
open System.Data.Entity
open System.Linq
open System.Web.Mvc
open Caelan.Frameworks.BIZ.Interfaces
open Caelan.Frameworks.BIZ.Classes

[<AbstractClass>]
type GenericUnitOfWorkController<'TUnitOfWork when 'TUnitOfWork :> IUnitOfWork and 'TUnitOfWork :> IDisposable and 'TUnitOfWork : (new : unit -> 'TUnitOfWork)>() = 
    inherit Controller()
    member __.UnitOfWork<'T>(call: Func<IUnitOfWork, 'T>) = using (new 'TUnitOfWork()) (fun manager -> call.Invoke(manager))
    member __.UnitOfWork(call: Action<IUnitOfWork>) = using (new 'TUnitOfWork()) (fun manager -> call.Invoke(manager))

    member this.Repository<'T, 'TRepository when 'TRepository :> IRepository>(call: Func<'TRepository, 'T>) =
        this.UnitOfWork(fun t -> call.Invoke(t.Repository<'TRepository>()))

    member this.Repository<'T, 'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>(call: Func<IRepository<'TEntity, 'TDTO>, 'T>) =
        this.UnitOfWork(fun t -> call.Invoke(t.Repository<'TEntity, 'TDTO>()))

    member this.RepositoryList<'TEntity, 'TDTO when 'TEntity : not struct and 'TEntity : equality and 'TEntity : null and 'TDTO : not struct and 'TDTO : equality and 'TDTO : null>() =
        this.Repository<seq<'TDTO>, 'TEntity, 'TDTO>(fun t -> t.List())

    member this.UnitOfWorkCallSaveChanges(call: Action<IUnitOfWork>) =
        this.UnitOfWork(fun t ->
            call.Invoke(t)
            t.SaveChanges() <> 0)

    member __.Empty() = EmptyResult()

[<AbstractClass>]
type UnitOfWorkController<'TContext when 'TContext :> DbContext>() =
    class
    inherit GenericUnitOfWorkController<UnitOfWork<'TContext>>()
    end