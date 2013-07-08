using System;
using Gdk;
using Utils;

namespace Sketch
{
	[System.ComponentModel.ToolboxItem(true)]
	public class SketchControl : Gtk.DrawingArea
	{
		private App m_app;
		private int m_previewFrame;
		private System.Threading.Thread m_previewThread;
		private double m_settingsWidth = 400.0;
		private double m_settingsHeight = 300.0;
		private CanvasHelper m_canvasViewHelper = null;

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
				if (m_preview == 1) {
					// Wait for the preview to finish.
					m_previewThread.Join();
				}

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
					m_previewThread = new System.Threading.Thread(RunPreview);
					m_previewThread.Start();
					while (m_preview != 2) {
						System.Threading.Thread.Sleep(0);
					}
				}
			}
		}

		public bool IsBusy() {
			return m_preview > 0;
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
			var mousedown = false;
			var strokeHelper = new StrokeHelper();
			StrokeAdvisor advisor = null;
			m_canvasViewHelper = new CanvasHelper ();
			m_canvasViewHelper.Step1_SetControl (this);
			m_canvasViewHelper.Step2_SetTargetResolution (m_settingsWidth, m_settingsHeight);
			m_canvasViewHelper.Step3_SetRenderDelegate (Render);
			m_canvasViewHelper.Step4_SetMouseToolActions (
				new MouseToolAction[] {
					new MouseToolAction () {
					Button = MouseToolAction.MouseButton.Left,
					Modifier = MouseToolAction.ModifierKey.None,
					MouseDown = (x, y) => {
						if (m_app == null) return;
						if (m_app.IsBusy()) return;
						
						advisor = new StrokeAdvisor(m_app);
						var frame = m_app.Data.Frames[m_app.SelectedFrame];
						strokeHelper.Step1_SetFrame(frame);
						strokeHelper.Step2_CreateStroke();
						strokeHelper.Step3_AddPoint(new Point(x, y));
						mousedown = true;
						advisor.Do(StrokeAdvisor.Event.StartStroke);
					},
					MouseMove = (x, y) => {
						if (m_app.IsBusy()) return;
						if (advisor == null) return;
						
						strokeHelper.Step3_AddPoint(new Point(x, y));
						strokeHelper.Step4_EndStroke(m_app);
						mousedown = false;
						advisor.Do(StrokeAdvisor.Event.EndStroke);
					},
					MouseUp = (x, y) => {
						if (!mousedown) return;
						if (m_app == null) return;
						if (m_app.IsBusy()) return;
						if (advisor == null) return;

						strokeHelper.Step3_AddPoint(new Point(x, y));
						advisor.Do(StrokeAdvisor.Event.Stroking);
					}
				}
			});
		}

		public void RefreshGraphics() {
			this.QueueDraw();
		}

		private void Render (Cairo.Context context) {
			if (m_app == null) {return;}

			if (m_preview == 0) {
				m_app.Draw(context);
			} else {
				var n = m_app.Data.Frames.Count;
				var frame = m_app.Data.Frames[m_previewFrame % n];
				context.Color = new Cairo.Color(0, 0, 0);
				frame.Draw(context);
			}
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

