using System;
using System.Web.Mvc;
using MyWcModel;
using MyWcModel.Abstract;
using UI.Controllers.Abstract;
using UI.Models.ViewModels;

namespace UI.Controllers
{
	//[Authorize(Roles = "Admin, Editor")]
	[Authorize]
	public class PosController : WcControllerBase
	{
		private readonly PosRepository repo = new PosRepository();

		public ActionResult Index()
		{
			CommonViewModel model = new CommonViewModel(ModelType.Pos, 1);
			ViewBag.Title = ModelType.Pos + THResources.Resources.List;
			return View("CommonList", model);
		}
		
		public ViewResult Edit(string id)
		{
			var pos = repo.GetById(id);
			ViewBag.Action = (id == "0" || id == null) ? THResources.Resources.Add : THResources.Resources.Edit;
			ViewBag.Title = ViewBag.Action + "--" + ModelType.Pos;
			ViewBag.Id = id;
			ViewBag.Entity = ModelType.Pos.ToString();
			return View("CommonEdit", pos);
		}

		// POST: pos/Edit/5
		[HttpPost]
		public ActionResult Edit(pos pos, string returnUrl = null)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (pos.Id.ToString() == "0") //newly created pos
					{
						try
						{
							var p = new pos
							{
								Entry = pos.Entry,
								EntryZht = pos.EntryZht,
								EntryZhs = pos.EntryZhs,
								EntryJap = pos.EntryJap
							};

							bool[] isOk = repo.Add(p);

							// duplicated entry
							if (isOk[0])
							{
								ViewBag.IsDuplicatedEntry = true;
								return View("CommonEdit", pos);
							}

							if (!isOk[1])  // add failed!
								return View("_DbActionErrorPartial");

							// add ok!
							if (!isOk[0] && isOk[1])
							{
								return RedirectToAction("Index");
							}
						}
						catch (Exception exception)
						{
							throw new Exception(exception.Message, exception.InnerException);
						}

					}
					else //edit pos
					{
						var Id = Convert.ToInt16(pos.Id);
						pos p = repo.GetById(Id.ToString());
						if (p != null)
						{
							p.Id = Id;
							p.Entry = pos.Entry;
							p.EntryZht = pos.EntryZht;
							p.EntryZhs = pos.EntryZhs;
							p.EntryJap = pos.EntryJap;
							repo.Update(p);
							//CacheHelper.Clear(repo.GetCacheName);
							return RedirectToAction("Index");
						}
					}

				}
				else return View("CommonEdit");

			}
			catch (Exception exception)
			{
				ViewBag.ErrorMessage = exception.Message;
				ViewBag.InnerMessage = exception.InnerException;
				return View("CommonEdit");
			}
			return null;
		}

		// GET: pos/Delete/5
		public ActionResult Delete(short id, string returnUrl = null)
		{
			try
			{
				if (repo.Delete(id.ToString()))
				{
					//CacheHelper.Clear(repo.GetCacheName);
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
