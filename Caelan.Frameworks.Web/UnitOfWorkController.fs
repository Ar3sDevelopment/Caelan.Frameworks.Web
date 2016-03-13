namespace Caelan.Frameworks.Web.Classes

open System
open System.Data.Entity
open System.Linq
open System.Web.Mvc
open UnitOfWork.NET.Classes
open UnitOfWork.NET.Interfaces

type ICRUDController<'TModel, 'TKey when 'TKey : equality> = 
    abstract Index : unit -> ActionResult
    abstract Detail : 'TKey -> ActionResult
    abstract Edit : 'TKey -> ActionResult
    abstract Edit : 'TModel -> ActionResult
    abstract Create : unit -> ActionResult
    abstract Create : 'TModel -> ActionResult
    abstract Delete : 'TKey -> ActionResult
    abstract Delete : 'TModel -> ActionResult

[<AbstractClass>]
type GenericUnitOfWorkController<'TUnitOfWork when 'TUnitOfWork :> IUnitOfWork>() = 
    inherit Controller()
    let uow = Activator.CreateInstance<'TUnitOfWork>()

[<AbstractClass>]
type UnitOfWorkController<'TContext when 'TContext :> DbContext>() = 
    inherit GenericUnitOfWorkController<UnitOfWork<'TContext>>()

[<AbstractClass>]
type UnitOfWorkController<'TContext, 'TModel, 'TKey when 'TContext :> DbContext and 'TKey : equality>() = 
    inherit UnitOfWorkController<'TContext>()
    
    interface ICRUDController<'TModel, 'TKey> with
        member this.Index() = this.Index()
        member this.Detail id = this.Detail id
        member this.Edit(id : 'TKey) = this.Edit id
        member this.Edit(model : 'TModel) = this.Edit model
        member this.Create() = this.Create()
        member this.Create model = this.Create model
        member this.Delete(id : 'TKey) = this.Delete id
        member this.Delete(model : 'TModel) = this.Delete model
    
    abstract Index : unit -> ActionResult
    abstract Detail : 'TKey -> ActionResult
    abstract Edit : 'TKey -> ActionResult
    
    [<HttpPost>]
    abstract Edit : 'TModel -> ActionResult
    
    abstract Create : unit -> ActionResult
    
    [<HttpPost>]
    abstract Create : 'TModel -> ActionResult
    
    abstract Delete : 'TKey -> ActionResult
    [<HttpPost>]
    abstract Delete : 'TModel -> ActionResult

[<AbstractClass>]
type GenericUnitOfWorkController<'TUnitOfWork, 'TModel, 'TKey when 'TUnitOfWork :> IUnitOfWork and 'TKey : equality>() = 
    inherit GenericUnitOfWorkController<'TUnitOfWork>()
    
    interface ICRUDController<'TModel, 'TKey> with
        member this.Index() = this.Index()
        member this.Detail id = this.Detail id
        member this.Edit(id : 'TKey) = this.Edit id
        member this.Edit(model : 'TModel) = this.Edit model
        member this.Create() = this.Create()
        member this.Create model = this.Create model
        member this.Delete(id : 'TKey) = this.Delete id
        member this.Delete(model : 'TModel) = this.Delete model
    
    abstract Index : unit -> ActionResult
    abstract Detail : 'TKey -> ActionResult
    abstract Edit : 'TKey -> ActionResult
    
    [<HttpPost>]
    abstract Edit : 'TModel -> ActionResult
    
    abstract Create : unit -> ActionResult
    
    [<HttpPost>]
    abstract Create : 'TModel -> ActionResult
    
    abstract Delete : 'TKey -> ActionResult
    [<HttpPost>]
    abstract Delete : 'TModel -> ActionResult