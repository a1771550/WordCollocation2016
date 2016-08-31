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
	public class WordController : WcControllerBase
	{
		private readonly WordRepository repo = new WordRepository();

		
		public ActionResult Index(int page = 1)
		{
			CommonViewModel model = new CommonViewModel(ModelType.Word, page);
			ViewBag.Title = ModelType.Word + THResources.Resources.List;
			return View("CommonList", model);
		}

		public ViewResult Edit(string id)
		{
			var word = repo.GetById(id);
			ViewBag.Action = (id == "0" || id == null) ? THResources.Resources.Add : THResources.Resources.Edit;
			ViewBag.Title = ViewBag.Action + "--" + ModelType.Word;
			ViewBag.Id = id;
			ViewBag.Entity = ModelType.Word.ToString();
			return View("CommonEdit", word);
		}

		// POST: Word/Edit/5
		[HttpPost]
		public ActionResult Edit(Word word, string returnUrl = null)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (word.Id.ToString() == "0") //newly created word
					{
						try
						{
							var w = new Word
							{
								Entry = word.Entry,
								EntryZht = word.EntryZht,
								EntryZhs = word.EntryZhs,
								EntryJap = word.EntryJap
							};

							bool[] isOk = repo.Add(w);

							// duplicated entry
							if (isOk[0])
							{
								ViewBag.IsDuplicatedEntry = true;
								return View("CommonEdit", word);
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
					else //edit word
					{
						var Id = Convert.ToInt16(word.Id);
						Word p = repo.GetById(Id.ToString());
						if (p != null)
						{
							p.Id = Id;
							p.Entry = word.Entry;
							p.EntryZht = word.EntryZht;
							p.EntryZhs = word.EntryZhs;
							p.EntryJap = word.EntryJap;
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

		// GET: Word/Delete/5
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
