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
	public class ColPosController : WcControllerBase
	{
		private readonly ColPosRepository repo = new ColPosRepository();

		public ActionResult Index()
		{
			CommonViewModel model = new CommonViewModel(ModelType.ColPos, 1);
			ViewBag.Title = ModelType.ColPos + THResources.Resources.List;
			return View("CommonList", model);
		}

		public ViewResult Edit(string id)
		{
			var colColPos = repo.GetById(id);
			ViewBag.Action = (id == "0" || id == null) ? THResources.Resources.Add : THResources.Resources.Edit;
			ViewBag.Title = ViewBag.Action + "--" + ModelType.ColPos;
			ViewBag.Id = id;
			ViewBag.Entity = ModelType.ColPos.ToString();
			return View("CommonEdit", colColPos);
		}

		// POST: ColPos/Edit/5
		[HttpPost]
		public ActionResult Edit(ColPos colColPos, string returnUrl = null)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (colColPos.Id.ToString() == "0") //newly created colColPos
					{
						try
						{
							var p = new ColPos
							{
								Entry = colColPos.Entry,
								EntryZht = colColPos.EntryZht,
								EntryZhs = colColPos.EntryZhs,
								EntryJap = colColPos.EntryJap
							};

							bool[] isOk = repo.Add(p);

							// duplicated entry
							if (isOk[0])
							{
								ViewBag.IsDuplicatedEntry = true;
								return View("CommonEdit", colColPos);
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
					else //edit colColPos
					{
						var Id = Convert.ToInt16(colColPos.Id);
						ColPos p = repo.GetById(Id.ToString());
						if (p != null)
						{
							p.Id = Id;
							p.Entry = colColPos.Entry;
							p.EntryZht = colColPos.EntryZht;
							p.EntryZhs = colColPos.EntryZhs;
							p.EntryJap = colColPos.EntryJap;
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

		// GET: ColPos/Delete/5
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
