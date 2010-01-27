﻿using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.IO;
using N2.Web.UI.WebControls;
using N2.Web;
using N2.Collections;

namespace N2.Edit.Navigation
{
	public class LoadTree : HelpfulHandler
	{
		public override void ProcessRequest(HttpContext context)
		{
            string target = context.Request["target"] ?? Targets.Preview;

			ContentItem selectedNode = GetSelectedItem(context.Request.QueryString);
			
			context.Response.ContentType = "text/plain";

			ItemFilter filter = N2.Context.Current.EditManager.GetEditorFilter(context.User);
			TreeNode tn = (TreeNode)N2.Web.Tree
				.From(selectedNode, 2)
				.LinkProvider(delegate(ContentItem node) { return N2.Edit.Web.UI.Controls.Tree.BuildLink(node, node.Path == selectedNode.Path, target); })
				.Filters(filter)
				.ToControl();
			
			Web.UI.Controls.Tree.AppendExpanderNodeRecursive(tn, filter, target);

			RenderControls(tn.Controls, context.Response.Output);
		}

		private static void RenderControls(IEnumerable controls, TextWriter output)
		{
			using (HtmlTextWriter writer = new HtmlTextWriter(output))
			{
				foreach (Control childNode in controls)
				{
					childNode.RenderControl(writer);
				}
			}
		}

		//public ILinkBuilder BuildLink(INode node, ContentItem selectedNode, string target)
		//{
		//    string className = node.ClassNames;

		//    ILinkBuilder builder = Link.To(node)
		//        .Target(target)
		//        .Class(className)
		//        .Href(node.PreviewUrl)
		//        .Text("<img src='" + N2.Web.Url.ToAbsolute(node.IconUrl) + "'/>" + node.Contents)
		//        .Attribute("rel", node.Path);

		//    return builder;
		//}
	}
}