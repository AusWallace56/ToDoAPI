using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models;
using ToDoAPI.DATA.EF;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        //GET - api/todo
        public IHttpActionResult GetToDos()
        {
            List<ToDoViewModel> toDos = db.ToDoItems.Include("Category").Select(t => new ToDoViewModel()
            {
                ToDoId = t.ToDoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            }).ToList<ToDoViewModel>();

            if (toDos.Count == 0)
            {
                return NotFound();
            }

            return Ok(toDos);
        }//End GetToDos()

        //GET - api/todo/id
        public IHttpActionResult GetToDo(int id)
        {
            ToDoViewModel toDo = db.ToDoItems.Include("Category").Where(t => t.ToDoId == id).Select(t => new ToDoViewModel()
            {

                ToDoId = t.ToDoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }

            }).FirstOrDefault();

            if (toDo == null)
            {
                return NotFound();
            }

            return Ok(toDo);

        }//End GetToDo()

        //POST - api/todo (HttpPost)
        public IHttpActionResult PostToDo(ToDoViewModel toDo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            ToDoItem newToDo = new ToDoItem()
            {
                Action = toDo.Action,
                Done = toDo.Done,
                CategoryId = toDo.CategoryId
            };

            db.ToDoItems.Add(newToDo);
            db.SaveChanges();

            return Ok();

        }//End PostToDo()

        //PUT - api/todo (HttpPut)
        public IHttpActionResult PutToDos(ToDoViewModel toDo)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            ToDoItem existingToDo = db.ToDoItems.Where(t => t.ToDoId == toDo.ToDoId).FirstOrDefault();

            if (existingToDo != null)
            {
                existingToDo.ToDoId = toDo.ToDoId;
                existingToDo.Action = toDo.Action;
                existingToDo.Done = toDo.Done;
                existingToDo.CategoryId = toDo.CategoryId;
                db.SaveChanges();
                return Ok();
            }

            return NotFound();
        }//End PutToDos()

        //DELETE - api/todo/id (HttpDelete)
        public IHttpActionResult DeleteToDo(int id)
        {
            ToDoItem toDo = db.ToDoItems.Where(t => t.ToDoId == id).FirstOrDefault();

            if (toDo != null)
            {
                db.ToDoItems.Remove(toDo);
                db.SaveChanges();
                return Ok();
            }

            return NotFound();
        }//End DeleteToDo()

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }//End class
}//End namespace
