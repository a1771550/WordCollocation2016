using System;
using System.Web.Mvc;
using BLL;
using BLL.Abstract;
using THResources;
using UI.Controllers.Abstract;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Controllers
{
	[Authorize(Roles = "Admin, Editor")]
	public class ExampleController : WcControllerBase
	{
		private readonly ExampleRepository repo = new ExampleRepository();

		//[Authorize]
		//// for demo purpose...
		//public ActionResult Index(int page = 1)
		//{
		//	var model = new ExampleViewModel(page);
		//	return View("Index",model);
		//}

		//[Authorize(Roles = "Editor")]
		//// GET: Word
		//public ActionResult IndexForEditor(int page = 1)
		//{
		//	return Index(page);
		//}

		[Authorize(Roles = "Admin")]
		public ActionResult Index(int page = 1)
		{
			var model = new ExampleViewModel(page);
			ViewBag.Title = ModelType.Example + THResources.Resources.List;
			return View("Index",model);
		}

		public ActionResult CreateForCollocation(long id)
		{
			var model = new ExampleViewModel(id, CreateMode.Collocation);
			ViewData["Title"] = "Create For Collocation";
			ViewBag.CollocationId = id;
			ViewBag.Controller = "Collocation";
			ViewData["CreateMode"] = CreateMode.Collocation;
			return View("Edit", model);
		}

		public ViewResult Edit(long id)
		{
			var model = new ExampleViewModel(id, CreateMode.Example);
			ViewData["Action"] = id == 0 ? THResources.Resources.Add : THResources.Resources.Edit;
			ViewBag.ExampleId = id;
			ViewBag.Controller = "Example";
			ViewData["CreateMode"] = CreateMode.Example;
			return View("Edit", model);
		}

		[HttpPost]
		public ActionResult Edit(Example example, string source, string collocationId, string createMode, string returnUrl = null)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (example.Id == 0) //newly created example
					{
						try
						{
							Example p = new Example
							{
								Entry = example.Entry,
								EntryZht = example.EntryZht,
								EntryZhs = example.EntryZhs,
								EntryJap = example.EntryJap,
								RemarkZht = example.RemarkZht,
								RemarkJap = example.RemarkJap,
								RemarkZhs = example.RemarkZhs,
								Source = source,
								CollocationId = long.Parse(collocationId)
							};

							bool[] isOk = repo.Add(p);

							// duplicated entry
							if (isOk[0])
							{
								ViewBag.IsDuplicatedEntry = true;
								return View("Edit", new ExampleViewModel(example));
							}

							if (!isOk[1])  // add failed!
								return View("_DbActionErrorPartial");

							// add ok!
							if (!isOk[0] && isOk[1])
							{
								if (createMode != null)
								{
									CreateMode mode = (CreateMode)Enum.Parse(typeof(CreateMode), createMode, true);
									switch (mode)
									{
										case CreateMode.Example:
											//if (User.IsInRole("Admin"))
											//	return RedirectToRoute(new { action = "IndexForAdmin" });
											//if (User.IsInRole("Editor"))
											//	return RedirectToRoute(new { action = "IndexForEditor" });
											return RedirectToAction("Index");
										case CreateMode.Collocation:
											//if (User.IsInRole("Admin"))
											//	return RedirectToRoute(new { controller = "Collocation", action = "IndexForAdmin" });
											//if (User.IsInRole("Editor"))
											//	return RedirectToRoute(new { controller = "Collocation", action = "IndexForEditor" });
											return RedirectToRoute(new {controller="Collocation",action="Index"});
									}
								}
							}
						}
						catch (Exception exception)
						{
							throw new Exception("Create Failed", exception.InnerException);
						}

					}
					else //edit example
					{
						Example p = repo.GetById(example.Id.ToString());
						if (p != null)
						{
							p.Id = example.Id;
							p.Entry = example.Entry;
							p.EntryZht = example.EntryZht;
							p.EntryZhs = example.EntryZhs;
							p.EntryJap = example.EntryJap;
							p.RemarkZht = example.RemarkZht;
							p.RemarkJap = example.RemarkJap;
							p.RemarkZhs = example.RemarkZhs;
							p.Source = example.Source;
							p.CollocationId = example.CollocationId;
							repo.Update(p);

							//if (User.IsInRole("Admin"))
							//	return RedirectToRoute(new { action = "IndexForAdmin" });
							//if (User.IsInRole("Editor"))
							//	return RedirectToRoute(new { action = "IndexForEditor" });

							return RedirectToAction("Index");
						}
					}

				}
				else return View("Edit");

			}
			catch (Exception exception)
			{
				ViewBag.ErrorMessage = exception.Message;
				ViewBag.InnerMessage = exception.InnerException;
				return View("Edit");
			}
			return null;
		}

		// GET: Example/Delete/5
		public ActionResult Delete(long id, string returnUrl = null)
		{
			try
			{
				if (repo.Delete(id.ToString()))
				{
					//if (User.IsInRole("Admin"))
					//	return RedirectToRoute(new { action = "IndexForAdmin" });
					//if (User.IsInRole("Editor"))
					//	return RedirectToRoute(new { action = "IndexForEditor" });
					return RedirectToAction("Index");
				}

				return View("_DbActionErrorPartial");
			}
			catch (Exception exception)
			{
				throw new Exception("Delete Failed", exception.InnerException);
			}
		}
	}
}