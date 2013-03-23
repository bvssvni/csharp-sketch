using System;
using Gdk;

namespace Sketch
{
	[System.ComponentModel.ToolboxItem(true)]
	public class SketchControl : Gtk.DrawingArea
	{
		private App m_app;
		private int m_previewFrame;

		/// <summary>
		/// Has three stages:
		/// 0 - no preview
		/// 1 - intermediate step to avoid thread race
		/// 2 - running preview
		/// </summary>
		private int m_preview = 0;

		public App App {
			get {return m_app;}
			set {
				m_app = value;
			}
		}

		public bool Preview {
			get {
				return m_preview > 0;
			}
			set {
				if (m_preview == 2 && !value) {
					// Tell the thread to step down.
					m_preview--;
					return;
				}
				if (m_preview == 1) return;
				if (m_preview == 0 && value) {
					m_preview = 1;
					System.Threading.Thread th = new System.Threading.Thread(RunPreview);
					th.Start();
				}
			}
		}

		private void RunPreview() {
			m_previewFrame = 0;
			m_preview = 2;

			while (m_preview > 1) {
				m_previewFrame++;
				Gtk.Application.Invoke(delegate {
					RefreshGraphics();
				});	
				System.Threading.Thread.Sleep(100);
			}

			m_preview = 0;

			Gtk.Application.Invoke(delegate {
				RefreshGraphics();
			});	
		}

		public SketchControl ()
		{
			// Insert initialization code here.
			this.ButtonPressEvent += delegate(object o, Gtk.ButtonPressEventArgs args) {
				if (m_preview > 0) return;
				if (m_app == null) return;
				if (!m_app.HasSelectedFrame) return;
				if (args.Event.Button != 1) return;

				var stroke = new Stroke();
				stroke.Add(new Point(args.Event.X, args.Event.Y));
				m_app.BeginStroke(stroke);
			};
			this.ButtonReleaseEvent += delegate(object o, Gtk.ButtonReleaseEventArgs args) {
				if (m_preview > 0) return;
				if (m_app == null) return;
				if (!m_app.HasSelectedFrame) return;
				if (args.Event.Button != 1) return;

				var lastStroke = m_app.LastStroke();
				if (lastStroke == null) return;
				
				lastStroke.Add(new Point(args.Event.X, args.Event.Y));

				m_app.EndStroke();

				m_app.RefreshTitle();
				m_app.RefreshGraphics();
			};
			this.MotionNotifyEvent += delegate(object o, Gtk.MotionNotifyEventArgs args) {
				if (m_preview > 0) return;
				var state = (ModifierType)args.Event.State;
				if ((state & ModifierType.Button1Mask) != 0) {
					if (m_app == null) return;
					if (!m_app.HasSelectedFrame) return;

					var lastStroke = m_app.LastStroke();
					if (lastStroke == null) return;

					lastStroke.Add(new Point(args.Event.X, args.Event.Y));

					RefreshGraphics();
				}
			};
			
			this.Events = EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | EventMask.PointerMotionMask;

		}

		public void RefreshGraphics() {
			this.QueueDraw();
		}

		protected override bool OnExposeEvent (Gdk.EventExpose ev)
		{
			if (m_app == null) return true;

			using (var context = Gdk.CairoHelper.Create (ev.Window)) {
				CairoFill.Fill(context, this, new Cairo.Color(1, 1, 1));

				if (m_preview == 0) {
					m_app.DrawSelectedFrame(context);
				} else {
					var n = m_app.FrameData.Frames.Count;
					var frame = m_app.FrameData.Frames[m_previewFrame % n];
					context.Color = new Cairo.Color(0, 0, 0);
					frame.Draw(context);
				}
			}

			base.OnExposeEvent (ev);
			// Insert drawing code here.
			return true;
		}

		protected override void OnSizeAllocated (Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated (allocation);
			// Insert layout code here.
		}

		protected override void OnSizeRequested (ref Gtk.Requisition requisition)
		{
			// Calculate desired size here.
			requisition.Height = 50;
			requisition.Width = 50;
		}
	}
}

