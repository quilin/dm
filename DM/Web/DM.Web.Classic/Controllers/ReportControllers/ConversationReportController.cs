using System;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ReportControllers
{
	public class ConversationReportController : DmControllerBase
	{
		private readonly IReportService reportService;
		private readonly IConversationService conversationService;
		private readonly IIntentionsManager intentionsManager;
		private readonly IUserProvider userProvider;

		public ConversationReportController(
			IReportService reportService,
			IConversationService conversationService,
			IIntentionsManager intentionsManager,
            IUserProvider userProvider
            )
		{
			this.reportService = reportService;
			this.conversationService = conversationService;
			this.intentionsManager = intentionsManager;
			this.userProvider = userProvider;
		}

		[HttpPost]
		public ActionResult Report(Guid[] messageIds)
		{
			var conversation = reportService.ReportPrivateMessages(messageIds);
			return Json(new
			{
				lastPageNumber = PagingHelper.GetTotalPages(conversationService.CountMessages(conversation.ConversationId), userProvider.CurrentSettings.MessagesPerPage),
				canReport = intentionsManager.IsAllowed(CommonIntention.Report)
			});
		}
	}
}