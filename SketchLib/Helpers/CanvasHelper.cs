using System;
using Cairo;
using Gtk;
using Gdk;

namespace Utils
{
	public class CanvasHelper
	{
		private Widget m_control;
		private RenderDelegate m_renderDelegate;
		private double m_settingsWidth;
		private double m_settingsHeight;
		private MouseToolAction[] m_mouseToolActions;
		private MouseToolAction m_currentMouseToolAction;
		private bool m_leftMouseDown = false;
		private bool m_rightMouseDown = false;
		private bool m_middleMouseDown = false;
		private Matrix m_mouseDownViewTransform;
		private Cairo.PointD m_mouseDownViewPosition;
		private Cairo.PointD m_mouseDownControlPosition;

		public delegate void RenderDelegate (Context context);

		public CanvasHelper()
		{
		}

		private Cairo.Rectangle CurrentControlBounds {
			get {
				return new Cairo.Rectangle (0.0, 
				                            0.0, 
				                            m_control.Allocation.Width, 
				                            m_control.Allocation.Height);
			}
		}

		private Cairo.Rectangle CurrentView {
			get {
				var controlBounds = this.CurrentControlBounds;
				var view = CairoCanvasViewModule.ViewRectangle (controlBounds, 
				                                                m_settingsWidth, 
				                                                m_settingsHeight);
				return view;
			}
		}

		private Matrix CurrentViewTransform {
			get {
				var viewTransform = CairoCanvasViewModule.ToViewFromControl (this.CurrentControlBounds, 
				                                                             this.CurrentView,
				                                                             m_settingsWidth,
				                                                             m_settingsHeight);
				return viewTransform;
			}
		}

		public void Step1_SetControl (Widget control) {
			m_control = control;
			m_control.ExposeEvent += (o, args) => {
				using (var context = Gdk.CairoHelper.Create (args.Event.Window)) {
					CairoFillModule.Fill(context, m_control, new Cairo.Color(1, 1, 1));
					context.Antialias = Cairo.Antialias.Subpixel;
					context.Save ();
					context.Transform (this.CurrentViewTransform);
					if (m_renderDelegate != null) {
						m_renderDelegate (context);
					}
					context.Restore ();
					CairoCanvasViewModule.Draw (context, 
					                            this.CurrentControlBounds,
					                            this.CurrentView);
				}
			};

			m_control.ButtonPressEvent += delegate(object o, Gtk.ButtonPressEventArgs args) {
				var button = args.Event.Button;
				switch (button) {
					case 1: m_leftMouseDown = true; break;
					case 2: m_middleMouseDown = true; break;
					case 3: m_rightMouseDown = true; break;
				}

				var viewTransform = this.CurrentViewTransform;
				m_mouseDownViewTransform = viewTransform;

				// Set mouse down locations.
				var px = args.Event.X;
				var py = args.Event.Y;
				this.m_mouseDownControlPosition = new Cairo.PointD (px, py);
				var inv = viewTransform.Clone () as Matrix;
				inv.Invert ();
				inv.TransformPoint (ref px, ref py);
				this.m_mouseDownViewPosition = new Cairo.PointD (px, py);
			};
			m_control.ButtonReleaseEvent += delegate(object o, Gtk.ButtonReleaseEventArgs args) {
				var button = args.Event.Button;
				switch (button) {
					case 1: m_leftMouseDown = false; break;
					case 2: m_middleMouseDown = false; break;
					case 3: m_rightMouseDown = false; break;
				}
			};
			m_control.MotionNotifyEvent += delegate(object o, Gtk.MotionNotifyEventArgs args) {
				// TEST
				// Console.WriteLine ("mouse move");
			};
			
			m_control.Events |= EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | EventMask.PointerMotionMask;

		}

		public void Step2_SetTargetResolution (double targetResolutionWidth,
		                                       double targetResolutionHeight) {
			m_settingsWidth = targetResolutionWidth;
			m_settingsHeight = targetResolutionHeight;
		}

		public void Step3_SetRenderDelegate (RenderDelegate renderDelegate) {
			m_renderDelegate = renderDelegate;
		}

		public void Step4_SetMouseToolActions (MouseToolAction[] mouseToolActions) {
			m_mouseToolActions = mouseToolActions;
		}
	}
}

