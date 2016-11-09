using UnityEngine;
using System.Collections;

namespace DarkArtsStudios.ScreenshotCreator.Examples {
	
	[RequireComponent(typeof(ScreenshotManager))]
	public class ScreenshotOnKeyPress : MonoBehaviour {
		
		public KeyCode takeScreenshotKey = KeyCode.F10;
		
		private ScreenshotManager screenshotManager = null;

		string newFilename
		{
			get
			{
				return string.Format( "Screenshot {0}.png", System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") );
			}
		}
		
		void Start()
		{
			screenshotManager = this.gameObject.GetComponent( typeof( ScreenshotManager ) ) as ScreenshotManager;
		}
		
		void Update()
		{
			if ( screenshotManager != null && Input.GetKeyDown( takeScreenshotKey ) )
			{
				string screenshotPath = screenshotManager.TakeScreenshot( newFilename );
				if (screenshotPath != "")
					Debug.Log( string.Format( "Saved Screenshot: {0}", screenshotPath ) );
			}
		}

	}
	
}
