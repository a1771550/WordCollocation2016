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
	public class ColWordController : WcControllerBase
	{
		private readonly ColWordRepository repo = new ColWordRepository();


		public ActionResult Index(int page = 1)
		{
			CommonViewModel model = new CommonViewModel(ModelType.ColWord, page);
			ViewBag.Title = ModelType.ColWord + THResources.Resources.List;
			return View("CommonList", model);
		}

		public ViewResult Edit(string id)
		{
			var colWord = repo.GetById(id);
			ViewBag.Action = (id == "0" || id == null) ? THResources.Resources.Add : THResources.Resources.Edit;
			ViewBag.Title = ViewBag.Action + "--" + ModelType.ColWord;
			ViewBag.Id = id;
			ViewBag.Entity = ModelType.ColWord.ToString();
			return View("CommonEdit", colWord);
		}

		// POST: ColWord/Edit/5
		[HttpPost]
		public ActionResult Edit(ColWord colWord, string returnUrl = null)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (colWord.Id.ToString() == "0") //newly created colWord
					{
						try
						{
							var w = new ColWord
							{
								Entry = colWord.Entry,
								EntryZht = colWord.EntryZht,
								EntryZhs = colWord.EntryZhs,
								EntryJap = colWord.EntryJap
							};

							bool[] isOk = repo.Add(w);

							// duplicated entry
							if (isOk[0])
							{
								ViewBag.IsDuplicatedEntry = true;
								return View("CommonEdit", colWord);
							}

							if (!isOk[1])  // add failed!
								return View("_DbActionErrorPartial");

							// add ok!
							if (!isOk[0] && isOk[1])
							{
								//if (User.IsInRole("Admin"))
								//	return RedirectToAction("IndexForAdmin");
								//if (User.IsInRole("Editor"))
								//	return RedirectToAction("IndexForEditor");
								return RedirectToAction("Index");
							}
						}
						catch (Exception exception)
						{
							throw new Exception(exception.Message, exception.InnerException);
						}

					}
					else //edit colWord
					{
						var Id = Convert.ToInt16(colWord.Id);
						ColWord p = repo.GetById(Id.ToString());
						if (p != null)
						{
							p.Id = Id;
							p.Entry = colWord.Entry;
							p.EntryZht = colWord.EntryZht;
							p.EntryZhs = colWord.EntryZhs;
							p.EntryJap = colWord.EntryJap;
							repo.Update(p);
							//CacheHelper.Clear(repo.GetCacheName);
							//if (User.IsInRole("Admin"))
							//	return RedirectToAction("IndexForAdmin");
							//if (User.IsInRole("Editor"))
							//	return RedirectToAction("IndexForEditor");
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

		// GET: ColWord/Delete/5
		public ActionResult Delete(short id, string returnUrl = null)
		{
			try
			{
				if (repo.Delete(id.ToString()))
				{
					//CacheHelper.Clear(repo.GetCacheName);
					//if (User.IsInRole("Admin"))
					//	return RedirectToAction("IndexForAdmin");
					//if (User.IsInRole("Editor"))
					//	return RedirectToAction("IndexForEditor");
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
