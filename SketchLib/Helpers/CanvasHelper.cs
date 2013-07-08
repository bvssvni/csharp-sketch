using System;
using Cairo;
using Gtk;

namespace Utils
{
	public class CanvasHelper
	{
		private Widget m_control;
		private RenderDelegate m_renderDelegate;
		private double m_settingsWidth;
		private double m_settingsHeight;

		public delegate void RenderDelegate (Context context);
		public delegate void PositionDelegate (double x, double y);

		public CanvasHelper()
		{
		}

		public void Step1_SetControl (Widget control) {
			m_control = control;
			m_control.ExposeEvent += (o, args) => {
				using (var context = Gdk.CairoHelper.Create (args.Event.Window)) {
					CairoFillModule.Fill(context, m_control, new Cairo.Color(1, 1, 1));
					context.Antialias = Cairo.Antialias.Subpixel;
					
					var controlBounds = new Rectangle (0.0, 
					                                   0.0, 
					                                   control.Allocation.Width, 
					                                   control.Allocation.Height);
					var view = CairoCanvasViewModule.ViewRectangle (controlBounds, 
					                                                m_settingsWidth, 
					                                                m_settingsHeight);
					var viewTransform = CairoCanvasViewModule.ToViewFromControl (controlBounds, 
					                                                             view,
					                                                             m_settingsWidth,
					                                                             m_settingsHeight);
					context.Save ();
					context.Transform (viewTransform);
					if (m_renderDelegate != null) {
						m_renderDelegate (context);
					}
					context.Restore ();
					CairoCanvasViewModule.Draw (context, controlBounds, view);
				}
			};
		}

		public void Step2_SetTargetResolution (double targetResolutionWidth,
		                                       double targetResolutionHeight) {
			m_settingsWidth = targetResolutionWidth;
			m_settingsHeight = targetResolutionHeight;
		}

		public void Step3_SetRenderDelegate (RenderDelegate renderDelegate) {
			m_renderDelegate = renderDelegate;
		}
	}
}

